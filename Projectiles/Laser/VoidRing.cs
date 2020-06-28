using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Laser
{
	public class VoidRing : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Ring");
		}
		public override void SetDefaults()
		{
			projectile.width = 96;
			projectile.height = 96;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 120; k++)
			{
				Vector2 circularLength = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 18));
				Vector2 circularPos = new Vector2(projectile.ai[0] * 2 + circularLength.X, 0).RotatedBy(MathHelper.ToRadians(k * 3) + projectile.rotation);
				Color color = Color.Black;
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
			Lighting.AddLight(projectile.Center, 1f, 0.5f, 1f);
			projectile.rotation += MathHelper.ToRadians(2);
			projectile.alpha += 4;
			if (projectile.timeLeft < 12)
			{
				projectile.ai[0] -= 2;
				if(projectile.ai[0] <= 0)
				{
					projectile.Kill();
				}
			}
			else if (projectile.ai[1] == 0)
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
			if(projectile.timeLeft % 15 == 0)
			{
				projectile.friendly = true;
			}
			else
			{
				projectile.friendly = false;
			}
		}
	}
}