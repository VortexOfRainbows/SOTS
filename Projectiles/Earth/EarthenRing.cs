using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{
	public class EarthenRing : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthen Ring");
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 60; k++)
			{
				Vector2 circularPos = new Vector2(Projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians(k * 6) + Projectile.rotation);
				Color color = new Color(100, 100, 100, 0);
				Vector2 drawPos = Projectile.Center + circularPos - Main.screenPosition;
				color = Projectile.GetAlpha(color) * 0.1f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 2.55f / 255f, (255 - Projectile.alpha) * 1.9f / 255f, 0);
			Projectile.rotation += MathHelper.ToRadians(3);
			Projectile.alpha += 4;
			if (Projectile.timeLeft < 12)
			{
				Projectile.alpha += 3;
			}
			if (Projectile.ai[1] == 0)
			{
				Projectile.ai[0]++;
				if(Projectile.ai[0] >= 22)
				{
					Projectile.ai[1] = 1;
				}
			}
			if (Projectile.ai[1] == 1)
			{
				Projectile.ai[0]--;
				if (Projectile.ai[0] <= 16)
				{
					Projectile.ai[1] = 0;
				}
			}
		}
	}
}