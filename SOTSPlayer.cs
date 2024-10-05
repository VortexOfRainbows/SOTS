using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Items;
using SOTS.Items.Permafrost;
using SOTS.Items.Planetarium;
using SOTS.Items.Wings;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Pyramid;
using SOTS.Items.Earth;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.BiomeChest;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Base;
using SOTS.Projectiles.Planetarium;
using SOTS.Void;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.NPCs.Boss.Curse;
using SOTS.Projectiles.Pyramid;
using SOTS.Projectiles.Minions;
using SOTS.Projectiles.Permafrost;
using SOTS.Items.Celestial;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Inferno;
using SOTS.Projectiles.Nature;
using SOTS.Items.Crushers;
using SOTS.Dusts;
using SOTS.Projectiles.Evil;
using SOTS.Projectiles;
using SOTS.Projectiles.Tide;
using SOTS.Items.Fishing;
using SOTS.Items.Tide;
using Terraria.ModLoader.IO;
using SOTS.Items.AbandonedVillage;
using SOTS.Projectiles.Chaos;
using SOTS.NPCs.Boss.Lux;
using SOTS.Items.Tools;
using SOTS.Common.GlobalNPCs;
using SOTS.Buffs.DilationSickness;
using Terraria.Audio;
using static SOTS.SOTS;
using SOTS.Items.Invidia;
using SOTS.Projectiles.Lightning;
using SOTS.Projectiles.Camera;
using Terraria.Localization;
using SOTS.Projectiles.Pyramid.GhostPepper;
using SOTS.Common.Systems;
using SOTS.FakePlayer;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using SOTS.Buffs.ConduitBoosts;
using SOTS.Items.Chaos;
using SOTS.Buffs.Debuffs;
using System.Security.Permissions;
using Terraria.Chat;
using SOTS.Helpers;

namespace SOTS
{
	public class SOTSPlayer : ModPlayer
	{
		private int LogInMessageTimer = 7;
		public override void SetControls()
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				if (LogInMessageTimer > 0)
					LogInMessageTimer -= 1; 
				if (LogInMessageTimer == 0)
				{
					SOTSTexturePackEnabled = IsSOTSTexturePackEnabled();
					if (SOTS.SOTSTexturePackEnabled)
					{
						LogInMessageTimer = -1;
						Main.NewText(Language.GetTextValue("Mods.SOTS.Common.worldEnterThanks"), new Color(255, 150, 255));
					}
					else
					{
						LogInMessageTimer = -1;
						Main.NewText(Language.GetTextValue("Mods.SOTS.Common.worldEnter"), new Color(20, 255, 40));
					}
					ImportantTilesWorld.RequestNewPackets();
				}
			}
		}
        public override void OnEnterWorld()
		{
			SOTSTexturePackEnabled = IsSOTSTexturePackEnabled();
			if (Main.netMode != NetmodeID.Server)
			{
				if (SOTS.SOTSTexturePackEnabled)
					Main.NewText(Language.GetTextValue("Mods.SOTS.Common.worldEnterThanks"), new Color(255, 150, 255));
				else
					Main.NewText(Language.GetTextValue("Mods.SOTS.Common.worldEnter"), new Color(20, 255, 40));
			}
		}
        public static SOTSPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<SOTSPlayer>();
		}
		public static int[] typhonBlacklist;
		public static int[] typhonWhitelist;
		public static int[] symbioteBlacklist;
		public static int[] harmonyWhitelist;
		public static bool pyramidBattle = false;
		public static void LoadArrays()
		{
			FakePlayerHelper.Initialize();
			typhonBlacklist = new int[] { ModContent.ProjectileType<ArcColumn>(), ModContent.ProjectileType<PhaseColumn>(), ModContent.ProjectileType<MacaroniBeam>(), 
				ModContent.ProjectileType<GenesisArc>(), ModContent.ProjectileType<GenesisCore>(), ModContent.ProjectileType<Projectiles.Earth.VibrantShard>(), 
				ModContent.ProjectileType<BlazingArrow>(), ModContent.ProjectileType<DimensionShredderLightning>() };
			symbioteBlacklist = new int[] { ModContent.ProjectileType<BloomingHook>(), ModContent.ProjectileType<BloomingHookMinion>(), ModContent.ProjectileType<CrystalSerpentBody>(), ProjectileID.AbigailCounter, ModContent.ProjectileType<FreshGreenyCounter>() };
			typhonWhitelist = new int[] { ModContent.ProjectileType<HardlightArrow>() };
			harmonyWhitelist = new int[] { BuffID.Honey, ModContent.BuffType<Frenzy>(), BuffID.Panic, BuffID.ParryDamageBuff, BuffID.ShadowDodge };
		}
		public int UniqueVisionNumber = -1;
		public static Color VoidMageColor(Player player, bool sourceTimeFreeze = true)
        {
			SOTSPlayer sPlayer = ModPlayer(player);
			if(SOTS.Config.coloredTimeFreeze || !sourceTimeFreeze)
			{
				switch (sPlayer.UniqueVisionNumber % 8)
				{
					case 0: //geo, earth
						return new Color(224, 131, 29);
					case 1: //electro, evil
						return new Color(255, 45, 43);
					case 2: //anemo, otherworld
						return new Color(163, 114, 241);
					case 3: //cyro, permafrost
						return new Color(172, 202, 199);
					case 4: //pyro, inferno
						return new Color(255, 157, 1);
					case 5: //hydro, tidal
						return new Color(88, 141, 240);
					case 6: //dendro, nature
						return new Color(172, 216, 78);
					case 7: //masterless, chaos
						return new Color(245, 145, 255);
				}
			}
			return Color.White;
        }
        public static Color VisionColor(Player player)
        {
            SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
            Color DestinationColor = Color.DarkGray;
            int uniqueGem = modPlayer.UniqueVisionNumber % 8;
            switch (uniqueGem)
            {
                case 0: //geo
                    DestinationColor = Color.Orange;
                    break;
                case 1: //electro
                    DestinationColor = Color.BlueViolet;
                    break;
                case 2: //anemo
                    DestinationColor = Color.Turquoise;
                    break;
                case 3: //cyro
                    DestinationColor = Color.LightSkyBlue;
                    break;
                case 4: //pyro
                    DestinationColor = Color.OrangeRed;
                    break;
                case 5: //hydro
                    DestinationColor = Color.DodgerBlue;
                    break;
                case 6: //dendro
                    DestinationColor = Color.Green;
                    break;
            }
            return DestinationColor;
        }
        public override void SaveData(TagCompound tag)
		{
			tag["UniqueVisionNumber"] = UniqueVisionNumber;
        }
        public override void LoadData(TagCompound tag)
		{
			UniqueVisionNumber = tag.GetInt("UniqueVisionNumber");
		}
		public void TrailStuff()
		{
			FluidCurse = false;
			if (Player.HasBuff(ModContent.BuffType<FluidCurse>()))
            {
				PetFluidCurse();
				FluidCurse = true;
			}
			float mult = Player.statLife / (float)Player.statLifeMax2;
			if (mult < 0) mult = 0;
			mult = (float)Math.Sqrt(mult);
			if (mult > 1) mult = 1;
			FluidCurseMult = 4 + (int)(60 * (1 - mult));
			if (FluidCurseMult > 60)
				FluidCurseMult = 60;
		}
		public int oldHeldProj = -1;
		public bool zoneLux = false;
		public bool zonePolaris = false;
		public bool oldTimeFreezeImmune = false;
		public bool TimeFreezeImmune = true;
		public bool VoidAnomaly = false;
		public bool VMincubator = false;
		public bool normalizedGravity = false;
		public bool VisionVanity = false;
		public float PotionBuffDegradeRate = 1f;
		public bool noMoreConstructs = false;
		public bool CanKillNPC = false;
		public bool CreativeFlightButtonPressed = false;
		public bool CurseAura = false;
		public bool FluidCurse = false;
		public float FluidCurseMult = 120;
		public bool petPepper = false;
		public bool petAdvisor = false;
		public int petPinky = -1;
		public int petFreeWisp = -1;
		public int symbioteDamage = -1;
        public int BundleSnakeDamage = -1;
        public bool rippleEffect = false;
		public int rippleTimer = 0;
		public int rippleBonusDamage = 0;
		public bool doomDrops = false;
		public bool baguetteDrops = false;
		public int baguetteLength = 0;
		public int baguetteLengthCounter = 0;
		public int halfLifeRegen = 0;
		public int additionalHeal = 0;
		public int additionalPotionMana = 0;
		public int darkEyeShader = 0;
		public int platformShader = 0;
		public int HoloEyeDamage = 0;
		public bool HoloEyeIsVanity = false;
		public bool HoloEye = false;
		public bool HoloEyeAttack = false;
		public bool HoloEyeAutoAttack = false;
		public float blinkPackMult = 1f;
		public bool rainbowGlowmasks = false;
		public int skywardBlades = 0;
		public float cursorRadians = 0;
		public float BlinkedAmount = 0;
		public int BlinkType = 0;
		public int BlinkDamage = 0;
		public bool ElementalBlink = false;
        public bool ElementalBlinkBuff = false;
        public int typhonRange = 0;
		public bool weakerCurse = false;
		public bool VibrantArmor = false;
		public int brokenFrigidSword = 0;
		public int shardSpellExtra = 0;
		public int frigidJavelinBoost = 0;
		public bool frigidJavelinNoCost = false;
		public int orbitalCounter
        {
			get => SOTSWorld.GlobalCounter + Player.whoAmI * 30;
        }
		public int shardOnHit = 0;
		public int bonusShardDamage = 0;
		public int phaseCannonIndex = -1;
		public float assassinateNum = 1;
		public int assassinateFlat = 0;
		public bool assassinate = false;
		public int polarCannons = 0;
		public float meleeItemScale = 1f;

		public Vector2 starCen;

		public int mourningStarFire = 0;

		public bool VoidspaceFlames = false;
		public bool AutoReuseAnything = false;
		public bool InfinityPouch = false;

		public bool PlanetariumBiome => Player.InModBiome<Biomes.PlanetariumBiome>();
		public bool PhaseBiome => Player.InModBiome<Biomes.PhaseBiome>();
		public bool AnomalyBiome => Player.InModBiome<Biomes.AnomalyBiome>();
		public bool PyramidBiome => Player.InModBiome<Biomes.PyramidBiome>();
		public bool backUpBow = false;
		public bool backUpBowVisual = false;
		public bool DoubleVisionActive = false;
		public int BonusFishingLines = 0;
		public bool Lockpick = false;
		public int onhit = 0;
		public int onhitdamage = 0;
		public int OnHitCD = 0;
		public float attackSpeedMod = 1;
		//some important variables 2

		public bool PurpleBalloon = false;
		public int StartingDamage = 0;
		public bool PushBack = false; // marble protecter effect
		public bool HarvestersScythe = false;

		public bool SupernovaEmblem = false; //bloodstained jewel effect
		public bool snakeSling = false; //snakeskin sling effect
		public bool CurseVision = false;
		public float curseVisionCounter = 0;
		public bool RubyMonolith = false;
		public bool RubyMonolithIsNOTVanity = false;
		public bool CanCurseSwap = false;
		public bool CurseSwap = false;

		public int CritLifesteal = 0; //crit clover
		public float maxCritLifestealPerSecond = 0;
		public float maxCritLifestealPerSecondTimer = 0;
		public float CritManasteal = 0f; //starbelt
		public float maxCritManastealPerSecond = 0;
		public float maxCritManastealPerSecondTimer = 0;
		public float CritVoidsteal = 0f; //crit void charm
		public float maxCritVoidStealPerSecond = 0;
		public float maxCritVoidStealPerSecondTimer = 0;
		public int CritBonusDamage = 0; //crit coin + amplfiier
		public float CritBonusMultiplier = 1f;
		public bool CritFire = false; //hellfire icosahedron
		public bool CritFrost = false; //borealis icosahedron
		public bool CritCurseFire = false; //cursed icosahedron
		public bool CritNightmare = false;
		public bool BlueFire = false;
		public bool BlueFireOrange = false;
		public bool EndothermicAfterburner = false;
		public bool ParticleRelocator = false;
		public int CactusSpineDamage = 0;
		public bool netUpdate = false;
		public bool BlazingQuiver = false;
		public bool SerpentSpine = false;

		public bool PlasmaShrimpVanity = false;
		public bool PlasmaShrimp = false;
		public bool RubyRing = false;
		public bool AmberRing = false;
        public bool InverseAmberRing = false;
        public bool TopazRing = false;
		public bool InverseTopazRing = false;
		private int InverseTopazRingCD = 0;
		public bool EmeraldRing = false;
		public bool DiamondRing = false;
        public bool InverseDiamondRing = false;
        public bool AmethystRing = false;
		public bool LazyCrafterAmulet = false;
		public int bonusPickaxePower = 0;
		public int previousDefense = 0;
		public float AmmoConsumptionModifier = 0.0f;
		public bool AmmoRegather = false;
		public float AmmoRegatherDelay = 0f;
		public bool PotionStacking = false;
		public bool DrainDebuffs = false;
		public bool SparkleDamage = false;
		public bool ConduitBelt = false;
		public bool SpiritSymphony = false;
		public bool hasSoaringInsigniaFake = false;
		public bool GoldenTrowel = false;
		public bool AnomalyLocator = false;
		public bool StatShareMeleeAndSummon = false;
        public bool StatShareMeleeAndMagic = false;
        public bool StatShareAll = false;
		public int BrassWhipDelay = 0;
		public float DamageGenerateMoney = 0;
		public bool KeepersBox = false;
        public bool PrevKeepersBox = false;
		public bool WishingStar = false;
		public bool AcidInject = false, Earthdrive = false;
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			MachinaBoosterPlayer testPlayer = Player.GetModPlayer<MachinaBoosterPlayer>();
			VoidPlayer voidPlayer = Player.GetModPlayer<VoidPlayer>();
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)SOTSMessageType.SOTSSyncPlayer);
			packet.Write((byte)Player.whoAmI);
			packet.Write(testPlayer.creativeFlight);
			packet.Write(voidPlayer.lootingSouls);
			packet.Send(toWho, fromWho);
		}
        public override void CopyClientState(ModPlayer clientClone)/* tModPorter Suggestion: Replace Item.Clone usages with Item.CopyNetStateTo */
        {
			//will need to fix this later...
        }
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
			// Here we would sync something like an RPG stat whenever the player changes it.
			SOTSPlayer clone = clientPlayer as SOTSPlayer;
			if(netUpdate)
			{
				if (clone.skywardBlades != skywardBlades)
				{
					// Send a Mod Packet with the changes.
					var packet = Mod.GetPacket();
					packet.Write((byte)SOTSMessageType.SyncPlayerKnives);
					packet.Write((byte)Player.whoAmI);
					packet.Write(skywardBlades);
					packet.Write(cursorRadians);
					packet.Send();
				}
                if (clone.UniqueVisionNumber != UniqueVisionNumber)
                {
                    var packet = Mod.GetPacket();
					packet.Write((byte)SOTSMessageType.SyncVisionNumber);
					packet.Write((byte)Player.whoAmI);
					packet.Write(UniqueVisionNumber);
					packet.Send();
				}
				netUpdate = false;
			}
        }
        public int bladeAlpha = 0;
        int foamParticleCounter = 0;
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public void FoamStuff()
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
						particle.position += Player.velocity * 0.85f;
				}
			}
			foamParticleCounter++;
			if (foamParticleCounter >= 1200)
			{
				foamParticleCounter = 0;
				ResetFoamLists();
			}
		}
		public void ResetFoamLists()
		{
			List<CurseFoam> temp = new List<CurseFoam>();
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				if (foamParticleList1[i].active && foamParticleList1[i] != null)
					temp.Add(foamParticleList1[i]);
			}
			foamParticleList1 = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				foamParticleList1.Add(temp[i]);
			}
		}
		public override void ProcessTriggers(TriggersSet triggersSet)
        {
            bool canBlink = !Player.mount.Active && !(Player.grappling[0] >= 0) && !Player.frozen && !Player.CCed && !Player.dead;
            if (SOTS.BlinkHotKey.JustPressed)
            {
				if (!Player.HasBuff(BuffID.ChaosState) && BlinkType == 1 && canBlink)
				{
					Vector2 toCursor = Main.MouseWorld - Player.Center;
					Projectile.NewProjectile(Player.GetSource_Misc("SOTS:Blink"), Player.Center, toCursor.SafeNormalize(Vector2.Zero), 
						ModContent.ProjectileType<Blink1>(), 0, 0, Player.whoAmI);
				}
			}
			if (SOTS.ArmorSetHotKey.JustPressed)
            {
                if (!Player.HasBuff<ChaosState2>() && ElementalBlink && canBlink && Player.whoAmI == Main.myPlayer)
                {
					Vector2 finalLocation = Main.MouseWorld - new Vector2(0, Player.height / 2);
                    Vector2 toCursor = finalLocation - Player.Center;
					float damage = Player.statDefense * 2f;
					int type = -1;
					if(ElementalBlinkBuff)
					{
						type = -2;
						damage = Player.GetTotalDamage<VoidGeneric>().ApplyTo(damage);
					}
                    Projectile.NewProjectile(Player.GetSource_Misc("SOTS:Blink"), Player.Center, toCursor.SafeNormalize(Vector2.Zero), 
						ModContent.ProjectileType<RelocatorBeam>(), (int)damage, 0, Player.whoAmI, finalLocation.X, finalLocation.Y, type);
                }
                if (!HoloEyeIsVanity)
					HoloEyeAttack = true;
				if (CanCurseSwap)
					CurseSwap = true;
			}
			else
			{
				if (!HoloEyeIsVanity)
					HoloEyeAttack = false;
				CurseSwap = false;
			}
			if (SOTS.MachinaBoosterHotKey.JustPressed)
			{
				CreativeFlightButtonPressed = true;
			}
			else
			{
				CreativeFlightButtonPressed = false;
			}
		}
		private int[] probes = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
		private int[] probesAqueduct = new int[] { -1, -1, -1, -1, -1, -1, -1, -1};
		private int[] probesTinyPlanet = new int[] { -1, -1, -1, -1, -1, -1, -1, -1};
        private int[] ArtifactProbes = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        public int aqueductNum = 0;
		public int aqueductDamage = -1;
        public int artifactProbeDamage = -1;
        public int artifactProbeNum = 0;
		public int tPlanetNum = 0;
		public int tPlanetDamage = -1;
        private int lastAqueductMax = 0;
        private int lastPlanetMax = 0;
		private int lastArtifactMax = 0;
        public void runPets(ref int Probe, int type, int damage = 0, float knockback = 0, bool skipTimeleftReset = false, float ai0 = 0f, float ai1 = 0f)
		{
			if (Main.myPlayer == Player.whoAmI)
			{
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(Player.GetSource_Misc("SOTS:Pets"), Player.Center, Vector2.Zero, type, damage, knockback, Player.whoAmI, ai0, ai1);
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != type || Main.projectile[Probe].owner != Player.whoAmI)
				{
					Probe = Projectile.NewProjectile(Player.GetSource_Misc("SOTS:Pets"), Player.Center, Vector2.Zero, type, damage, knockback, Player.whoAmI, ai0, ai1);
				}
				if(!skipTimeleftReset)
					Main.projectile[Probe].timeLeft = 6;
			}
		}
		public void PetFluidCurse()
		{
			runPets(ref probes[4], ModContent.ProjectileType<FluidFollower>(), 0, 0, true);
			runPets(ref probes[5], ModContent.ProjectileType<ClairvoyanceShade>(), 0, 0, true);
		}
		public void doPlanetAqueduct()
		{
			if (aqueductNum > 8) aqueductNum = 8;
			if (tPlanetNum > 8) tPlanetNum = 8;
            if (artifactProbeNum > 16) artifactProbeNum = 16;
            if (lastAqueductMax != aqueductNum)
			{
				for (int i = 0; i < 8; i++)
					probesAqueduct[i] = -1;
			}
			for (int i = 0; i < aqueductNum; i++)
			{
				runPets(ref probesAqueduct[i], ModContent.ProjectileType<Rainbolt>(), aqueductDamage + 1);
			}
			if (lastPlanetMax != tPlanetNum)
			{
				for (int i = 0; i < 8; i++)
					probesTinyPlanet[i] = -1;
			}
			for (int i = 0; i < tPlanetNum; i++)
			{
				runPets(ref probesTinyPlanet[i], ModContent.ProjectileType<TinyPlanetTear>(), tPlanetDamage + 1);
            }
			if(lastArtifactMax != artifactProbeNum)
            {
                for (int i = 0; i < 16; i++)
                    ArtifactProbes[i] = -1;
            }
			if(artifactProbeNum > 8)
			{
				artifactProbeDamage = (int)(artifactProbeDamage * 0.75f);
			}
            for (int i = 0; i < artifactProbeNum; i++)
            {
				float special = i;
				if (i >= 8)
					special += 0.5f;
                runPets(ref ArtifactProbes[i], ModContent.ProjectileType<BlizzardProbe>(), artifactProbeDamage, 0f, false, special, special / 8f * 90f);
            }
			lastArtifactMax = artifactProbeNum;
            artifactProbeDamage = artifactProbeNum = 0;
        }
		public void doCurseAura()
        {
			if(CurseAura || CurseVision)
			{
				int idClosest = -1;
				float visionDist = 1600;
				float auraDist = 270;
				float bestDist = visionDist;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					float distance = Vector2.Distance(npc.Center, Player.Center);
					if (npc.CanBeChasedBy() && distance <= visionDist && npc.realLife == -1)
					{
						if(distance < bestDist && !npc.buffImmune[ModContent.BuffType<CurseVision>()])
						{
							idClosest = i;
							bestDist = distance;
						}
						if (CurseAura && distance <= auraDist)
							npc.AddBuff(ModContent.BuffType<Buffs.PharaohsCurse>(), 120);
					}
				}
				float mult = (1 - 1f * curseVisionCounter / 60f);
				if (mult < 0) 
					mult = 0;
				if (idClosest >= 0)
				{
					NPC npc = Main.npc[idClosest];
					npc.AddBuff(ModContent.BuffType<CurseVision>(), 3);
					if(Main.myPlayer == Player.whoAmI)
					{
						Vector2 spawnLoc = new Vector2(npc.Center.X, npc.position.Y - 32);
						float hypo = (float)Math.Sqrt(npc.width * npc.width + npc.height * npc.height);
						hypo += 12f;
						for (int i = -1; i <= 1; i++)
						{
							Vector2 circular = new Vector2(hypo / 2f * i, 0).RotatedBy(MathHelper.ToRadians(orbitalCounter * 3f + curseVisionCounter * 1.7f));
							circular.X *= 0.8f;
							circular.Y *= 0.3f;
							Dust dust = Dust.NewDustPerfect(spawnLoc + circular, ModContent.DustType<CopyDust4>());
							dust.noGravity = true;
							dust.color = new Color(220, 80, 80, 40);
							dust.velocity += circular * 0.01f;
							dust.fadeIn = 0.1f;
							dust.alpha = (int)(255f * mult);
							if (i == 0)
							{
								dust.velocity *= 0.3f;
								dust.scale = 1.0f;
							}
							else
							{
								dust.velocity *= 0.1f;
								dust.scale = 0.8f;
							}
						}
					}
				}
			}
        }
        public override void PostUpdateMiscEffects()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			Vector2 detect = AncientGoldSpikeTile.HurtTiles(Player.position, Player.width, Player.height);
			if(detect.Y != 0f)
			{
				int damage3 = Main.DamageVar(50);
				Player.Hurt(PlayerDeathReason.ByOther(3), damage3, 0, false, false, -1, false, knockback: 3.0f);
			}
		}
		int fireIcoCD = 0;
		int iceIcoCD = 0;
		int cursedIcoCD = 0;
		int nightmareArmCD = 0;
		public static void decrement(ref int number)
		{
			if (number > 0)
				number--;
			else
				number = 0;
		}
        public override void PostUpdate()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			decrement(ref nightmareArmCD);
			decrement(ref fireIcoCD);
			decrement(ref iceIcoCD);
			decrement(ref cursedIcoCD);
			decrement(ref OnHitCD);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(Player);
			maxCritVoidStealPerSecond = (VoidPlayer.baseVoidGain + voidPlayer.bonusVoidGain) * 2; //max stored voidgain is 2x the void gain stat
			maxCritVoidStealPerSecondTimer += (VoidPlayer.baseVoidGain + voidPlayer.bonusVoidGain + CritVoidsteal) / 300f; //takes 10 seconds to fully restore the available pool of critsteal
			//Add critvoidsteal to the timer in some way to make it scale well with multiple voidsteal accessories. Same logic applies to other stat steals
			if (maxCritVoidStealPerSecondTimer > maxCritVoidStealPerSecond)
			{
				maxCritVoidStealPerSecondTimer = maxCritVoidStealPerSecond;
			}

			maxCritLifestealPerSecond = (Player.lifeRegen * 3) + 6; //max stored lifesteal is 3x lifeRegen (1 lifeRegen = 0.5 life per second) speed + 6 
			maxCritLifestealPerSecondTimer += (Player.lifeRegen + 3 + CritLifesteal) / 60f; //max stored lifesteal regenerates at the twice rate as normal regen (excluding movement based factors) (basically regenerates 3 life to the pool per second, faster with increased regen)
			if (maxCritLifestealPerSecondTimer > maxCritLifestealPerSecond)
			{
				maxCritLifestealPerSecondTimer = maxCritLifestealPerSecond;
			}

			maxCritManastealPerSecond = 30 + Player.statManaMax2 / 6; //max stored manasteal is 30 + 1/6th of the mana max
			maxCritManastealPerSecondTimer += (6f + CritManasteal / 1.5f) / 60f; //max stored voidsteal regenerates at the twice rate as normal voidRegen (basically regenerates 6 mana to the pool per second, the pool grows with larger max mana)
			if (maxCritManastealPerSecondTimer > maxCritManastealPerSecond)
			{
				maxCritManastealPerSecondTimer = maxCritManastealPerSecond;
			}
			PrevKeepersBox = KeepersBox;
            KeepersBox = false;
        }
        public override bool? CanHitNPCWithItem(Item item, NPC target)
        {
			if(CanKillNPC && item.DamageType == DamageClass.Melee && target.townNPC)
			{
				return null;
            }
            return base.CanHitNPCWithItem(item, target);
        }
		public void ResetVisionID(bool serverCommand = false)
        {
			UniqueVisionNumber = Main.rand.Next(40);
			if(NetmodeID.Server == Main.netMode && serverCommand)
			{
                var packet = Mod.GetPacket();
                packet.Write((byte)SOTSMessageType.SyncVisionNumber);
                packet.Write((byte)Player.whoAmI);
                packet.Write(UniqueVisionNumber);
                packet.Send(-1, -1);
            }
        }
		public override void PreUpdate()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			if (UniqueVisionNumber == -1)
				ResetVisionID(false);
			base.PreUpdate();
		}
		public static int ApplyDamageClassModWithGeneric(Player player, DamageClass damageClass, int startingDamage)
        {
			int originalDamage = startingDamage;
			StatModifier AndGeneric = player.GetTotalDamage(damageClass);
			return (int)AndGeneric.ApplyTo(originalDamage);
        }
        public static float GetAttackSpeedMultWithGeneric(Player player, DamageClass damageClass)
        {
            float AndGeneric = player.GetTotalAttackSpeed(damageClass);
            return AndGeneric;
        }
        public static int ApplyAttackSpeedClassModWithGeneric(Player player, DamageClass damageClass, float startingUseTime)
		{
			float AndGeneric = player.GetTotalAttackSpeed(damageClass);
            return (int)(startingUseTime / AndGeneric);
		}
        public override void UpdateEquips()
        {
            int defenseToConvert = Player.statDefense;
            if (defenseToConvert > 30)
            {
                defenseToConvert = 30;
            }
            previousDefense = defenseToConvert;
            if (DiamondRing)
            {
                Player.statDefense -= defenseToConvert / 3;
                Player.GetDamage(DamageClass.Generic) += defenseToConvert * 0.01f;
            }
            DiamondRing = false;
        }
        public override void PostUpdateEquips()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
            {
				return;
            }
			TrailStuff();
			doCurseAura();
			if (petAdvisor)
				runPets(ref probes[0], ModContent.ProjectileType<AdvisorPet>());
			if (petPepper)
				runPets(ref probes[1], ModContent.ProjectileType<GhostPepper>());
			if (HoloEye)
				runPets(ref probes[2], ModContent.ProjectileType<HoloEye>(), HoloEyeDamage + 1);
			if (petPinky >= 0)
				runPets(ref probes[3], ModContent.ProjectileType<PetPutridPinkyCrystal>(), petPinky);
			if (RubyMonolith)
				runPets(ref probes[6], ModContent.ProjectileType<RubyMonolith>());
			if (petFreeWisp >= 0)
				runPets(ref probes[7], ModContent.ProjectileType<WispOrange>(), petFreeWisp + 1);
			if (VisionVanity)
			{
				runPets(ref probes[8], ModContent.ProjectileType<VisionWeapon>());
			}
			else if (backUpBowVisual)
			{
				runPets(ref probes[8], ModContent.ProjectileType<BackupBowVisual>());
			}
			if (PlasmaShrimp)
			{
				runPets(ref probes[9], ModContent.ProjectileType<Projectiles.Tide.PlasmaShrimp>());
			}
			doPlanetAqueduct();
			if (rippleEffect)
			{
				float healthPercent = (float)Player.statLife / (float)Player.statLifeMax2;
				int timerMax = (int)(70 * healthPercent) + 20;
				if (rippleTimer > timerMax)
				{
					if (Main.myPlayer == Player.whoAmI)
						Projectile.NewProjectile(Player.GetSource_Misc("Ripple buffs or accessory"), Player.Center, new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))), ModContent.ProjectileType<Projectiles.Tide.RippleWave>(), 20 + rippleBonusDamage, 0f, Player.whoAmI, 1, 0);
					rippleTimer -= timerMax;
				}
			}
			else
			{
				rippleTimer = 0;
			}
			hasSoaringInsigniaFake = false;
			if (!SOTSWorld.downedLux && SOTS.ServerConfig.NerfInsignia)
			{
				if (Player.empressBrooch)
				{
					hasSoaringInsigniaFake = true;
					Player.empressBrooch = false;
					Player.rocketTimeMax = 11;
				}
				else
				{
					Player.rocketTimeMax = 7; //7 is the default found in Terraria's code
				}
			}
			else
			{
				Player.rocketTimeMax = 7;
			}
			ReplaceCritWithDamage();
            StatShare();
        }
		public void ReplaceCritWithDamage()
		{
			if(InverseDiamondRing)
            {
                float critGeneric = Player.GetCritChance(DamageClass.Generic);
                float critMelee = Player.GetCritChance(DamageClass.Melee);
                float critRanged = Player.GetCritChance(DamageClass.Ranged);
                float critMagic = Player.GetCritChance(DamageClass.Magic);
                float critVGeneric = Player.GetCritChance<VoidGeneric>();
                float critVMelee = Player.GetCritChance<VoidMelee>();
                float critVRanged = Player.GetCritChance<VoidRanged>();
                float critVMagic = Player.GetCritChance<VoidMagic>();
                Player.GetCritChance(DamageClass.Generic) -= critGeneric + 4;
                Player.GetCritChance(DamageClass.Melee) -= critMelee + 4;
                Player.GetCritChance(DamageClass.Ranged) -= critRanged + 4;
                Player.GetCritChance(DamageClass.Magic) -= critMagic + 4;
                Player.GetCritChance<VoidGeneric>() -= critVGeneric + 4;
                Player.GetCritChance<VoidMelee>() -= critVMelee + 4;
                Player.GetCritChance<VoidRanged>() -= critVRanged + 4;
                Player.GetCritChance<VoidMagic>() -= critVMagic + 4;
				Player.GetDamage(DamageClass.Generic) *= 1 + critGeneric / 100f;
                Player.GetDamage(DamageClass.Melee) *= 1 + critMelee / 100f;
                Player.GetDamage(DamageClass.Ranged) *= 1 + critRanged / 100f;
                Player.GetDamage(DamageClass.Magic) *= 1 + critMagic / 100f;
				Player.GetDamage<VoidGeneric>() *= 1 + critVGeneric / 100f;
                Player.GetDamage<VoidMelee>() *= 1 + critVMelee / 100f;
                Player.GetDamage<VoidRanged>() *= 1 + critVRanged / 100f;
                Player.GetDamage<VoidMagic>() *= 1 + critVMagic / 100f;
            }
            InverseDiamondRing = false;
        }
		public void StatShare()
        {
            float meleeAdditiveBonus = (Player.GetDamage(DamageClass.Melee).Additive - 1) * 0.5f;
            float meleeFlatBonus = Player.GetDamage(DamageClass.Melee).Flat * 0.5f;
            float meleeMultiplicativeBonus = (Player.GetDamage(DamageClass.Melee).Multiplicative - 1) * 0.5f + 1;
            float meleeBaseBonus = Player.GetDamage(DamageClass.Melee).Base * 0.5f;
            float rangedAdditiveBonus = (Player.GetDamage(DamageClass.Ranged).Additive - 1) * 0.5f;
            float rangedFlatBonus = Player.GetDamage(DamageClass.Ranged).Flat * 0.5f;
            float rangedMultiplicativeBonus = (Player.GetDamage(DamageClass.Ranged).Multiplicative - 1) * 0.5f + 1;
            float rangedBaseBonus = Player.GetDamage(DamageClass.Ranged).Base * 0.5f;
            float magicAdditiveBonus = (Player.GetDamage(DamageClass.Magic).Additive - 1) * 0.5f;
            float magicFlatBonus = Player.GetDamage(DamageClass.Magic).Flat * 0.5f;
            float magicMultiplicativeBonus = (Player.GetDamage(DamageClass.Magic).Multiplicative - 1) * 0.5f + 1;
            float magicBaseBonus = Player.GetDamage(DamageClass.Magic).Base * 0.5f;
            float summonAdditiveBonus = (Player.GetDamage(DamageClass.Summon).Additive - 1) * 0.5f;
            float summonFlatBonus = Player.GetDamage(DamageClass.Summon).Flat * 0.5f;
            float summonMultiplicativeBonus = (Player.GetDamage(DamageClass.Summon).Multiplicative - 1) * 0.5f + 1;
            float summonBaseBonus = Player.GetDamage(DamageClass.Summon).Base * 0.5f;
            if (StatShareMeleeAndSummon)
            {
                if (meleeAdditiveBonus > 0)
                    Player.GetDamage(DamageClass.Summon) += meleeAdditiveBonus;
                if (meleeFlatBonus > 0)
                    Player.GetDamage(DamageClass.Summon).Flat += meleeFlatBonus;
                if (meleeMultiplicativeBonus > 1)
                    Player.GetDamage(DamageClass.Summon) *= meleeMultiplicativeBonus;
                if (meleeBaseBonus > 0)
                    Player.GetDamage(DamageClass.Summon).Base += meleeBaseBonus;
                if (summonAdditiveBonus > 0)
                    Player.GetDamage(DamageClass.Melee) += summonAdditiveBonus;
                if (summonFlatBonus > 0)
                    Player.GetDamage(DamageClass.Melee).Flat += summonFlatBonus;
                if (summonMultiplicativeBonus > 1)
                    Player.GetDamage(DamageClass.Melee) *= summonMultiplicativeBonus;
                if (summonBaseBonus > 0)
                    Player.GetDamage(DamageClass.Melee).Base += summonBaseBonus;
            }
            if (StatShareMeleeAndMagic)
            {
                if (meleeAdditiveBonus > 0)
                    Player.GetDamage(DamageClass.Magic) += meleeAdditiveBonus;
                if (meleeFlatBonus > 0)
                    Player.GetDamage(DamageClass.Magic).Flat += meleeFlatBonus;
                if (meleeMultiplicativeBonus > 1)
                    Player.GetDamage(DamageClass.Magic) *= meleeMultiplicativeBonus;
                if (meleeBaseBonus > 0)
                    Player.GetDamage(DamageClass.Magic).Base += meleeBaseBonus;
                if (magicAdditiveBonus > 0)
                    Player.GetDamage(DamageClass.Melee) += magicAdditiveBonus;
                if (magicFlatBonus > 0)
                    Player.GetDamage(DamageClass.Melee).Flat += magicFlatBonus;
                if (magicMultiplicativeBonus > 1)
                    Player.GetDamage(DamageClass.Melee) *= magicMultiplicativeBonus;
                if (magicBaseBonus > 0)
                    Player.GetDamage(DamageClass.Melee).Base += magicBaseBonus;
            }
			if(StatShareAll)
            {
                if (meleeAdditiveBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic) += meleeAdditiveBonus;
                    Player.GetDamage(DamageClass.Melee) -= meleeAdditiveBonus;
                }
                if (meleeFlatBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic).Flat += meleeFlatBonus;
                    Player.GetDamage(DamageClass.Melee).Flat -= meleeFlatBonus;
                }
                if (meleeMultiplicativeBonus > 1)
                {
                    Player.GetDamage(DamageClass.Generic) *= meleeMultiplicativeBonus;
                    Player.GetDamage(DamageClass.Melee) /= meleeMultiplicativeBonus;
                }
                if (meleeBaseBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic).Base += meleeBaseBonus;
                    Player.GetDamage(DamageClass.Melee).Base -= meleeBaseBonus;
                }
                if (rangedAdditiveBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic) += rangedAdditiveBonus;
                    Player.GetDamage(DamageClass.Ranged) -= rangedAdditiveBonus;
                }
                if (rangedFlatBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic).Flat += rangedFlatBonus;
                    Player.GetDamage(DamageClass.Ranged).Flat -= rangedFlatBonus;
                }
                if (rangedMultiplicativeBonus > 1)
                {
                    Player.GetDamage(DamageClass.Generic) *= rangedMultiplicativeBonus;
                    Player.GetDamage(DamageClass.Ranged) /= rangedMultiplicativeBonus;
                }
                if (rangedBaseBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic).Base += rangedBaseBonus;
                    Player.GetDamage(DamageClass.Ranged).Base -= rangedBaseBonus;
                }
                if (magicAdditiveBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic) += magicAdditiveBonus;
                    Player.GetDamage(DamageClass.Magic) -= magicAdditiveBonus;
                }
                if (magicFlatBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic).Flat += magicFlatBonus;
                    Player.GetDamage(DamageClass.Magic).Flat -= magicFlatBonus;
                }
                if (magicMultiplicativeBonus > 1)
                {
                    Player.GetDamage(DamageClass.Generic) *= magicMultiplicativeBonus;
                    Player.GetDamage(DamageClass.Magic) /= magicMultiplicativeBonus;
                }
                if (magicBaseBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic).Base += magicBaseBonus;
                    Player.GetDamage(DamageClass.Magic).Base -= magicBaseBonus;
                }
                if (summonAdditiveBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic) += summonAdditiveBonus;
                    Player.GetDamage(DamageClass.Summon) -= summonAdditiveBonus;
                }
                if (summonFlatBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic).Flat += summonFlatBonus;
                    Player.GetDamage(DamageClass.Summon).Flat -= summonFlatBonus;
                }
                if (summonMultiplicativeBonus > 1)
                {
                    Player.GetDamage(DamageClass.Generic) *= summonMultiplicativeBonus;
                    Player.GetDamage(DamageClass.Summon) /= summonMultiplicativeBonus;
                }
                if (summonBaseBonus > 0)
                {
                    Player.GetDamage(DamageClass.Generic).Base += summonBaseBonus;
                    Player.GetDamage(DamageClass.Summon).Base -= summonBaseBonus;
                }
            }
			if(Earthdrive)
			{
				float meleeSpeed = Player.GetAttackSpeed(DamageClass.Melee) - 1;
                float miningSpeed = 1 - Player.pickSpeed;
				if (meleeSpeed > 0)
					Player.pickSpeed -= meleeSpeed;
				//Main.NewText(Player.pickSpeed);
				if (miningSpeed > 0)
					Player.GetAttackSpeed(DamageClass.Melee) += miningSpeed;
            }
            StatShareMeleeAndSummon = StatShareMeleeAndMagic = StatShareAll = Earthdrive = false;
        }
		public override void ResetEffects()
		{
			if (Player.isDisplayDollOrInanimate || Player.isHatRackDoll || Player.isFirstFractalAfterImage)
			{
				return;
			}
			BlazingQuiver = WishingStar = AcidInject = false;
			oldTimeFreezeImmune = TimeFreezeImmune;
			TimeFreezeImmune = true;
			if(VMincubator)
            {
				if(SOTSWorld.GlobalFrozen)
                {
					Player.AddBuff(ModContent.BuffType<VoidMetamorphosis>(), 30, true);
					Player.AddBuff(ModContent.BuffType<DilationSickness>(), SOTSWorld.GlobalTimeFreeze * 2 + 600, true);
                }
            }
			VoidAnomaly = false;
			VMincubator = false;
			zoneLux = zonePolaris = false;
			if (NPC.AnyNPCs(ModContent.NPCType<Lux>()) || NPC.AnyNPCs(ModContent.NPCType<NewPolaris>()))
			{
				for(int i = 0; i < Main.npc.Length; i++)
                {
					if(Main.npc[i].active && Main.npc[i].Distance(Player.Center) < 3200)
                    {
                        if (Main.npc[i].type == ModContent.NPCType<NewPolaris>())
                        {
                            zonePolaris = true;
                        }
                        if (Main.npc[i].type == ModContent.NPCType<Lux>())
                        {
                            zoneLux = true;
                        }
                    }
                }
            }
			CactusSpineDamage = 0;
			HarvestersScythe = false;
			ParticleRelocator = false;
			pyramidBattle = false;
			if (normalizedGravity && !Player.GetModPlayer<MachinaBoosterPlayer>().creativeFlight)
            {
				Player.gravity = Player.defaultGravity;
            }
			normalizedGravity = false; 
			noMoreConstructs = false;
			CanKillNPC = false;
			baguetteDrops = false;
			if (baguetteLengthCounter >= 180)
			{
				if(baguetteLength > 0)
					baguetteLength--;
				baguetteLengthCounter = baguetteLength * 3;
			}
			if (baguetteLength > 0)
				baguetteLengthCounter++;
			else
            {
				baguetteLengthCounter = 0;
            }
			doomDrops = false;
			Player.lifeRegen += halfLifeRegen / 2;
			halfLifeRegen = 0;
			if (Player.HasBuff(BuffID.ChaosState))
            {
				BlinkedAmount = 0;
			}
			if(BlinkedAmount > 0 && BlinkedAmount < 2)
            {
				BlinkedAmount -= 0.002f;
				if (BlinkedAmount < 0) BlinkedAmount = 0;
			}
			if(Player.whoAmI == Main.myPlayer)
			{
				cursorRadians = (Main.MouseWorld - Player.Center).ToRotation();
				if(skywardBlades >= 0)
				{
					netUpdate = true;
				}
				if(skywardBlades == 0)
                {
					skywardBlades = -1;
					netUpdate = true;
				}
			}
			if (skywardBlades >= 0)
			{
				if (Player.HeldItem.type == ModContent.ItemType<SkywardBlades>())
				{
					if (bladeAlpha > 0)
						bladeAlpha -= 5;
					else
						bladeAlpha = 0;
				}
				else
				{
					if (bladeAlpha < 255)
						bladeAlpha += 5;
					else
						bladeAlpha = 255;
				}
			}
			additionalHeal = additionalPotionMana = 0;
			HoloEyeAutoAttack = ElementalBlink = ElementalBlinkBuff = false;
			blinkPackMult = 1f;
			BlinkDamage = 0;
			BlinkType = 0;
			VisionVanity = backUpBowVisual = rippleEffect = false;
			rippleBonusDamage = 0;
			symbioteDamage = BundleSnakeDamage = - 1;
			petPinky = -1;
			petFreeWisp = -1;
			petPepper = petAdvisor = rainbowGlowmasks = HoloEyeIsVanity = HoloEye = false;
			HoloEyeDamage = darkEyeShader = platformShader = 0;
			aqueductDamage = -1;
			lastAqueductMax = aqueductNum;
			aqueductNum = 0;
			tPlanetDamage = -1;
			lastPlanetMax = tPlanetNum;
			tPlanetNum = 0;
			RubyMonolith = false;
			RubyMonolithIsNOTVanity = AnomalyLocator = false;
			int voidspacePiecesWorn = 0, chaosPiecesWorn = 0;
			for (int i = 9 + Player.extraAccessorySlots; i < Player.armor.Length; i++) //checking vanity slots
            {
				Item item = Player.armor[i];
				if (item.type == ModContent.ItemType<Items.Conduit.AnomalyLocator>())
					AnomalyLocator = true;
				if (item.type == ModContent.ItemType<CursedApple>())
				{
					petPepper = true;
				}
				if (item.type == ModContent.ItemType<Calculator>())
				{
					petAdvisor = true;
				}
				if (item.type == ModContent.ItemType<PeanutButter>())
				{
					petPinky = 0;
				}
				if (item.type == ModContent.ItemType<SkywareBattery>())
				{
					rainbowGlowmasks = true;
				}
				if (item.type == ModContent.ItemType<TwilightAssassinsCirclet>())
				{
					if (!HoloEye)
                    {
						HoloEyeIsVanity = true;
						int damage = ApplyDamageClassModWithGeneric(Player, DamageClass.Summon, 33);
						HoloEyeDamage += damage;
					}
					HoloEye = true;
				}
				if (item.type == ModContent.ItemType<CursedRobe>())
					RubyMonolith = true;
				if (item.type == ModContent.ItemType<VisionAmulet>())
                {
					VisionVanity = true;
				}
				if (item.type == ModContent.ItemType<BackupBow>())
					backUpBowVisual = true;
				if (item.type == ModContent.ItemType<MachinaBooster>())
				{
					MachinaBoosterPlayer MachinaBoosterPlayer = Player.GetModPlayer<MachinaBoosterPlayer>();
					if(!MachinaBoosterPlayer.canCreativeFlight)
                    {
						MachinaBoosterPlayer.HaloDust();
					}
				}
				/*if (item.type == ModContent.ItemType<SubspaceLocket>())
				{
					SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(Player.dye[i].type);
				}*/
			}
			for (int i = 0; i < Player.inventory.Length; i++)
			{
				Item item = Player.inventory[i];
				if (item.type == ModContent.ItemType<TwilightAssassinsCirclet>() && item.favorited)
				{
					if (!HoloEye)
					{
						HoloEyeIsVanity = true;
						HoloEyeDamage += ApplyDamageClassModWithGeneric(Player, DamageClass.Summon, 33);
					}
					HoloEye = true;
					break;
				}
				if (!item.IsAir)
				{
					PrefixItem.SetInventorySlot(item, i);
				}
			}
			for (int i = 0; i < 10; i++) //iterating through armor + accessories
			{
				Item item = Player.armor[i];
				if (item.type == ModContent.ItemType<Items.Conduit.AnomalyLocator>())
					AnomalyLocator = true;
				if (item.type == ModContent.ItemType<TheDarkEye>())
				{
					darkEyeShader = GameShaders.Armor.GetShaderIdFromItemId(Player.dye[i].type);
				}
				if (item.type == ModContent.ItemType<PlatformGenerator>() || item.type == ModContent.ItemType<FortressGenerator>())
				{
					platformShader = GameShaders.Armor.GetShaderIdFromItemId(Player.dye[i].type);
				}
				if (item.type == ModContent.ItemType<TwilightAssassinsCirclet>())
				{
					HoloEyeIsVanity = false;
				}
				if (item.type == ModContent.ItemType<VoidspaceLeggings>() ||
					item.type == ModContent.ItemType<VoidspaceBreastplate>() || 
					item.type == ModContent.ItemType<VoidspaceMask>())
				{
					voidspacePiecesWorn++;
                }
                if (item.type == ModContent.ItemType<ElementalLeggings>() ||
                    item.type == ModContent.ItemType<ElementalBreastplate>() ||
                    item.type == ModContent.ItemType<ElementalHelmet>())
                {
                    chaosPiecesWorn++;
                }
                /*if (item.type == ModContent.ItemType<SubspaceLocket>())
				{
					SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(Player.dye[i].type);
				}*/
            }
			if(voidspacePiecesWorn > 0)
			{
				Lighting.AddLight(Player.Center, new Vector3(0.5f, 0.88f, 0.62f) * voidspacePiecesWorn * 0.3f);
            }
            if (chaosPiecesWorn > 0)
            {
                Lighting.AddLight(Player.Center, Vector3.Lerp(Vector3.One, ColorHelper.ChaosPink.ToVector3(), 0.5f) * chaosPiecesWorn * 0.5f);
            }
            typhonRange = assassinateFlat = shardSpellExtra = frigidJavelinBoost = 0;
            assassinateNum = 1;
			assassinate = VibrantArmor = frigidJavelinNoCost = false;
            brokenFrigidSword = brokenFrigidSword > 0 ? brokenFrigidSword - 1 : brokenFrigidSword;
			if (SOTSWorld.GlobalCounter % 360 == 0)
			{
				netUpdate = true;
			}
			shardOnHit = bonusShardDamage = 0;
			if (onhit > 0)
			{
				onhit--;
			}
			// Do something here to increase attack speed
			Player.GetAttackSpeed(DamageClass.Generic) += attackSpeedMod - 1;
			attackSpeedMod = 1;
			Lockpick = false;
			DoubleVisionActive = false;
			backUpBow = VoidspaceFlames = AutoReuseAnything = InfinityPouch = PurpleBalloon = false;
			PushBack = false;

			SupernovaEmblem = false;
			snakeSling = false;
			if (CurseVision)
			{
				if (curseVisionCounter < 60)
				{
					curseVisionCounter++;
					if(Player.HasBuff(ModContent.BuffType<RubyMonolithAttack>()))
					{
						curseVisionCounter += 4;
					}
				}
				if (curseVisionCounter > 60)
					curseVisionCounter = 60;
			}
			else
				curseVisionCounter = -60;
			CurseVision = false;

			CritLifesteal = 0;
			CritVoidsteal = 0f;
			CritManasteal = 0f;
			CritBonusDamage = 0;
			CritBonusMultiplier = 1f;
			CritFire = false;
			CritFrost = false;
			CritCurseFire = false;
			CritNightmare = false;
			CurseAura = false;
			CanCurseSwap = false;
			BlueFire = false;
			BlueFireOrange = false;
			EndothermicAfterburner = false;
			ParticleRelocator = false;
			if (PyramidBiome)
				Player.AddBuff(ModContent.BuffType<Buffs.PharaohsCurse>(), 16, false); 
			polarCannons = 0;
			meleeItemScale = 1f;
			SerpentSpine = false;
			PlasmaShrimpVanity = false;
			PlasmaShrimp = false;
			if(LazyCrafterAmulet) //this needs to be done in both Detours and Here due to how the original recipe functions determine when to update recipes
			{
				Player.adjTile[TileID.WorkBenches] = true;
				Player.adjTile[TileID.Furnaces] = true;
				Player.adjTile[TileID.Anvils] = true;
				Player.adjTile[TileID.AlchemyTable] = true;
				Player.adjTile[TileID.Bottles] = true;
				Player.adjTile[TileID.Tables] = true;
				Player.alchemyTable = true;
			}
			if(InverseTopazRing && Main.myPlayer == Player.whoAmI)
			{
				if(InverseTopazRingCD <= 0)
				{
                    GrantRandomRingBuff(Player);
                    InverseTopazRingCD = 1200;
                }
				else
				{
					InverseTopazRingCD--;
				}
            }
			RubyRing = AmberRing = TopazRing = EmeraldRing = AmethystRing = LazyCrafterAmulet = InverseTopazRing = false;
			AmmoConsumptionModifier = DamageGenerateMoney = 0.0f;
			bonusPickaxePower = 0;
			AmmoRegather = PotionStacking = SparkleDamage = ConduitBelt = GoldenTrowel = false;
			SpiritSymphony = false;
			if (AmmoRegatherDelay < 120)
				AmmoRegatherDelay++;
			if (BrassWhipDelay > 0)
				BrassWhipDelay--;
        }
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            return base.AddStartingItems(mediumCoreDeath);
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
			//Fish Set 1
			int power = attempt.playerFishingConditions.BaitPower + attempt.playerFishingConditions.PolePower;
			int baitType = attempt.playerFishingConditions.BaitItemType;
			int liquidType = attempt.inHoney ? 2 : attempt.inLava ? 1 : 0;
			if (ScaleCatch2(power, 0, 100, 8, 24) && (Player.ZoneSkyHeight || Player.Center.Y < Main.worldSurface * 16 * 0.5f))
				itemDrop = ModContent.ItemType<TinyPlanetFish>();
			if(Player.ZoneBeach && liquidType == 0 && ScaleCatch2(power, 0, 100, 100, 200))
				itemDrop = ModContent.ItemType<PistolShrimp>();

			if (Player.ZoneBeach && liquidType == 0 && Main.rand.NextBool(225))
				itemDrop = ModContent.ItemType<CrabClaw>(); 


			if (ScaleCatch2(power, 0, 90, 150, 750) && Player.ZoneBeach && liquidType == 0)
				itemDrop = ModContent.ItemType<PinkJellyfishStaff>(); 
			else if (ScaleCatch2(power, 0, 70, 30, 150) && Player.ZoneBeach && liquidType == 0 && baitType == ItemID.PinkJellyfish) //Checks for pink jellyfish bait
				itemDrop = ModContent.ItemType<PinkJellyfishStaff>();

			if (ScaleCatch2(power, 0, 90, 150, 750) && Player.ZoneRockLayerHeight && liquidType == 0)
				itemDrop = ModContent.ItemType<BlueJellyfishStaff>();
			else if (ScaleCatch2(power, 0, 70, 30, 150) && Player.ZoneRockLayerHeight && liquidType == 0 && baitType == ItemID.BlueJellyfish) //Checks blue jellyfish bait
				itemDrop = ModContent.ItemType<BlueJellyfishStaff>();
			else if (ScaleCatch2(power, 0, 70, 30, 150) && Player.ZoneRockLayerHeight && liquidType == 0 && baitType == ItemID.GreenJellyfish) //Checks green jellyfish bait
				itemDrop = ModContent.ItemType<BlueJellyfishStaff>();

			if (ScaleCatch2(power, 0, 30, 5, 10) && PyramidBiome && liquidType == 0)
				itemDrop = ModContent.ItemType<SeaSnake>(); 
			else if (ScaleCatch2(power, 0, 40, 7, 11) && PyramidBiome && liquidType == 0)
				itemDrop = ModContent.ItemType<PhantomFish>(); 
			else if (ScaleCatch2(power, 20, 80, 7, 20) && PyramidBiome && liquidType == 0) //gains the same rarity as Phantom Fish when at 80, fails to catch below 20 power
				itemDrop = ModContent.ItemType<Curgeon>(); 
			else if (ScaleCatch2(power, 0, 200, 100, 300) && PyramidBiome && liquidType == 0) //1/300 at 0, 1/200 at 100, 1/100 at 200, etc
				itemDrop = ModContent.ItemType<ZephyrousZeppelin>(); 
			else if (ScaleCatch2(power, 0, 200, 100, 300) && PyramidBiome && liquidType == 0) //1/300 at 0, 1/200 at 100, 1/100 at 200, etc
				itemDrop = ItemID.ZephyrFish; 
			else if (!Player.HasBuff(BuffID.Crate))
			{
				if (ScaleCatch2(power, 0, 200, 20, 200) && PyramidBiome && liquidType == 0)
					itemDrop = ModContent.ItemType<PyramidCrate>(); 
			}
			else if (ScaleCatch2(power, 0, 200, 10, 100) && PyramidBiome && liquidType == 0)
					itemDrop = ModContent.ItemType<PyramidCrate>(); 
			else
			{
				bool cratePotion = Player.HasBuff(BuffID.Crate);
                if (attempt.playerFishingConditions.PoleItemType == ModContent.ItemType<TwilightFishingPole>() && ScaleCatch2(power, 0, 100, cratePotion ? 8 : 16, cratePotion ? 80 : 160))
                {
                    itemDrop = Main.hardMode ? ModContent.ItemType<OtherworldCrate>() : ModContent.ItemType<PlanetariumCrate>();
                }
            }

		}
        /** minPower is the minimum power required, and yields a 1/maxRate chance of catching
		*	maxPower is the maximum power required, and yields a 1/minRate chance of catching
		*	rates are overall rounded down
		*	anything below minPower will fail to catch
		*	pre condition: minPower < maxPower, minRate < maxRate
		*	post condition: returns true at a specific chance.	*/
        public static bool ScaleCatch2(int power, int minPower, int maxPower, int minRate, int maxRate)
		{
			if (power < minPower)
			{
				return false;
			}
			int fixRate = maxRate - minRate;
			power -= minPower;
			maxPower -= minPower;
			float powerRate = (float)power / maxPower;
			int rate = maxRate - (int)(fixRate * powerRate);
			if (rate < minRate)
			{
				rate = minRate;
			}
			return Main.rand.NextBool(rate);
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (PushBack)
			{
				if(Main.myPlayer == Player.whoAmI)
				{
					Vector2 toNPC = (Player.Center - npc.Center).SafeNormalize(Vector2.Zero);
					int Proj = Projectile.NewProjectile(Player.GetSource_OnHurt(npc), npc.Center - toNPC * 5, toNPC, ProjectileID.JavelinFriendly, 12, 25f, Player.whoAmI);
					Main.projectile[Proj].timeLeft = 15;
					Main.projectile[Proj].netUpdate = true;
				}
			}
		}
		public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
		{
			if (hit.Crit && CritNightmare && projectile != null && projectile.type != ModContent.ProjectileType<EvilGrowth>() && projectile.type != ModContent.ProjectileType<EvilStrike>())
			{
				if (nightmareArmCD <= 0)
				{
					nightmareArmCD = 360;
					if (Main.myPlayer == Player.whoAmI)
					{
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<EvilGrowth>(), (int)(hit.SourceDamage * 0.1f), 0, Player.whoAmI, 0, target.whoAmI);
					}
				}
			}
			DebuffNPC instancedTarget = target.GetGlobalNPC<DebuffNPC>();
			if (target.life <= 0) //If the enemy DIES in this hit
			{
				if (AmmoRegather && projectile != null)
				{
					if (projectile.owner == Main.myPlayer)
						instancedTarget.AddAmmoToList(projectile); //since the ammo adding happens soley on clientn, and the kill function is called on player, ammo regathering only occurs to the player who gets the final kill
					List<int> localizedAmmoList = instancedTarget.ammoRegatherList;
					for (int i = 0; i < localizedAmmoList.Count; i++)
					{
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, new Vector2(Main.rand.NextFloat(6f, 8f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))), ModContent.ProjectileType<AmmoRegainProj>(), 0, 0, Main.myPlayer, localizedAmmoList[i]);
					}
                }
                if (SupernovaEmblem && projectile.type != ModContent.ProjectileType<Seeker>() && target.realLife == -1)
                {
                    Projectiles.Planetarium.SupernovaHammer.SpawnSeekers(new EntitySource_OnHit(Player, target), target.Center, 1, (int)(hit.SourceDamage * 1.5f), -1);
                }
            }
			if (Main.myPlayer == Player.whoAmI && AmmoRegather && !target.immortal && projectile != null)
			{
				if (projectile.CountsAsClass(DamageClass.Ranged))
				{
					SOTSProjectile instancedProj = projectile.GetGlobalProjectile<SOTSProjectile>();
					int ammoType = instancedProj.AmmoUsedID;
					if (instancedProj.AmmoUsedID >= 0)
					{
						float chance = AmmoRegatherDelay / 120f * 0.12f; //12% base chance to regather ammo, but decreases every successful regather to 0%, linearly coming back to 12% after 2 seconds
						if (Main.rand.NextFloat(1) < chance)
						{
							Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, new Vector2(Main.rand.NextFloat(5f, 7f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))), ModContent.ProjectileType<AmmoRegainProj>(), 0, 0, Main.myPlayer, ammoType);
							AmmoRegatherDelay = 0;
						}
						else //only add to NPC ammo count if missed initial chance of regather
						{
							instancedTarget.AddAmmoToList(projectile);
						}
					}
				}
			}
		}
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
		{
			if (hit.Crit && CritNightmare)
			{
				if (nightmareArmCD <= 0)
				{
					nightmareArmCD = 360;
					if (Main.myPlayer == Player.whoAmI)
					{
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<EvilGrowth>(), (int)(hit.SourceDamage * 0.1f), 0, Player.whoAmI, 0, target.whoAmI);
					}
				}
			}
			DebuffNPC instancedTarget = target.GetGlobalNPC<DebuffNPC>();
			if (target.life <= 0)
			{
				if (AmmoRegather)
				{
					//since the ammo adding happens soley on clientn, and the kill function is called on player, ammo regathering only occurs to the player who gets the final kill
					List<int> localizedAmmoList = instancedTarget.ammoRegatherList;
					for (int i = 0; i < localizedAmmoList.Count; i++)
					{
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, new Vector2(Main.rand.NextFloat(6f, 8f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))), ModContent.ProjectileType<AmmoRegainProj>(), 0, 0, Main.myPlayer, localizedAmmoList[i]);
					}
                }
                if (SupernovaEmblem)
                {
                    Projectiles.Planetarium.SupernovaHammer.SpawnSeekers(new EntitySource_OnHit(Player, target), target.Center, 1, (int)(hit.SourceDamage * 1.5f), -1);
                }
            }
		}
		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			if (ParticleRelocator)
			{
				NPC collidingNPC = null;
				for(int i = 0; i < 200; i++)
                {
					NPC npc = Main.npc[i];
					if(npc.active && !npc.friendly && npc.Hitbox.Intersects(Player.Hitbox))
                    {
						collidingNPC = npc;
						break;
                    }
                }
				if (collidingNPC != null && Main.myPlayer == Player.whoAmI && !Player.HasBuff(BuffID.ChaosState))
				{
					Vector2 toNPC = collidingNPC.Center - Player.Center;
					int direction = 1;
					if (Player.Center.X > collidingNPC.Center.X)
						direction = -1;
					Vector2 otherSide = new Vector2(collidingNPC.Center.X + (collidingNPC.width / 2 + 96) * direction, Player.Center.Y - 16 + toNPC.X * 0.1f);
					Projectile.NewProjectile(new EntitySource_OnHit(collidingNPC, Player), Player.Center, toNPC.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<RelocatorBeam>(), Player.statDefense + 1, collidingNPC.whoAmI, Player.whoAmI, otherSide.X, otherSide.Y);
					modifiers.FinalDamage *= 0; //Take no damage from colliding
					Player.immuneTime = 4;
					Player.immune = true;
				}
			}
		}
        public override void OnHurt(Player.HurtInfo info)
		{
			if (Main.myPlayer == Player.whoAmI)
			{
				int finalDamage = (int)info.Damage;
				if (VMincubator && finalDamage < Player.statLife && !Player.HasBuff(ModContent.BuffType<DilationSickness>()))
				{
					if (!info.PvP)
					{
						SOTSWorld.SetTimeFreeze(Player, 240 + finalDamage);
					}
				}
				else if (VoidAnomaly)
				{
					if (!info.PvP)
					{
						Player.AddBuff(ModContent.BuffType<VoidMetamorphosis>(), 240 + finalDamage, true);
					}
				}
			}
			if (Main.myPlayer == Player.whoAmI && OnHitCD <= 0)
			{
				if (info.Damage > 5)
				{
					if (shardOnHit > 0)
					{
						for (int i = 0; i < shardOnHit; i++)
						{
							Vector2 circularSpeed = new Vector2(0, -12).RotatedBy(MathHelper.ToRadians(i * (360f / shardOnHit)));
							Projectile.NewProjectile(Player.GetSource_Misc("PreHurt by anything"), Player.Center, circularSpeed, ModContent.ProjectileType<ShatterShard>(), 10 + bonusShardDamage, 3f, Player.whoAmI);
						}
					}
					if (CactusSpineDamage > 0)
					{
						int amt = Main.rand.Next(14, 24);
						for (int i = 0; i < 18; i++)
						{
							Vector2 circularSpeed = new Vector2(0, -Main.rand.NextFloat(1.6f, 2.8f)).RotatedBy(MathHelper.ToRadians(i * (360f / amt)) + Main.rand.NextFloat(-5, 5));
							Projectile.NewProjectile(Player.GetSource_Misc("PreHurt by anything"), Player.Center, circularSpeed, ModContent.ProjectileType<CactusSpine>(), CactusSpineDamage, 1.5f, Player.whoAmI);
						}
					}
				}
				if (OnHitCD <= 0)
				{
					onhitdamage = info.Damage;
					onhit = 2;
				}
				OnHitCD = 15;
			}
			if (AmberRing && Main.myPlayer == Player.whoAmI)
				GrantRandomRingBuff(Player);
			if(InverseAmberRing)
            {
                if (Player.statLife <= Player.statLifeMax2 * 0.9f)
                {
					IncreaseBuffDurations(Player, 0, -0.5f, 0, true);
                }
            }
			if(DamageGenerateMoney > 0 && Main.myPlayer == Player.whoAmI)
			{
				VoidPlayer.SpawnCoins(Player, DamageGenerateMoney * (10 + info.Damage + info.SourceDamage));
			}
        }
        //int shotCounter = 0;
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//shotCounter++;
			if(PurpleBalloon && item.fishingPole > 0)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(50));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<PurpleBobber>(), damage, type, Player.whoAmI);
			}
			/*if(snakeSling && item.DamageType == DamageClass.Ranged && item.damage > 3 && shotCounter % 5 == 0)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(8));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X * 1.1f, perturbedSpeed.Y * 1.1f, ModContent.ProjectileType<Pebble>(), damage, knockback, Player.whoAmI);
			}*/
			/*if(backUpBow && item.DamageType == DamageClass.Ranged)
			{
				Vector2 perturbedSpeed = -velocity;
				Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<BackupArrow>(), (int)(damage * 0.45f) + 1, knockback, Player.whoAmI);
			}*/
			if(DoubleVisionActive && item.fishingPole > 0)
			{
				for(int i = BonusFishingLines; i > 0; i--)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i % 2 == 0 ? i * 6 : i * -6));
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, Player.whoAmI);
				}
			}
			return true;
		}
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			if(item.useAmmo == AmmoID.Arrow && BlazingQuiver)
            {
				if(type == ProjectileID.WoodenArrowFriendly)
                {
					type = ModContent.ProjectileType<BlazingArrow>();
					damage += 2;
                }
            }
        }
        public override float UseAnimationMultiplier(Item item)
		{
			return UseTimeMultiplier(item);
		}
		//public override float UseTimeMultiplier(Item item)
		//{
		//	float standard = attackSpeedMod;
		//	int time = item.useAnimation;
		//	int cannotPass = 2;
		//	float current = time / standard;
		//	if (current < cannotPass)
		//	{
		//		standard = time / 2f;
		//	}
		//	if (item.channel == false || item.type == ModContent.ItemType<Items.OlympianAxe>())
		//		return 1f / standard;
		//	return base.UseTimeMultiplier(item);
		//}
		public override void ModifyHitNPCWithProj(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
		{
			if (SparkleDamage && !target.immortal)
			{
				if (Main.myPlayer == Player.whoAmI && (target.lifeMax <= target.life) && (projectile == null || projectile.type != ModContent.ProjectileType<Projectiles.Earth.Glowmoth.IlluminationSparkle>()))
				{
					float direction = Player.direction;
					if (projectile != null)
						direction = Math.Sign(projectile.velocity.X);
					for (int i = 0; i < 10; i++)
					{
						Projectile.NewProjectile(new EntitySource_OnHit(projectile, target), target.Center, new Vector2(1, 0) * direction, ModContent.ProjectileType<Projectiles.Earth.Glowmoth.IlluminationSparkle>(), 1, 1f, Main.myPlayer, target.whoAmI, 4 + 6 * i);
					}
				}
			}
		}
		public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Item, consider using ModifyHitNPC instead */
		{
			float damageMultiplier = CritBonusMultiplier; //since this value is 1, and crit damage does 2x damage, a value of 1.2f will increase damage by 40% on the players side (assuming crit damage as 100% base).
			if (item.type == ModContent.ItemType<AncientSteelSword>() || item.type == ModContent.ItemType<AncientSteelGreatPickaxe>())
			{
				damageMultiplier += 0.5f;
			}
			modifiers.CritDamage *= damageMultiplier;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (ModPlayer(Player).SpiritSymphony)
			{
				if (target.type != ModContent.NPCType<Lux>() && DebuffNPC.spirits.Contains(target.type))
				{
					modifiers.FinalDamage *= 2;
				}
			}
			if (InverseDiamondRing)
				modifiers.DisableCrit();
			else
            {
                modifiers.CritDamage.Flat += CritBonusDamage;
            }
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (curseVisionCounter >= 60)
			{
				if (target.HasBuff(ModContent.BuffType<CurseVision>()))
				{
					curseVisionCounter = -60;
					if (Main.myPlayer == Player.whoAmI)
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<VisionFlare>(), (int)(hit.SourceDamage * 1.4f), 0, Player.whoAmI);
				}
			}
			if(hit.Crit)
			{
				if (CritManasteal > 0 && maxCritManastealPerSecondTimer > 0)
				{
					maxCritManastealPerSecondTimer -= CritManasteal;
					if (Main.myPlayer == Player.whoAmI)
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<HealProj>(), 1, 0, Player.whoAmI, CritManasteal, 3);
				}
				if (CritLifesteal > 0 && maxCritLifestealPerSecondTimer > 0)
				{
					maxCritLifestealPerSecondTimer -= CritLifesteal;
					if (Main.myPlayer == Player.whoAmI)
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<HealProj>(), 0, 0, Player.whoAmI, CritLifesteal, 6);
				}
				if (CritVoidsteal > 0 && maxCritVoidStealPerSecondTimer > 0)
				{
					maxCritVoidStealPerSecondTimer -= CritVoidsteal;
					if (Main.myPlayer == Player.whoAmI)
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<HealProj>(), 2, 0, Player.whoAmI, CritVoidsteal, 5);
				}
				int randBuff = Main.rand.Next(3);
				if (randBuff == 2 && CritCurseFire)
				{
					bool canTrigger = Main.rand.NextFloat(1) >= 1 * (cursedIcoCD / 120f);
					if (canTrigger)
					{
						cursedIcoCD = 180;
						SOTSUtils.PlaySound(SoundID.Item93, (int)target.Center.X, (int)target.Center.Y, 0.9f);
						target.AddBuff(BuffID.CursedInferno, 900, false);
						int numberProjectiles = 4;
						int rand = Main.rand.Next(360);
						if (Main.myPlayer == Player.whoAmI)
						{
							for (int i = 0; i < numberProjectiles; i++)
							{
								Vector2 perturbedSpeed = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(i * 90 + rand));
								Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CursedThunder>(), hit.SourceDamage, 0, Player.whoAmI, 2);
							}
						}
					}
				}
				else if (randBuff == 1 && (CritFrost || CritCurseFire))
				{
					bool canTrigger = Main.rand.NextFloat(1) >= 1 * (iceIcoCD / 120f);
					if (canTrigger)
					{
						iceIcoCD = 180;
						target.AddBuff(BuffID.Frostburn, 900, false);
						if (Main.myPlayer == Player.whoAmI)
						{
							if (CritFrost)
								Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<IcePulseSummon>(), hit.SourceDamage * 2, 0, Player.whoAmI, 3);
							else
								Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<IcePulseSummon>(), hit.SourceDamage, 0, Player.whoAmI, 3);
						}
					}
				}
				else if (randBuff == 0 && (CritFire || CritCurseFire))
				{
					bool canTrigger = Main.rand.NextFloat(1) >= 1 * (fireIcoCD / 120f);
					if (canTrigger)
					{
						fireIcoCD = 180;
						target.AddBuff(BuffID.OnFire, 900, false);
						if (Main.myPlayer == Player.whoAmI)
						{
							if (CritCurseFire && CritFire)
							{
								Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<SharangaBlastSummon>(), hit.SourceDamage * 2, 0, Player.whoAmI, 3);
							}
							else
								Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<SharangaBlastSummon>(), hit.SourceDamage, 0, Player.whoAmI, 3);
						}
					}
				}
			}
			if (target.life <= 0)
			{
				if (Main.myPlayer == Player.whoAmI)
				{
					if (BlueFireOrange && BlueFire)
					{
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<BluefireCrush>(), (int)(hit.SourceDamage * 0.7f), 0, Main.myPlayer, 2);
					}
					else if (BlueFire)
					{
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<BluefireCrush>(), (int)(hit.SourceDamage * 0.4f), 0, Main.myPlayer);
					}
					else if (BlueFireOrange)
					{
						Projectile.NewProjectile(new EntitySource_OnHit(Player, target), target.Center, Vector2.Zero, ModContent.ProjectileType<BluefireCrush>(), (int)(hit.SourceDamage * 0.3f), 0, Main.myPlayer, 1);
                    }
                    if (TopazRing)
                    {
						GrantRandomRingBuff(Player);
                    }
                }
			}
		}
		public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
			if(healValue > 0)
				healValue += additionalHeal;
        }
        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            if (healValue > 0)
                healValue += additionalPotionMana;
		}
        public override bool PreItemCheck()
		{
			return base.PreItemCheck();
        }
		public float screenShakeMultiplier = 0f;
        public override void ModifyScreenPosition()
        {
			Vector2 screenDimensions = new Vector2(Main.screenWidth, Main.screenHeight);
			bool seenSubspace = false;
			bool seenCamera = false;
			for(int i = 0; i < 1000; i++)
            {
				Projectile projectile = Main.projectile[i];
				if(projectile.type == ModContent.ProjectileType<Projectiles.Celestial.SubspaceEye>() && projectile.active)
                {
					seenSubspace = true;
					int current = projectile.alpha;
					current -= 50;
					if (current < 0)
						current = 0;
					float percent = current / 205f;
					if((int)projectile.ai[1] == -1)
					{
						percent *= 0.5f;
						Vector2 toSubEye = projectile.Center - Player.Center;
						if (toSubEye.Length() < 4000f)
							Main.screenPosition.X = (Main.screenPosition.X * (1f - percent)) + ((projectile.Center.X - (screenDimensions.X / 2)) * percent);
					}
					else
					{
						Vector2 toSubEye = projectile.Center - Player.Center;
						if (toSubEye.Length() < 4000f)
							Main.screenPosition = (Main.screenPosition * (1f - percent)) + ((new Vector2(projectile.Center.X, projectile.Center.Y) - (screenDimensions / 2)) * percent);
					}
					break;
                }
				if(!seenSubspace)
				{
					if (projectile.type == ModContent.ProjectileType<DreamingFrame>() && projectile.active && projectile.owner == Main.myPlayer) //need to include a lerp back to normal so the transition isn't jarring (this will be done with a new death projectile)
					{
						float percent = projectile.alpha / 255f;
						percent = Math.Clamp(percent, 0, 1);
						seenCamera = true;
						Main.screenPosition = Vector2.Lerp(Main.screenPosition, new Vector2((int)projectile.Center.X, (int)projectile.Center.Y) - (screenDimensions / 2), 0.25f * (1 - percent));
						Main.screenPosition.X = (int)Main.screenPosition.X;
						Main.screenPosition.Y = (int)Main.screenPosition.Y;
					}
					else if (!seenCamera && projectile.type == ModContent.ProjectileType<FluidFollower>() && projectile.active && projectile.owner == Main.myPlayer)
					{
						Vector2 toSubEye = projectile.Center - Player.Center;
						if (toSubEye.Length() < 4000f)
							Main.screenPosition = new Vector2((int)projectile.Center.X, (int)projectile.Center.Y) - (screenDimensions / 2);
					}
				}
            }
			if(screenShakeMultiplier > 0)
			{
				Vector2 offset = new Vector2(0, Main.rand.NextFloat(1f) * screenShakeMultiplier).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
				Main.screenPosition += offset;
				screenShakeMultiplier -= 0.75f;
				screenShakeMultiplier *= 0.95f;
			}
			else
            {
				screenShakeMultiplier = 0;
            }
			base.ModifyScreenPosition();
        }
        public override void UpdateBadLifeRegen()
		{
			if (Player.HasBuff(ModContent.BuffType<AbyssalInferno>()))
            {
				if(Player.lifeRegen > 0)
					Player.lifeRegen = 0;
				Player.lifeRegenTime = 0;
				Player.lifeRegen -= 60;
            }
			base.UpdateBadLifeRegen();
        }
		float delayPotionCounter = 0;
        public override void PreUpdateBuffs()
        {
			bool DelayPotionDegrade = false;
			if(PotionBuffDegradeRate <= 1f)
            {
				float Increment = 1f - PotionBuffDegradeRate;
				delayPotionCounter += Increment;
				if(delayPotionCounter >= 1)
                {
					delayPotionCounter -= 1;
					DelayPotionDegrade = true;
				}
			}
			bool inverseAmberRingInRange = InverseAmberRing && Player.statLife > Player.statLifeMax2 * 0.9f;

            if (DrainDebuffs && !inverseAmberRingInRange)
			{
				int totalDrainedDebuffs = 0;
				for (int i = 0; i < Player.buffTime.Length; i++)
				{
					int type = Player.buffType[i];
					if (Player.HasBuff(type) && Main.debuff[type] && type != ModContent.BuffType<DilationSickness>() && type != BuffID.PotionSickness && type != BuffID.Chilled)
					{
						if(Player.buffTime[i] > 30) 
                        {
							Player.buffTime[i]--;
							totalDrainedDebuffs++;
						}
					}
				}
				int drainAmt = 1;
				if (totalDrainedDebuffs >= 1)
					IncreaseBuffDurations(Player, drainAmt, 0f, drainAmt, true);
			}
			if(inverseAmberRingInRange)
			{
				DelayPotionDegrade = true;
			}
			DrainDebuffs = false;
			if (Player.HasBuff(ModContent.BuffType<Harmony>()) || DelayPotionDegrade)
            {
				IncreaseBuffDurations(Player, 1, 0, 1, false, InverseAmberRing);
			}
			PotionBuffDegradeRate = 1f;
            InverseAmberRing = false;
        }
		public static void IncreaseBuffDurations(Player player, int time, float timeBonusMultiplier = 0, int maximumTimeBonus = 1, bool affectAll = false, bool allowUnder30Seconds = false)
		{
			for (int i = 0; i < player.buffTime.Length; i++)
			{
				int type = player.buffType[i];
				if (!Main.debuff[type] && (((player.buffTime[i] > 1800 || harmonyWhitelist.Contains(type) || allowUnder30Seconds) && type != ModContent.BuffType<Harmony>()) || affectAll))
				{
					if (type == ModContent.BuffType<Attuned>())
						continue;
					int totalIncrease = time;
					if(timeBonusMultiplier != 0)
                    {
						int bonusTime = (int)(timeBonusMultiplier * player.buffTime[i]); //gets a percentage increase
						totalIncrease += bonusTime;
                    }
					if (totalIncrease > maximumTimeBonus)
						totalIncrease = maximumTimeBonus;
					player.buffTime[i] += totalIncrease;
				}
			}
		}
		public static void GrantRandomRingBuff(Player player)
        {
			player.AddBuff(Main.rand.NextFromList(BuffID.Swiftness,
				BuffID.Regeneration, BuffID.Ironskin, BuffID.Wrath,
				BuffID.Rage, ModContent.BuffType<SoulAccess>(), ModContent.BuffType<Roughskin>(),
				BuffID.Thorns, BuffID.ManaRegeneration, ModContent.BuffType<DiamondSkin>()), 1800, false);
        }
		public static void GrantRandomWishingStarBuff(Player player, int duration)
        {
			player.AddBuff(Main.rand.NextFromList(BuffID.Swiftness, BuffID.Wrath,
				BuffID.Rage, BuffID.ManaRegeneration, BuffID.MagicPower), duration * 60 + 50, false);
        }
		public static bool ZoneForest(Player player)
		{
			return !player.GetModPlayer<SOTSPlayer>().PyramidBiome && player.ZoneForest;
		}
        public override void ModifyItemScale(Item item, ref float scale)
        {
			if(item.CountsAsClass(DamageClass.Melee))
				scale *= meleeItemScale;
		}
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
			if(AmmoConsumptionModifier > 0)
            {
				if (Main.rand.NextFloat(1f) < AmmoConsumptionModifier)
					return false;
            }
            return base.CanConsumeAmmo(weapon, ammo);
        }
        public override void OnConsumeAmmo(Item weapon, Item ammo)
        {
			if (InfinityPouch)
			{
				ammo.stack++;
				float voidCost = 1f;
				if (ammo.value >= 100) //If item is worth silver, cost more
				{
					voidCost = 2;
                }
				if (ammo.value >= 10000) //gold
				{
					voidCost = 3;
				}
				if( ammo.value >= 1000000) //platinum
				{
					voidCost = 4;
				}
                Player.VoidPlayer().voidMeter -= voidCost;
            }
        }
        public override bool? CanAutoReuseItem(Item item)
        {
			if(AutoReuseAnything)
			{
				return true;
			}
            return base.CanAutoReuseItem(item);
        }
		public int ManaSpentCounter = 0;
        public override void OnConsumeMana(Item item, int manaConsumed)
        {
			if(Player.whoAmI == Main.myPlayer && item.CountsAsClass(DamageClass.Magic))
            {
				if (WishingStar)
				{
					if (!Items.ChestItems.WishingStar.IsAlternate)
					{
						ManaSpentCounter += manaConsumed;
						if (ManaSpentCounter >= 100)
                        {
                            GrantRandomWishingStarBuff(Player, ManaSpentCounter);
                            CastWishingStar(Player, Main.MouseWorld, 100);
							ManaSpentCounter -= 100;
						}
					}
				}
				else
					ManaSpentCounter = 0;
            }
        }
		public static void CastWishingStar(Player player, Vector2 position, int damage)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				int direction = player.Center.X < position.X ? 1 : -1;
				Vector2 spawnPos = new Vector2(MathHelper.Lerp(player.Center.X - Main.rand.NextFloat(1250, 1450) * direction, position.X, 0.4f), MathHelper.Lerp(player.Center.Y, position.Y, 0.25f) - Main.rand.NextFloat(750, 950));

                Projectile.NewProjectile(player.GetSource_Misc("SOTS:WishingStar"), spawnPos, Main.rand.NextVector2Circular(32, 32), ModContent.ProjectileType<WishingStarProj>(), damage, 1f, Main.myPlayer, position.X, position.Y, Items.ChestItems.WishingStar.IsAlternate ? -1 : 0);
			}
		}
    }
}



