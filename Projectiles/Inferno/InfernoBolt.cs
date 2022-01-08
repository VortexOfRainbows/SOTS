using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Prim.Trails;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Inferno
{    
    public class InfernoBolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Inferno Bolt");
		}
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20; 
            projectile.timeLeft = 180;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = true; 
            projectile.tileCollide = false;    
            projectile.ignoreWater = true; 
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Color color = new Color(100, 100, 100, 0);
            float scale = projectile.scale;
            for(int i = 0; i < 5; i++)
            {
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, color, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            bool other = projectile.ai[1] == -1 || projectile.ai[1] == -2;
            if (runOnce)
            {
                if(!other)
                    Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 34, 0.9f, 0.5f);
                else if(projectile.ai[1] == -2)
                    Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 62, 0.7f, 0.2f);
                if (Main.netMode != NetmodeID.Server)
                    SOTS.primitives.CreateTrail(new StarTrail(projectile, VoidPlayer.InfernoColorAttempt(0.4f), VoidPlayer.InfernoColorAttempt(0.4f), 10));
                runOnce = false;
                for (int i = 0; i < 5; i++)
                {
                    int width = (int)(projectile.width * projectile.scale * 1.0f);
                    Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(width, width) / 2, width, width, ModContent.DustType<CopyDust4>());
                    dust.velocity = dust.velocity * 0.8f + projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f) * (float)Math.Sqrt(projectile.velocity.Length());
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1));
                    dust.scale *= 1.4f;
                }
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            if(Main.netMode != NetmodeID.Server)
            {
                for(int i = 0; i < 7; i++)
                {
                    int width = (int)(projectile.width * projectile.scale * 1.0f);
                    Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(width, width) / 2, width, width, ModContent.DustType<CopyDust4>());
                    dust.velocity = dust.velocity * 0.8f + projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f) * (float)Math.Sqrt(projectile.velocity.Length());
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1));
                    dust.scale *= 1.4f;
                }
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if(Main.rand.NextBool(3))
                target.AddBuff(BuffID.OnFire, 180, false);
            else
                VoidPlayer.VoidBurn(mod, target, 30, 270);
        }
        public override void AI()
        {
            bool other = projectile.ai[1] == -1 || projectile.ai[1] == -2;
            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(-0.9f * projectile.ai[0]));
            projectile.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 0.07f;
            projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2 * (other ? -1 : 1);
            if (Main.rand.NextBool(8))
            {
                Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1));
                dust.scale *= 1.2f;
                projectile.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 0.36f;
            }
        }
	}	
}
			