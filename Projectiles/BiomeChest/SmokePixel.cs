using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{
	public class SmokePixel : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Pathogen Cloud");
		}
		public override void SetDefaults()
		{
			Projectile.width = 96;
			Projectile.height = 96;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 130;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ai[0] = 32;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 240; k++)
			{
				Vector2 lengthMod = new Vector2(1.0f, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * 10));
				Vector2 circularModifier = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(k * 1.5f) + Projectile.rotation);
				Vector2 circularLength = new Vector2(1.5f + lengthMod.X, 0).RotatedBy(MathHelper.ToRadians(k * 27));
				Vector2 circularPos = new Vector2(Projectile.ai[0] * 2, 0).RotatedBy(MathHelper.ToRadians(k * 1.5f) + Projectile.rotation);
				Vector2 bonus = new Vector2(circularLength.X, 0).RotatedBy(MathHelper.ToRadians(k * 1.5f) + Projectile.rotation);
				Color color = new Color(100, 100, 100, 0);
				float mod = Math.Abs(circularModifier.X);
				if (mod < 0.25f) mod = 0.25f;
				circularPos.Y *= 0.5f + mod;
				circularPos += bonus;
				Vector2 drawPos = Projectile.Center + circularPos - Main.screenPosition;
				color = Projectile.GetAlpha(color);
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
				float circularRot = circularPos.ToRotation();
				float dist = circularPos.Length();
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, (int)dist, texture.Height), Projectile.GetAlpha(Color.White), circularRot, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			
			return false;
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 1f, 0.5f, 1f);
			Projectile.rotation += MathHelper.ToRadians(1);
			Projectile.alpha += 2;
			Projectile.ai[0] = 32;
			Projectile.ai[1]++;
		}
	}
}