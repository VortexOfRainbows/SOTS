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
			Projectile.friendly = true;
			Projectile.ranged = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.width = 24;
			Projectile.height = 14;
			Projectile.alpha = 255;
			Projectile.timeLeft = 20;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = VoidPlayer.VibrantColorAttempt(Projectile.whoAmI * 12);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			color = Projectile.GetAlpha(color);
			for (int j = 0; j < 3; j++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num2];
				Color color2 = VoidPlayer.VibrantColorAttempt(Projectile.whoAmI * 12) * 0.66f;
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.5f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.5f;
				dust.velocity += Projectile.velocity * 0.25f;
			}
		}
		public override void AI()
		{
			Projectile.alpha -= 30;
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;
			int num2 = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
			Dust dust = Main.dust[num2];
			Color color2 = VoidPlayer.VibrantColorAttempt(Projectile.whoAmI * 12);
			dust.color = color2;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 0.7f;
			dust.alpha = Projectile.alpha;
			dust.velocity *= 0.1f;
			dust.velocity += Projectile.velocity * 0.1f;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
	}
}
		
			