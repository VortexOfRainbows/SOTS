using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.MinionBuffs;
using Terraria.Audio;

namespace SOTS.Projectiles.Minions
{
	public class TerminatorSquirrel : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Terminator Squirrel");
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		Vector2[] trailPos = new Vector2[12];
		bool runOnce = true;
		float glow = 14f;
		public override bool PreAI()
		{
			//Projectile.SetDamageBasedOnOriginalDamage(Projectile.owner);
			if (glow > 0)
				glow -= 0.5f;
			if (runOnce)
			{
				Projectile.ai[1] = 80f;
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
			Vector2 current = Projectile.Center + new Vector2(4 * Projectile.spriteDirection, 16).RotatedBy(Projectile.rotation) + Projectile.velocity;
			Vector2 velo = Projectile.velocity * 0.2f;
			velo.Y += 1.75f;
			velo = velo.RotatedBy(Projectile.rotation);
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
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return true;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Minions/TerminatorSquirrelTrail").Value;
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 current = Projectile.Center + new Vector2(4 * Projectile.spriteDirection, 12).RotatedBy(Projectile.rotation);
			Vector2 previousPosition = current;
			Color color = new Color(120, 90, 90, 0);
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 1f;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				if (trailPos[k] == Vector2.Zero || betweenPositions.Length() > 3000)
				{
					return true;
				}
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
						Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin2, scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				previousPosition = currentPos;
			}
			if (!afterImage)
				return true;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.4f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.ignoreWater = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
		}
        public override void PostDraw(Color lightColor)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Minions/TerminatorSquirrelArm").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Minions/TerminatorSquirrelArmGlow").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Main.spriteBatch.Draw(texture, drawPos + gunVelocity, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
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
				Main.spriteBatch.Draw(texture2, drawPos + gunVelocity + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
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
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float orbital = modPlayer.orbitalCounter;
			#region Active check
			gunVelocity *= 0.95f;
			afterImage = false;
			if(Projectile.velocity.Length() > 16f)
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
				Projectile.timeLeft = 2;
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
				if (Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner)
				{
					if (proj == Projectile)
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
			Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1800f)
			{
				if(distanceToIdlePosition > 2400f)
					for (int i = 0; i < trailPos.Length; i++)
					{
						trailPos[i] = Vector2.Zero;
					}
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.03f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width * 2 && other.type == Projectile.type)
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
			float targetWidth = Projectile.width;
			bool foundTarget = false;

			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
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
							targetWidth = (float)Math.Sqrt(npc.width * npc.height);
							foundTarget = true;
						}
					}
				}
			}
			#endregion

			#region Movement

			Projectile.ai[0]++;
			// Default movement parameters (here for attacking)
			float firerate = 66f;
			float speed = 22f;
			float inertia = 8f;
			float rotationDirection = 0;

			if (foundTarget)
			{
				Projectile.ai[1]++;
				// Minion has a target: attack (here, fly towards the enemy)
				if (distanceFromTarget > 320f + targetWidth * 0.8f)
				{
					Vector2 direction = targetCenter - Projectile.Center;
					// The immediate range around the target (so it doesn't latch onto it when close)
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
				}
				else if (distanceFromTarget < 60f + targetWidth * 0.8f)
				{
					speed = 12f;
					Vector2 direction = Projectile.Center - targetCenter;
					// If it is too close, move away
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
				}
				else
				{
					Vector2 direction = targetCenter - Projectile.Center;
					Projectile.velocity = (Projectile.velocity * (inertia - 1)) / inertia;
					if (Projectile.ai[0] >= firerate)
					{
						Projectile.ai[0] -= firerate;
						float shootspeed = 16.5f;
						for(int i = 0; i < 2 + Main.rand.Next(2) + Main.rand.Next(2) + Main.rand.Next(2); i++)
						{
							Vector2 newDirection = direction.RotatedByRandom(MathHelper.ToRadians(15 + i * 3)).SafeNormalize(Vector2.Zero);
							Vector2 offset = newDirection * Projectile.width / 2;
							newDirection *= shootspeed;
							Vector2 projVelo = newDirection;
							projVelo.X += Main.rand.Next(-10, 11) * (0.1f + 0.02f * i);
							projVelo.Y += Main.rand.Next(-14, 4) * (0.1f + 0.03f * i);
							if (Projectile.owner == Main.myPlayer)
							{
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + offset, projVelo, ModContent.ProjectileType<TerminatorAcorn>(), (int)(Projectile.damage * (1f - i * 0.125f)) + 1, Projectile.knockBack, Projectile.owner, i);
								Projectile.netUpdate = true;
							}
						}
						gunVelocity = -4.75f * direction.SafeNormalize(Vector2.Zero);
						Projectile.velocity += -direction.SafeNormalize(Vector2.Zero);
						glow = 16f;
					}
					float reposition = 0.13f;
					if (Projectile.Center.Y > targetCenter.Y + targetWidth / 1.75f + 16)
						Projectile.velocity.Y -= reposition;
					if (Projectile.Center.Y < targetCenter.Y - targetWidth / 1.75f - 16)
						Projectile.velocity.Y += reposition;
					if (Projectile.Center.X < targetCenter.X && Projectile.Center.X > targetCenter.X - targetWidth / 1.75f - 72)
						Projectile.velocity.X -= reposition;
					if (Projectile.Center.X > targetCenter.X && Projectile.Center.X < targetCenter.X + targetWidth / 1.75f + 72)
						Projectile.velocity.X += reposition;
				}
				direction2 = targetCenter - Projectile.Center;
				rotationDirection = direction2.ToRotation();
			}
			else
			{
				if (Projectile.ai[0] >= firerate)
				{
					Projectile.ai[0] -= firerate;
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
				Projectile.rotation = num1 * 0.06f;
				Projectile.spriteDirection = Projectile.velocity.X < 0 ? -1 : 1;
			}
			else
			{
				Projectile.spriteDirection = 1;
				if(targetCenter.X < Projectile.Center.X)
				{
					rotationDirection -= MathHelper.ToRadians(180);
					Projectile.spriteDirection = -1;
				}
				Projectile.rotation = rotationDirection;
			}
			cataloguePos();
			#endregion
		}
	}
}