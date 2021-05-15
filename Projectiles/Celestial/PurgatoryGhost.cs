using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{
	public class PurgatoryGhost : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purgatory Ghost");
		}
		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 26;
			projectile.penetrate = -1;
			projectile.timeLeft = 510;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
			projectile.friendly = false;
			lockedVelo = true;
		}
		Vector2[] trailPos = new Vector2[20];
		public void TrailPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Celestial/PurgatoryGhostTail");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center + new Vector2(-12 * projectile.spriteDirection, 0).RotatedBy(projectile.rotation);
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = Color.White;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = projectile.GetAlpha(color);
				float max = betweenPositions.Length() / (4f * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation() - (projectile.spriteDirection == -1 ? (float)Math.PI : 0), drawOrigin, scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
				previousPosition = currentPos;
			}
		}
		public void cataloguePos()
		{
			Vector2 current = projectile.Center + new Vector2(-12 * projectile.spriteDirection, 0).RotatedBy(projectile.rotation);
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2 = mod.GetTexture("Projectiles/Celestial/PurgatoryGhost");
			TrailPreDraw(spriteBatch, lightColor);
			float rotation = projectile.rotation;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, color * (1f - (projectile.alpha / 255f)), rotation, drawOrigin, 1f, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		bool runOnce = true;
		float counter = 0;
		float scaleVelocity = 1f;
		Vector2 lockVelo = Vector2.Zero;
		bool lockedVelo = false;
        public override void AI()
		{
			if(projectile.timeLeft % 2 == 0 && Main.myPlayer == projectile.owner)
            {
				projectile.netUpdate = true;
			}
			if (projectile.ai[1] == 0)
			{
				for (int i = 0; i < 360; i += 30)
				{
					Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 107);
					dust.noGravity = true;
					dust.velocity *= 0.1f;
					dust.velocity += circularLocation * 0.3f;
					dust.scale *= 1.35f;
				}
				for (int i = 0; i < 360; i += 30)
				{
					Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += circularLocation * 0.15f;
					dust.scale *= 2.5f;
					dust.fadeIn = 0.1f;
					dust.color = new Color(33, 100, 33);
				}
				if (Main.myPlayer == projectile.owner)
				{
					Vector2 perturbedSpeed = (projectile.velocity.SafeNormalize(Vector2.Zero) * 3.75f * Main.rand.NextFloat(0.5f, 1.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<PurgatoryLightning>(), projectile.damage, 1f, Main.myPlayer, Main.rand.Next(2));
				}
				projectile.ai[1] = Main.rand.Next(2) * 2 - 1;
			}
			Vector2 circular = new Vector2(0, 15 * scaleVelocity).RotatedBy(MathHelper.ToRadians(projectile.ai[0] * 4.5f * projectile.ai[1]));
			scaleVelocity *= 0.99f;
			if (lockedVelo)
			{
				scaleVelocity = 1.2f;
				projectile.velocity = lockVelo;
			}
			projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * initialSpeed;
			projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(circular.X));
			float minDist = 800;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			if (projectile.friendly == true && projectile.damage > 0 && projectile.hostile == false && !lockedVelo)
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.CanBeChasedBy())
					{
						dX = target.Center.X - projectile.Center.X;
						dY = target.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						if (distance < minDist)
						{
							bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
							if (lineOfSight)
							{
								minDist = distance;
								target2 = i;
							}
						}
					}
				}
				if (target2 != -1)
				{
					if (counter < 1f)
						counter += 0.005f;
					else
						counter = 1;
					NPC toHit = Main.npc[target2];
					if (toHit.active == true)
					{
						Vector2 toNpc = toHit.Center - projectile.Center;
						toNpc = toNpc.SafeNormalize(Vector2.Zero) * initialSpeed;
						projectile.velocity = projectile.velocity * (1 - counter) + toNpc * counter;
						lockVelo = projectile.velocity;
					}
				}
				else
                {
					lockVelo = projectile.velocity;
                }
			}
		}
		float initialSpeed = 0;
        public override bool PreAI()
		{
			if(projectile.velocity.Length() > 0.01f)
			{
				projectile.rotation = projectile.velocity.ToRotation();
				if (projectile.velocity.X < 0)
				{
					projectile.spriteDirection = -1;
					projectile.rotation -= MathHelper.ToRadians(180);
				}
				else
				{
					projectile.spriteDirection = 1;
				}
			}
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				projectile.velocity = projectile.velocity * -2f;
				initialSpeed = projectile.velocity.Length();
			}
			if (!runOnce)
			{
				cataloguePos();
			}
			checkPos();
			projectile.ai[0]++;
			return true;
		}
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if (iterator >= trailPos.Length)
				projectile.Kill();
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.velocity.X);
			writer.Write(projectile.velocity.Y);
			writer.Write(lockVelo.X);
			writer.Write(lockVelo.Y);
			writer.Write(projectile.tileCollide);
			writer.Write(projectile.friendly);
			writer.Write(lockedVelo);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.velocity.X = reader.ReadSingle();
			projectile.velocity.Y = reader.ReadSingle();
			lockVelo.X = reader.ReadSingle();
			lockVelo.Y = reader.ReadSingle();
			projectile.tileCollide = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
			lockedVelo = reader.ReadBoolean();
			base.ReceiveExtraAI(reader);
		}
		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < trailPos.Length; k++)
			{
				for (int i = 0; i < (int)(1 + 0.25f * (20 - k)); i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(trailPos[k].X  - 4, trailPos[k].Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 2.5f - k * 0.1f;
					dust.scale *= 1.75f;
					dust.fadeIn = 0.1f;
					dust.color = new Color(160, 255, 140, 0);
				}
			}
		}
    }
}
		