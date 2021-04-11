using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace SOTS.Projectiles.Minions
{
	public class NatureSpirit : SpiritMinion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Spirit");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;   
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.ignoreWater = true;
			projectile.localNPCHitCooldown = 10;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public override void AI() 
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(mod.BuffType("NatureSpiritAid"));
			}
			if (player.HasBuff(mod.BuffType("NatureSpiritAid")))
			{
				projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
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
			if (Main.myPlayer == player.whoAmI)
				projectile.ai[1] = ofTotal;
			#endregion

			#region Find target
			float distanceFromTarget = 1000f;
			Vector2 targetCenter = projectile.Center;
			bool foundTarget = false;
			float npcWidthHeight = 0;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, player.Center);
				if (between < distanceFromTarget) 
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
					npcWidthHeight = (float)Math.Sqrt(npc.width * npc.height);
				}
			}
			if (!foundTarget) 
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy()) 
					{
						float between = Vector2.Distance(npc.Center, player.Center);
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height);
						
						bool closeThroughWall = between < 100f; //should attack semi-reliably through walls
						if (inRange && (lineOfSight || closeThroughWall) && between < distanceFromTarget)
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
							npcWidthHeight = (float)Math.Sqrt(npc.width * npc.height);
						}
					}
				}
			}
			#endregion

			#region Movement
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			float speed = 13.5f;
			if(projectile.ai[0] > 0)
            {
				projectile.ai[0] *= 0.975f;
				projectile.ai[0] -= 0.1f;
            }
			else
            {
				projectile.ai[0] = 0;
            }
			if (foundTarget)
			{
				Vector2 toPos = targetCenter + new Vector2(72 + npcWidthHeight + projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (360f / total * projectile.ai[1])));
				Vector2 direction = toPos - projectile.Center;
				float distance = direction.Length();
				bool inRange = distance < 96 + npcWidthHeight;
				direction = direction.SafeNormalize(Vector2.Zero);
				if (distance > speed)
				{
					distance = speed;
				}
				direction *= distance;
				projectile.velocity = direction;
				Vector2 toNPC = targetCenter - projectile.Center;
				int fireRate = 72;
				if((int)(modPlayer.orbitalCounter + (float)fireRate / total * projectile.ai[1]) % fireRate == 0 && inRange)
				{
					Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 43, 0.4f);
					if (Main.myPlayer == projectile.owner)
					{
						Projectile.NewProjectile(projectile.Center, toNPC.SafeNormalize(Vector2.Zero) * 3, mod.ProjectileType("NatureBeam"), projectile.damage, projectile.knockBack, projectile.owner);
					}
					projectile.ai[0] = 32;
				}
			}
			else
			{
				GoIdle();
				Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
				float distanceToIdlePosition = vectorToIdlePosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
				{
					projectile.position = idlePosition;
					projectile.velocity *= 0.1f;
					projectile.netUpdate = true;
				}
			}
			#endregion

			#region Animation and visuals
			Lighting.AddLight(projectile.Center, 2.0f * 0.5f * ((255 - projectile.alpha) / 255f), 2.4f * 0.5f * ((255 - projectile.alpha) / 255f), 1.8f * 0.5f * ((255 - projectile.alpha) / 255f));
			#endregion

			if (Main.myPlayer == player.whoAmI)
			{
				projectile.netUpdate = true;
			}
		}
	}
}