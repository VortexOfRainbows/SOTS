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
			Projectile.width = 96;
			Projectile.height = 96;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 120; k++)
			{
				Vector2 circularLength = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 18));
				Vector2 circularPos = new Vector2(Projectile.ai[0] * 2 + circularLength.X, 0).RotatedBy(MathHelper.ToRadians(k * 3) + Projectile.rotation);
				Color color = Color.Black;
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
			Lighting.AddLight(Projectile.Center, 1f, 0.5f, 1f);
			Projectile.rotation += MathHelper.ToRadians(2);
			Projectile.alpha += 4;
			if (Projectile.timeLeft < 12)
			{
				Projectile.ai[0] -= 2;
				if(Projectile.ai[0] <= 0)
				{
					Projectile.Kill();
				}
			}
			else if (Projectile.ai[1] == 0)
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
			if(Projectile.timeLeft % 15 == 0)
			{
				Projectile.friendly = true;
			}
			else
			{
				Projectile.friendly = false;
			}
		}
	}
}