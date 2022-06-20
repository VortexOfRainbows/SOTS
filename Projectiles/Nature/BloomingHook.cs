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
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private float aiCounter2
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blooming Hook");
			Main.projFrames[Projectile.type] = 14;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minionSlots = 0f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Nature/BloomingVine");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Player owner = Main.player[Projectile.owner];
			if (owner.active && !owner.dead)
			{
				Vector2 distanceToOwner = Projectile.Center - owner.Center;
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
					Main.spriteBatch.Draw(texture, drawPos + dynamicAddition, null, Projectile.GetAlpha(lightColor), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, Projectile.scale, SpriteEffects.None, 0f); 
				}
			}
			return true;
		}
		int frame = 0;
		Vector2 rotateVector = new Vector2(4, 0);
		public Vector2 FindTarget()
		{
			Player player = Main.player[Projectile.owner];
			float distanceFromTarget = 640f;
			Vector2 targetCenter = Projectile.Center;
			bool foundTarget = false;

			if (Projectile.timeLeft > 100)
			{
				Projectile.timeLeft = 300;
			}
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
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
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

						if (((closest || !foundTarget) && inRange) && lineOfSight)
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			if(targetCenter != Projectile.Center)
				return targetCenter;
			return new Vector2(-1, -1);
		}
		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Vector2 target = FindTarget();
			aiCounter++;
			if (!owner.active || owner.dead)
			{
				Projectile.Kill();
			}
			Vector2 distanceToOwner = owner.Center - Projectile.Center;
			Vector2 distanceToOwner2 = owner.Center - Projectile.Center;
			Vector2 distanceToTarget = target - Projectile.Center;
			Vector2 distanceToTarget2 = target - Projectile.Center;

			distanceToTarget.Normalize();
			rotateVector += distanceToTarget * 1;
			rotateVector = new Vector2(4, 0).RotatedBy(rotateVector.ToRotation());

			if (distanceToOwner2.Length() >= 64)
			{
				distanceToOwner.Normalize();
				Projectile.velocity = distanceToOwner * (distanceToOwner2.Length() - 64);
			}
			else if (owner.Center.Y < Projectile.Center.Y)
			{
				Projectile.velocity.Y = -2f;
			}
			else
			{
				Vector2 dynamicAddition = new Vector2(0.15f, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 2));
				Vector2 added = new Vector2(0.9f, 0).RotatedBy(Projectile.rotation);
				if (target.X == -1 && target.Y == -1)
					added = new Vector2(0, 0);
				Projectile.velocity = added + dynamicAddition;
			}

			float overlapVelocity = 0.4f;
			for (int i = 0; i < Main.maxNPCs; i++)
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

			Projectile.rotation = rotateVector.ToRotation();
			aiCounter2++;
			if (aiCounter2 >= 75 && ((target.X != -1 && target.Y != -1) || frame != 0))
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter >= 5)
				{
					frame++;
					if (frame == 7)
					{
						SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f, -0.4f);
						if (Main.myPlayer == Projectile.owner)
						{
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, rotateVector * 1f, ModContent.ProjectileType<FriendlyFlowerBolt>(), Projectile.damage, 1f, owner.whoAmI);
							Projectile.netUpdate = true;
						}
					}
					if (frame >= 13)
					{
						aiCounter2 = 0;
						frame = 0;
					}
					Projectile.frameCounter = 0;
				}
			}
			Projectile.frame = frame;
		}
	}
}