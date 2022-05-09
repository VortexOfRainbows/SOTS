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
			Projectile.width = 30;
			Projectile.height = 26;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 510;
			Projectile.melee = true;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 0;
			Projectile.friendly = false;
			lockedVelo = true;
			if (Main.myPlayer == Projectile.owner)
			{
				Projectile.netUpdate = true;
			}
		}
		Vector2[] trailPos = new Vector2[16];
		public void TrailPreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Celestial/PurgatoryGhostTail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center + new Vector2(-12 * Projectile.spriteDirection, 0).RotatedBy(Projectile.rotation);
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = Color.White;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color);
				float max = betweenPositions.Length() / (4f * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation() - (Projectile.spriteDirection == -1 ? (float)Math.PI : 0), drawOrigin, scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
				previousPosition = currentPos;
			}
		}
		public void cataloguePos()
		{
			Vector2 current = Projectile.Center + new Vector2(-12 * Projectile.spriteDirection, 0).RotatedBy(Projectile.rotation);
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Celestial/PurgatoryGhost").Value;
			TrailPreDraw(spriteBatch, lightColor);
			float rotation = Projectile.rotation;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, color * (1f - (Projectile.alpha / 255f)), rotation, drawOrigin, 1f, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		bool runOnce = true;
		float counter = 0;
		float scaleVelocity = 1f;
		Vector2 lockVelo = Vector2.Zero;
		bool lockedVelo = false;
        public override void AI()
		{
			if (Projectile.ai[1] == 0)
			{
				for (int i = 0; i < 360; i += 30)
				{
					Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 107);
					dust.noGravity = true;
					dust.velocity *= 0.1f;
					dust.velocity += circularLocation * 0.3f;
					dust.scale *= 1.35f;
				}
				for (int i = 0; i < 360; i += 30)
				{
					Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += circularLocation * 0.15f;
					dust.scale *= 2.5f;
					dust.fadeIn = 0.1f;
					dust.color = new Color(33, 100, 33);
				}
				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 perturbedSpeed = (Projectile.velocity.SafeNormalize(Vector2.Zero) * 3.75f * Main.rand.NextFloat(0.5f, 1.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<PurgatoryLightning>(), Projectile.damage, 1f, Main.myPlayer, Main.rand.Next(2));
				}
				Projectile.ai[1] = Main.rand.Next(2) * 2 - 1;
			}
			Vector2 circular = new Vector2(0, 15 * scaleVelocity).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] * 4.5f * Projectile.ai[1]));
			scaleVelocity *= 0.99f;
			if (lockedVelo)
			{
				scaleVelocity = 1.2f;
				Projectile.velocity = lockVelo;
			}
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * initialSpeed;
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(circular.X));
			float minDist = 800;
			int target2 = -1;
			float distance;
			if (Projectile.active && Projectile.friendly && Projectile.damage > 0 && !Projectile.hostile && !lockedVelo)
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if (target.CanBeChasedBy())
					{
						distance = Vector2.Distance(target.Center, Projectile.Center);
						if (distance < minDist)
						{
							minDist = distance;
							target2 = i;
						}
					}
				}
				if (target2 != -1)
				{
					if (Main.myPlayer == Projectile.owner)
					{
						Projectile.netUpdate = true;
					}
					if (counter < 1f)
						counter += 0.005f;
					else
						counter = 1;
					NPC toHit = Main.npc[target2];
					if (toHit.active == true)
					{
						Vector2 toNpc = toHit.Center - Projectile.Center;
						toNpc = toNpc.SafeNormalize(Vector2.Zero) * initialSpeed;
						Projectile.velocity = Projectile.velocity * (1 - counter) + toNpc * counter;
						lockVelo = Projectile.velocity;
					}
				}
				else
                {
					lockVelo = Projectile.velocity;
                }
			}
		}
		float initialSpeed = 0;
        public override bool PreAI()
		{
			if(Projectile.velocity.Length() > 0.01f)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (Projectile.velocity.X < 0)
				{
					Projectile.spriteDirection = -1;
					Projectile.rotation -= MathHelper.ToRadians(180);
				}
				else
				{
					Projectile.spriteDirection = 1;
				}
			}
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				Projectile.velocity = Projectile.velocity * -2f;
				initialSpeed = Projectile.velocity.Length();
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
				}
			}
			if (!runOnce)
			{
				cataloguePos();
			}
			checkPos();
			Projectile.ai[0]++;
			return true;
		}
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if (iterator >= trailPos.Length)
				Projectile.Kill();
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(initialSpeed);
			writer.Write(Projectile.velocity.X);
			writer.Write(Projectile.velocity.Y);
			writer.Write(lockVelo.X);
			writer.Write(lockVelo.Y);
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(lockedVelo);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			initialSpeed = reader.ReadSingle();
			Projectile.velocity.X = reader.ReadSingle();
			Projectile.velocity.Y = reader.ReadSingle();
			lockVelo.X = reader.ReadSingle();
			lockVelo.Y = reader.ReadSingle();
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			lockedVelo = reader.ReadBoolean();
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
		