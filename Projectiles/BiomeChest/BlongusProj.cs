using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Helpers;
using System;
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
            Texture2D textureGlow = ModContent.Request<Texture2D>(this.Texture + "Glow").Value;
            Vector2 drawOrigin = new Vector2(Projectile.direction == -1 ? texture.Width - 50 : 50, 36);
            int height = texture.Height / Main.projFrames[Type];
            Rectangle rect = new Rectangle(0, Projectile.frame * height, texture.Width, height);
            float framePercent = (MathF.Max(1, Projectile.frame) - 1) / ((float)Main.projFrames[Type] - 1);
            float scale = 1.0f + framePercent * 0.1f;
            Color c = ColorHelper.RedEvilColor * 0.5f;
            c.A = 0;
            for (int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(3 - framePercent * 2.5f, 0).RotatedBy(i * MathHelper.Pi / 3f);
                Main.EntitySpriteDraw(texture, circular + Projectile.Center - Main.screenPosition, rect, c * (1.0f - 0.6f * framePercent), Projectile.rotation, drawOrigin, Projectile.scale * scale, Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, lightColor, Projectile.rotation, drawOrigin, Projectile.scale * scale, Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(textureGlow, Projectile.Center - Main.screenPosition, rect, Color.Lerp(lightColor, Color.White, framePercent) * 1.5f, Projectile.rotation, drawOrigin, Projectile.scale * scale, Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[0] == -1;
        }
        private bool RunOnce = true;
        private float Counter = 0;
        public override void AI()
        {
            if(RunOnce)
            {
                SOTSUtils.PlaySound(SoundID.Item97, Projectile.Center, 1.0f, 0.45f, 0.05f);
                RunOnce = false;
            }
            float lightIntensity = MathF.Min(0.5f, Counter / 120f);
            Lighting.AddLight(Projectile.Center, ColorHelper.RedEvilColor.ToVector3() * lightIntensity);
            Counter++;
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
                    Projectile.direction = -(int)SOTSUtils.SignNoZero(toProjFromNPC.X);
                    float rotateToEnemy = MathHelper.Clamp(toProjFromNPC.X * 0.04f, -1f, 1f);
                    Projectile.rotation = rotateToEnemy;
                }
                else if(Projectile.frame < 8)
                {
                    Projectile.frame = 8;
                    Projectile.friendly = false;
                }
                if(Projectile.friendly && Projectile.frame < 8)
                {
                    SOTSUtils.PlaySound(SoundID.Item98, Projectile.Center, 1.0f, 1f, 0);
                    Projectile.friendly = false;
                }
                frameMax = 13;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.X * 0.05f;
                if(Projectile.velocity.Length() < 14f)
                    Projectile.velocity += Projectile.velocity.SNormalize() * 0.035f;
                int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 320, Projectile, true);
                if(target != -1)
                {
                    NPC npc = Main.npc[target];
                    Vector2 toNPC = npc.Center + new Vector2(0, -npc.height / 3) - Projectile.Center;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SNormalize() * (Projectile.velocity.Length() + 0.06f), lightIntensity * 0.14f);
                }
                Vector2 sVelo = Projectile.velocity.SNormalize();
                float rate = SOTS.Config.lowFidelityMode ? .5f : .34f;
                for(float i = 0; i < 1; i += rate)
                {
                    if (Main.rand.NextBool())
                    {
                        if (Main.rand.NextBool())
                        {
                            PixelDust.Spawn(Projectile.Center - sVelo * 2 + Projectile.velocity * i, 0, 0, Main.rand.NextVector2Circular(0.3f, 0.3f), ColorHelper.RedEvilColor * lightIntensity, 6);
                        }
                        else
                        {
                            Dust d = Dust.NewDustDirect(Projectile.Center - sVelo * 2 + Projectile.velocity * i - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>(), newColor: ColorHelper.RedEvilColor * lightIntensity);
                            d.fadeIn = 0.2f;
                            d.noGravity = true;
                            d.scale = d.scale * 0.4f + 0.4f;
                            d.velocity = d.velocity * 0.2f + Projectile.velocity * -0.2f + Main.rand.NextVector2Circular(0.34f, 0.34f);
                        }
                    }
                }
            }
            Projectile.frameCounter ++;
            int targetFrame = Projectile.frame >= 6 ? 6 : Projectile.frame <= 1 ? 3 : 11;
            if (Projectile.frameCounter >= targetFrame)
            {
                Projectile.frame++;
                Projectile.frameCounter -= targetFrame;
                if (Projectile.frame >= Main.projFrames[Type])
                {
                    Projectile.Kill();
                }
                Projectile.frame %= frameMax;
            }
            if (Projectile.frame >= 8)
            {
                if (!Projectile.friendly)
                {
                    int count = 45;
                    for(int i = 0; i < count; i++)
                    {
                        Vector2 circular = new Vector2(1, 0).RotatedBy(i / (float)count * MathHelper.TwoPi);
                        float rand = Main.rand.NextFloat();
                        PixelDust.Spawn(Projectile.Center + Projectile.velocity * 0.4f, 0, 0, Main.rand.NextVector2Circular(0.34f, 0.34f) + circular * (12 * (1 - rand)), ColorHelper.RedEvilColor * (.8f - 0.4f * rand), 6).scale += rand * 2;
                    }
                    SOTSUtils.PlaySound(SoundID.DD2_KoboldExplosion, Projectile.Center, 0.7f, 1f, 0);
                    SOTSUtils.PlaySound(SoundID.NPCDeath19, Projectile.Center, 1.5f, 0.4f, 0);
                    Projectile.friendly = true;
                }
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
                    float flatLifeBonus = MathF.Min(target.life * 0.05f, 40); //Max 40 damage from life
                    float maxLifeBonus = MathF.Min(target.lifeMax * 0.1f, 30); //Max 30 damage from max life
                    if (flatLifeBonus >= 40)
                        flatLifeBonus += MathF.Min(target.life * 0.01f - 40, 60); //Then 1% more damage per life, up to 100
                    if (flatLifeBonus >= 100)
                        flatLifeBonus += MathF.Min(target.life * 0.0025f - 100, 100); //Then 0.25% more damage per life, up to 200
                    if (flatLifeBonus >= 200)
                        flatLifeBonus += MathF.Min(target.life * 0.001f - 200, 700); //Then 0.1% more damage per life, up to 1000
                    if (flatLifeBonus >= 1000)
                        flatLifeBonus += target.life * 0.0005f; //Then 0.05% more damage per life
                    float bleedMultiplier = 2.5f;
                    float damageBonus = flatLifeBonus + maxLifeBonus;
                    float bonusDamagePercent = 1.0f + 0.2f * target.life / target.lifeMax; //Deal up to 20% more damage based on enemy health percent
                    if(target.HasBuff(BuffID.Bleeding) || (target.TryGetGlobalNPC(out DebuffNPC dNPC) && dNPC.BleedingCurse > 0))
                    {
                        damageBonus *= bleedMultiplier;
                    }
                    modifiers.SourceDamage.Base += (int)(damageBonus + 0.99f);
                    modifiers.FinalDamage *= bonusDamagePercent;
                }
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = 24;
            if(Projectile.frame >= 8)
            {
                size = 100;
            }
            hitbox = new Rectangle((int)Projectile.Center.X - size/2, (int)Projectile.Center.Y - size/2, size, size);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 6;
			height = 6;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.X != Projectile.velocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (oldVelocity.Y != Projectile.velocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if(Projectile.frame < 8)
            {
                int count = 20;
                for (int i = 0; i < count; i++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(i / (float)count * MathHelper.TwoPi);
                    float rand = Main.rand.NextFloat();
                    PixelDust.Spawn(Projectile.Center + Projectile.velocity * 0.4f, 0, 0, Main.rand.NextVector2Circular(0.34f, 0.34f) + circular * (6 * (1 - rand)), ColorHelper.RedEvilColor * (.8f - 0.4f * rand), 6).scale += rand;
                }
            }
		}
	}
}