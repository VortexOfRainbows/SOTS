using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Void;
using SOTS.NPCs.Boss;
using SOTS.Items;
using SOTS.Buffs;
using SOTS.NPCs.Constructs;
using SOTS.Items.Celestial;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Inferno;
using SOTS.NPCs.Boss.Advisor;
using SOTS.Items.Pyramid;
using SOTS.Items.Otherworld;
using SOTS.Items.Pyramid.PyramidWalls;
using SOTS.NPCs.TreasureSlimes;
using SOTS.Items.Fragments;
using SOTS.Dusts;
using SOTS.Items.Crushers;
using SOTS.Items.Nature;
using SOTS.Items.Tools;
using SOTS.Items.Slime;
using SOTS.Items.Evil;
using SOTS.Items.Tide;
using SOTS.Items.Permafrost;
using SOTS.Items.Otherworld.Blocks;
using SOTS.Items.Inferno;
using SOTS.Items.Chaos;
using SOTS.Items.GhostTown;
using SOTS.NPCs.Phase;

namespace SOTS.NPCs
{
    public class SOTSNPCs : GlobalNPC
	{
		/// <summary>
		/// Gets the damage of a NPC prior Expert, Master, Journey modifiers
		/// </summary>
		public static int GetBaseDamage(NPC npc)
        {
			return (int)(npc.damage / (Main.GameModeInfo.EnemyDamageMultiplier * npc.strengthMultiplier));
        }
		public static int FindTarget_Basic(Vector2 center, float minDistance = 2000f, object attacker = null, bool needsLOS = false)
		{
			return FindTarget_Basic(center, out float _, minDistance, attacker, needsLOS);
		}
		public static int FindTarget_Basic(Vector2 center, out float dist, float minDistance = 2000f, object attacker = null, bool needsLOS = false)
		{
			int target = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].CanBeChasedBy(attacker))
				{
					float distance = (center - Main.npc[i].Center).Length();
					if (distance < minDistance && (!needsLOS || Collision.CanHitLine(center - new Vector2(16, 16), 32, 32, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height)))
					{
						target = i;
						minDistance = distance;
					}
				}
			}
			dist = minDistance;
			return target;
		}
		public static int FindTarget_Ignore(Vector2 center, List<int> ignore, float minDistance = 2000f, object attacker = null)
		{
			return FindTarget_Ignore(center, out float _, ignore, minDistance);
		}
		public static int FindTarget_Ignore(Vector2 center, out float dist, List<int> ignore, float minDistance = 2000f, object attacker = null)
		{
			int target = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].CanBeChasedBy(attacker) && !ignore.Contains(i))
				{
					float distance = (center - Main.npc[i].Center).Length();
					if (distance < minDistance)
					{
						target = i;
						minDistance = distance;
					}
				}
			}
			dist = minDistance;
			return target;
		}
		public static bool isPolarisNPC(int type)
        {
			return (type == ModContent.NPCType<BulletSnakeHead>() || type == ModContent.NPCType<BulletSnakeWing>() || type == ModContent.NPCType<BulletSnakeBody>() || type == ModContent.NPCType<BulletSnakeEnd>() || type == ModContent.NPCType<Polaris>() || type == ModContent.NPCType<PolarisLaser>() || type == ModContent.NPCType<PolarisCannon>());
		}
        public override bool PreAI(NPC npc)
		{
			if(isPolarisNPC(npc.type))
			{
				if (Main.rand.NextBool(2))
				{
					npc.HitSound = SoundID.NPCHit4;
				}
				else
				{
					npc.HitSound = SoundID.Item50;
				}
				if(npc.DeathSound != SoundID.NPCDeath14)
					npc.DeathSound = SoundID.NPCDeath14;
			}
			return base.PreAI(npc);
        }
        public override void HitEffect(NPC npc, int hitDirection, double damage)
		{
			if (isPolarisNPC(npc.type))
			{
				if (npc.life <= 0)
				{
					for (int i = 0; i < 3; i++)
					{
						Gore.NewGore(npc.position + new Vector2((float)(npc.width * Main.rand.Next(100)) / 100f, (float)(npc.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, npc.velocity * 0.5f, Main.rand.Next(61, 64), 1f);
					}
					for (int i = 0; i < 15; i++)
					{
						int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - new Vector2(5), npc.width, npc.height, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[num1];
						dust.velocity *= 2f;
						dust.velocity += npc.velocity * 0.2f;
						dust.noGravity = true;
						dust.scale += 1.0f;
						dust.color = new Color(200, 250, 250, 100);
						dust.fadeIn = 0.1f;
						dust.scale *= 1.5f;
						dust.alpha = npc.alpha;
					}
				}
			}
			else if (npc.type == ModContent.NPCType<PhaseAssaulterHead>() || npc.type == ModContent.NPCType<PhaseAssaulterBody>() || npc.type == ModContent.NPCType<PhaseAssaulterTail>())
			{
				if (npc.life > 0)
				{
					int num = 0;
					while (num < damage / npc.lifeMax * 40.0)
					{
						if (Main.rand.NextBool(3))
							Dust.NewDust(npc.position, npc.width, npc.height, DustID.Gold, (float)(2 * hitDirection), -2f, 0, default, 0.9f);
						else
						{
							Dust.NewDust(npc.position, npc.width, npc.height, DustID.Platinum, (float)(2 * hitDirection), -2f, 0, default, 0.9f);
						}
						num++;
					}
				}
				else
				{
					{
						for (int k = 0; k < 20; k++)
						{
							if (k % 4 == 0)
							{
								Dust.NewDust(npc.position, npc.width, npc.height, 242, (float)(2 * hitDirection), -2f, 0, default, 2f);
							}
							if (Main.rand.NextBool(3))
								Dust.NewDust(npc.position, npc.width, npc.height, DustID.Gold, (float)(2 * hitDirection), -2f, 0, default, 0.9f);
							else
							{
								Dust.NewDust(npc.position, npc.width, npc.height, DustID.Platinum, (float)(2 * hitDirection), -2f, 0, default, 0.9f);
							}
						}
						if(npc.type == ModContent.NPCType<PhaseAssaulterHead>())
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PhaseAssaulter/PhaseAssaulterHeadGore"), 1f);
						if (npc.type == ModContent.NPCType<PhaseAssaulterBody>())
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PhaseAssaulter/PhaseAssaulterBodyGore"), 1f);
						if (npc.type == ModContent.NPCType<PhaseAssaulterTail>())
						{
							PhaseAssaulterTail tail = npc.modNPC as PhaseAssaulterTail;
							if(Main.netMode != NetmodeID.Server)
                            {
								for (int i = 0; i < tail.trailPos.Length; i++)
                                {
									if(Main.rand.NextBool(3))
									{
										Dust.NewDust(tail.trailPos[i] - new Vector2(8, 8), 8, 8, 242, (float)(2 * hitDirection), -2f, 0, default, 2f);
									}
                                }
                            }
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PhaseAssaulter/PhaseAssaulterTailGore"), 1f);
						}
					}
				}
			}
            base.HitEffect(npc, hitDirection, damage);
        }
        public override void SetDefaults(NPC npc)
        {
			if(npc.type == NPCID.BlackRecluse || npc.type == NPCID.WallCreeper || npc.type == NPCID.WallCreeperWall || npc.type == NPCID.BlackRecluseWall || npc.type == NPCID.JungleCreeperWall || npc.type == NPCID.JungleCreeper)
            {
				npc.buffImmune[ModContent.BuffType<WebbedNPC>()] = true;
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			hitBy(npc, player, null, item, ref damage, ref knockback, ref crit);
			base.ModifyHitByItem(npc, player, item, ref damage, ref knockback, ref crit);
		}
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			hitBy(npc, Main.player[Projectile.owner], projectile, null, ref damage, ref knockback, ref crit);
			base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }
		public void hitBy(NPC npc, Player player, Projectile projectile, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (modPlayer.assassinate && !npc.boss)
			{
				npc.AddBuff(ModContent.BuffType<Assassination>(), 30 * modPlayer.assassinateFlat);
				float mult = 1 - modPlayer.assassinateNum;
				int life = npc.life - (damage - (npc.defense + 1) / 2);
				if ((life < npc.lifeMax * mult || life <= modPlayer.assassinateFlat) && npc.HasBuff(ModContent.BuffType<Assassination>()))
				{
					damage += 2 * (life + modPlayer.assassinateFlat + ((npc.defense + 1) / 2));
					//crit = true;
					for (int i = 0; i < 60; i++)
					{
						Vector2 rotate = new Vector2(npc.width / 2 + 4, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 120 + npc.whoAmI * 7 + 120 * i));
						int num1 = Dust.NewDust(new Vector2(npc.Center.X + rotate.X - 4, npc.Center.Y + rotate.Y - 4), 0, 0, 235);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].scale *= 2f;
						Main.dust[num1].velocity *= 1.5f;
					}
				}
			}
		}
		public static List<int> Zombies = null;
		public override void NPCLoot(NPC npc)
		{
			Player player = Main.player[Main.myPlayer];
			if(npc.target <= 255)
			{
				player = Main.player[npc.target];
			}
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			bool ZoneForest = !player.GetModPlayer<SOTSPlayer>().PyramidBiome && !player.ZoneDesert && !player.ZoneCorrupt && !player.ZoneDungeon && !player.ZoneDungeon && !player.ZoneHoly && !player.ZoneMeteor && !player.ZoneJungle && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneGlowshroom && !player.ZoneUndergroundDesert && (player.ZoneDirtLayerHeight || player.ZoneOverworldHeight) && !player.ZoneBeach;
			if(NPCID.Sets.BelongsToInvasionOldOnesArmy[npc.type])
			{
				return;
			}
			if (npc.lifeMax > 5 && !npc.SpawnedFromStatue)
			{
				if (Main.rand.NextBool(100) || (npc.type == NPCID.PigronCorruption || npc.type == NPCID.PigronHallow || npc.type == NPCID.PigronCrimson))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AlmondMilk>(), 1);

				if (Main.rand.NextBool(35))
				{
					//priorities: otherworld > tide > nature > permafrost > earth >  inferno
					//additional: evil & chaos (will not spawn in addition to forest)
					if (player.ZoneSkyHeight || player.ZoneMeteor)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfOtherworld>(), Main.rand.Next(2) + 1);
					else if (player.ZoneBeach || player.ZoneDungeon)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfTide>(), Main.rand.Next(2) + 1);
					else if (ZoneForest || player.ZoneJungle)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfNature>(), Main.rand.Next(2) + 1);
					else if (player.ZoneSnow)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfPermafrost>(), Main.rand.Next(2) + 1);
					else if (player.ZoneUndergroundDesert || player.ZoneDesert || player.GetModPlayer<SOTSPlayer>().PyramidBiome || player.ZoneRockLayerHeight)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfEarth>(), Main.rand.Next(2) + 1);
					else if (player.ZoneUnderworldHeight)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfInferno>(), Main.rand.Next(2) + 1);
						if(SOTSWorld.downedSubspace)
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SanguiteBar>(), Main.rand.Next(2) + 4);
					}
				}
				else if (Main.rand.NextBool(34))
				{
					if (player.ZoneCorrupt || player.ZoneCrimson)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfEvil>(), Main.rand.Next(2) + 1);
					if (player.ZoneHoly)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfChaos>(), Main.rand.Next(2) + 1);
				}

				if (player.ZoneSnow && Main.rand.NextBool(100))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StrawberryIcecream>(), 1);
				if (player.ZoneBeach && Main.rand.NextBool(100))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CoconutMilk>(), 1);
				if (player.ZoneDungeon && Main.rand.NextBool(120))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AvocadoSoup>(), 1);

				if (npc.type == NPCID.ZombieMushroom || npc.type == NPCID.ZombieMushroomHat || npc.type == NPCID.MushiLadybug || npc.type == NPCID.AnomuraFungus || npc.type == NPCID.FungiBulb || npc.type == NPCID.FungoFish || npc.type == NPCID.GiantFungiBulb)
					if (Main.rand.NextBool(10))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CookedMushroom>(), 1);

				if (modPlayer.PlanetariumBiome)
					if (Main.rand.NextBool(100))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DigitalCornSyrup>(), 1);

				if (npc.type == NPCID.WallofFlesh)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<HungryHunter>(), 1);

				if (npc.type == NPCID.WyvernHead)
				{
					if (Main.rand.NextBool(5))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GiantHarpyFeather, 1);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfOtherworld>(), Main.rand.Next(2) + 1);
				}
				if(npc.type == NPCID.VoodooDemon || npc.type == NPCID.BoneSerpentHead)
				{
					if(Main.rand.NextBool(2) || Main.expertMode)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfInferno>(), Main.rand.Next(2) + 1);
				}

				if (npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinWarrior || npc.type == NPCID.GoblinSorcerer)
				{
					if (Main.rand.Next(5) <= 1 && !Main.expertMode)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AncientSteelBar>(), 1);
					if (Main.rand.NextBool(2) && Main.expertMode)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AncientSteelBar>(), 1);
				}

				if (npc.type == NPCID.PirateCaptain || npc.type == NPCID.PirateCorsair || npc.type == NPCID.PirateCrossbower || npc.type == NPCID.PirateDeadeye || npc.type == NPCID.Parrot)
				{
					if (Main.rand.NextBool(10))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Chocolate>(), 1);
				}

				if (npc.type == NPCID.ElfCopter && Main.rand.NextBool(12))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<HelicopterParts>(), 1);

				if (npc.type == NPCID.UndeadMiner && Main.rand.NextBool(50))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ManicMiner>(), 1);

				if (npc.type == NPCID.BlueSlime && Main.rand.NextBool(240))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FireSpitter>(), 1);

				if (npc.type == NPCID.Crab && Main.rand.NextBool(18))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CrabClaw>(), 1);

				if (npc.type == NPCID.PossessedArmor && Main.rand.NextBool(90))
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PossessedHelmet>(), 1);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PossessedChainmail>(), 1);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PossessedGreaves>(), 1);
				}

				if (npc.type == NPCID.PinkJellyfish && Main.rand.NextBool(60))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PinkJellyfishStaff>(), 1);
				if ((npc.type == NPCID.BlueJellyfish || npc.type == NPCID.GreenJellyfish) && Main.rand.NextBool(50))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BlueJellyfishStaff>(), 1);

				if(npc.type == NPCID.QueenBee && (!NPC.downedBoss1 || Main.rand.NextBool(20)))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RoyalJelly>(), 1);
				if (npc.type == ModContent.NPCType<PutridPinkyPhase2>() && (!NPC.downedBoss1 || Main.rand.NextBool(20)))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PeanutButter>(), 1);
				if (npc.type == NPCID.SkeletronHead && (!NPC.downedBoss1 || Main.rand.NextBool(20)))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Baguette>(), 1);
				if (npc.type == NPCID.GreekSkeleton && Main.rand.NextBool(20))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<OlympianAxe>(), 1);
				if(Zombies == null)
                {
					Zombies = new List<int>() { NPCID.Zombie, NPCID.ZombieDoctor, NPCID.ZombieElf, NPCID.ZombieElfBeard,
					NPCID.ZombieElfGirl, NPCID.ZombieEskimo, NPCID.ZombieMushroom, NPCID.ZombieMushroomHat, NPCID.ZombiePixie, NPCID.ZombieRaincoat,
					NPCID.ZombieSuperman, NPCID.ZombieSweater, NPCID.ZombieXmas, NPCID.ArmedZombie, NPCID.ArmedZombieCenx, NPCID.ArmedZombieEskimo,
					NPCID.ArmedZombiePincussion, NPCID.ArmedZombieSlimed, NPCID.ArmedZombieSwamp, NPCID.ArmedZombieTwiggy, NPCID.BaldZombie, NPCID.BigBaldZombie, NPCID.BigFemaleZombie,
					NPCID.BigPincushionZombie, NPCID.BigRainZombie, NPCID.BigSlimedZombie, NPCID.BigSwampZombie, NPCID.BigTwiggyZombie, NPCID.BigZombie,
					NPCID.FemaleZombie, NPCID.PincushionZombie, NPCID.SlimedZombie, NPCID.SmallBaldZombie, NPCID.SmallFemaleZombie, NPCID.SmallPincushionZombie,
					NPCID.SmallRainZombie, NPCID.SmallSlimedZombie, NPCID.SmallSwampZombie, NPCID.SmallTwiggyZombie, NPCID.SmallZombie, NPCID.SwampZombie, NPCID.TwiggyZombie};
				}
				if(Zombies.Contains(npc.type) && Main.rand.NextBool(80))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ZombieHand>(), 1);
				if(npc.boss && !Main.expertMode && (npc.type == NPCID.BrainofCthulhu || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail))
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PyramidKey>(), 1);
				}
				if(npc.type == ModContent.NPCType<NatureConstruct>() || npc.type == ModContent.NPCType<EarthenConstruct>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>() || npc.type == ModContent.NPCType<PermafrostConstruct>() || npc.type == ModContent.NPCType<TidalConstruct>() || npc.type == ModContent.NPCType<EvilConstruct>() || npc.type == ModContent.NPCType<InfernoConstruct>() || npc.type == ModContent.NPCType<ChaosConstruct>())
				{
					if(Main.rand.NextBool(50))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CrushingResistor>(), 1);
					if (Main.rand.NextBool(75))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ElectromagneticDeterrent>(), 1);
					if (Main.rand.NextBool(30) && npc.type != ModContent.NPCType<OtherworldlyConstructHead2>() && npc.type != ModContent.NPCType<OtherworldlyConstructHead>())
					{
						if(npc.type == ModContent.NPCType<NatureConstruct>())
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MantisGrip>(), 1);
						if (npc.type == ModContent.NPCType<EarthenConstruct>())
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Earthshaker>(), 1);
						if (npc.type == ModContent.NPCType<PermafrostConstruct>())
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<EndothermicAfterburner>(), 1);
						if (npc.type == ModContent.NPCType<TidalConstruct>())
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PiscesPuncher>(), 1);
						if (npc.type == ModContent.NPCType<EvilConstruct>())
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DeathSpiral>(), 1);
						if (npc.type == ModContent.NPCType<InfernoConstruct>())
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<IncineratorGlove>(), 1);
						if (npc.type == ModContent.NPCType<ChaosConstruct>())
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ParticleRelocator>(), 1);
					}
					else
					{
						int type = ModContent.ItemType<NaturePlating>();
						int amt = Main.rand.Next(20, 41);
						if (npc.type == ModContent.NPCType<NatureConstruct>())
							type = ModContent.ItemType<NaturePlating>();
						if (npc.type == ModContent.NPCType<EarthenConstruct>())
							type = ModContent.ItemType<EarthenPlating>();
						if (npc.type == ModContent.NPCType<PermafrostConstruct>())
							type = ModContent.ItemType<PermafrostPlating>();
						if (npc.type == ModContent.NPCType<TidalConstruct>())
							type = ModContent.ItemType<TidePlating>();
						if (npc.type == ModContent.NPCType<EvilConstruct>())
							type = ModContent.ItemType<EvilPlating>();
						if (npc.type == ModContent.NPCType<ChaosConstruct>())
							type = ModContent.ItemType<ChaosPlating>();
						if (npc.type == ModContent.NPCType<InfernoConstruct>())
							type = ModContent.ItemType<InfernoPlating>();
						if (npc.type == ModContent.NPCType<OtherworldlyConstructHead2>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead>())
						{
							if (npc.type == ModContent.NPCType<OtherworldlyConstructHead2>())
								amt = Main.rand.Next(5, 11);
							type = ModContent.ItemType<DullPlating>();
						}
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, type, amt);
					}
					if ((npc.type == ModContent.NPCType<OtherworldlyConstructHead>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>()) && Main.rand.NextBool(100))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PhaseCannon>(), 1);
				}
				if(npc.type == ModContent.NPCType<NatureSlime>())
				{
					if (Main.rand.NextBool(50))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BotanicalSymbiote>(), 1);
				}
				if(npc.type == ModContent.NPCType<TwilightScouter>() || npc.type == ModContent.NPCType<PhaseAssaulterHead>())
				{
					if (Main.rand.NextBool(20))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GravityAnchor>(), 1);
				}
				if (npc.type == ModContent.NPCType<TwilightDevil>() || npc.type == ModContent.NPCType<PhaseSpeeder>())
				{
					if (Main.rand.NextBool(20))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TwilightBeads>(), 1);
				}
				if (Main.rand.NextBool(70) && npc.type == ModContent.NPCType<TwilightScouter>())
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ThundershockShortbow>(), 1);
				if (Main.rand.NextBool(70) && npc.type == ModContent.NPCType<TwilightDevil>())
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GravityAnchor>(), 1);
			}
		}
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (player.GetModPlayer<SOTSPlayer>().PhaseBiome) //spawnrates for this biome have to be very high due to how npc spawning in sky height works.
			{
				spawnRate = (int)(spawnRate * 1.0f);
				maxSpawns = (int)(maxSpawns * 0.9f); //slightly less maximum spawns
			}
			if (player.GetModPlayer<SOTSPlayer>().PyramidBiome)
			{
				spawnRate = (int)(spawnRate * 0.175f); //basically setting to 105
				maxSpawns = (int)(maxSpawns * 1.5f);
			}
			if (player.GetModPlayer<SOTSPlayer>().PlanetariumBiome) //spawnrates for this biome have to be very high due to how npc spawning in sky height works.
			{
				spawnRate = (int)(spawnRate * 0.08f); //essentially setting it to 48
				maxSpawns = (int)(maxSpawns * 1.5f);
			}
			if(player.HasBuff(ModContent.BuffType<IntimidatingPresence>()))
            {
				spawnRate = (int)(spawnRate * 10); //makes thing spawn at 1/10th the speed
				maxSpawns = (int)(maxSpawns * 0.5f); //cut max spawns in half
            }
			if (spawnRate < 1)
				spawnRate = 1;
		}
		public static bool CorrectBlockBelowPlanetarium(int i, int j, ref int dist)
		{
			bool flag = false;
			for (int k = 0; k <= dist; k++)
			{
				Tile tile = Framing.GetTileSafely(i, j + k);
				bool correctType = tile.TileType == ModContent.TileType<DullPlatingTile>() || tile.TileType == ModContent.TileType<AvaritianPlatingTile>() || tile.TileType == ModContent.TileType<PortalPlatingTile>();
				if (tile.active() && (Main.tileSolid[tile.TileType] || correctType) && tile.nactive())
				{
					dist = k;
					flag = true;
					break;
				}
			}
			return flag;
		}
		public static int WallType(int type)
        {
			if (type == ModContent.WallType<UnsafePyramidWallWall>() || type == ModContent.WallType<UnsafePyramidBrickWallWall>() || type == ModContent.WallType<TrueSandstoneWallWall>())
				return 1;
			if (type == ModContent.WallType<UnsafeCursedTumorWallWall>())
				return 2;
			return -1;
        }
		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			float constructRateMultiplier = 1f;
			if (SOTSPlayer.ModPlayer(player).noMoreConstructs || player.HasBuff(ModContent.BuffType<IntimidatingPresence>()))
				constructRateMultiplier = 0f;
			bool ZoneForest = SOTSPlayer.ZoneForest(player);
			bool ZonePlanetarium = spawnInfo.player.GetModPlayer<SOTSPlayer>().PlanetariumBiome;
			if (spawnInfo.player.GetModPlayer<SOTSPlayer>().PyramidBiome)
			{
				int tileWall = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY - 1].wall;
				bool isValidTile = spawnInfo.spawnTileType == ModContent.TileType<PyramidSlabTile>() || spawnInfo.spawnTileType == ModContent.TileType<PyramidBrickTile>() || spawnInfo.spawnTileType == ModContent.TileType<TrueSandstoneTile>();
				bool isValidWall = WallType(tileWall) == 1;
				bool isCurseValid = spawnInfo.spawnTileType == ModContent.TileType<CursedTumorTile>() || spawnInfo.spawnTileType == ModContent.TileType<CursedHive>()
					|| WallType(tileWall) == 2;
				if (isValidTile || (isValidWall && !isCurseValid))
				{
					pool.Clear();
					pool.Add(ModContent.NPCType<SnakePot>(), 0.35f);
					pool.Add(ModContent.NPCType<Snake>(), 1f);
					pool.Add(ModContent.NPCType<LostSoul>(), 0.6f);
					pool.Add(ModContent.NPCType<PyramidTreasureSlime>(), 0.02f);
					pool.Add(NPCID.SandSlime, 0.35f);
					if(Main.hardMode)
                    {
						pool.Add(NPCID.Mummy, 0.5f);
					}
				}
				else if(isCurseValid)
				{
					pool.Clear();
					if (Main.hardMode)
					{
						pool.Add(ModContent.NPCType<BleedingGhast>(), 0.1f);
						pool.Add(ModContent.NPCType<FlamingGhast>(), 0.1f);
						pool.Add(ModContent.NPCType<Ghast>(), 0.1f);
					}
					else
					{
						pool.Add(ModContent.NPCType<Ghast>(), 0.25f);
					}
					pool.Add(ModContent.NPCType<LostSoul>(), 0.1f);
					pool.Add(ModContent.NPCType<PyramidTreasureSlime>(), 0.02f);
					pool.Add(ModContent.NPCType<Teratoma>(), 0.25f);
					pool.Add(ModContent.NPCType<Maligmor>(), 0.15f);
				}
			}
			else if (ZonePlanetarium)
			{
				pool.Clear();
				int distanceDown = 6;
				bool correctBlock = CorrectBlockBelowPlanetarium(spawnInfo.spawnTileX, spawnInfo.spawnTileY, ref distanceDown);
				if (correctBlock)
				{
					pool.Add(ModContent.NPCType<HoloSlime>(), 0.4f);
					pool.Add(ModContent.NPCType<HoloEye>(), 0.1f);
					pool.Add(ModContent.NPCType<HoloBlade>(), 0.175f);
					pool.Add(ModContent.NPCType<TwilightDevil>(), 0.04f);
					pool.Add(ModContent.NPCType<OtherworldlyConstructHead>(), 0.02f * constructRateMultiplier);
					pool.Add(ModContent.NPCType<TwilightScouter>(), 0.01f);
				}
			}
			else if (ZoneForest)
			{
				if (SOTSWorld.downedPinky)
				{
					pool.Add(ModContent.NPCType<FluxSlime>(), SpawnCondition.OverworldDaySlime.Chance * 0.05f);
					pool.Add(ModContent.NPCType<NatureSlime>(), SpawnCondition.OverworldDaySlime.Chance * 0.10f);
				}
				else
				{
					pool.Add(ModContent.NPCType<NatureSlime>(), SpawnCondition.OverworldDaySlime.Chance * 0.15f);
				}
				if(NPC.downedSlimeKing)
					pool.Add(ModContent.NPCType <BlueSlimer>(), SpawnCondition.OverworldDaySlime.Chance * 0.1f);
				pool.Add(ModContent.NPCType<BasicTreasureSlime>(), SpawnCondition.OverworldDaySlime.Chance * 0.03f);
				if (player.statLifeMax2 >= 120)
				{
					float overworldChance = 0.01f;
					if (Main.bloodMoon)
						overworldChance = 0.005f;
					pool.Add(ModContent.NPCType<NatureConstruct>(), SpawnCondition.Overworld.Chance * overworldChance * constructRateMultiplier);
				}
			}
			else if (player.ZoneCorrupt || player.ZoneCrimson)
			{
				if (SOTSWorld.downedPinky)
				{
					pool.Add(ModContent.NPCType<FluxSlime>(), 0.075f);
				}
				if(player.ZoneCrimson)
					pool.Add(ModContent.NPCType<CrimsonTreasureSlime>(), 0.05f);
				if (player.ZoneCorrupt)
					pool.Add(ModContent.NPCType<CorruptionTreasureSlime>(), 0.05f);
			}
			else if (player.ZoneHoly && Main.hardMode)
			{
				float rateMult = 1f;
				if (!Main.dayTime)
					rateMult = 3f;
				if(player.ZoneOverworldHeight)
					pool.Add(ModContent.NPCType<ChaosConstruct>(), 0.006f * constructRateMultiplier * rateMult);
				pool.Add(ModContent.NPCType<HallowTreasureSlime>(), 0.0075f);
			}
			if (player.ZoneBeach && !spawnInfo.player.ZonePeaceCandle) //guarenteed to not spawn when a peace candle is nearby
			{
				if (player.statLifeMax2 >= 120)
				{
					if (NPC.downedBoss1)
					{
						if (NPC.downedBoss3)
						{
							pool.Add(ModContent.NPCType<TidalConstruct>(), SpawnCondition.OceanMonster.Chance * 0.025f * constructRateMultiplier);
						}
						else
						{
							pool.Add(ModContent.NPCType<TidalConstruct>(), SpawnCondition.OceanMonster.Chance * 0.015f * constructRateMultiplier);
						}
					}
					else
						pool.Add(ModContent.NPCType<TidalConstruct>(), SpawnCondition.OceanMonster.Chance * 0.005f * constructRateMultiplier);
				}
			}
			else if (!player.ZoneBeach)
			{
				if (player.ZoneDesert || player.ZoneUndergroundDesert || (player.ZoneRockLayerHeight && !player.ZoneDungeon && !player.ZoneJungle && !player.ZoneSnow))
				{
					if(player.statLifeMax2 >= 120)
                    {
						if (player.ZoneCorrupt || player.ZoneHoly || player.ZoneCrimson)
						{
							if (player.statLifeMax2 >= 140)
								pool.Add(ModContent.NPCType<EarthenConstruct>(), 0.00125f * constructRateMultiplier);
						}
						else if (player.ZoneRockLayerHeight && !player.ZoneUndergroundDesert)
						{
							if(player.statLifeMax2 >= 140)
								pool.Add(ModContent.NPCType<EarthenConstruct>(), 0.0025f * constructRateMultiplier);
						}
						else if(player.ZoneDesert && !player.ZoneUndergroundDesert)
							pool.Add(ModContent.NPCType<EarthenConstruct>(), 0.01f * constructRateMultiplier); //this is desert spawn so it shouldn't require additional healthgating
						else if(player.ZoneUndergroundDesert)
							pool.Add(ModContent.NPCType<EarthenConstruct>(), 0.002f * constructRateMultiplier);

					}
				}
			}
			if((player.ZoneCrimson || player.ZoneCorrupt) && (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight) && Main.hardMode)
			{
				float rateMult = 1f;
				if (Main.dayTime)
					rateMult = 1.5f;
				pool.Add(ModContent.NPCType<EvilConstruct>(), 0.006f * constructRateMultiplier * rateMult);
			}
			if (player.ZoneDungeon)
			{
				pool.Add(ModContent.NPCType<DungeonTreasureSlime>(), SpawnCondition.DungeonNormal.Chance * 0.015f); //this is about 75% of dungeon slime spawn rate
			}
			if (spawnInfo.player.ZoneSnow)
			{
				if(!spawnInfo.player.ZoneCorrupt && !spawnInfo.player.ZoneCrimson && (NPC.downedBoss1 || NPC.downedGoblins) && spawnInfo.player.ZoneOverworldHeight)
					pool.Add(ModContent.NPCType<ArcticGoblin>(), SpawnCondition.Overworld.Chance * 0.1f);
				if (player.statLifeMax2 >= 120)
					pool.Add(ModContent.NPCType<PermafrostConstruct>(), (spawnInfo.spawnTileType == TileID.IceBlock || spawnInfo.spawnTileType == TileID.SnowBlock ? 0.02f : 0.015f) * constructRateMultiplier);
				if (spawnInfo.spawnTileY <= Main.rockLayer && spawnInfo.spawnTileY >= Main.worldSurface)
				{
					pool.Add(ModContent.NPCType<IceTreasureSlime>(), spawnInfo.spawnTileType == TileID.IceBlock || spawnInfo.spawnTileType == TileID.SnowBlock ? 0.03f : 0.01f);
				}
				else if (spawnInfo.spawnTileY <= Main.maxTilesY - 200 && spawnInfo.spawnTileY >= Main.rockLayer)
				{
					pool.Add(ModContent.NPCType<IceTreasureSlime>(), spawnInfo.spawnTileType == TileID.IceBlock || spawnInfo.spawnTileType == TileID.SnowBlock ? 0.03f : 0.01f);
				}
			}
			else if(Main.invasionType == InvasionID.GoblinArmy && spawnInfo.player.ZoneOverworldHeight && spawnInfo.player.ZoneSnow && !spawnInfo.player.ZoneCorrupt && !spawnInfo.player.ZoneCrimson)
			{
				pool.Add(ModContent.NPCType<ArcticGoblin>(), 0.1f);
			}
			else if(player.ZoneJungle)
			{
				if (player.statLifeMax2 >= 120)
					pool.Add(ModContent.NPCType<NatureConstruct>(), ((SpawnCondition.SurfaceJungle.Chance * 0.03f) + (SpawnCondition.UndergroundJungle.Chance * 0.0075f)) * constructRateMultiplier);
				pool.Add(ModContent.NPCType<JungleTreasureSlime>(), (SpawnCondition.SurfaceJungle.Chance * 0.05f) + (SpawnCondition.UndergroundJungle.Chance * 0.015f));
			}
			if (spawnInfo.player.ZoneUnderworldHeight)
			{
				pool.Add(ModContent.NPCType<LesserWisp>(), SpawnCondition.Underworld.Chance * 0.07f);
				if(NPC.downedBoss3)
					pool.Add(ModContent.NPCType<ShadowTreasureSlime>(), SpawnCondition.Underworld.Chance * 0.02f);
				if(Main.hardMode)
					pool.Add(ModContent.NPCType<InfernoConstruct>(), SpawnCondition.Underworld.Chance * 0.015f * constructRateMultiplier);
			}
			if (spawnInfo.spawnTileY <= Main.rockLayer)
			{
				pool.Add(ModContent.NPCType<GoldenTreasureSlime>(), SpawnCondition.Underground.Chance * 0.02f);
			}
			else if (spawnInfo.spawnTileY <= Main.maxTilesY - 200)
			{
				pool.Add(ModContent.NPCType<GoldenTreasureSlime>(), SpawnCondition.Underground.Chance * 0.03f);
			}
			if (spawnInfo.player.ZoneSkyHeight)
			{
				if (spawnInfo.player.GetModPlayer<SOTSPlayer>().PhaseBiome)
				{
					if (NPC.CountNPCS(ModContent.NPCType<PhaseSpeeder>()) < 2) //only two speeders max
						pool.Add(ModContent.NPCType<PhaseSpeeder>(), SpawnCondition.Sky.Chance * 10f);
					if (NPC.CountNPCS(ModContent.NPCType<PhaseAssaulterHead>()) < 1) //only one assaulter max
						pool.Add(ModContent.NPCType<PhaseAssaulterHead>(), SpawnCondition.Sky.Chance * 5f);
				}
				if(!ZonePlanetarium)
					pool.Add(ModContent.NPCType<TwilightScouter>(), SpawnCondition.Sky.Chance * 0.4f);
			}
		}
	}
}