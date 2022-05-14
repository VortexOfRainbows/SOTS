using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Void;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Dusts;
using SOTS.NPCs.Constructs;
using System.Collections.Generic;

namespace SOTS.Projectiles.Minions
{
	public class EvilSpirit : SpiritMinion
	{
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(orbitalCounter);
			writer.Write(targetID);
			writer.Write(canAttack);
			writer.Write(readyToFight);
			base.SendExtraAI(writer); //required
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			orbitalCounter = reader.ReadSingle();
			targetID = reader.ReadInt32();
			canAttack = reader.ReadBoolean();
			readyToFight = reader.ReadBoolean();
			base.ReceiveExtraAI(reader); //required
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evil Spirit");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
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
		List<EvilEye> eyes = new List<EvilEye>();
		float orbitalCounter = 2;
		float range = 36;
		float eyeAlphaMult = 1f;
		public void UpdateEyes(bool draw = false, int ring = -2, int npcID = -1)
		{
			for (int i = 0; i < eyes.Count; i++)
			{
				float rangeMult = range / 36f;
				EvilEye eye = eyes[i];
				int direction = -1;
				float rotation = Projectile.rotation + MathHelper.ToRadians(orbitalCounter * direction);
				if (draw)
				{
					eye.Draw(Projectile.Center, rotation, rangeMult, eyeAlphaMult);
				}
				else
				{
					if (i == ring)
					{
						eye.Fire(Projectile.Center, npcID);
					}
					else
						eye.Update(Projectile.Center, rotation, rangeMult);
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			Color color2 = Color.Black;
			for (int i = 0; i < 2; i++)
			{
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(color2) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					spriteBatch.Draw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				}
				color2 = VoidPlayer.EvilColor * 1f;
			}
			return false;
		}
        public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = VoidPlayer.EvilColor * 1.0f;
			if (Main.netMode != NetmodeID.Server) //pretty sure drawcode doesn't run in multiplayer anyways but may as well
				UpdateEyes(true, -2);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.NextFloat(-2.5f, 2.5f);
				float y = Main.rand.NextFloat(-2.5f, 2.5f);
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, Projectile.GetAlpha(color), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
		}
        public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public void dustSound()
		{
			if (Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Projectile.Center, new Vector2(8f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))), ModContent.ProjectileType<EvilExplosion>(), Projectile.damage, 0f, Projectile.owner, 1, 0);
			}
		}
		bool runOnce = true;
		bool readyToFight = false;
		bool canAttack = false;
		int targetID = -1;
		public const int cooldown = 210;
		public const int totalShots = 16;
		public const int slamDelay = 40;
		public const int delayBetweenShots = 3;
		public const int timeBeforeShooting = 40;
		public int maxTimeFiring()
        {
			return totalShots * delayBetweenShots + delayBetweenShots - 1;
        }
		public override bool PreAI()
		{
			if(runOnce)
			{
				for (int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(range, 0).RotatedBy(MathHelper.ToRadians(i * 45f));
					eyes.Add(new EvilEye(circular, Projectile.damage / 2, true));
				}
				runOnce = false;
			}
			UpdateEyes(false, -2);
			return true;
		}
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float rotateAmt = 1;

			#region Active check
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<EvilSpiritAid>());
			}
			if (player.HasBuff(ModContent.BuffType<EvilSpiritAid>()))
			{
				Projectile.timeLeft = 6;
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
				Projectile.ai[1] = ofTotal;
			#endregion

			#region Find target
			float distanceFromTarget = 960f;
			Vector2 targetCenter = Projectile.Center;
			bool foundTarget = false;
			// This code is required if your minion weapon has the targeting feature
			if(canAttack && targetID == -1 && Projectile.ai[0] <= timeBeforeShooting)
			{
				if (player.HasMinionAttackTargetNPC)
				{
					NPC npc = Main.npc[player.MinionAttackTargetNPC];
					//float between = Vector2.Distance(npc.Center, Projectile.Center);
					float between2 = Vector2.Distance(npc.Center, player.Center);
					if (between2 < distanceFromTarget)
					{
						distanceFromTarget = between2;
						targetCenter = npc.Center;
						foundTarget = true;
						targetID = npc.whoAmI;
					}
				}
				if (!foundTarget)
				{
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC npc = Main.npc[i];
						if (npc.CanBeChasedBy())
						{
							float between = Vector2.Distance(npc.Center, Projectile.Center);
							float between2 = Vector2.Distance(npc.Center, player.Center);
							bool inRange = between < distanceFromTarget;
							bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

							bool closeThroughWall = between < 240f; //15 blocks through walls //should attack semi-reliably through walls
							if (inRange && (lineOfSight || closeThroughWall) && between2 < distanceFromTarget)
							{
								distanceFromTarget = between2;
								targetCenter = npc.Center;
								foundTarget = true;
								targetID = i;
							}
						}
					}
				}
			}
			if(targetID != -1)
			{
				NPC npc = Main.npc[targetID];
				if (npc.CanBeChasedBy())
                {
					targetCenter = npc.Center;
				}
				else
                {
					targetCenter = Projectile.Center;
					targetID = -1;
                }
			}
			#endregion

			#region Movement
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			float speed = 4f;
			if (foundTarget || targetID != -1 || Projectile.ai[0] > timeBeforeShooting)
			{
				if (Projectile.alpha >= 255)
				{
					Projectile.Center = targetCenter;
				}
				else if (Projectile.alpha > 0)
				{
					speed += 16f * (Projectile.alpha / 255f);
				}

				Vector2 direction = targetCenter - Projectile.Center;
				float distance = direction.Length();
				direction = direction.SafeNormalize(Vector2.Zero);
				if (distance > speed)
				{
					distance = speed;
				}
				direction *= distance;
				Projectile.velocity = direction;
				Projectile.alpha += 8;
				if (Projectile.alpha > 255)
				{
					int totalTime = timeBeforeShooting + maxTimeFiring();
					Projectile.alpha = 255;
					if (readyToFight)
						Projectile.ai[0]++;
					if (Projectile.ai[0] > (totalTime + 20 + slamDelay))
					{
						targetID = -1;
						canAttack = false;
						Projectile.ai[0] = -cooldown;
						Projectile.alpha = 0;
						readyToFight = false;
						Projectile.netUpdate = true;
						dustSound();
					}
					else
					{
						if (Projectile.ai[0] >= timeBeforeShooting && Projectile.ai[0] < totalTime) //total of 30 projectiles
						{
							int bonus = (int)Projectile.ai[0];
							if (bonus > 96)
								bonus = 96;
							range = 32 + bonus;
							int remaining = (int)Projectile.ai[0] - timeBeforeShooting;
							int id = remaining / delayBetweenShots;
							if(remaining % delayBetweenShots == 0)
                            {
								UpdateEyes(false, id % 8, targetID);
                            }
                        }
						float rangeMult = Projectile.ai[0] / (float)timeBeforeShooting * 2f;
						if (rangeMult > 1)
							rangeMult = 1;
						float baseR = 32;
						if(Projectile.ai[0] > totalTime + 20)
                        {
							float rangeMultS = 1 - (Projectile.ai[0] - totalTime - 20) / (float)slamDelay;
							float additionalSinusoid = (float)Math.Sin(MathHelper.ToRadians(240 * (1 - rangeMultS)));
							rangeMult = (float)Math.Sqrt(rangeMultS);
							baseR = 32 * (1 + additionalSinusoid * 3f);
							baseR *= rangeMult;
							rotateAmt *= rangeMultS;
						}
						float sinusoid = (float)Math.Sin(MathHelper.ToRadians(450 * Projectile.ai[0] / (totalTime + 20 + slamDelay)));
						range = baseR + 128 * rangeMult + baseR * sinusoid;
						float mult = Projectile.ai[0] / (float)timeBeforeShooting;
						if (mult > 1)
							mult = 1;
						rotateAmt += mult * 2f * rangeMult; //speed is at 3 degrees / frame, 
						readyToFight = true;
					}
				}
			}
			else
			{
				targetID = -1;
				GoIdle();
				readyToFight = false;
				if(!canAttack)
                {
					if (Projectile.ai[0] < 0)
						Projectile.ai[0]++;
					else
						canAttack = true;
					range = 32f * (1 + Projectile.ai[0] / cooldown);
					eyeAlphaMult = 1 + Projectile.ai[0] / cooldown;

				}
				else
				{
					eyeAlphaMult = 1;
					if (range > 32)
						range -= 2;
					else if (range > 30)
						range = 32;
				}
				if(Projectile.ai[0] > 0)
				{
					canAttack = true;
					Projectile.ai[0] -= 6f;
				}	
				else if(canAttack)
					Projectile.ai[0] = 0;
				Projectile.alpha -= 12;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
				Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
				float distanceToIdlePosition = vectorToIdlePosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
				{
					Projectile.ai[0] = 0;
					Projectile.alpha = 0;
					Projectile.position = idlePosition;
					Projectile.velocity *= 0.1f;
					Projectile.netUpdate = true;
				}
			}
			#endregion

			Lighting.AddLight(Projectile.Center, VoidPlayer.EvilColor.ToVector3());
			MoveAwayFromOthers();
			if (Main.myPlayer == player.whoAmI)
			{
				Projectile.netUpdate = true;
			}
			orbitalCounter += rotateAmt;
		}
	}
}