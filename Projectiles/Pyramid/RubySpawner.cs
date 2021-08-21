using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Pyramid
{    
    public class RubySpawner : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Spawner");
		}
        public override void SetDefaults()
        {
			projectile.height = 60;
			projectile.width = 60;
			projectile.magic = false;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 130;
			projectile.tileCollide = false;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects1 = SpriteEffects.None;
			Texture2D texture1 = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture1.Width/2, texture1.Height/2);
			Color alpha = (Color)this.GetAlpha(lightColor);
			Color color1 = alpha * 1.0f;
			Color color2 = Color.Lerp(alpha, Color.Black, 0.5f);
			color2.A = alpha.A;
			float num1 =  0.95f + (projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
			Color color4 = color2 * num1;
			float scale = 0.4f + projectile.scale * 0.8f * num1;
			for(int i = 0; i < 1; i++)
			{
				Main.spriteBatch.Draw(Main.extraTexture[50], projectile.Center - Main.screenPosition, null, color4, -projectile.rotation + 0.35f, origin, scale, effects1 ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(Main.extraTexture[50], projectile.Center - Main.screenPosition, null, alpha, -projectile.rotation, origin, projectile.scale, effects1 ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(texture1, projectile.Center - Main.screenPosition, null, color1, -projectile.rotation * 0.7f, origin, projectile.scale, effects1 ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(Main.extraTexture[50], projectile.Center - Main.screenPosition, null, alpha * 0.8f, projectile.rotation * 0.5f, origin, projectile.scale * 0.9f, effects1, 0.0f);
				alpha.A = 0;
			}
			return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255);
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
		bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
            {
				runOnce = false;
				projectile.scale = 0.0f;
            }
			Player player = Main.player[projectile.owner];
			projectile.ai[0]++;
			if(projectile.ai[0] <= 50)
			{
				Vector2 circular = new Vector2((projectile.width - 16) * projectile.scale, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45)) + projectile.rotation * 7);
				int num2 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) + circular - new Vector2(5), 0, 0, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(255, 164, 164, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 0.2f;
				dust.scale += 1.05f;
				dust.alpha = projectile.alpha;
				dust.velocity *= 0.05f;
				dust.velocity += circular.RotatedBy(MathHelper.ToRadians(80)).SafeNormalize(Vector2.Zero);
				projectile.rotation += MathHelper.ToRadians(6);
				projectile.scale += 0.0175f;
			}
			else if (projectile.ai[0] <= 90)
			{
				projectile.rotation += MathHelper.ToRadians(2);
			}
			else
			{
				projectile.rotation -= MathHelper.ToRadians(10);
				projectile.scale -= 0.027f;
				if(projectile.scale <= 0)
                {
					projectile.Kill();
                }
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 25; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(255, 164, 164, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
				dust.alpha = projectile.alpha;
				dust.velocity *= 1.5f;
			}
		}
	}
}
		