using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using System;
using System.Formats.Tar;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{    
    public class BlongusProj : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 11;
		}
        public override void SetDefaults()
        {
			Projectile.width = 72;
			Projectile.height = 80;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 1200;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(Projectile.direction == -1 ? texture.Width - 50 : 50, 36);
            int height = texture.Height / Main.projFrames[Type];
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * height, texture.Width, height), lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[0] == -1;
        }
        public override void AI()
        {
			int StuckTo = (int)Projectile.ai[0];
            int frameMax = 2;
            if(StuckTo >= 0)
            {
                if (Projectile.timeLeft < 20)
                    Projectile.timeLeft = 20;
                NPC target = Main.npc[StuckTo];
                if(target.active && !target.dontTakeDamage)
                {
                    Vector2 toProjFromNPC = new Vector2(Projectile.ai[1], Projectile.ai[2]);
                    Projectile.Center = toProjFromNPC + target.Center;
                    Projectile.rotation = toProjFromNPC.ToRotation() + (Projectile.direction == -1 ? 0 : MathF.PI);
                }
                else
                {
                    if(Projectile.frame < 8)
                        Projectile.frame = 8;
                }
                Projectile.friendly = false;
                frameMax = 13;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.X * 0.06f;
                Projectile.velocity += Projectile.velocity.SNormalize() * 0.05f;
                int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 320, Projectile, true);
                if(target != -1)
                {
                    NPC npc = Main.npc[target];
                    Vector2 toNPC = npc.Center - Projectile.Center;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SNormalize() * Projectile.velocity.Length(), 0.05f);
                }
            }
            Projectile.frameCounter ++;
            int targetFrame = Projectile.frame >= 6 ? 5 : Projectile.frame <= 1 ? 5 : 10;
            if (Projectile.frameCounter > targetFrame)
            {
                Projectile.frame++;
                Projectile.frameCounter -= targetFrame;
                if (Projectile.frame >= 8)
                    Projectile.friendly = true;
                if (Projectile.frame >= Main.projFrames[Type])
                {
                    Projectile.Kill();
                }
                Projectile.frame %= frameMax;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == -1)
            {
                Projectile.ai[0] = target.whoAmI;
                Projectile.ai[1] = Projectile.Center.X - target.Center.X;
                Projectile.ai[2] = Projectile.Center.Y - target.Center.Y;
                Projectile.netUpdate = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			int StuckTo = (int)Projectile.ai[0];
            if (StuckTo >= 0 && Projectile.frame >= 8)
            {
                target = Main.npc[StuckTo];
                if (target.active)
                {
                    float lifeMultiplier = 0.04f;
                    float maxLifeMultiplier = 0.001f;
                    float bleedMultiplier = 2.5f;
                    float damageBonus = target.life * lifeMultiplier + target.lifeMax * maxLifeMultiplier;
                    if(target.HasBuff(BuffID.Bleeding) || (target.TryGetGlobalNPC(out DebuffNPC dNPC) && dNPC.BleedingCurse > 0))
                    {
                        damageBonus *= bleedMultiplier;
                    }
                    modifiers.SourceDamage.Base += (int)(damageBonus + 0.99f);
                }
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = 16;
            if(Projectile.frame >= 8)
            {
                size = 80;
            }
            hitbox = new Rectangle((int)Projectile.Center.X - size/2, (int)Projectile.Center.Y - size/2, size, size);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 6;
			height = 6;
            return true;
        }
		public override void OnKill(int timeLeft)
        {

		}
	}
}