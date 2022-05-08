using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Projectiles.Nature
{
	public class BloomingHook : ModProjectile
	{
		private float aiCounter
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}
		private float aiCounter2
		{
			get => projectile.ai[1];
			set => projectile.ai[1] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blooming Hook");
			Main.projFrames[projectile.type] = 14;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 38;
			projectile.height = 38;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.minion = true;
			projectile.minionSlots = 0f;
			projectile.penetrate = -1;
			projectile.timeLeft = 300;
			projectile.netImportant = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Nature/BloomingVine");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Player owner = Main.player[projectile.owner];
			if (owner.active && !owner.dead)
			{
				Vector2 distanceToOwner = projectile.Center - owner.Center;
				float radius = distanceToOwner.Length() / 2;
				if (distanceToOwner.X < 0)
				{
					radius = -radius;
				}
				Vector2 centerOfCircle = owner.Center + distanceToOwner/2;
				float startingRadians = distanceToOwner.ToRotation();
				for(int i = 9; i > 0; i--)
				{
					Vector2 rotationPos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(18 * i));
					rotationPos.Y /= 4f;
					rotationPos = rotationPos.RotatedBy(startingRadians);
					Vector2 pos = rotationPos += centerOfCircle;
					Vector2 dynamicAddition = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(i * 36 + aiCounter * 2));
					Vector2 drawPos = pos - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, projectile.GetAlpha(drawColor), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, projectile.scale, SpriteEffects.None, 0f); 
				}
			}
			return true;
		}
		int frame = 0;
		Vector2 rotateVector = new Vector2(4, 0);
		public Vector2 FindTarget()
		{
			Player player = Main.player[projectile.owner];
			float distanceFromTarget = 640f;
			Vector2 targetCenter = projectile.Center;
			bool foundTarget = false;

			if (projectile.timeLeft > 100)
			{
				projectile.timeLeft = 300;
			}
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, projectile.Center);
				bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
				if (between < 800f && lineOfSight)
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

						if (((closest || !foundTarget) && inRange) && lineOfSight)
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			if(targetCenter != projectile.Center)
				return targetCenter;
			return new Vector2(-1, -1);
		}
		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			Vector2 target = FindTarget();
			aiCounter++;
			if (!owner.active || owner.dead)
			{
				projectile.Kill();
			}
			Vector2 distanceToOwner = owner.Center - projectile.Center;
			Vector2 distanceToOwner2 = owner.Center - projectile.Center;
			Vector2 distanceToTarget = target - projectile.Center;
			Vector2 distanceToTarget2 = target - projectile.Center;

			distanceToTarget.Normalize();
			rotateVector += distanceToTarget * 1;
			rotateVector = new Vector2(4, 0).RotatedBy(rotateVector.ToRotation());

			if (distanceToOwner2.Length() >= 64)
			{
				distanceToOwner.Normalize();
				projectile.velocity = distanceToOwner * (distanceToOwner2.Length() - 64);
			}
			else if (owner.Center.Y < projectile.Center.Y)
			{
				projectile.velocity.Y = -2f;
			}
			else
			{
				Vector2 dynamicAddition = new Vector2(0.15f, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 2));
				Vector2 added = new Vector2(0.9f, 0).RotatedBy(projectile.rotation);
				if (target.X == -1 && target.Y == -1)
					added = new Vector2(0, 0);
				projectile.velocity = added + dynamicAddition;
			}

			float overlapVelocity = 0.4f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width && other.type == projectile.type)
				{
					if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
					else projectile.velocity.X += overlapVelocity;

					if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
					else projectile.velocity.Y += overlapVelocity;
				}
			}

			projectile.rotation = rotateVector.ToRotation();
			aiCounter2++;
			if (aiCounter2 >= 75 && ((target.X != -1 && target.Y != -1) || frame != 0))
			{
				projectile.frameCounter++;
				if (projectile.frameCounter >= 5)
				{
					frame++;
					if (frame == 7)
					{
						SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 30, 0.7f, -0.4f);
						if (Main.myPlayer == projectile.owner)
						{
							Projectile.NewProjectile(projectile.Center, rotateVector * 1f, ModContent.ProjectileType<FriendlyFlowerBolt>(), projectile.damage, 1f, owner.whoAmI);
							projectile.netUpdate = true;
						}
					}
					if (frame >= 13)
					{
						aiCounter2 = 0;
						frame = 0;
					}
					projectile.frameCounter = 0;
				}
			}
			projectile.frame = frame;
		}
	}
}