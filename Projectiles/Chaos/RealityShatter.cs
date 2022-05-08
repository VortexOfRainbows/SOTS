using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Chaos
{
	public class RealityShatter : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Shattering Laser");
		}
		public override void SetDefaults() 
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.timeLeft = 40;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.localNPCHitCooldown = 30;
			projectile.usesLocalNPCImmunity = true;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
		float scaleMult = 1f;
		int counter = 0;
		public override void AI() 
		{
			if(runOnce)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 92, 1.1f, 0.4f);
			}
			float bonus = projectile.ai[0];
			if (bonus > 8)
				bonus = 8;
			counter++;
			if(projectile.ai[1] >= 0 && projectile.ai[0] > 0)
			{
				NPC presumedTarget = Main.npc[(int)projectile.ai[1]];
				if (presumedTarget.CanBeChasedBy())
				{
					if (counter == (int)(12 + bonus))
					{
						if (Main.myPlayer == projectile.owner)
						{
							Projectile.NewProjectile(presumedTarget.Center, projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45))), ModContent.ProjectileType<RealityShatter>(), projectile.damage, projectile.knockBack, Main.myPlayer, projectile.ai[0] - 1, (int)projectile.ai[1]);
						}
					}
				}
				else
					projectile.ai[1] = -1;
			}
			scaleMult = (0.8f + 0.05f * bonus) * projectile.timeLeft / 40f;
			runOnce = false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			if(projectile.timeLeft > 20)
			{
				Vector2 finalPoint = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * (maxDistance);
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, finalPoint, 12f * scaleMult * projectile.scale, ref point))
				{
					return true;
				}
				finalPoint = projectile.Center - projectile.velocity.SafeNormalize(Vector2.Zero) * (maxDistance);
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, finalPoint, 12f * scaleMult * projectile.scale, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 8f, ref point);
		}
		public const float maxDistance = 150;
		public void Draw(SpriteBatch spriteBatch, int type)
		{
			if (runOnce)
				return;
			float alphaScale = 1f;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Texture2D textureBlack = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/BlackLuxLaser");
			Vector2 originBlack = new Vector2(textureBlack.Width / 2, textureBlack.Height / 2);
			float length = texture.Width * 0.5f;
			Vector2 unit = projectile.velocity.SafeNormalize(Vector2.Zero);
			float windUp = counter / 9f;
			float maxLength = maxDistance / length;
			float min = MathHelper.Lerp(-maxLength, maxLength, MathHelper.Clamp(windUp * 0.33f, 0, 1));
			float max = MathHelper.Lerp(-maxLength, maxLength, MathHelper.Clamp(windUp, 0, 1));
			int count = 0;
			for (float i = min; i <= max; i++)
			{
				count++;
				Vector2 position = projectile.Center + unit * length * i;
				float radians = MathHelper.ToRadians((Math.Abs(i) + VoidPlayer.soulColorCounter) * 3 + projectile.whoAmI * 60);
				Color color = VoidPlayer.pastelAttempt(radians);
				color.A = 0;
				float mult = 1;
				float sinusoid = 1.0f + (0.1f + 0.1f * (float)Math.Sin(MathHelper.ToRadians(Math.Abs(i) * 16 + VoidPlayer.soulColorCounter * 4f))) * projectile.scale;
				float scale = projectile.scale * scaleMult * sinusoid * (0.3f + 0.7f * (float)Math.Sin(MathHelper.Lerp(0, MathHelper.Pi, count / (max - min))));
				Vector2 drawPos = position - Main.screenPosition;
				if (type == 1 && i != min && i != max)
					spriteBatch.Draw(texture, drawPos, null, color * alphaScale * mult, projectile.velocity.ToRotation(), origin, new Vector2(1f, scale), SpriteEffects.None, 0f);
				else if(type == 0)
					spriteBatch.Draw(textureBlack, drawPos, null, Color.White * alphaScale * mult, projectile.velocity.ToRotation(), originBlack , new Vector2(1f, scale), SpriteEffects.None, 0f);
			}
			return;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Draw(spriteBatch, 1);
			DrawBlack(spriteBatch);
			return false;
		}
        public void DrawBlack(SpriteBatch spriteBatch)
        {
			Draw(spriteBatch, 0);
        }
	}
}