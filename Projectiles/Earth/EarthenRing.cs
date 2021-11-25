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
			projectile.width = 48;
			projectile.height = 48;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 60; k++)
			{
				Vector2 circularPos = new Vector2(projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians(k * 6) + projectile.rotation);
				Color color = new Color(100, 100, 100, 0);
				Vector2 drawPos = projectile.Center + circularPos - Main.screenPosition;
				color = projectile.GetAlpha(color) * 0.1f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 2.55f / 255f, (255 - projectile.alpha) * 1.9f / 255f, 0);
			projectile.rotation += MathHelper.ToRadians(3);
			projectile.alpha += 4;
			if (projectile.timeLeft < 12)
			{
				projectile.alpha += 3;
			}
			if (projectile.ai[1] == 0)
			{
				projectile.ai[0]++;
				if(projectile.ai[0] >= 22)
				{
					projectile.ai[1] = 1;
				}
			}
			if (projectile.ai[1] == 1)
			{
				projectile.ai[0]--;
				if (projectile.ai[0] <= 16)
				{
					projectile.ai[1] = 0;
				}
			}
		}
	}
}