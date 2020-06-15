using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs;

namespace SOTS.Projectiles.Minions
{
	public class PenguinCopter : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Penguin Copter");
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 72;
			projectile.height = 50;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
		}
		Vector2 direction2;
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

			#region Active check
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<AerialAssistance>());
			}
			if (player.HasBuff(ModContent.BuffType<AerialAssistance>()))
			{
				projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 48f;
			int count = 0;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && projectile.type == other.type)
				{
					count++;
				}
				if(i == projectile.whoAmI)
				{
					break;
				}
			}
			float minionPositionOffsetX = (10 + count * 80) * -player.direction;
			idlePosition.X += minionPositionOffsetX; 

			// Teleport to player if distance is too big
			Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1800f)
			{
				projectile.position = idlePosition;
				projectile.velocity *= 0.1f;
				projectile.netUpdate = true;
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.034f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
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
			bool foundTarget = false;

			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, projectile.Center);
				bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
				bool closeThroughWall = between < 60f;

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
					if (npc.CanBeChasedBy())
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
							foundTarget = true;
						}
					}
				}
			}
			#endregion

			#region Movement

			// Default movement parameters (here for attacking)
			float speed = 16f;
			float inertia = 13f;
			float rotationDirection = 0;

			if (foundTarget)
			{
				projectile.ai[0]++;
				projectile.ai[1]++;
				// Minion has a target: attack (here, fly towards the enemy)
				if (distanceFromTarget > 375f)
				{
					Vector2 direction = targetCenter - projectile.Center;
					// The immediate range around the target (so it doesn't latch onto it when close)
					direction.Normalize();
					direction *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
					direction2 = direction;
				}
				else if (distanceFromTarget < 100f)
				{
					speed = 24f;
					Vector2 direction = projectile.Center - targetCenter;
					// If it is too close, move away
					direction.Normalize();
					direction *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
					direction2 = direction;
				}
				else
				{
					Vector2 direction = targetCenter - projectile.Center;
					int firerate = 36;
					projectile.velocity = (projectile.velocity * (inertia - 1)) / inertia;
					if (projectile.ai[0] >= firerate)
					{
						projectile.ai[0] = 0;
						float shootspeed = Main.rand.Next(4,9);
						
						direction = direction.RotatedByRandom(MathHelper.ToRadians(36));
						direction.Normalize();
						Vector2 offset = direction * projectile.width/2;
						direction *= shootspeed;
						Vector2 projVelo = direction;
						direction2 = direction;
						if (projectile.owner == Main.myPlayer)
						{
							Projectile.NewProjectile(projectile.Center + offset, projVelo, mod.ProjectileType("PenguinMissile"), projectile.damage, projectile.knockBack, projectile.owner);
							projectile.netUpdate = true;
						}
					}
				}
				direction2 = targetCenter - projectile.Center;
				rotationDirection = direction2.ToRotation();
			}
			else
			{
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
				projectile.rotation = num1 * 0.05f;
				projectile.spriteDirection = projectile.velocity.X < 0 ? 1 : -1;
			}
			else
			{
				projectile.spriteDirection = -1;
				if(targetCenter.X < projectile.Center.X)
				{
					rotationDirection -= MathHelper.ToRadians(180);
					projectile.spriteDirection = 1;
				}
				projectile.rotation = rotationDirection;
			}

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 6;
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}

			// Some visuals here
			Lighting.AddLight(projectile.Center, Color.White.ToVector3() * 0.7f);
			#endregion
		}
	}
}