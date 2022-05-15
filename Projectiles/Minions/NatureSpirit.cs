using System;
using Microsoft.Xna.Framework;
using SOTS.Buffs.MinionBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{
	public class NatureSpirit : SpiritMinion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Spirit");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;   
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 10;
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
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<NatureSpiritAid>());
			}
			if (player.HasBuff(ModContent.BuffType<NatureSpiritAid>()))
			{
				Projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
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
			if (Main.myPlayer == player.whoAmI)
				Projectile.ai[1] = ofTotal;
			#endregion

			#region Find target
			float distanceFromTarget = 1000f;
			Vector2 targetCenter = Projectile.Center;
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
			if(Projectile.ai[0] > 0)
            {
				Projectile.ai[0] *= 0.975f;
				Projectile.ai[0] -= 0.1f;
            }
			else
            {
				Projectile.ai[0] = 0;
            }
			if (foundTarget)
			{
				Vector2 toPos = targetCenter + new Vector2(72 + npcWidthHeight + Projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (360f / total * Projectile.ai[1])));
				Vector2 direction = toPos - Projectile.Center;
				float distance = direction.Length();
				bool inRange = distance < 96 + npcWidthHeight;
				direction = direction.SafeNormalize(Vector2.Zero);
				if (distance > speed)
				{
					distance = speed;
				}
				direction *= distance;
				Projectile.velocity = direction;
				Vector2 toNPC = targetCenter - Projectile.Center;
				int fireRate = 72;
				if((int)(modPlayer.orbitalCounter + (float)fireRate / total * Projectile.ai[1]) % fireRate == 0 && inRange)
				{
					SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 43, 0.4f);
					if (Main.myPlayer == Projectile.owner)
					{
						Projectile.NewProjectile(Projectile.Center, toNPC.SafeNormalize(Vector2.Zero) * 3, Mod.Find<ModProjectile>("NatureBeam").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
					Projectile.ai[0] = 32;
				}
			}
			else
			{
				GoIdle();
				Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
				float distanceToIdlePosition = vectorToIdlePosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
				{
					Projectile.position = idlePosition;
					Projectile.velocity *= 0.1f;
					Projectile.netUpdate = true;
				}
			}
			#endregion

			Lighting.AddLight(Projectile.Center, 2.0f * 0.5f * ((255 - Projectile.alpha) / 255f), 2.4f * 0.5f * ((255 - Projectile.alpha) / 255f), 1.8f * 0.5f * ((255 - Projectile.alpha) / 255f));
			MoveAwayFromOthers();

			if (Main.myPlayer == player.whoAmI)
			{
				Projectile.netUpdate = true;
			}
		}
	}
}