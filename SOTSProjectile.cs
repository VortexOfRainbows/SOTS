using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.NPCs.ArtificialDebuffs;
using SOTS.Projectiles;
using SOTS.Projectiles.Earth;
using SOTS.Projectiles.Evil;
using SOTS.Projectiles.Inferno;
using SOTS.Projectiles.Laser;
using SOTS.Projectiles.Nature;
using SOTS.Projectiles.Otherworld;
using SOTS.Projectiles.Permafrost;
using SOTS.Void;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.SOTS;

namespace SOTS
{
	public class SOTSProjectile : GlobalProjectile
	{
		public static int[] immuneToTimeFreeze;
		public static int[] isChargeWeapon;
		public static void LoadArrays()
		{
			immuneToTimeFreeze = new int[]
			{
				ModContent.ProjectileType<VisionWeapon>()
			};
			isChargeWeapon = new int[]
			{
				ProjectileID.LastPrismLaser,
				ModContent.ProjectileType<PrismOrb>(),
				ModContent.ProjectileType<Starshot>(),
				ModContent.ProjectileType<EarthshakerPickaxe>()
			};
		}
		public static bool CanBeTimeFrozen(Projectile proj)
        {
			if(proj.owner >= 0)
			{
				Player player = Main.player[proj.owner];
				SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
				if (sPlayer.oldTimeFreezeImmune || sPlayer.TimeFreezeImmune)
                {
					if((ProjectileID.Sets.LightPet[proj.type] || Main.projPet[proj.type]) && !proj.minion)
                    {
						return false;
                    }
					if (proj.aiStyle == 7) //grappling hook
						return false;
					if(immuneToTimeFreeze.Contains(proj.type) || isChargeWeapon.Contains(proj.type))
                    {
						return false;
					}
					if (sPlayer.oldHeldProj == proj.whoAmI || player.heldProj == proj.whoAmI)
                    {
						return false;
                    }
				}
			}
			return true;
        }
		public override bool InstancePerEntity => true;
		public override void PostAI(Projectile projectile)
		{
			NatureSlimeUnit(projectile);
			HomingUnit(projectile);
			counter++;
			if (projectile.arrow || projectile.type == ModContent.ProjectileType<ChargedHardlightArrow>())
				FrostFlakeUnit(projectile, frostFlake - 2);
			AffixUnit(projectile);
		}
		public int timeFrozen = 0;
		public bool netUpdateTime = false;
		public bool frozen = false;
		public float aiSpeedCounter = 0;
		public float aiSpeedMultiplier = 1;
		public static void SetTimeFreeze(Player player, Projectile proj, int time)
		{
			SOTSProjectile instancedProjectile = proj.GetGlobalProjectile<SOTSProjectile>();
			instancedProjectile.timeFrozen = time;
			instancedProjectile.frozen = false;
			if ((player == null && Main.netMode == NetmodeID.Server) || Main.netMode != NetmodeID.SinglePlayer)
				instancedProjectile.SendClientChanges(player, proj, 1);
		}
		public const float GlobalFreezeSlowdownDuration = 12f;
		public float GlobalFreezeSlowdownCounter = 0;
		public static bool GlobalFreezeSlowdown(Projectile proj)
		{
			SOTSProjectile instancedProjectile = proj.GetGlobalProjectile<SOTSProjectile>();
			float percent = 1 - instancedProjectile.counter / GlobalFreezeSlowdownDuration;
			instancedProjectile.GlobalFreezeSlowdownCounter += percent;
			if(instancedProjectile.GlobalFreezeSlowdownCounter >= 1)
            {
				instancedProjectile.GlobalFreezeSlowdownCounter -= 1;
				return false;
			}
			return true;
		}
		public static bool UpdateWhileFrozen(Projectile proj, int i)
		{
			SOTSProjectile instancedProjectile = proj.GetGlobalProjectile<SOTSProjectile>();
			if (instancedProjectile.timeFrozen > 0 || instancedProjectile.frozen)
			{
				if (!instancedProjectile.frozen)
				{
					if (instancedProjectile.aiSpeedMultiplier > 0)
					{
						instancedProjectile.aiSpeedMultiplier -= 1 / DebuffNPC.timeBeforeFullFreeze;
					}
					else
					{
						instancedProjectile.aiSpeedMultiplier = 0;
						instancedProjectile.frozen = true;
					}
				}
				else
				{
					if (instancedProjectile.timeFrozen > 1)
					{
						instancedProjectile.timeFrozen--;
					}
					else
					{
						instancedProjectile.aiSpeedMultiplier += 1 / DebuffNPC.timeBeforeFullFreeze;
						if (instancedProjectile.aiSpeedMultiplier > 1)
						{
							instancedProjectile.aiSpeedMultiplier = 1;
							instancedProjectile.timeFrozen = 0;
							instancedProjectile.frozen = false;
						}
					}
				}
				if (instancedProjectile.timeFrozen == 0 && Main.netMode == NetmodeID.Server)
				{
					instancedProjectile.netUpdateTime = true;
				}
				proj.whoAmI = i;
			}
			else
			{
				instancedProjectile.frozen = false;
			}
			instancedProjectile.aiSpeedCounter += instancedProjectile.aiSpeedMultiplier;
			if (instancedProjectile.aiSpeedCounter >= 1)
			{
				instancedProjectile.aiSpeedCounter -= 1;
			}
			else
				return true;
			return false;
		}
		public int frostFlake = 0;
		public int affixID = 0;
		public bool hasHitYet = false;
		public bool effect = true;
		public int counter = 0;
		public int petAdvisorID = -1;
		private float spinCounter = 0;
		public void SendClientChanges(Player player, Projectile projectile, int type = 0)
		{
			// Send a Mod Packet with the changes.
			if(type == 0)
			{
				var packet = mod.GetPacket();
				packet.Write((byte)SOTSMessageType.SyncGlobalProj);
				packet.Write((byte)player.whoAmI);
				packet.Write(projectile.identity);
				packet.Write(frostFlake);
				packet.Write(affixID);
				packet.Send();
			}
			if (type == 1) //can be called by server or player
			{
				int playerWhoAmI = player != null ? player.whoAmI : -1;
				var packet = mod.GetPacket();
				packet.Write((byte)SOTSMessageType.SyncGlobalProjTime);
				packet.Write(playerWhoAmI);
				packet.Write(projectile.whoAmI);
				packet.Write(timeFrozen);
				packet.Write(frozen);
				packet.Send();
			}
		}
		private Vector2 initialVelo = Vector2.Zero;
		public void AffixUnit(Projectile projectile)
        {
			if(affixID < 0)
            {
				affixID = -affixID;
				if (Main.myPlayer == projectile.owner && Main.netMode == NetmodeID.MultiplayerClient)
					SendClientChanges(Main.player[projectile.owner], projectile);
			}
			if(affixID > 0) //not else if
            {
				if(affixID == 1) //Ancient Steel Longbow
				{
					if (projectile.extraUpdates < 1)
						projectile.extraUpdates++;
					for (int i = 0; i < 3; i++)
					{
						Vector2 spawnPos = Vector2.Lerp(projectile.Center, projectile.oldPosition + projectile.Size / 2, i * 0.34f);
						Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, DustID.Silver, 0, 0, 0, Color.LightGray);
						dust.noGravity = true;
						dust.scale = 1.1f;
						dust.velocity = Vector2.Zero;
					}
					if (initialVelo == Vector2.Zero)
						initialVelo = projectile.velocity;
					else
						initialVelo -= (initialVelo - projectile.velocity) * 0.4f; //only recieve 40% of arrows usual gravity
					if (projectile.velocity.X == initialVelo.X && projectile.velocity.Y != initialVelo.Y)
						projectile.velocity = initialVelo;
				}
				if (affixID == 2) //Blaspha
				{
					for (int i = 0; i < 2; i++)
					{
						if(Main.rand.NextBool(7))
						{
							Vector2 spawnPos = Vector2.Lerp(projectile.Center, projectile.oldPosition + projectile.Size / 2, i * 0.5f);
							Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, DustID.Fire);
							dust.noGravity = true;
							dust.scale += 0.2f;
							dust.scale *= 1.1f;
							dust.velocity *= 0.5f;
							dust.velocity += projectile.velocity * 0.1f;
						}
						else if(Main.rand.NextBool(14))
						{
							Vector2 spawnPos = Vector2.Lerp(projectile.Center, projectile.oldPosition + projectile.Size / 2, i * 0.5f);
							Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>());
							dust.velocity *= 0.1f;
							dust.noGravity = true;
							dust.scale += 0.2f;
							dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1f));
							dust.fadeIn = 0.1f;
							dust.scale *= 1.2f;
							dust.velocity += projectile.velocity * 0.1f;
						}
					}
				}
				if(affixID == 3) //Chaos Chamber
				{
					for (int i = 0; i < 4; i++)
					{
						if(Main.rand.NextBool(2))
						{
							Vector2 spawnPos = Vector2.Lerp(projectile.Center, projectile.oldPosition + projectile.Size / 2, i * 0.25f);
							Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>());
							dust.velocity *= 0.33f;
							dust.noGravity = true;
							dust.scale *= 0.3f;
							dust.scale += 0.8f;
							dust.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians((projectile.whoAmI + Main.GameUpdateCount) * 12 + i * 4), true);
							dust.alpha = (int)(projectile.alpha * 0.5f + 125);
							dust.fadeIn = 0.1f;
							dust.velocity += projectile.velocity * 0.05f;
						}
					}
					float homingRange = (float)(180 + 64 * Math.Sqrt(counter));
					if (homingRange > 640)
						homingRange = 640;
					int target = SOTSNPCs.FindTarget_Basic(projectile.Center, homingRange, this);
					if (target >= 0)
					{
						NPC npc = Main.npc[target];
						projectile.velocity = Vector2.Lerp(projectile.velocity, (npc.Center - projectile.Center).SafeNormalize(Vector2.Zero) * (projectile.velocity.Length() + 3), 0.055f);
					}
				}
			}
		}
		public void FrostFlakeUnit(Projectile projectile, int level)
        {
			if (level < -1)
				return;
			else if(level <= 0)
            {
				frostFlake += 2;
				if (Main.myPlayer == projectile.owner && Main.netMode == NetmodeID.MultiplayerClient)
					SendClientChanges(Main.player[projectile.owner], projectile);
            }
			if (projectile.type == ModContent.ProjectileType<ChargedHardlightArrow>())
				return;
			spinCounter += projectile.velocity.Length() * MathHelper.ToRadians(0.75f) * projectile.direction;
			if (level == 1)
			{
				if (!Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(projectile.Center + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
					dust.noGravity = true;
					dust.scale = 1;
					dust.velocity = Vector2.Zero;
					dust.fadeIn = 0.1f;
				}
			}
			else if(level == 2)
			{
				for(int i = 0; i < 5; i++)
				{
					Vector2 spawnPos = Vector2.Lerp(projectile.Center, projectile.oldPosition + projectile.Size / 2, i * 0.2f);
					Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
					dust.noGravity = true;
					dust.scale = 1.2f;
					dust.velocity = Vector2.Zero;
					dust.fadeIn = 0.1f;
				}
			}
			for(int i = 0; i < level; i++)
            {
				if(Main.rand.NextBool(2))
				{
					Dust dust = Dust.NewDustDirect(projectile.Center + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
					dust.noGravity = true;
					dust.scale = dust.scale * 0.5f + (1 + level) * 0.5f;
					dust.velocity = (dust.velocity * 0.3f + Main.rand.NextVector2Circular(0.6f, 0.6f) + projectile.velocity * 0.5f);
					dust.fadeIn = 0.1f;
				}
			}
			if(initialVelo == Vector2.Zero)
				initialVelo = projectile.velocity;
			if(projectile.velocity.X == initialVelo.X && projectile.velocity.Y != initialVelo.Y)
				projectile.velocity = initialVelo;
        }
        public void HomingUnit(Projectile projectile)
		{
			if (hasHitYet || !projectile.active || projectile.damage <= 0 || counter > 900f || SOTSPlayer.typhonBlacklist.Contains(projectile.type))
				return;
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float distP = Vector2.Distance(player.Center, projectile.Center);
			if (!player.active || distP > 2000f)
				return;
			if (modPlayer.petAdvisor && counter >= 5 && modPlayer.typhonRange > 0)
			{
				if (petAdvisorID == -1)
				{
					//Main.NewText("Advisor Check " + projectile.whoAmI);
					for (int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if (proj.active && proj.owner == projectile.owner && proj.type == ModContent.ProjectileType<AdvisorPet>())
						{
							petAdvisorID = i;
							break;
						}
					}
				}
				else
				{
					Projectile proj = Main.projectile[petAdvisorID];
					if (!(proj.active && proj.owner == projectile.owner && proj.type == ModContent.ProjectileType<AdvisorPet>()))
					{
						petAdvisorID = -1;
					}
				}
			}
			if (counter >= 5)
			{
				if (modPlayer.typhonRange > 0)
				{
					float minDist = modPlayer.typhonRange * 2;
					int target2 = -1;
					float speed = projectile.velocity.Length();
					bool capable = speed > 1f && (projectile.ranged || projectile.melee || projectile.magic || projectile.thrown || (!projectile.sentry && !projectile.minion)) && (projectile.modProjectile == null || projectile.modProjectile.ShouldUpdatePosition()) && (projectile.modProjectile == null || projectile.modProjectile.CanDamage());
					if (projectile.friendly == true && projectile.hostile == false && player.heldProj != projectile.whoAmI && (capable || SOTSPlayer.typhonWhitelist.Contains(projectile.type)))
					{
						//Main.NewText("past Check " + projectile.whoAmI);
						for (int i = 0; i < Main.npc.Length; i++)
						{
							NPC target = Main.npc[i];
							if (target.CanBeChasedBy())
							{
								float distance = Vector2.Distance(projectile.Center, target.Center);
								if (distance < minDist)
								{
									Rectangle increasedHitbox = new Rectangle(projectile.Hitbox.X - modPlayer.typhonRange, projectile.Hitbox.Y - modPlayer.typhonRange, projectile.width + 2 * modPlayer.typhonRange, projectile.height + 2 * modPlayer.typhonRange);
									if (target.Hitbox.Intersects(increasedHitbox))
									{
										if (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height))
										{
											minDist = distance;
											target2 = i;
										}
									}
								}
							}
						}
						if (target2 != -1)
						{
							NPC toHit = Main.npc[target2];
							if (toHit.active)
							{
								Vector2 goTo = (toHit.Center - projectile.Center).SafeNormalize(Vector2.Zero) * speed;
								Vector2 velocity1 = projectile.velocity.SafeNormalize(Vector2.Zero);
								Vector2 velocity2 = goTo.SafeNormalize(Vector2.Zero);
								float close = (velocity1 - velocity2).Length() * 40f;
								projectile.velocity = goTo;
								if (petAdvisorID != -1 && effect)
								{
									Projectile proj = Main.projectile[petAdvisorID];
									if ((int)((90 - (int)close) * 2.5 + 45) < 255)
									{
										LaserTo(petAdvisorID, projectile, 90 - (int)close);
										float recalc = (close - 40) / 40f; //1 max, -1 min
										AdvisorPet pet = (AdvisorPet)proj.modProjectile;
										float num = -1f * recalc;
										pet.eyeReset = num - 1.5f;
										pet.fireToX = projectile.Center.X;
										pet.fireToY = projectile.Center.Y;
										pet.glow = 11.5f + 3.5f * recalc;
										Main.PlaySound(SoundID.Item, (int)proj.Center.X, (int)proj.Center.Y, 8, 1.35f * (0.75f + 0.5f * recalc));
									}
									effect = false;
								}
								hasHitYet = true;
							}
						}
					}
				}
			}
		}
		public bool hasFrostBloomed = false;
		public int bloomingHookAssignment = -1;
		public void NatureSlimeUnit(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			if (player.active && projectile.minion && projectile.active && !SOTSPlayer.symbioteBlacklist.Contains(projectile.type) && (Main.projPet[projectile.type] || VoidPlayer.isVoidMinion(projectile.type)) && projectile.damage > 0)
			{
				SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
				if(modPlayer.symbioteDamage > 0 && projectile.owner == Main.myPlayer)
				{
					if (bloomingHookAssignment == -1)
					{
						bloomingHookAssignment = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<BloomingHookMinion>(), modPlayer.symbioteDamage, 0.4f, Main.myPlayer, projectile.identity);
					}
					Projectile hook = Main.projectile[bloomingHookAssignment];
					if (!hook.active || hook.type != ModContent.ProjectileType<BloomingHookMinion>())
					{
						bloomingHookAssignment = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<BloomingHookMinion>(), modPlayer.symbioteDamage, 0.4f, Main.myPlayer, projectile.identity);
					}
					hook.timeLeft = 6;
				}
			}
			else
				return;
        }
        public override void Kill(Projectile projectile, int timeLeft)
        {
			if(!hasFrostBloomed && frostFlake >= 3)
			{
				FrostBloom(projectile);
			}
			if (affixID == 2)
			{
				if (Main.myPlayer == projectile.owner)
				{
					Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-120, 120))) * Main.rand.NextFloat(0.4f, 0.6f), ModContent.ProjectileType<InfernoSeeker>(), (int)(projectile.damage * 0.5f), projectile.knockBack, Main.myPlayer, Main.rand.Next(3));
				}
			}
		}
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			if (!hasFrostBloomed && frostFlake >= 3)
				FrostBloom(projectile);
			if(affixID == 1)
			{
				if (Main.myPlayer == projectile.owner)
				{
					for (int i = 0; i < 3; i++)
					{
						float randAmt = 5 + 12 * i;
						Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-randAmt, randAmt))) * Main.rand.NextFloat(0.2f, 0.3f), ModContent.ProjectileType<SteelShrapnel>(), (int)(projectile.damage), projectile.knockBack, Main.myPlayer, Main.rand.Next(3));
					}
				}
			}
		}
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
		{
			if (!hasFrostBloomed && frostFlake >= 3)
				FrostBloom(projectile);
			if (affixID == 1)
			{
				if (Main.myPlayer == projectile.owner)
				{
					for (int i = 0; i < 6; i++)
					{
						Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-180, 180f))) * Main.rand.NextFloat(0.2f, 0.3f), ModContent.ProjectileType<SteelShrapnel>(), (int)(projectile.damage), projectile.knockBack, Main.myPlayer, Main.rand.Next(3));
					}
				}
			}
			return base.OnTileCollide(projectile, oldVelocity);
        }
        public void FrostBloom(Projectile projectile)
		{
			hasFrostBloomed = true;
			int ffValue = frostFlake - 2;
			frostFlake = 0;
			Vector2 manipulateVelo = projectile.oldVelocity;
			//TODO: add these visual effect onto the residual projectile to increase multiplayer compatability
			if (Main.myPlayer == projectile.owner)
			{
				if (ffValue > 0 && projectile.type != ModContent.ProjectileType<ChargedHardlightArrow>())
				{
					float damageMult = 2;
					if (ffValue == 2)
						damageMult = 6;
					Projectile.NewProjectile(projectile.Center, manipulateVelo, ModContent.ProjectileType<FrostflakePulse>(), (int)(projectile.damage * damageMult), projectile.knockBack, Main.myPlayer, ffValue, spinCounter);
				}
				if (Main.netMode == NetmodeID.MultiplayerClient)
					SendClientChanges(Main.player[projectile.owner], projectile);
			}
		}
        public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
		{
			if (frostFlake == 4 && projectile.type != ModContent.ProjectileType<ChargedHardlightArrow>() && !hasFrostBloomed)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 spawnPos = Vector2.Lerp(projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * 8, projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * -120f, i * 0.3f);
					float percent = (1 - 0.3f * i);
					float alphaMult = (counter - i * 7) / 14f;
					if (alphaMult > 0.7f)
						alphaMult = 0.7f;
					else if (alphaMult < 0)
						alphaMult = 0;
					float dist1 = 8 * percent;
					float dist2 = 6 * percent;
					DrawStar(spawnPos, alphaMult, projectile.velocity.ToRotation(), spinCounter + MathHelper.ToRadians(30 * i), 6, dist1, dist2, 0.6f);
				}
			}
			/*(if (bloomingHookAssignment != -1)
			{
				Projectile hook = Main.projectile[bloomingHookAssignment];
				if (hook.active && hook.type == ModContent.ProjectileType<BloomingHookMinion>())
                {
					BloomingHookMinion minion = hook.modProjectile as BloomingHookMinion;
					minion.Draw(spriteBatch, lightColor);
                }
			}*/
            return base.PreDraw(projectile, spriteBatch, lightColor);
        }
        public static void LaserTo(int advisorId, Projectile projectile, int extraAlpha)
		{
			Player player = Main.player[projectile.owner];
			int alpha = (int)(extraAlpha * 2.5 + 40);
			Projectile proj = Main.projectile[advisorId];
			Vector2 projPos = proj.Center + new Vector2(0, 8);
			Vector2 toProjectile = projectile.Center - projPos;
			Vector2 newtoProjectile = toProjectile.SafeNormalize(Vector2.Zero) * 4;
			Vector2 currentPos = projPos;
			Vector2 savePos = projPos;
			Color color = new Color(255, 182, 242, 0);
			int remaining = 3000;
			int interator = 0;
			currentPos += newtoProjectile * 2;
			while (remaining > 0)
			{
				remaining--;
				interator++;
				currentPos += newtoProjectile;
				int num1 = Dust.NewDust(new Vector2(currentPos.X - 4, currentPos.Y - 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, alpha);
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.2f;
				dust.noGravity = true;
				dust.color = color;
				dust.fadeIn = 0.2f;
				dust.scale *= 1.25f;
				dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[1].type);
				toProjectile = projectile.Center - currentPos;
				if (toProjectile.Length() < Math.Sqrt(proj.width * projectile.height) + 8 && remaining > 60)
				{
					for (int i = 0; i < 360; i += 10)
					{
						Vector2 currentFromProjectile = currentPos - projectile.Center;
						currentFromProjectile = currentFromProjectile.RotatedBy(MathHelper.ToRadians(i));
						currentFromProjectile += projectile.Center;
						interator++;
						num1 = Dust.NewDust(new Vector2(currentFromProjectile.X - 4, currentFromProjectile.Y - 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, alpha);
						dust = Main.dust[num1];
						dust.velocity *= 0.2f;
						dust.noGravity = true;
						dust.color = color;
						dust.fadeIn = 0.2f;
						dust.scale *= 1.25f;
						dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[1].type);
					}
					remaining = 20;
					currentPos = new Vector2((float)Math.Sqrt(proj.width * projectile.height) + 8, 0).RotatedBy(projectile.velocity.ToRotation()) + projectile.Center;
				}
				if (remaining < 20)
				{
					newtoProjectile = projectile.velocity.SafeNormalize(Vector2.Zero) * 3;
				}
				savePos = currentPos;
			}
			currentPos = savePos;
			for (int i = 0; i < 8; i++)
			{
				newtoProjectile = projectile.velocity.RotatedBy(MathHelper.ToRadians(-160)).SafeNormalize(Vector2.Zero) * 3;
				currentPos += newtoProjectile;
				interator++;
				int num1 = Dust.NewDust(new Vector2(currentPos.X - 4, currentPos.Y - 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, alpha);
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.2f;
				dust.noGravity = true;
				dust.color = color;
				dust.fadeIn = 0.2f;
				dust.scale *= 1.25f;
				dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[1].type);
			}
			currentPos = savePos;
			for (int i = 0; i < 8; i++)
			{
				newtoProjectile = projectile.velocity.RotatedBy(MathHelper.ToRadians(160)).SafeNormalize(Vector2.Zero) * 3;
				currentPos += newtoProjectile;
				interator++;
				int num1 = Dust.NewDust(new Vector2(currentPos.X - 4, currentPos.Y - 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, alpha);
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.2f;
				dust.noGravity = true;
				dust.color = color;
				dust.fadeIn = 0.2f;
				dust.scale *= 1.25f;
				dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[1].type);
			}
		}
		public static void DrawStar(Vector2 location, float alphaMult, float rotation, float spin = 0, int pointAmount = 6, float innerDistAdd = 10, float innerDistMin = 8, float xCompress = 0.6f, int density = 180)
		{
			DrawStar(location, new Color(116, 125, 238, 0), alphaMult, rotation, spin, pointAmount, innerDistAdd, innerDistMin, xCompress, density);
		}
		public static void DrawStar(Vector2 location, Color color, float alphaMult, float rotation, float spin = 0, int pointAmount = 6, float innerDistAdd = 10, float innerDistMin = 8, float xCompress = 0.6f, int density = 180)
		{
			Vector2 fireFrom = location; 
			Texture2D texture = ModContent.GetTexture("SOTS/Assets/StrangeGradient");
			for (float k = 0; k < 360; k += 360 / (float)density)
			{
				float length = innerDistAdd + innerDistMin;
				float rad = MathHelper.ToRadians(k);
				float rand = 0;
				float x = (float)Math.Cos(rad + rand);
				float y = (float)Math.Sin(rad + rand);
				float mult = (Math.Abs((rad * (pointAmount / 2) % (float)Math.PI) - (float)Math.PI / 2) * innerDistAdd) + innerDistMin;//triangle wave function
				Vector2 circular = new Vector2(x, y).RotatedBy(spin) * mult;
				circular.X *= xCompress;
				Vector2 scale = new Vector2(circular.Length() / length, 0.5f);
				circular = circular.RotatedBy(rotation);
				Main.spriteBatch.Draw(texture, fireFrom + circular - Main.screenPosition, null, color * alphaMult * (1 / (circular.Length() / length)), circular.ToRotation(), new Vector2(texture.Width / 2, texture.Height / 2), scale * 1.25f, SpriteEffects.None, 0f);
			}
		}
		public static void DustStar(Vector2 location, Vector2 velocity, float rotation, int total = 30, float spin = 0, int pointAmount = 6, float innerDistAdd = 10, float innerDistMin = 8, float xCompress = 0.6f, float scaleMult = 1f)
		{
			for (float k = 0; k < total; k++)
			{
				float rad = MathHelper.ToRadians(k * 360f / total);
				float rand = 0;
				float x = (float)Math.Cos(rad + rand);
				float y = (float)Math.Sin(rad + rand);
				float mult = (Math.Abs((rad * (pointAmount / 2) % (float)Math.PI) - (float)Math.PI / 2) * innerDistAdd) + innerDistMin;//triangle wave function
				Vector2 circular = new Vector2(x, y).RotatedBy(spin) * mult;
				circular.X *= xCompress;
				circular = circular.RotatedBy(rotation);
				Dust dust = Dust.NewDustDirect(circular + location - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
				dust.noGravity = true;
				dust.scale = (dust.scale * 0.5f + 1) * scaleMult;
				dust.velocity = dust.velocity * 0.1f + velocity;
				dust.fadeIn = 0.1f;
			}
		}
	}
}