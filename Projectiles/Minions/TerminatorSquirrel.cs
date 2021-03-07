using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles.Minions
{
	public class TerminatorSquirrel : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terminator Squirrel");
			Main.projFrames[projectile.type] = 1;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		Vector2[] trailPos = new Vector2[12];
		bool runOnce = true;
		float glow = 14f;
		public override bool PreAI()
		{
			if (glow > 0)
				glow -= 0.5f;
			if (runOnce)
			{
				projectile.ai[1] = 80f;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			return base.PreAI();
		}
		public void cataloguePos()
		{
			Vector2 current = projectile.Center + new Vector2(4 * projectile.spriteDirection, 16).RotatedBy(projectile.rotation) + projectile.velocity;
			Vector2 velo = projectile.velocity * 0.2f;
			velo.Y += 1.75f;
			velo = velo.RotatedBy(projectile.rotation);
			velo.Y += 1;
			for (int i = 0; i < trailPos.Length; i++)
			{
				if(trailPos[i] != Vector2.Zero)
					trailPos[i] += velo * (trailPos.Length - i)/(float)trailPos.Length;
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return true;
			Texture2D texture2 = mod.GetTexture("Projectiles/Minions/TerminatorSquirrelTrail");
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 current = projectile.Center + new Vector2(4 * projectile.spriteDirection, 12).RotatedBy(projectile.rotation);
			Vector2 previousPosition = current;
			Color color = new Color(120, 90, 90, 0);
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 1f;
				if (trailPos[k] == Vector2.Zero)
				{
					return true;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color *= 0.95f;
				float max = betweenPositions.Length() / (4 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 4; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f * scale;
						float y = Main.rand.Next(-10, 11) * 0.1f * scale;
						if (j <= 1)
						{
							x = 0;
							y = 0;
						}
						Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin2, scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				previousPosition = currentPos;
			}
			if (!afterImage)
				return true;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.4f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Minions/TerminatorSquirrelArm");
			Texture2D texture2 = mod.GetTexture("Projectiles/Minions/TerminatorSquirrelArmGlow");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Main.spriteBatch.Draw(texture, drawPos + gunVelocity, null, drawColor, projectile.rotation, drawOrigin, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			Color color = new Color(100, 100, 100, 0);
			for (int j = 0; j < glow / 2; j++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				if (j <= 1)
				{
					x = 0;
					y = 0;
				}
				Main.spriteBatch.Draw(texture2, drawPos + gunVelocity + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
		}
		Vector2 direction2;
		Vector2 gunVelocity = Vector2.Zero;
		bool afterImage = false;
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return false;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float orbital = modPlayer.orbitalCounter;
			#region Active check
			gunVelocity *= 0.95f;
			afterImage = false;
			if(projectile.velocity.Length() > 16f)
            {
				afterImage = true;
            }
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<TerminatorSquirrelBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<TerminatorSquirrelBuff>()))
			{
				projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 26f;

			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner)
				{
					if (proj == projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			float minionPositionOffsetX = (18 + ofTotal * 42) * -player.direction;
			idlePosition.X += minionPositionOffsetX; 

			// Teleport to player if distance is too big
			Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1800f)
			{
				if(distanceToIdlePosition > 2400f)
					for (int i = 0; i < trailPos.Length; i++)
					{
						trailPos[i] = Vector2.Zero;
					}
				projectile.position = idlePosition;
				projectile.velocity *= 0.1f;
				projectile.netUpdate = true;
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.03f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width * 2 && other.type == projectile.type)
				{
					if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
					else projectile.velocity.X += overlapVelocity;

					if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
					else projectile.velocity.Y += overlapVelocity;
				}
			}
			#endregion

			#region Find target
			// Starting search distance
			float distanceFromTarget = 800f;
			Vector2 targetCenter = projectile.Center;
			float targetWidth = projectile.width;
			bool foundTarget = false;

			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, projectile.Center);
				bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
				bool closeThroughWall = between < 120f;

				if (between < 1200f && (lineOfSight || closeThroughWall))
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
			if (!foundTarget)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy() && npc.active)
					{
						float between = Vector2.Distance(npc.Center, projectile.Center);
						bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
						// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
						// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
						bool closeThroughWall = between < 60f;
						if (((closest || !foundTarget) && inRange) && (lineOfSight || closeThroughWall))
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							targetWidth = (float)Math.Sqrt(npc.width * npc.height);
							foundTarget = true;
						}
					}
				}
			}
			#endregion

			#region Movement

			projectile.ai[0]++;
			// Default movement parameters (here for attacking)
			float firerate = 66f;
			float speed = 22f;
			float inertia = 8f;
			float rotationDirection = 0;

			if (foundTarget)
			{
				projectile.ai[1]++;
				// Minion has a target: attack (here, fly towards the enemy)
				if (distanceFromTarget > 320f + targetWidth * 0.8f)
				{
					Vector2 direction = targetCenter - projectile.Center;
					// The immediate range around the target (so it doesn't latch onto it when close)
					direction.Normalize();
					direction *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
				}
				else if (distanceFromTarget < 60f + targetWidth * 0.8f)
				{
					speed = 12f;
					Vector2 direction = projectile.Center - targetCenter;
					// If it is too close, move away
					direction.Normalize();
					direction *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
				}
				else
				{
					Vector2 direction = targetCenter - projectile.Center;
					projectile.velocity = (projectile.velocity * (inertia - 1)) / inertia;
					if (projectile.ai[0] >= firerate)
					{
						projectile.ai[0] -= firerate;
						float shootspeed = 16.5f;
						for(int i = 0; i < 2 + Main.rand.Next(2) + Main.rand.Next(2) + Main.rand.Next(2); i++)
						{
							Vector2 newDirection = direction.RotatedByRandom(MathHelper.ToRadians(15 + i * 3)).SafeNormalize(Vector2.Zero);
							Vector2 offset = newDirection * projectile.width / 2;
							newDirection *= shootspeed;
							Vector2 projVelo = newDirection;
							projVelo.X += Main.rand.Next(-10, 11) * (0.1f + 0.02f * i);
							projVelo.Y += Main.rand.Next(-14, 4) * (0.1f + 0.03f * i);
							if (projectile.owner == Main.myPlayer)
							{
								Projectile.NewProjectile(projectile.Center + offset, projVelo, mod.ProjectileType("TerminatorAcorn"), (int)(projectile.damage * (1f - i * 0.125f)) + 1, projectile.knockBack, projectile.owner, i);
								projectile.netUpdate = true;
							}
						}
						gunVelocity = -4.75f * direction.SafeNormalize(Vector2.Zero);
						projectile.velocity += -direction.SafeNormalize(Vector2.Zero);
						glow = 16f;
					}
					float reposition = 0.13f;
					if (projectile.Center.Y > targetCenter.Y + targetWidth / 1.75f + 16)
						projectile.velocity.Y -= reposition;
					if (projectile.Center.Y < targetCenter.Y - targetWidth / 1.75f - 16)
						projectile.velocity.Y += reposition;
					if (projectile.Center.X < targetCenter.X && projectile.Center.X > targetCenter.X - targetWidth / 1.75f - 72)
						projectile.velocity.X -= reposition;
					if (projectile.Center.X > targetCenter.X && projectile.Center.X < targetCenter.X + targetWidth / 1.75f + 72)
						projectile.velocity.X += reposition;
				}
				direction2 = targetCenter - projectile.Center;
				rotationDirection = direction2.ToRotation();
			}
			else
			{
				if (projectile.ai[0] >= firerate)
				{
					projectile.ai[0] -= firerate;
				}
				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 100f)
				{
					// Speed up the minion if it's away from the player
					speed = 11f;
					inertia = 20f;
				}
				else
				{
					// Slow down the minion if closer to the player
					speed = 3f;
					inertia = 30f;
				}
				if (distanceToIdlePosition > 20f)
				{
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (projectile.velocity == Vector2.Zero)
				{
					// If there is a case where it's not moving at all, give it a little "poke"
					projectile.velocity.X = -0.15f;
					projectile.velocity.Y = -0.05f;
				}
			}
			#endregion

			#region Animation and visuals
			// So it will lean slightly towards the direction it's moving
			if(!foundTarget)
			{
				float num1 = projectile.velocity.X;
				if(projectile.velocity.X > 20)
				{
					num1 = 20;
				}
				if (projectile.velocity.X < -20)
				{
					num1 = 20;
				}
				projectile.rotation = num1 * 0.06f;
				projectile.spriteDirection = projectile.velocity.X < 0 ? -1 : 1;
			}
			else
			{
				projectile.spriteDirection = 1;
				if(targetCenter.X < projectile.Center.X)
				{
					rotationDirection -= MathHelper.ToRadians(180);
					projectile.spriteDirection = -1;
				}
				projectile.rotation = rotationDirection;
			}
			cataloguePos();
			#endregion
		}
	}
}