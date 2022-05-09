using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs.MinionBuffs;

namespace SOTS.Projectiles.Minions
{
	public class PenguinCopter : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Penguin Copter");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 72;
			Projectile.height = 50;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
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
			Player player = Main.player[Projectile.owner];

			#region Active check
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<AerialAssistance>());
			}
			if (player.HasBuff(ModContent.BuffType<AerialAssistance>()))
			{
				Projectile.timeLeft = 2;
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
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Projectile.type == other.type)
				{
					count++;
				}
				if(i == Projectile.whoAmI)
				{
					break;
				}
			}
			float minionPositionOffsetX = (10 + count * 80) * -player.direction;
			idlePosition.X += minionPositionOffsetX; 

			// Teleport to player if distance is too big
			Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1800f)
			{
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.034f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width && other.type == Projectile.type)
				{
					if (Projectile.position.X < other.position.X) Projectile.velocity.X -= overlapVelocity;
					else Projectile.velocity.X += overlapVelocity;

					if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
					else Projectile.velocity.Y += overlapVelocity;
				}
			}
			#endregion

			#region Find target
			// Starting search distance
			float distanceFromTarget = 800f;
			Vector2 targetCenter = Projectile.Center;
			bool foundTarget = false;

			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
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
					if (npc.CanBeChasedBy() && npc.active)
					{
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
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
				Projectile.ai[0]++;
				Projectile.ai[1]++;
				// Minion has a target: attack (here, fly towards the enemy)
				if (distanceFromTarget > 375f)
				{
					Vector2 direction = targetCenter - Projectile.Center;
					// The immediate range around the target (so it doesn't latch onto it when close)
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
					direction2 = direction;
				}
				else if (distanceFromTarget < 100f)
				{
					speed = 24f;
					Vector2 direction = Projectile.Center - targetCenter;
					// If it is too close, move away
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
					direction2 = direction;
				}
				else
				{
					Vector2 direction = targetCenter - Projectile.Center;
					int firerate = 36;
					Projectile.velocity = (Projectile.velocity * (inertia - 1)) / inertia;
					if (Projectile.ai[0] >= firerate)
					{
						Projectile.ai[0] = 0;
						float shootspeed = Main.rand.Next(4,9);
						
						direction = direction.RotatedByRandom(MathHelper.ToRadians(36));
						direction.Normalize();
						Vector2 offset = direction * Projectile.width/2;
						direction *= shootspeed;
						Vector2 projVelo = direction;
						direction2 = direction;
						if (Projectile.owner == Main.myPlayer)
						{
							Projectile.NewProjectile(Projectile.Center + offset, projVelo, mod.ProjectileType("PenguinMissile"), Projectile.damage, Projectile.knockBack, Projectile.owner);
							Projectile.netUpdate = true;
						}
					}
				}
				direction2 = targetCenter - Projectile.Center;
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
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (Projectile.velocity == Vector2.Zero)
				{
					// If there is a case where it's not moving at all, give it a little "poke"
					Projectile.velocity.X = -0.15f;
					Projectile.velocity.Y = -0.05f;
				}
			}
			#endregion

			#region Animation and visuals
			// So it will lean slightly towards the direction it's moving
			if(!foundTarget)
			{
				float num1 = Projectile.velocity.X;
				if(Projectile.velocity.X > 20)
				{
					num1 = 20;
				}
				if (Projectile.velocity.X < -20)
				{
					num1 = 20;
				}
				Projectile.rotation = num1 * 0.05f;
				Projectile.spriteDirection = Projectile.velocity.X < 0 ? 1 : -1;
			}
			else
			{
				Projectile.spriteDirection = -1;
				if(targetCenter.X < Projectile.Center.X)
				{
					rotationDirection -= MathHelper.ToRadians(180);
					Projectile.spriteDirection = 1;
				}
				Projectile.rotation = rotationDirection;
			}

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 6;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.7f);
			#endregion
		}
	}
}