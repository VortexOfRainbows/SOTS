using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;

namespace SOTS.Projectiles.Minions
{    
    public class CursedBlade : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Blade");
			//ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			//ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			//ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.tileCollide = false;
			projectile.friendly = true;
			//projectile.minion = true;
			//projectile.minionSlots = 0f;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.ignoreWater = true;
			projectile.localNPCHitCooldown = 10;
		}
		private const int attackTimerMax = 150;
        public override bool? CanHitNPC(NPC target)
        {
            return projectile.ai[0] >= attackTimerMax;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			canAttack = false;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.alpha);
			writer.Write(canAttack);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.alpha = reader.ReadInt32();
			canAttack = reader.ReadBoolean();
			base.ReceiveExtraAI(reader);
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public void Idle()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
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
			Vector2 toLocation;
			projectile.velocity *= 0.1f;
			toLocation.X = player.Center.X;
			toLocation.Y = player.Center.Y - 64 + Main.player[projectile.owner].gfxOffY;
			float rotation = modPlayer.orbitalCounter + projectile.ai[1] / total * 360f;
			Vector2 circular = new Vector2(48, 0).RotatedBy(MathHelper.ToRadians(rotation));
			circular.Y *= 0.2f;
			Vector2 goTo = toLocation + circular;
			goTo -= projectile.Center;
			Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
			float dist = 9f + goTo.Length() * 0.02f;
			if (dist > goTo.Length())
				dist = goTo.Length();
			projectile.velocity = newGoTo * dist;
			projectile.rotation = projectile.velocity.X * 0.04f + MathHelper.ToRadians(135);
		}
		bool foundTarget = false;
		int targetWhoAmI = -1;
		bool canAttack = true;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");

			#region Active check
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<Buffs.CursedBlade>());
			}
			if (player.HasBuff(ModContent.BuffType<Buffs.CursedBlade>()))
			{
				projectile.timeLeft = 6;
			}
			#endregion
			#region Find target
			float distanceFromTarget = 400f;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, projectile.Center);
				float between2 = Vector2.Distance(npc.Center, player.Center);
				if (between2 < distanceFromTarget + 240)
				{
					distanceFromTarget = between;
					foundTarget = true;
					targetWhoAmI = player.MinionAttackTargetNPC;
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
						float between2 = Vector2.Distance(npc.Center, player.Center);
						bool inRange = between < distanceFromTarget;
						if (inRange && between2 < distanceFromTarget * 2f)
						{
							distanceFromTarget = between;
							foundTarget = true;
							targetWhoAmI = i;
						}
					}
				}
			}
			#endregion

			#region Movement
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			float speed = 5f;
			if (foundTarget && canAttack && targetWhoAmI != -1 && Main.npc[targetWhoAmI].CanBeChasedBy())
			{
				NPC npc = Main.npc[targetWhoAmI];
				float Twidth = (float)Math.Sqrt(npc.height * npc.width);
				Vector2 goTo = npc.Center - projectile.Center;
				float sinM = (float)Math.Sin(MathHelper.ToRadians(projectile.ai[0] - 35)) * 32;
				if (projectile.ai[0] > attackTimerMax + 30)
                {
					sinM = -(72 + Twidth);
					speed *= 4;
				}
				float distance = goTo.Length() - (72 + Twidth + sinM);
				if (distance < 4 || projectile.ai[0] > 15)
				{
					projectile.ai[0] += 9;
				}
				goTo = goTo.SafeNormalize(Vector2.Zero);
				if (speed > distance)
				{
					speed = distance;
				}
				goTo *= speed;
				projectile.velocity = goTo;
				projectile.rotation = goTo.ToRotation() + MathHelper.Pi/4;
			}
			else
			{
				projectile.hide = false;
				targetWhoAmI = -1;
				foundTarget = false;
				if (projectile.ai[0] > 0)
                {
					projectile.ai[0]--;
                }
				else
                {
					canAttack = true;
				}
				Idle();
				if (Main.myPlayer == player.whoAmI && (idlePosition - projectile.Center).Length() > 1200f)
				{
					projectile.position = idlePosition;
					projectile.velocity *= 0.1f;
					projectile.netUpdate = true;
				}
			}
			#endregion
			Lighting.AddLight(projectile.Center, 2.4f * 0.5f * ((255 - projectile.alpha) / 255f), 2.2f * 0.5f * ((255 - projectile.alpha) / 255f), 1.4f * 0.5f * ((255 - projectile.alpha) / 255f));

			if (Main.myPlayer == player.whoAmI)
			{
				projectile.netUpdate = true;
			}
		}
	}
}
		