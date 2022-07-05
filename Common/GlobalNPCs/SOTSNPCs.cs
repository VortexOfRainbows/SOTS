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
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using System.Linq;
using SOTS.NPCs;
using SOTS.Items.Otherworld.FromChests;
using Terraria.GameContent.Bestiary;
using SOTS.Biomes;

namespace SOTS.Common.GlobalNPCs
{
    public class SOTSNPCs : GlobalNPC
	{
		/// <summary>
		/// Gets the damage of a NPC prior Expert, Master, and Journey modifiers
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
			SetDebuffImmunities(npc);
			return base.PreAI(npc);
        }
        public override void HitEffect(NPC npc, int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (isPolarisNPC(npc.type))
			{
				if (npc.life <= 0)
				{
					for (int i = 0; i < 3; i++)
					{
						Gore.NewGore(npc.GetSource_Death(), npc.position + new Vector2((float)(npc.width * Main.rand.Next(100)) / 100f, (float)(npc.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, npc.velocity * 0.5f, Main.rand.Next(61, 64), 1f);
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
							Gore.NewGore(npc.GetSource_Death(), npc.position, npc.velocity, ModGores.GoreType("Gores/PhaseAssaulter/PhaseAssaulterHeadGore"), 1f);
						if (npc.type == ModContent.NPCType<PhaseAssaulterBody>())
							Gore.NewGore(npc.GetSource_Death(), npc.position, npc.velocity, ModGores.GoreType("Gores/PhaseAssaulter/PhaseAssaulterBodyGore"), 1f);
						if (npc.type == ModContent.NPCType<PhaseAssaulterTail>())
						{
							PhaseAssaulterTail tail = npc.ModNPC as PhaseAssaulterTail;
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
							Gore.NewGore(npc.GetSource_Death(), npc.position, npc.velocity, ModGores.GoreType("Gores/PhaseAssaulter/PhaseAssaulterTailGore"), 1f);
						}
					}
				}
			}
            base.HitEffect(npc, hitDirection, damage);
        }
        public void SetDebuffImmunities(NPC npc)
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
			hitBy(npc, Main.player[projectile.owner], projectile, null, ref damage, ref knockback, ref crit);
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
        public override void ModifyGlobalLoot(GlobalLoot globalLoot) //global rules, such as fragments, soulds, ectoplasm
        {
			globalLoot.Add(ItemDropRule.Common(ModContent.ItemType<AlmondMilk>(), 100, 1, 1));
			globalLoot.Add(ItemDropRule.ByCondition(new ItemDropConditions.PermafrostFragmentDropCondition(), ModContent.ItemType<StrawberryIcecream>(), 100, 1, 1));
			globalLoot.Add(ItemDropRule.ByCondition(new ItemDropConditions.BeachDropCondition(), ModContent.ItemType<CoconutMilk>(), 100, 1, 1));
			globalLoot.Add(ItemDropRule.ByCondition(new ItemDropConditions.PermafrostFragmentDropCondition(), ModContent.ItemType<AvocadoSoup>(), 120, 1, 1));
			globalLoot.Add(ItemDropRule.ByCondition(new ItemDropConditions.PlanetariumDropCondition(), ModContent.ItemType<DigitalCornSyrup>(), 100, 1, 1));

			LeadingConditionRule otherworld = new LeadingConditionRule(new ItemDropConditions.OtherworldFragmentDropCondition());
			LeadingConditionRule tide = new LeadingConditionRule(new ItemDropConditions.TideFragmentDropCondition());
			LeadingConditionRule nature = new LeadingConditionRule(new ItemDropConditions.NatureFragmentDropCondition());
			LeadingConditionRule permafrost = new LeadingConditionRule(new ItemDropConditions.PermafrostFragmentDropCondition());
			LeadingConditionRule earth = new LeadingConditionRule(new ItemDropConditions.EarthFragmentDropCondition());
			LeadingConditionRule inferno = new LeadingConditionRule(new ItemDropConditions.InfernoFragmentDropCondition());
			LeadingConditionRule evil = new LeadingConditionRule(new ItemDropConditions.EvilFragmentDropCondition());
			LeadingConditionRule chaos = new LeadingConditionRule(new ItemDropConditions.ChaosFragmentDropCondition());

			otherworld.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FragmentOfOtherworld>(), 35, 1, 2)).OnFailedConditions(
				tide.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FragmentOfTide>(), 35, 1, 2)).OnFailedConditions(
					nature.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FragmentOfNature>(), 35, 1, 2)).OnFailedConditions(
						permafrost.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FragmentOfPermafrost>(), 35, 1, 2)).OnFailedConditions(
							earth.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FragmentOfEarth>(), 35, 1, 2)).OnFailedConditions(
								inferno.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FragmentOfInferno>(), 35, 1, 2)))))));
			evil.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FragmentOfEvil>(), 35, 1, 2)).OnFailedConditions(chaos.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FragmentOfChaos>(), 35, 1, 2)));
			globalLoot.Add(ItemDropRule.ByCondition(new ItemDropConditions.DownedSubspaceDropCondition(), ModContent.ItemType<SanguiteBar>(), 35, 4, 5, 1));
			globalLoot.Add(otherworld);
			globalLoot.Add(tide);
			globalLoot.Add(nature);
			globalLoot.Add(permafrost);
			globalLoot.Add(earth);
			globalLoot.Add(inferno);
			globalLoot.Add(evil);
			globalLoot.Add(chaos);
		}
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) //modify loot and vanilla loot
		{
			LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
			if (npc.type == NPCID.PigronCorruption || npc.type == NPCID.PigronHallow || npc.type == NPCID.PigronCrimson) //if npc is pigron
            {
				npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<AlmondMilk>(), 2, 1)); //guaranteed in expert, 50% in normal
			}
			if (npc.type == NPCID.WyvernHead)
			{
				npcLoot.Add(ItemDropRule.Common(ItemID.GiantHarpyFeather, 5, 1, 1));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfOtherworld>(), 1, 1, 2));
			}
			if (npc.type == NPCID.WallofFlesh)
			{
				notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HungryHunter>(), 1, 1, 1));
				npcLoot.Add(notExpert);
			}
			if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
			{
				LeadingConditionRule leadingConditionRule = new(new Conditions.LegacyHack_IsABoss());
				leadingConditionRule.OnSuccess(notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PyramidKey>())));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.BrainofCthulhu)
			{
				notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PyramidKey>(), 1, 1, 1));
				npcLoot.Add(notExpert);
			}
			/*if (npc.type == NPCID.PossessedArmor)
			{
				npcLoot.Add(ItemDropRule.OneFromOptions(30, new int[] { ModContent.ItemType<PossessedHelmet>(), ModContent.ItemType<PossessedChainmail>(), ModContent.ItemType<PossessedGreaves>() }));
			}*/
			if (npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinWarrior || npc.type == NPCID.GoblinSorcerer) //40% in normal, 50% in expert
			{
				notExpert.OnSuccess(new CommonDrop(ModContent.ItemType<AncientSteelBar>(), chanceDenominator: 5, chanceNumerator: 2))
					.OnFailedConditions(new CommonDrop(ModContent.ItemType<AncientSteelBar>(), chanceDenominator: 2));
				npcLoot.Add(notExpert);
			}
			if (npc.type == ModContent.NPCType<NatureSlime>())
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BotanicalSymbiote>(), 50, 1, 1));
			}
			if (npc.type == ModContent.NPCType<TwilightScouter>() || npc.type == ModContent.NPCType<PhaseAssaulterHead>())
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GravityAnchor>(), 20, 1, 1));
			}
			if (npc.type == ModContent.NPCType<TwilightDevil>() || npc.type == ModContent.NPCType<PhaseSpeeder>())
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TwilightBeads>(), 20, 1, 1));
			}
			if (npc.type == ModContent.NPCType<TwilightScouter>())
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ThundershockShortbow>(), 50, 1, 1));
			if (npc.type == ModContent.NPCType<TwilightDevil>())
				npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<GravityAnchor>(), 70, 50));
			if (npc.type == ModContent.NPCType<NatureConstruct>() || npc.type == ModContent.NPCType<EarthenConstruct>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>() || npc.type == ModContent.NPCType<PermafrostConstruct>() || npc.type == ModContent.NPCType<TidalConstruct>() || npc.type == ModContent.NPCType<EvilConstruct>() || npc.type == ModContent.NPCType<InfernoConstruct>() || npc.type == ModContent.NPCType<ChaosConstruct>())
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrushingResistor>(), 50));
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ElectromagneticDeterrent>(), 75));
				if (npc.type != ModContent.NPCType<OtherworldlyConstructHead2>() && npc.type != ModContent.NPCType<OtherworldlyConstructHead>())
				{
					if (npc.type == ModContent.NPCType<NatureConstruct>())
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MantisGrip>(), 30));
					if (npc.type == ModContent.NPCType<EarthenConstruct>())
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Earthshaker>(), 30));
					if (npc.type == ModContent.NPCType<PermafrostConstruct>())
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EndothermicAfterburner>(), 30));
					if (npc.type == ModContent.NPCType<TidalConstruct>())
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PiscesPuncher>(), 30));
					if (npc.type == ModContent.NPCType<EvilConstruct>())
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DeathSpiral>(), 30));
					if (npc.type == ModContent.NPCType<InfernoConstruct>())
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IncineratorGlove>(), 30));
					if (npc.type == ModContent.NPCType<ChaosConstruct>())
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ParticleRelocator>(), 30));
				}
				else
				{
					int type = ModContent.ItemType<NaturePlating>();
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
					if (npc.type == ModContent.NPCType<OtherworldlyConstructHead>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>())
					{
						type = ModContent.ItemType<DullPlating>();
						npcLoot.Add(ItemDropRule.Common(type, 1, 5, 10));
					}
					else
						npcLoot.Add(ItemDropRule.Common(type, 1, 20, 40));
				}
				if (npc.type == ModContent.NPCType<OtherworldlyConstructHead>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>())
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhaseCannon>(), 100));
			}
			if (npc.type == NPCID.VoodooDemon || npc.type == NPCID.BoneSerpentHead)
			{
				npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<FragmentOfInferno>(), 2, 1)); //guaranteed in expert, 50% in normal
			}
			if (npc.type == NPCID.ZombieMushroom || npc.type == NPCID.ZombieMushroomHat || npc.type == NPCID.MushiLadybug || npc.type == NPCID.AnomuraFungus || npc.type == NPCID.FungiBulb || npc.type == NPCID.FungoFish || npc.type == NPCID.GiantFungiBulb)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CookedMushroom>(), 10));
			if (npc.type == NPCID.PirateCaptain || npc.type == NPCID.PirateCorsair || npc.type == NPCID.PirateCrossbower || npc.type == NPCID.PirateDeadeye || npc.type == NPCID.Parrot)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Chocolate>(), 10));
			if (npc.type == NPCID.PinkJellyfish)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PinkJellyfishStaff>(), 60));
			if (npc.type == NPCID.BlueJellyfish || npc.type == NPCID.GreenJellyfish)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlueJellyfishStaff>(), 50));
			if (npc.type == NPCID.ElfCopter)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HelicopterParts>(), 12));
			if (npc.type == NPCID.UndeadMiner)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ManicMiner>(), 50));
			if (npc.type == NPCID.BlueSlime)
				npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<FireSpitter>(), 240, 200));
			if (npc.type == NPCID.Crab)
				npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<CrabClaw>(), 20, 18));
			if (npc.type == NPCID.GreekSkeleton)
				npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<OlympianAxe>(), 25, 20));
			if (DebuffNPC.Zombies.Contains(npc.type))
				npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<ZombieHand>(), 100, 80));
			LeadingConditionRule preEoC = new LeadingConditionRule(new ItemDropConditions.PreBoss1DropCondition());
			LeadingConditionRule postEoC = new LeadingConditionRule(new ItemDropConditions.PostBoss1DropCondition());
			if (npc.type == NPCID.QueenBee)
            {
				preEoC.OnSuccess(ItemDropRule.Common(ModContent.ItemType<RoyalJelly>(), 1));
				postEoC.OnSuccess(ItemDropRule.Common(ModContent.ItemType<RoyalJelly>(), 20));
				npcLoot.Add(postEoC);
				npcLoot.Add(preEoC);
			}
			if (npc.type == ModContent.NPCType<PutridPinkyPhase2>())
			{
				preEoC.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PeanutButter>(), 1));
				postEoC.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PeanutButter>(), 20));
				npcLoot.Add(postEoC);
				npcLoot.Add(preEoC);
			}
			if (npc.type == NPCID.SkeletronHead)
			{
				preEoC.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Baguette>(), 1));
				postEoC.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Baguette>(), 20));
				npcLoot.Add(postEoC);
				npcLoot.Add(preEoC);
			}
			LeadingConditionRule postAdvisor = new LeadingConditionRule(new ItemDropConditions.DownedAdvisorDropCondition());
			if (npc.type == ModContent.NPCType<TwilightScouter>() || npc.type == ModContent.NPCType<TwilightDevil>())
			{
				postAdvisor.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TwilightShard>(), 3));
				npcLoot.Add(postAdvisor);
			}
			LeadingConditionRule prePharaoh = new LeadingConditionRule(new ItemDropConditions.PreCurseDropCondition());
			LeadingConditionRule postPharaoh = new LeadingConditionRule(new ItemDropConditions.DownedCurseDropCondition());
			if (npc.type == ModContent.NPCType<Teratoma>() || npc.type == ModContent.NPCType<Maligmor>() || npc.type == ModContent.NPCType<Ghast>())
			{
				IItemDropRule alternative = ItemDropRule.Common(ModContent.ItemType<SoulResidue>(), 1, 1, 2);
				postPharaoh.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CursedMatter>(), 3, 1, 2)).OnFailedRoll(alternative);
				prePharaoh.OnSuccess(alternative);
				npcLoot.Add(postPharaoh);
				npcLoot.Add(prePharaoh);
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
				if (tile.HasTile && (Main.tileSolid[tile.TileType] || correctType) && tile.HasUnactuatedTile)
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
			Player player = spawnInfo.Player;
			float constructRateMultiplier = 1f;
			if (SOTSPlayer.ModPlayer(player).noMoreConstructs || player.HasBuff(ModContent.BuffType<IntimidatingPresence>()))
				constructRateMultiplier = 0f;
			bool ZoneForest = SOTSPlayer.ZoneForest(player);
			bool ZonePlanetarium = spawnInfo.Player.GetModPlayer<SOTSPlayer>().PlanetariumBiome;
			if (spawnInfo.Player.GetModPlayer<SOTSPlayer>().PyramidBiome)
			{
				int tileWall = Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY - 1].WallType;
				bool isValidTile = spawnInfo.SpawnTileType == ModContent.TileType<PyramidSlabTile>() || spawnInfo.SpawnTileType == ModContent.TileType<PyramidBrickTile>() || spawnInfo.SpawnTileType == ModContent.TileType<TrueSandstoneTile>();
				bool isValidWall = WallType(tileWall) == 1;
				bool isCurseValid = spawnInfo.SpawnTileType == ModContent.TileType<CursedTumorTile>() || spawnInfo.SpawnTileType == ModContent.TileType<CursedHive>()
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
				bool correctBlock = CorrectBlockBelowPlanetarium(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY, ref distanceDown);
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
			else if (player.ZoneHallow && Main.hardMode)
			{
				float rateMult = 1f;
				if (!Main.dayTime)
					rateMult = 3f;
				if(player.ZoneOverworldHeight)
					pool.Add(ModContent.NPCType<ChaosConstruct>(), 0.006f * constructRateMultiplier * rateMult);
				pool.Add(ModContent.NPCType<HallowTreasureSlime>(), 0.0075f);
			}
			if (player.ZoneBeach && !spawnInfo.Player.ZonePeaceCandle) //guarenteed to not spawn when a peace candle is nearby
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
						if (player.ZoneCorrupt || player.ZoneHallow || player.ZoneCrimson)
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
			if (spawnInfo.Player.ZoneSnow)
			{
				if(!spawnInfo.Player.ZoneCorrupt && !spawnInfo.Player.ZoneCrimson && (NPC.downedBoss1 || NPC.downedGoblins) && spawnInfo.Player.ZoneOverworldHeight)
					pool.Add(ModContent.NPCType<ArcticGoblin>(), SpawnCondition.Overworld.Chance * 0.1f);
				if (player.statLifeMax2 >= 120)
					pool.Add(ModContent.NPCType<PermafrostConstruct>(), (spawnInfo.SpawnTileType == TileID.IceBlock || spawnInfo.SpawnTileType == TileID.SnowBlock ? 0.02f : 0.015f) * constructRateMultiplier);
				if (spawnInfo.SpawnTileY <= Main.rockLayer && spawnInfo.SpawnTileY >= Main.worldSurface)
				{
					pool.Add(ModContent.NPCType<IceTreasureSlime>(), spawnInfo.SpawnTileType == TileID.IceBlock || spawnInfo.SpawnTileType == TileID.SnowBlock ? 0.03f : 0.01f);
				}
				else if (spawnInfo.SpawnTileY <= Main.maxTilesY - 200 && spawnInfo.SpawnTileY >= Main.rockLayer)
				{
					pool.Add(ModContent.NPCType<IceTreasureSlime>(), spawnInfo.SpawnTileType == TileID.IceBlock || spawnInfo.SpawnTileType == TileID.SnowBlock ? 0.03f : 0.01f);
				}
			}
			else if(Main.invasionType == InvasionID.GoblinArmy && spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.ZoneSnow && !spawnInfo.Player.ZoneCorrupt && !spawnInfo.Player.ZoneCrimson)
			{
				pool.Add(ModContent.NPCType<ArcticGoblin>(), 0.1f);
			}
			else if(player.ZoneJungle)
			{
				if (player.statLifeMax2 >= 120)
					pool.Add(ModContent.NPCType<NatureConstruct>(), ((SpawnCondition.SurfaceJungle.Chance * 0.03f) + (SpawnCondition.UndergroundJungle.Chance * 0.0075f)) * constructRateMultiplier);
				pool.Add(ModContent.NPCType<JungleTreasureSlime>(), (SpawnCondition.SurfaceJungle.Chance * 0.05f) + (SpawnCondition.UndergroundJungle.Chance * 0.015f));
			}
			if (spawnInfo.Player.ZoneUnderworldHeight)
			{
				pool.Add(ModContent.NPCType<LesserWisp>(), SpawnCondition.Underworld.Chance * 0.07f);
				if(NPC.downedBoss3)
					pool.Add(ModContent.NPCType<ShadowTreasureSlime>(), SpawnCondition.Underworld.Chance * 0.02f);
				if(Main.hardMode)
					pool.Add(ModContent.NPCType<InfernoConstruct>(), SpawnCondition.Underworld.Chance * 0.015f * constructRateMultiplier);
			}
			if (spawnInfo.SpawnTileY <= Main.rockLayer)
			{
				pool.Add(ModContent.NPCType<GoldenTreasureSlime>(), SpawnCondition.Underground.Chance * 0.02f);
			}
			else if (spawnInfo.SpawnTileY <= Main.maxTilesY - 200)
			{
				pool.Add(ModContent.NPCType<GoldenTreasureSlime>(), SpawnCondition.Underground.Chance * 0.03f);
			}
			if (spawnInfo.Player.ZoneSkyHeight)
			{
				if (spawnInfo.Player.GetModPlayer<SOTSPlayer>().PhaseBiome)
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
        public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			SpawnConditionBestiaryInfoElement sky = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky;
			ModBiomeBestiaryInfoElement Planetarium = ModContent.GetInstance<PlanetariumBiome>().ModBiomeBestiaryInfoElement;
			ModBiomeBestiaryInfoElement Pyramid = ModContent.GetInstance<PyramidBiome>().ModBiomeBestiaryInfoElement;
			if (npc.type == ModContent.NPCType<HoloSlime>() || npc.type == ModContent.NPCType<HoloBlade>() || npc.type == ModContent.NPCType<HoloEye>() || npc.type == ModContent.NPCType<TwilightDevil>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>())
			{
				FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("A holographic mimic of an average slime. Its source of energy is unknown.");

				if(npc.type == ModContent.NPCType<HoloBlade>())
					flavorText = new FlavorTextBestiaryInfoElement("A holographic mimic of a sword. Its source of energy is unknown.");
				if (npc.type == ModContent.NPCType<HoloEye>())
					flavorText = new FlavorTextBestiaryInfoElement("A holographic mimic of an eyeball. Its source of energy is unknown.");

				if (npc.type == ModContent.NPCType<TwilightDevil>())
					flavorText = new FlavorTextBestiaryInfoElement("A mindless drone that protects the Planetarium. It seems to be controlled centrally, as nothing hides behind its cloak.");
				if (npc.type == ModContent.NPCType<OtherworldlyConstructHead>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>())
					flavorText = new FlavorTextBestiaryInfoElement("Like all constructs, its body serves as a container for the volatile spirit inside. As Otherworld Spirits are relatively docile, Otherworld Constructs have among the simplest internals.");

				bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
					Planetarium,
					flavorText
				});
			}
			if (npc.type == ModContent.NPCType<PhaseSpeeder>() || npc.type == ModContent.NPCType<PhaseAssaulterHead>() || npc.type == ModContent.NPCType<TwilightScouter>())
			{
				FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("It's fast. It's deadly. It's origin is unknown, but it certainly wants you dead. Uses the fabric of existence as its shield.");
				if (npc.type == ModContent.NPCType<TwilightScouter>())
					flavorText = new FlavorTextBestiaryInfoElement("A mindless drone that collects data for the Planetarium. Its internal circuitry is not particularly complex.");
				if (npc.type == ModContent.NPCType<PhaseAssaulterHead>())
					flavorText = new FlavorTextBestiaryInfoElement("Phase Assaulters make up the defensive core of Phase Ore nodes. They can be observed acting as shields for Phase Speeders, while also possessing a potent hyperlight laser.");
				bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
					sky,
					flavorText
				});
			}
			if (npc.type == ModContent.NPCType<LostSoul>() || npc.type == ModContent.NPCType<Snake>() || npc.type == ModContent.NPCType<SnakePot>() || npc.type == ModContent.NPCType<WallMimic>())
			{
				FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("A trapped soul that resides in the Pyramid. It has not come to rest, as it still seems to be searching for something.");
				if (npc.type == ModContent.NPCType<Snake>())
					flavorText = new FlavorTextBestiaryInfoElement("A long limbless reptile which has no eyelids, a short tail, and jaws that are capable of considerable extension. This particular one has a venomous bite, and it calls the Pyramid home.");
				if (npc.type == ModContent.NPCType<SnakePot>())
					flavorText = new FlavorTextBestiaryInfoElement("A group of snakes that uses a pot for defense. When the pot breaks, the snakes inside are forced to scatter.");
				if (npc.type == ModContent.NPCType<WallMimic>())
					flavorText = new FlavorTextBestiaryInfoElement("The curse housed within the Pyramid is more capable of corruption than the world evils. Under its influence, even the walls of the Pyramid turn into monsters!");
				bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
					Pyramid,
					flavorText
				});
			}
		}
    }
}