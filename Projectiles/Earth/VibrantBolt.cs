using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Earth 
{    
    public class VibrantBolt : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Bolt");
		}
        public override void SetDefaults()
        {
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.width = 24;
			projectile.height = 14;
			projectile.alpha = 255;
			projectile.timeLeft = 20;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = VoidPlayer.VibrantColorAttempt(projectile.whoAmI * 12);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			color = projectile.GetAlpha(color);
			for (int j = 0; j < 3; j++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num2];
				Color color2 = VoidPlayer.VibrantColorAttempt(projectile.whoAmI * 12) * 0.66f;
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.5f;
				dust.alpha = projectile.alpha;
				dust.velocity *= 0.5f;
				dust.velocity += projectile.velocity * 0.25f;
			}
		}
		public override void AI()
		{
			projectile.alpha -= 30;
			if (projectile.alpha < 0)
				projectile.alpha = 0;
			int num2 = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
			Dust dust = Main.dust[num2];
			Color color2 = VoidPlayer.VibrantColorAttempt(projectile.whoAmI * 12);
			dust.color = color2;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 0.7f;
			dust.alpha = projectile.alpha;
			dust.velocity *= 0.1f;
			dust.velocity += projectile.velocity * 0.1f;
			projectile.rotation = projectile.velocity.ToRotation();
		}
	}
}
		
			