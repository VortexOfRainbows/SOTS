using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Anomaly
{
	public class UltraLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
		}
		public override void SetDefaults() 
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 90;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
		}
		Vector2 finalPosition = Vector2.Zero;
		bool hasInit = false;
		float scaleMult = 1f;
		public override void AI() 
		{
			if(!hasInit)
			{
				SOTSUtils.PlaySound(SoundID.Item92, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, -0.3f);
			}
			InitializeLaser();
		}
		public void InitializeLaser()
		{
			Color color = ColorHelpers.VoidAnomaly;
			color.A = 0;
			Vector2 startingPosition = Projectile.Center;
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			for (int b = 0; b < 640; b++)
			{
				startingPosition += Projectile.velocity * 2.5f;
				finalPosition = startingPosition;
				int i = (int)startingPosition.X / 16;
				int j = (int)startingPosition.Y / 16;
				if (WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(i, j))
				{
					break;
				}
				bool extra = !hasInit;
				int chance = SOTS.Config.lowFidelityMode ? 36 : 12;
				if (Main.rand.NextBool(chance) || extra)
				{
					Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 0.75f);
					dust.noGravity = true;
					if (!extra)
					{
						dust.velocity = dust.velocity * 0.25f;
						dust.fadeIn = 17;
					}
					else
					{
						dust.velocity *= 1.25f;
						dust.velocity += Projectile.velocity * Main.rand.NextFloat(5f, 8f);
						dust.fadeIn = 12;
						dust.scale *= 1.25f;
					}
					dust.velocity.X += Main.rand.NextFloat(-3, 3f);
				}
			}
			for (int i = 3; i > 0; i--)
			{
				Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(16, 16), 26, 26, ModContent.DustType<CopyDust4>(), 0, 0, 0, color, 1.5f);
				dust.noGravity = true;
				dust.velocity = dust.velocity * 0.2f + Projectile.velocity * Main.rand.NextFloat(0.1f, 1.0f);
				if (i == 2)
					dust.velocity += new Vector2(0, -Main.rand.NextFloat(0.1f, 0.3f));
				dust.fadeIn = 0.2f;
			}
			hasInit = true;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			if(Projectile.timeLeft > 20)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, finalPosition, 20f * scaleMult * Projectile.scale, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			if (!hasInit)
				return;
			float alphaScale = 1f;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 toEnd = finalPosition - Projectile.Center;
			float maxLength = toEnd.Length() / texture.Height * 4;
			Color color;
			float width;
			for (int j = 0; j <= 2; j++)
			{
				float percent = 0f;
				if (j == 0)
				{
					color = Color.Black;
					width = 1f;
				}
				else
                {
					color = new Color(100, 100, 100, 0);
					width = 1.0f + j * 0.25f;
                }
				for (float i = 0; i < maxLength; i++)
				{
					if(percent < 1)
						percent += 0.1f;
					if (!SOTS.Config.lowFidelityMode || (int)(i % 2) == 0)
					{
						Vector2 position = Vector2.Lerp(Projectile.Center, finalPosition, i / maxLength);
						//float radians = MathHelper.ToRadians((Math.Abs(i) + ColorHelpers.soulColorCounter) * 2);
						//Color color = ColorHelpers.pastelAttempt(radians);
						//color.A = 0;
						//float mult = 1;
						//float sinusoid = 1.0f + (0.1f + 0.1f * (float)Math.Sin(MathHelper.ToRadians(Math.Abs(i) * 16 + ColorHelpers.soulColorCounter * 4f))) * Projectile.scale;
						//float scale = Projectile.scale * scaleMult * sinusoid * (1 - 0.9f * (float)Math.Abs(i) / maxLength);
						Vector2 drawPos = position - Main.screenPosition;
						spriteBatch.Draw(texture, drawPos, null, color * alphaScale, Projectile.velocity.ToRotation() + MathHelper.PiOver2, origin, new Vector2(width * (float)Math.Sqrt(percent), 0.25f), SpriteEffects.None, 0f);
					}
				}
			}
			return;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Draw(Main.spriteBatch);
			return false;
		}
	}
}