using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using SOTS.Projectiles.Pyramid;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;

namespace SOTS.Projectiles.Minions
{    
    public class CursedBlade : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Blade");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.tileCollide = false;
			projectile.friendly = true;
			//projectile.minion = true;
			//projectile.minionSlots = 0f;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.ignoreWater = true;
			projectile.localNPCHitCooldown = 10;
		}
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Minions/CursedBladeHilt");
			Texture2D texture2 = ModContent.GetTexture("SOTS/Projectiles/Minions/CursedBladePart");
			Texture2D texture3 = ModContent.GetTexture("SOTS/Projectiles/Minions/CursedBladeEnd");
			Vector2 drawPos = projectile.Center- Main.screenPosition;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			int length = (int)(11f * (1 - projectile.ai[0] / attackTimerMax));
			float rotation = projectile.rotation;
			for (int i = 0; i < length + 1; i++)
			{
				Color color1 = lightColor;
				if(i != length)
				{
					Vector2 toProj2 = new Vector2(6 + i * 2, 0).RotatedBy(rotation);
					spriteBatch.Draw(texture2, projectile.Center + toProj2 - Main.screenPosition, null, color1, rotation + MathHelper.Pi / 4, new Vector2(4, 4), projectile.scale, SpriteEffects.None, 0f);
				}
				else
				{
					Vector2 toProj2 = new Vector2(7 + i * 2, 0).RotatedBy(rotation);
					spriteBatch.Draw(texture3, projectile.Center + toProj2 - Main.screenPosition, null, color1, rotation + MathHelper.Pi / 4, new Vector2(6, 6), projectile.scale, SpriteEffects.None, 0f);
				}
			}
			spriteBatch.Draw(texture, drawPos, null, lightColor, rotation + MathHelper.Pi / 4, origin, projectile.scale * 1.0f, SpriteEffects.None, 0f);
			return false;
		}
		private const int attackTimerMax = 180;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return projectile.ai[0] >= attackTimerMax + 10 || projHitbox.Intersects(targetHitbox);
        }
        public override bool? CanHitNPC(NPC target)
        {
            return projectile.ai[0] >= attackTimerMax && target.whoAmI == targetWhoAmI;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			canAttack = false;
			projectile.netUpdate = true;
			Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<CursedStab>(), projectile.damage, 0, Main.myPlayer, 0, target.whoAmI);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.alpha);
			writer.Write(canAttack);
			writer.Write(foundTarget);
			writer.Write(targetWhoAmI);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.alpha = reader.ReadInt32();
			canAttack = reader.ReadBoolean();
			foundTarget = reader.ReadBoolean();
			targetWhoAmI = reader.ReadInt32();
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
			projectile.rotation = projectile.velocity.X * 0.04f + MathHelper.Pi/2;
		}
		bool foundTarget = false;
		int targetWhoAmI = -1;
		bool canAttack = true;
		bool canDoDashSounds = true;
		int counter = 0;
		public void DoDusts()
		{
			float dustMult = (projectile.ai[0] / (attackTimerMax - 30));
			if (dustMult > 1)
				dustMult = 1;
			if (dustMult > 0)
            {
				float length = 38 * dustMult;
				for(float i = 0; i < length; i++)
                {
					for(int j = -1; j <= 1; j++)
					{
						if (Main.rand.NextBool(3))
						{
							float scaleMult = 0.64f + 0.36f * (1 - (i / length));
							if (j != 0)
							{
								if(i % 2 == 0)
								{
									scaleMult *= 0.8f;
									Vector2 away = new Vector2(10, (10 + i * 1.0f) * 0.4f * j).RotatedBy(projectile.rotation);
									foamParticleList1.Add(new CurseFoam(projectile.Center + away, new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f)) + away * 0.1f, (0.65f + Main.rand.NextFloat(-0.1f, 0.1f)) * scaleMult, false));
								}
							}
							else
							{
								Vector2 away = new Vector2(10 + i * 1.0f, 0).RotatedBy(projectile.rotation);
								foamParticleList1.Add(new CurseFoam(projectile.Center + away, new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f)) + away * 0.1f, (0.65f + Main.rand.NextFloat(-0.1f, 0.1f)) * scaleMult, false));
							}
						}
					}
				}
			}
        }
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
			float speed = 9f;
			if (foundTarget && canAttack && targetWhoAmI != -1 && Main.npc[targetWhoAmI].CanBeChasedBy())
			{
				NPC npc = Main.npc[targetWhoAmI];
				float Twidth = (float)Math.Sqrt(npc.height * npc.width);
				Vector2 goTo = npc.Center - projectile.Center;
				speed += goTo.Length() * 0.004f;
				float sinM = (float)Math.Sin(MathHelper.ToRadians(projectile.ai[0] - 35)) * 64;
				if (projectile.ai[0] > attackTimerMax)
				{
					sinM = -(72 + Twidth);
					speed *= 3;
				}
				else if (projectile.ai[0] > attackTimerMax - 10)
				{
					if (canDoDashSounds)
					{
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 92, 0.7f, 0.25f);
					}
					canDoDashSounds = false;
				}
				float distance = goTo.Length() - (90 + Twidth + sinM);
				if (distance < 4 || projectile.ai[0] > 10)
				{
					projectile.ai[0] += 5;
				}
				goTo = goTo.SafeNormalize(Vector2.Zero);
				if (speed > distance)
				{
					speed = distance;
				}
				goTo *= speed;
				projectile.velocity = goTo;
				projectile.rotation = (npc.Center - projectile.Center).ToRotation();
				if(projectile.ai[0] > attackTimerMax + 10)
                {
					projectile.ai[0] = attackTimerMax + 10;
				}
			}
			else
			{
				canDoDashSounds = true;
				projectile.hide = false;
				targetWhoAmI = -1;
				foundTarget = false;
				if (!canAttack && projectile.ai[0] > 0)
                {
					if(projectile.ai[0] > attackTimerMax - 10)
						projectile.ai[0]--;
					else
						projectile.ai[0] -= 0.6f;
				}
				else
                {
					projectile.ai[0] = 0;
					canAttack = true;
				}
				if(projectile.ai[0] < attackTimerMax - 5)
					Idle();
				else
					projectile.velocity *= 0.985f;
				if (Main.myPlayer == player.whoAmI && (idlePosition - projectile.Center).Length() > 1200f)
				{
					projectile.position = idlePosition;
					projectile.velocity *= 0.1f;
					projectile.netUpdate = true;
				}
			}
			#endregion
			if (canAttack || projectile.ai[0] >= attackTimerMax - 5)
				DoDusts();
			Lighting.AddLight(projectile.Center, 0.55f * ((255 - projectile.alpha) / 255f), 0.1f * ((255 - projectile.alpha) / 255f), 0.25f * ((255 - projectile.alpha) / 255f));

			if (Main.myPlayer == player.whoAmI)
			{
				counter++;
				if(counter % 30 == 0)
					projectile.netUpdate = true;
			}
			catalogueParticles();
		}
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public void catalogueParticles()
		{
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				CurseFoam particle = foamParticleList1[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList1.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						foamParticleList1.RemoveAt(i);
						i--;
					}
					else if (!particle.noMovement)
						particle.position += projectile.velocity * 0.975f;
				}
			}
		}
	}
}
		