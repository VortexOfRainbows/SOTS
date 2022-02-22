using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Chaos
{
	public class ChaosDiamondLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Shattering Laser");
		}

		public override void SetDefaults() 
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.timeLeft = 90;
			projectile.penetrate = -1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		float counter = 0;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
		float scaleMult = 1f;
		public override void AI() 
		{
			if(runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 92, 1.1f, -0.3f);
			}
			scaleMult = 3 * projectile.timeLeft / 90f;
			Vector2 position;
			if(runOnce)
			{
				for (float i = 40; i <= maxDistance; i += 8)
				{
					if (Main.rand.NextBool(10))
					{
						position = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * i;
						int dust2 = Dust.NewDust(position - new Vector2(12, 12), 16, 16, ModContent.DustType<Dusts.CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.velocity *= 2f;
						dust.velocity += projectile.velocity * 0.2f;
						dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
						dust.noGravity = true;
						dust.alpha = 90;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.5f * projectile.scale;
					}
				}
			}
			runOnce = false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			if(projectile.timeLeft > 20)
			{
				Vector2 finalPoint = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * (maxDistance - 80);
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, finalPoint, 20f * scaleMult * projectile.scale, ref point))
				{
					return true;
				}
				finalPoint = projectile.Center - projectile.velocity.SafeNormalize(Vector2.Zero) * (maxDistance - 80);
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, finalPoint, 20f * scaleMult * projectile.scale, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 8f, ref point);
		}
		public const float maxDistance = 3200;
		public void Draw(SpriteBatch spriteBatch, int type)
		{
			if (runOnce)
				return;
			float alphaScale = 1f;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D textureBlack = ModContent.GetTexture("SOTS/Projectiles/Chaos/BlackLuxLaser");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 originBlack = new Vector2(textureBlack.Width / 2, textureBlack.Height / 2);
			float length = texture.Width * 1f;
			Vector2 unit = projectile.velocity.SafeNormalize(Vector2.Zero);
			float maxLength = maxDistance / length;
			for (float i = -maxLength; i <= maxLength; i++)
			{
				Vector2 position = projectile.Center + unit * length * i;
				float radians = MathHelper.ToRadians((Math.Abs(i) + VoidPlayer.soulColorCounter) * 2);
				Color color = VoidPlayer.pastelAttempt(radians);
				color.A = 0;
				float mult = 1;
				float sinusoid = 1.0f + (0.1f + 0.1f * (float)Math.Sin(MathHelper.ToRadians(Math.Abs(i) * 16 + VoidPlayer.soulColorCounter * 4f))) * projectile.scale;
				float scale = projectile.scale * scaleMult * sinusoid * (1 - 0.9f * (float)Math.Abs(i) / maxLength);
				Vector2 drawPos = position - Main.screenPosition;
				if (type == 1)
					spriteBatch.Draw(texture, drawPos, null, color * alphaScale * mult, projectile.velocity.ToRotation(), origin, new Vector2(2f, scale), SpriteEffects.None, 0f);
				else
					spriteBatch.Draw(textureBlack, drawPos, null, Color.White * alphaScale * mult, projectile.velocity.ToRotation(), originBlack, new Vector2(2f, scale), SpriteEffects.None, 0f);
			}
			return;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Draw(spriteBatch, 1);
			return false;
		}
        public void DrawBlack(SpriteBatch spriteBatch)
        {
			Draw(spriteBatch, 0);
        }
	}
}