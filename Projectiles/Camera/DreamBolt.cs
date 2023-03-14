using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Prim.Trails;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Camera
{    
    public class DreamBolt : ModProjectile 
    {	
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22; 
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;    
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
		}
        public override bool? CanCutTiles()
        {
            return false;
        }
        public Color projColor => Projectile.ai[0] == 1 ? DreamingFrame.Green1 : DreamingSmog.PurpleGray;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Color color = projColor;
            color.A = 0;
            float scale = Projectile.scale;
            for(int i = 8; i > 0; i--)
            {
                float extraRotation = i * MathHelper.PiOver4;
                Vector2 circular = new Vector2(1.5f, 0).RotatedBy(extraRotation);
                if (Projectile.ai[0] == 1)
                    extraRotation = 0;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + circular, null, color * 0.45f, Projectile.rotation + extraRotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = (int)(Projectile.width * Projectile.scale * 1.0f);
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            if (runOnce)
            {
                if (Main.netMode != NetmodeID.Server)
                    SOTS.primitives.CreateTrail(new StarTrail(Projectile, projColor, projColor, 3, 12, Type));
                runOnce = false;
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            if(Main.netMode != NetmodeID.Server)
            {
                for(int i = 0; i < 10; i++)
                {
                    int width = (int)(Projectile.width * Projectile.scale * 1.0f);
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(width, width) / 2, width, width, ModContent.DustType<CopyDust4>());
                    dust.velocity = dust.velocity * 0.8f + Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f) * (float)Math.Sqrt(Projectile.velocity.Length());
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.color = projColor;
                    dust.scale *= 1.4f;
                }
            }
        }
        public override void AI()
        {
            int target = Common.GlobalNPCs.SOTSNPCs.FindTarget_Basic(Projectile.Center, 360f, Projectile);
            if (target != -1)
            {
                var normal = (Main.npc[target].Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, normal * (Projectile.velocity.Length() * 1.0125f + 1f), 0.07f);
            }
            else
            {
                Projectile.velocity *= 0.9925f;
                if (Projectile.ai[0] != 1)
                    Projectile.velocity.Y += 0.14f;
                else
                    Projectile.velocity.Y += 0.06f;
            }
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.color = projColor;
                dust.scale *= 1.2f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
	}	
}
			