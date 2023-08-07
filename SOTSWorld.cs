using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using SOTS.Items;
using SOTS.Items.ChestItems;
using SOTS.Items.Crushers;
using SOTS.Items.Fragments;
using SOTS.Items.SpiritStaves;
using SOTS.Items.Permafrost;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Potions;
using SOTS.Items.Pyramid;
using SOTS.Items.Void;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
using SOTS.Items.Flails;
using SOTS.Items.Secrets;
using SOTS.Items.Tools;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Otherworld.Blocks;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Items.Pyramid;
using SOTS.Items.Earth;
using static SOTS.SOTS;
using Terraria.Graphics.Effects;
using SOTS.Items.Furniture.Earthen;
using SOTS.Items.Chaos;
using Terraria.IO;
using SOTS.WorldgenHelpers;
using SOTS.Items.Furniture.AncientGold;
using Terraria.UI;
using SOTS.Void;
using SOTS.Items.Invidia;
using SOTS.Items.Furniture.Nature;
using SOTS.Items.Temple;
using SOTS.Items.Furniture.Permafrost;
using Terraria.GameContent.Biomes;
using SOTS.Items.Slime;
using SOTS.NPCs.Town;
using Terraria.DataStructures;
using SOTS.Items.Furniture.Functional;
using SOTS.Items.Conduit;
using Terraria.Graphics.Light;

namespace SOTS
{
    public class SOTSWorld : ModSystem
	{
		public static int GlobalCounter = 0;
		public const float GlobalFreezeStartup = 30f;
		public static int GlobalTimeFreeze = 0;
		public static bool GlobalFrozen = false;
		public static float GlobalFreezeCounter = 0;
		public static float GlobalSpeedMultiplier = 1;
		public static bool IsFrozenThisFrame = false;
		public static LightMode LastLightingMode;
		public static void SyncGemLocks(Player clientSender)
		{
			int playerWhoAmI = clientSender != null ? clientSender.whoAmI : -1;
			var packet = Instance.GetPacket();
			packet.Write((byte)SOTSMessageType.SyncGlobalGemLocks);
			packet.Write(playerWhoAmI);
			packet.Write(RubyKeySlotted);
			packet.Write(SapphireKeySlotted);
			packet.Write(EmeraldKeySlotted);
			packet.Write(TopazKeySlotted);
			packet.Write(AmethystKeySlotted);
			packet.Write(DiamondKeySlotted);
			packet.Write(AmberKeySlotted);
			packet.Write(DreamLampSolved);
			packet.Send();
		}
		public static void SyncTimeFreeze(Player clientSender)
		{
			int playerWhoAmI = clientSender != null ? clientSender.whoAmI : -1;
			var packet = Instance.GetPacket();
			packet.Write((byte)SOTSMessageType.SyncGlobalWorldFreeze);
			packet.Write(playerWhoAmI);
			packet.Write(GlobalTimeFreeze);
			packet.Write(GlobalFrozen);
			packet.Write(GlobalFreezeCounter);
			packet.Write(GlobalSpeedMultiplier);
			packet.Send();
		}
		public static void SyncGlobalCounter()
		{
			var packet = Instance.GetPacket();
			packet.Write((byte)SOTSMessageType.SyncGlobalCounter);
			packet.Write(GlobalCounter);
			packet.Send();
		}
		public static void SetTimeFreeze(Player clientSender, int time)
		{
			GlobalTimeFreeze = time;
			GlobalFrozen = false;
			if ((clientSender == null && Main.netMode == NetmodeID.Server) || Main.netMode != NetmodeID.SinglePlayer)
				SyncTimeFreeze(clientSender);
		}
		public static bool UpdateWhileFrozen() //returns true if frozen
		{
			if (GlobalTimeFreeze > 0 || GlobalFrozen)
			{
				if (!GlobalFrozen)
				{
					if (GlobalSpeedMultiplier > 0)
					{
						GlobalSpeedMultiplier -= 1 / GlobalFreezeStartup;
					}
					else
					{
						GlobalSpeedMultiplier = 0;
						GlobalFrozen = true;
					}
				}
				else
				{
					if (GlobalTimeFreeze > 1)
					{
						GlobalTimeFreeze--;
					}
					else
					{
						GlobalSpeedMultiplier += 1 / GlobalFreezeStartup;
						if (GlobalSpeedMultiplier > 1)
						{
							GlobalSpeedMultiplier = 1;
							GlobalTimeFreeze = 0;
							GlobalFrozen = false;
						}
					}
				}
				if (GlobalTimeFreeze == 0 && Main.netMode == NetmodeID.Server)
				{
					SyncTimeFreeze(null);
				}
			}
			else
			{
				GlobalFrozen = false;
			}
			GlobalFreezeCounter += GlobalSpeedMultiplier;
			if (GlobalFreezeCounter >= 1)
			{
				IsFrozenThisFrame = false;
				GlobalFreezeCounter -= 1;
			}
			else
			{
				IsFrozenThisFrame = true;
			}
			if(GlobalTimeFreeze == 0)
            {
				if (Main.netMode != NetmodeID.Server && Filters.Scene["VMFilter"].IsActive())
				{
					Filters.Scene["VMFilter"].Deactivate();
				}
			}
			else
			{
				if (Main.netMode != NetmodeID.Server)
				{
					if(!Filters.Scene["VMFilter"].IsActive())
						Filters.Scene.Activate("VMFilter", Main.LocalPlayer.Center).GetShader().UseColor(1, 1, 1).UseTargetPosition(Main.LocalPlayer.Center);
					if(Filters.Scene["VMFilter"].IsActive())
					{
						float progress = 1 - GlobalSpeedMultiplier;
						Filters.Scene["VMFilter"].GetShader().UseProgress(progress).UseTargetPosition(Main.LocalPlayer.Center).UseColor(SOTSPlayer.VoidMageColor(Main.LocalPlayer).ToVector3()).UseIntensity(SOTS.Config.coloredTimeFreeze ? 0.12f : 0);
					}
				}
			}
			return IsFrozenThisFrame;
		}
		public static void Update()
        {
			GlobalCounter++;
			if(GlobalCounter % 300 == 0)
            {
				if (Main.netMode == NetmodeID.Server)
					SyncGlobalCounter();
            }
			LastLightingMode = Lighting.Mode;
		}
		public override void PreSaveAndQuit()
		{
			SOTSConfig.voidBarNeedsLoading = 0;
			SOTSConfig.PreviousBarMode = 0;
			for(int i = 0; i < 1000; i ++)
            {
				Projectile projectile = Main.projectile[i];
				if(projectile.active && projectile.type == ModContent.ProjectileType<PressProjectile>())
                {
					PressProjectile press = projectile.ModProjectile as PressProjectile;
					press.ResetHydraulic();
                }
            }
			PreSaveAndQuit_AwaitThreadedTasks();
		}
		private void PreSaveAndQuit_AwaitThreadedTasks()
		{
			while (PhaseWorldgenHelper.Generating)
			{
			}
		}
		public static int SecretFoundMusicTimer = 0;
        public static int planetarium = 0;
		public static int pyramidBiome = 0;
		public static int phaseBiome = 0;

		public static bool downedGlowmoth = false;
		public static bool downedPinky = false;
		public static bool downedCurse = false;

		public static bool downedAmalgamation = false;
		public static bool downedLux = false;
		public static bool downedSubspace = false;
		public static bool downedAdvisor = false;

		public static bool RubyKeySlotted = false;
		public static bool SapphireKeySlotted = false;
		public static bool EmeraldKeySlotted = false;
		public static bool TopazKeySlotted = false;
		public static bool AmethystKeySlotted = false;
		public static bool DiamondKeySlotted = false;
		public static bool AmberKeySlotted = false;
		public static bool DreamLampSolved = false;
		public void ResetWorldVariables()
		{
			GlobalCounter = 0;
			GlobalTimeFreeze = 0;
			GlobalFrozen = false;
			GlobalFreezeCounter = 0;
			GlobalSpeedMultiplier = 1;
			downedGlowmoth = false;
			downedPinky = false;
			downedAdvisor = false;
			downedCurse = false;
			downedAmalgamation = false;
			downedLux = false;
			downedSubspace = false;

			RubyKeySlotted = false;
			SapphireKeySlotted = false;
			EmeraldKeySlotted = false;
			TopazKeySlotted = false;
			AmethystKeySlotted = false;
			DiamondKeySlotted = false;
			AmberKeySlotted = false;
			DreamLampSolved = false;
		}
		public override void OnWorldLoad()
		{
			SOTSConfig.voidBarNeedsLoading = 1;
            SOTSConfig.PreviousBarMode = 0;
			ResetWorldVariables();
		}
        public override void OnWorldUnload()
		{
			ResetWorldVariables();
		}
		public override void SaveWorldData(TagCompound tag)
		{
			tag["DownedGlowmoth"] = downedGlowmoth;
			tag["DownedPinky"] = downedPinky;
			tag["DownedCurse"] = downedCurse;
			tag["DownedAdvisor"] = downedAdvisor;
			tag["DownedAmalgamation"] = downedAmalgamation;
			tag["DownedLux"] = downedLux;
			tag["DownedSubspace"] = downedSubspace;

			tag["RubyKey"] = RubyKeySlotted;
			tag["SapphireKey"] = SapphireKeySlotted;
			tag["EmeraldKey"] = EmeraldKeySlotted;
			tag["TopazKey"] = TopazKeySlotted;
			tag["AmethystKey"] = AmethystKeySlotted;
			tag["DiamondKey"] = DiamondKeySlotted;
			tag["AmberKey"] = AmberKeySlotted;
			tag["DreamLamp"] = DreamLampSolved;
		}
        public override void LoadWorldData(TagCompound tag)
		{
			downedGlowmoth = tag.GetBool("DownedGlowmoth");
			downedPinky = tag.GetBool("DownedPinky");
			downedCurse = tag.GetBool("DownedCurse");
			downedAdvisor = tag.GetBool("DownedAdvisor");
			downedAmalgamation = tag.GetBool("DownedAmalgamation");
			downedLux = tag.GetBool("DownedLux");
			downedSubspace = tag.GetBool("DownedSubspace");

			RubyKeySlotted = tag.GetBool("RubyKey");
			SapphireKeySlotted = tag.GetBool("SapphireKey");
			EmeraldKeySlotted = tag.GetBool("EmeraldKey");
			TopazKeySlotted = tag.GetBool("TopazKey");
			AmethystKeySlotted = tag.GetBool("AmethystKey");
			DiamondKeySlotted = tag.GetBool("DiamondKey");
			AmberKeySlotted = tag.GetBool("AmberKey");
			DreamLampSolved = tag.GetBool("DreamLamp");
		}
		public override void NetSend(BinaryWriter writer) {
			BitsByte flags = new BitsByte();
			flags[0] = downedPinky;
			flags[1] = downedAdvisor;
			flags[2] = downedAmalgamation;
			flags[3] = downedCurse;
			flags[4] = downedLux;
			flags[5] = downedSubspace;
			flags[6] = downedGlowmoth;

			BitsByte gemFlags = new BitsByte();
			gemFlags[0] = RubyKeySlotted;
			gemFlags[1] = SapphireKeySlotted;
			gemFlags[2] = EmeraldKeySlotted;
			gemFlags[3] = TopazKeySlotted;
			gemFlags[4] = AmethystKeySlotted;
			gemFlags[5] = DiamondKeySlotted;
			gemFlags[6] = AmberKeySlotted;
			gemFlags[7] = DreamLampSolved;

			writer.Write(flags);
			writer.Write(gemFlags);
			writer.Write(GlobalCounter);
		}
		public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			downedPinky = flags[0];
			downedAdvisor = flags[1];
			downedAmalgamation = flags[2];
			downedCurse = flags[3];
			downedLux = flags[4];
			downedSubspace = flags[5];
			downedGlowmoth = flags[6];

			BitsByte gemFlags = reader.ReadByte();
			RubyKeySlotted = gemFlags[0];
			SapphireKeySlotted = gemFlags[1];
			EmeraldKeySlotted = gemFlags[2];
			TopazKeySlotted = gemFlags[3];
			AmethystKeySlotted = gemFlags[4];
			DiamondKeySlotted = gemFlags[5];
			AmberKeySlotted = gemFlags[6];
			DreamLampSolved = gemFlags[7];

			GlobalCounter = reader.ReadInt32();
		}
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int desert = tasks.FindIndex(genpass => genpass.Name.Equals("Full Desert"));
			tasks.Insert(desert + 1, new PassLegacy("SOTS: Additional Desert", AdjacentDesertGeneration));

			int genIndexOres = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
			int genIndexGeodes = tasks.FindIndex(genpass => genpass.Name.Equals("Lakes"));
			int genIndexTraps = tasks.FindIndex(genpass => genpass.Name.Equals("Traps"));
			int genIndexSunflowers = tasks.FindIndex(genpass => genpass.Name.Equals("Sunflowers"));
			int genIndexEnd = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
			int oceanTunnel = tasks.FindIndex(genpass => genpass.Name.Equals("Create Ocean Caves"));

			tasks.RemoveAt(oceanTunnel);
			tasks.Insert(oceanTunnel, new PassLegacy("SOTS: Ocean Caves", OceanCaveGeneration));
			tasks.Insert(genIndexOres, new PassLegacy("SOTS: Ores", GenSOTSOres));
			tasks.Insert(genIndexGeodes + 1, new PassLegacy("SOTS: Geodes", GenSOTSGeodes));
			tasks.Insert(genIndexTraps + 1, new PassLegacy("SOTS: Structures", delegate (GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = Language.GetTextValue("Mods.SOTS.ModifyWorldGenTasks.GeneratingSurfaceStructures");
				SOTSWorldgenHelper.PlaceSetpiecesInMushroomBiome();
				StarterHouseWorldgenHelper.GenerateStarterHouseFull();
				int iceY = -1;
				int iceX = -1;
				int totalChecks = 0;
				for (int xCheck = Main.rand.Next(Main.maxTilesX); xCheck != -1; xCheck = Main.rand.Next(Main.maxTilesX))
				{
					for (int ydown = 0; ydown != -1; ydown++)
					{
						Tile tile = Framing.GetTileSafely(xCheck, ydown);
						bool allValid = totalChecks > 100 || (SOTSWorldgenHelper.TrueTileSolid(xCheck + 1, ydown) && SOTSWorldgenHelper.TrueTileSolid(xCheck + 2, ydown) && SOTSWorldgenHelper.TrueTileSolid(xCheck - 1, ydown) && SOTSWorldgenHelper.TrueTileSolid(xCheck - 2, ydown));
						if (tile.HasTile && tile.TileType == TileID.SnowBlock)
						{
							iceY = ydown;
							break;
						}
						else if (tile.HasTile)
						{
							break;
						}
					}
					if (iceY != -1)
					{
						iceX = xCheck;
						break;
					}
					totalChecks++;
				}
				int iceArtifactPositionX = iceX;
				int iceArtifactPositionY = iceY;
				SOTSWorldgenHelper.GenerateIceRuin(iceX, iceY);

				int dungeonSide = -1; // -1 = dungeon on left, 1 = dungeon on right
				if (Main.dungeonX > (int)(Main.maxTilesX / 2))
				{
					dungeonSide = 1;
				}
				bool coconutGenerated = false;
				while (!coconutGenerated)
				{
					int direction = dungeonSide;
					int fromBorder = 70 + Main.rand.Next(20);
					if (direction == -1) //go to the opposite side of the dungeon
					{
						fromBorder = Main.maxTilesX - fromBorder;
					}
					for (int j = 0; j < Main.maxTilesY; j++)
					{
						Tile tile = Framing.GetTileSafely(fromBorder, j);
						if (tile.LiquidType == 0 && tile.LiquidAmount > 1)
						{
							SOTSWorldgenHelper.GenerateCoconutIsland(Mod, fromBorder, j, direction);
							coconutGenerated = true;
							break;
						}
					}
				}
			}));
			tasks.Insert(genIndexSunflowers + 1, new PassLegacy("SOTS: Abandoned Village", delegate (GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = Language.GetTextValue("Mods.SOTS.ModifyWorldGenTasks.GeneratingAbandonedVillage");
				//AbandonedVillageWorldgenHelper.PlaceAbandonedVillage();
				SOTSWorldgenHelper.PlacePeanuts();
			}));
			tasks.Insert(genIndexEnd + 5, new PassLegacy("SOTS: Planetarium", delegate (GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = Language.GetTextValue("Mods.SOTS.ModifyWorldGenTasks.GeneratingSkyArtifacts");
				int dungeonSide = -1; // -1 = dungeon on left, 1 = dungeon on right
				if (Main.dungeonX > (int)(Main.maxTilesX / 2))
				{
					dungeonSide = 1;
				}
				SOTSWorldgenHelper.FindAndGenerateDamocles(dungeonSide);
				SOTSWorldgenHelper.SpamCrystals(true);
				SOTSWorldgenHelper.FindAndGenerateBigGeode(-dungeonSide); //jungle is opp dungeon side
				int pX = -1;
				int checks = 0;
				if (dungeonSide == -1)
				{
					int xCheck = Main.rand.Next(400, Main.maxTilesX / 2);
					for (; xCheck != -1; xCheck = Main.rand.Next(400, Main.maxTilesX / 2))
					{
						pX = xCheck;
						bool validLocation = false;
						for (int ydown = 0; ydown != -1; ydown++)
						{
							Tile tile = Framing.GetTileSafely(xCheck, ydown);
							if (tile.HasTile && (tile.TileType == TileID.SnowBlock || tile.TileType == TileID.IceBlock))
							{
								validLocation = true;
								break;
							}
							else if (tile.HasTile && Main.tileSolid[tile.TileType])
							{
								break;
							}
						}
						checks++;
						if (validLocation || checks >= 50)
						{
							bool force = false;
							if (checks >= 55)
							{
								force = true;
							}
							int yLocation = 140;
							if (Main.maxTilesX > 4000) //small worlds
							{
								yLocation = 120;
							}
							if (Main.maxTilesX > 6000) //medium worlds
							{
								yLocation = 130;
							}
							if (Main.maxTilesX > 8000) //big worlds
							{
								yLocation = 140;
							}
							if (SOTSWorldgenHelper.GeneratePlanetariumFull(Mod, pX, yLocation, force))
							{
								break;
							}
						}
					}
				}
				if (dungeonSide == 1)
				{
					int xCheck = Main.rand.Next(Main.maxTilesX / 2, Main.maxTilesX - 400);
					for (; xCheck != -1; xCheck = Main.rand.Next(Main.maxTilesX / 2, Main.maxTilesX - 400))
					{
						pX = xCheck;
						bool validLocation = false;
						for (int ydown = 0; ydown != -1; ydown++)
						{
							Tile tile = Framing.GetTileSafely(xCheck, ydown);
							if (tile.HasTile && (tile.TileType == TileID.SnowBlock || tile.TileType == TileID.IceBlock))
							{
								validLocation = true;
								break;
							}
							else if(tile.HasTile && Main.tileSolid[tile.TileType])
                            {
								break;
                            }
						}
						checks++;
						if (validLocation || checks >= 50)
						{
							bool force = false;
							if (checks >= 55)
							{
								force = true;
							}
							if (SOTSWorldgenHelper.GeneratePlanetariumFull(Mod, pX, 140, force))
							{
								break;
							}
						}
					}
				}

				bool hasDoneEvil = false;
				int overrideCounter = 0;
				bool hasDoneJungle = false;
				bool hasDoneDesert = false;
				int xCord = Main.rand.Next(240, Main.maxTilesX - 240);
				for (; xCord != -1; xCord = Main.rand.Next(240, Main.maxTilesX - 240))
				{
					overrideCounter++;
					if (hasDoneEvil && hasDoneJungle && hasDoneDesert)
                    {
						xCord = -1;
						return;
                    }
					for (int ydown = 0; ydown != -1; ydown++)
					{
						Tile tile = Framing.GetTileSafely(xCord, ydown);
						if (tile.HasTile && Main.tileSolid[tile.TileType])
						{
							if(tile.TileType == TileID.JungleGrass || tile.TileType == TileID.JunglePlants || tile.TileType == TileID.JunglePlants2 || overrideCounter > 100)
                            {
								int y = 140 + Main.rand.Next(50);
								if(!hasDoneJungle)
								{
									hasDoneJungle = SOTSWorldgenHelper.GenerateBiomeChestIslands(xCord, y, 3, Mod);
								}
								break;
							}
							if (tile.TileType == TileID.Crimstone || tile.TileType == TileID.CrimsonGrass || (tile.TileType == TileID.Crimsand && overrideCounter > 20) || overrideCounter > 100)
							{
								int y = 140 + Main.rand.Next(50);
								if (!hasDoneEvil)
								{
									hasDoneEvil = SOTSWorldgenHelper.GenerateBiomeChestIslands(xCord, y, 0, Mod);
								}
								break;
							}
							if (tile.TileType == TileID.Ebonstone || tile.TileType == TileID.CorruptGrass || (tile.TileType == TileID.Ebonsand && overrideCounter > 20) || overrideCounter > 100)
							{
								int y = 140 + Main.rand.Next(50);
								if (!hasDoneEvil)
								{
									hasDoneEvil = SOTSWorldgenHelper.GenerateBiomeChestIslands(xCord, y, 1, Mod);
								}
								break;
							}
							if (tile.TileType == TileID.Sand || overrideCounter > 100)
							{
								int y = 140 + Main.rand.Next(50);
								if (!hasDoneDesert)
								{
									hasDoneDesert = SOTSWorldgenHelper.GenerateBiomeChestIslands(xCord, y, 5, Mod);
								}
								break;
							}
							break;
						}
					}
				}
			}));
			tasks.Insert(genIndexEnd + 6, new PassLegacy("SOTS: Pyramid", delegate (GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = Language.GetTextValue("Mods.SOTS.ModifyWorldGenTasks.GeneratingAPyramid");
				PyramidWorldgenHelper.GenerateSOTSPyramid(Mod);
				SOTSWorldgenHelper.SpamCrystals(false);
			}));
			tasks.Insert(genIndexEnd + 7, new PassLegacy("SOTS: GemStructures", delegate (GenerationProgress progress, GameConfiguration configuration)
			{
				progress.Message = Language.GetTextValue("Mods.SOTS.ModifyWorldGenTasks.GeneratingGemStructures");
				GemStructureWorldgenHelper.GenerateGemStructures();
				SOTSWorldgenHelper.CleanUpFloatingTrees(); //Updates tile frames for trees
			}));
		}
		private void AdjacentDesertGeneration(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Language.GetTextValue("Mods.SOTS.AdjacentDesertGeneration.SEverywhere");
			int centerX = WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width / 2;
			int widthX = WorldGen.UndergroundDesertLocation.Width / 2;
			int centerY = 0;
			int direction = (centerX > Main.maxTilesX / 2) ? 1 : -1;
			for(int j = 0; j < Main.maxTilesY; j++)
            {
				Tile tile = Framing.GetTileSafely(centerX, j);
				if((tile.HasTile && tile.TileType == TileID.Sand) || (tile.WallType == WallID.HardenedSand))
                {
					centerY = j;
					break;
                }
            }
			Point toSpawnDesert = new Point(centerX + widthX * direction + WorldGen.genRand.Next(-10, 11), centerY - 60);
			Desertify(toSpawnDesert);
		}
		private void Desertify(Point origin)
		{
			DunesBiome dunesBiome = WorldGen.configuration.CreateBiome<DunesBiome>();
			dunesBiome.Place(origin, WorldGen.structures);
		}
		private void OceanCaveGeneration(GenerationProgress progress, GameConfiguration configuration)
		{
			int dungeonSide = 0; // 0 = dungeon on left, 1 = dungeon on right
			if (Main.dungeonX > (int)(Main.maxTilesX / 2))
			{
				dungeonSide = 1;
			}
			Mod Calamity;
			bool calAvailable = ModLoader.TryGetMod("CalamityMod", out Calamity);
			if (calAvailable)
			{
				dungeonSide = dungeonSide == 0 ? 1 : 0; //ocean cave will not generate in calamities sulphiric sea
			}
			for (int side = 0; side < 2; side++)
			{
				if ((!calAvailable && (WorldGen.genRand.NextBool(4) || WorldGen.drunkWorldGen)) || side == dungeonSide) //SOTS will always generate a ocean cave on the same side as the dungeon
				{
					progress.Message = Lang.gen[90].Value;
					int x = WorldGen.genRand.Next(55, 95);
					if (side == 1)
					{
						x = WorldGen.genRand.Next(Main.maxTilesX - 95, Main.maxTilesX - 55);
					}
					int y;
					for (y = 0; !Main.tile[x, y].HasTile; y++) { } ;
					WorldGen.oceanCave(x, y);
				}
			}
		}
		private void GenSOTSOres(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Language.GetTextValue("Mods.SOTS.Common.GenSOTSOres");
			SOTSWorldgenHelper.GenerateEvostoneInMushroomBiome();
			float max = 240;
			if (Main.maxTilesX > 6000) //medium worlds
				max = 360;
			if (Main.maxTilesX > 8000) //big worlds
				max = 480;
			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 0.2f); k++)
			{
				int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int y = WorldGen.genRand.Next((int)WorldGen.rockLayerLow, (int)(WorldGen.rockLayer + Main.maxTilesY - 200) / 2);
				if (SOTSWorldgenHelper.GenerateFrigidIceOre(x, y))
				{
					max--;
					if (max <= 0)
						return;
				}
			}
		}
		private void GenSOTSGeodes(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Language.GetTextValue("Mods.SOTS.Common.GenSOTSGeodes");
			int max = 60;
			if (Main.maxTilesX > 6000) //medium worlds
				max = 90;
			if (Main.maxTilesX > 8000) //big worlds
				max = 120;
			int top = (int)WorldGen.rockLayerLow;
			int bottom = (int)(Main.maxTilesY - 240);
			float range = bottom - top;
			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 0.2f); k++)
			{
				int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int y = WorldGen.genRand.Next(top, bottom);
				Tile tile = Main.tile[x, y];
				bool valid = tile.TileType == TileID.Stone || tile.TileType == TileID.Dirt;
				if (valid)
				{
					float percent = (y - top) / range + Main.rand.NextFloat(-0.1f, 0.1f);
					percent = MathHelper.Clamp(percent, 0, 1);
					int size = (int)MathHelper.Lerp(8, 18, percent);
					float depthMult = size / 16f;
					SOTSWorldgenHelper.GenerateVibrantGeode((int)x, (int)y, size, (int)(size * Main.rand.NextFloat(0.9f, 1.1f)), depthMult, (float)Math.Sqrt(depthMult));
					max--;
					if (max <= 0)
						break;
				}
			}
		}
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
			planetarium = tileCounts[ModContent.TileType<DullPlatingTile>()] + tileCounts[ModContent.TileType<AvaritianPlatingTile>()];// - 50 * tileCounts[ModContent.TileType<Items.Gems.SOTSGemLockTiles>()];
			phaseBiome = tileCounts[ModContent.TileType<PhaseOreTile>()];
			pyramidBiome = tileCounts[ModContent.TileType<SarcophagusTile>()] + tileCounts[ModContent.TileType<RefractingCrystalBlockTile>()] + tileCounts[ModContent.TileType<AcediaGatewayTile>()];
		}
        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            base.ModifyHardmodeTasks(list);
        }
        public override void PostWorldGen()
		{
			string worldName = Main.worldName;

			List<int> starItemPool2 = new List<int>() { ModContent.ItemType<SkywareBattery>(), ModContent.ItemType<Poyoyo>(), ModContent.ItemType<SupernovaHammer>(), ModContent.ItemType<StarshotCrossbow>(), ModContent.ItemType<LashesOfLightning>(), ModContent.ItemType<Starbelt>(), ModContent.ItemType<TwilightAssassinsCirclet>() };
			List<int> lightItemPool2 = new List<int>() { ModContent.ItemType<HardlightQuiver>(), ModContent.ItemType<CodeCorrupter>(), ModContent.ItemType<PlatformGenerator>(), ModContent.ItemType<Calculator>(), ModContent.ItemType<TwilightAssassinsLeggings>(), ModContent.ItemType<TwilightFishingPole>(), ModContent.ItemType<ChainedPlasma>(), ModContent.ItemType<OtherworldlySpiritStaff>() };
			List<int> fireItemPool2 = new List<int>() { ModContent.ItemType<BlinkPack>(), ModContent.ItemType<FlareDetonator>(), ModContent.ItemType<VibrancyModule>(), ModContent.ItemType<CataclysmMusketPouch>(), ModContent.ItemType<TerminatorAcorns>(), ModContent.ItemType<TwilightAssassinsChestplate>(), ModContent.ItemType<InfernoHook>() };

			List<int> starItemPool = new List<int>() { ModContent.ItemType<SkywareBattery>(), ModContent.ItemType<Poyoyo>(), ModContent.ItemType<SupernovaHammer>(), ModContent.ItemType<StarshotCrossbow>(),ModContent.ItemType<LashesOfLightning>(), ModContent.ItemType<Starbelt>(), ModContent.ItemType<TwilightAssassinsCirclet>() };
			List<int> lightItemPool = new List<int>() { ModContent.ItemType<HardlightQuiver>(), ModContent.ItemType<CodeCorrupter>(), ModContent.ItemType<PlatformGenerator>(), ModContent.ItemType<Calculator>(), ModContent.ItemType<TwilightAssassinsLeggings>(), ModContent.ItemType<TwilightFishingPole>(), ModContent.ItemType<ChainedPlasma>(), ModContent.ItemType<OtherworldlySpiritStaff>() };
			List<int> fireItemPool = new List<int>() { ModContent.ItemType<BlinkPack>(), ModContent.ItemType<FlareDetonator>(), ModContent.ItemType<VibrancyModule>(), ModContent.ItemType<CataclysmMusketPouch>(), ModContent.ItemType<TerminatorAcorns>(), ModContent.ItemType<TwilightAssassinsChestplate>(), ModContent.ItemType<InfernoHook>() };

			List<int> LihzahrdItems2 = new List<int>() { ModContent.ItemType<LihzahrdTail>(), ModContent.ItemType<Revolution>(), ModContent.ItemType<SupernovaScatter>(), ModContent.ItemType<Helios>(), ModContent.ItemType<Pyrocide>() };
			List<int> LihzahrdItems = new List<int>() { ModContent.ItemType<LihzahrdTail>(), ModContent.ItemType<Revolution>(), ModContent.ItemType<SupernovaScatter>(), ModContent.ItemType<Helios>(), ModContent.ItemType<Pyrocide>() };
			// Iterate chests
			GemStructureWorldgenHelper.FillChestsWithLoot();
			foreach (Chest chest in Main.chest.Where(c => c != null))
			{
				// Get a chest
				var tile = Main.tile[chest.x, chest.y]; // the chest tile 
				if (tile.TileType == ModContent.TileType<LockedStrangeChest>() || tile.TileType == ModContent.TileType<LockedSkywareChest>() || tile.TileType == ModContent.TileType<LockedMeteoriteChest>())
				{
					int type = tile.TileType == ModContent.TileType<LockedStrangeChest>() ? 0 : tile.TileType == ModContent.TileType<LockedSkywareChest>() ? 1 : 2;
					int slot = 39;
					for (int i = 0; i < 39; i++)
					{
						if (chest.item[i].type == ItemID.None && i < slot)
						{
							slot = i;
						}
					}
					int firstType = 0;
					if (type == 0)
					{
						firstType = lightItemPool2[Main.rand.Next(lightItemPool2.Count)];
						if (lightItemPool.Count > 0)
						{
							int rand = Main.rand.Next(lightItemPool.Count);
							firstType = lightItemPool[rand];
							lightItemPool.RemoveAt(rand);
						}
					}
					else if (type == 1)
					{
						firstType = starItemPool2[Main.rand.Next(starItemPool2.Count)];
						if (starItemPool.Count > 0)
						{
							int rand = Main.rand.Next(starItemPool.Count);
							firstType = starItemPool[rand];
							starItemPool.RemoveAt(rand);
						}
					}
					else
					{
						firstType = fireItemPool2[Main.rand.Next(fireItemPool2.Count)];
						if (fireItemPool.Count > 0)
						{
							int rand = Main.rand.Next(fireItemPool.Count);
							firstType = fireItemPool[rand];
							fireItemPool.RemoveAt(rand);
						}
					}
					chest.item[slot].SetDefaults(firstType); //add primary item to chest loot
					slot++;

					if (!Main.rand.NextBool(3)) //Adds ores and shards to chest
					{
						int amt = Main.rand.Next(5, 9); //5 to 8
						int rand = Main.rand.Next(9);
						int secondType = ModContent.ItemType<HardlightAlloy>();
						if (type == 0 || rand == 0)
							secondType = ModContent.ItemType<HardlightAlloy>();
						if (type == 1 || rand == 1)
							secondType = ModContent.ItemType<StarlightAlloy>();
						if (type == 2 || rand == 2)
							secondType = ModContent.ItemType<OtherworldlyAlloy>();
						chest.item[slot].SetDefaults(secondType);
						chest.item[slot].stack = amt;
						slot++;
					}
					else
					{
						int secondType = ModContent.ItemType<TwilightShard>();
						int amt = Main.rand.Next(6, 10); //6 to 9
						chest.item[slot].SetDefaults(secondType);
						chest.item[slot].stack = amt;
						slot++;
					}

					if (!Main.rand.NextBool(3)) //Adds ammo or stars to chest
					{
						int amt = Main.rand.Next(66, 101); //66 to 100
						int[] ammoItems = new int[] { ItemID.JestersArrow, ItemID.HellfireArrow, ItemID.MeteorShot, ItemID.FallenStar, ModContent.ItemType<ExplosiveKnife>() };
						int rand = Main.rand.Next(ammoItems.Length);
						int thirdType = ammoItems[rand];
						if (thirdType == ItemID.FallenStar) //cut quantity if stars 13 - 20
						{
							amt /= 5;
						}
						chest.item[slot].SetDefaults(thirdType);
						chest.item[slot].stack = amt;
						slot++;
					}
					if(Main.rand.Next(5) <= 2) //adds healing
                    {
						int fourthType = ItemID.RestorationPotion;
						int amt = Main.rand.Next(10, 15); //10 to 14
						chest.item[slot].SetDefaults(fourthType);
						chest.item[slot].stack = amt;
						slot++;
					}
					if (!Main.rand.NextBool(5)) //adds first potions 80%
					{
						int amt = Main.rand.Next(2) + 1; //1 to 2
						int[] potions1 = new int[] { ModContent.ItemType<AssassinationPotion>(), ModContent.ItemType<BrittlePotion>(), ModContent.ItemType<RoughskinPotion>(), ModContent.ItemType<SoulAccessPotion>(), ModContent.ItemType<VibePotion>(), ItemID.LifeforcePotion, ItemID.HeartreachPotion, ItemID.ManaRegenerationPotion, ItemID.MagicPowerPotion, ItemID.AmmoReservationPotion, ItemID.InfernoPotion};
						int rand = Main.rand.Next(potions1.Length);
						int fifthType = potions1[rand];
						chest.item[slot].SetDefaults(fifthType);
						chest.item[slot].stack = amt;
						slot++;
					}
					if (Main.rand.NextBool(5)) //adds second potions 20%
					{
						int amt = 1;
						int[] potions2 = new int[] { ModContent.ItemType<BlightfulTonic>(), ModContent.ItemType<GlacialTonic>(), ModContent.ItemType<SeismicTonic>(), ModContent.ItemType<StarlightTonic>(), ModContent.ItemType<DoubleVisionPotion>() };
						int rand = Main.rand.Next(potions2.Length);
						int fifthType = potions2[rand];
						chest.item[slot].SetDefaults(fifthType);
						chest.item[slot].stack = amt;
						slot++;
					}

					if (!Main.rand.NextBool(3)) //Adds torches
					{
						int amt = Main.rand.Next(15, 30); //15 to 29
						int sixthType = ItemID.Torch;
						chest.item[slot].SetDefaults(sixthType);
						chest.item[slot].stack = amt;
						slot++;
					}
					if (Main.rand.NextBool(5)) //20%
					{
						int amt = 1; 
						int seventhType = ModContent.ItemType<StrangeKey>();
						if (type == 0)
						{
							if (Main.rand.NextBool(7))
								seventhType = ModContent.ItemType<StrangeKey>();
							else if(Main.rand.NextBool(2))
								seventhType = ModContent.ItemType<SkywareKey>();
							else
								seventhType = ModContent.ItemType<MeteoriteKey>();

						}
						if (type == 1)
						{
							if (Main.rand.NextBool(7))
								seventhType = ModContent.ItemType<SkywareKey>();
							else if (Main.rand.NextBool(2))
								seventhType = ModContent.ItemType<StrangeKey>();
							else
								seventhType = ModContent.ItemType<MeteoriteKey>();
						}
						if (type == 2)
						{
							if (Main.rand.NextBool(7))
								seventhType = ModContent.ItemType<MeteoriteKey>();
							else if (Main.rand.NextBool(2))
								seventhType = ModContent.ItemType<SkywareKey>();
							else
								seventhType = ModContent.ItemType<StrangeKey>();
						}
						chest.item[slot].SetDefaults(seventhType);
						chest.item[slot].stack = amt;
						slot++;
					}
					if (Main.rand.NextBool(2))
					{
						chest.item[slot].SetDefaults(ItemID.GoldCoin);
						chest.item[slot].stack = Main.rand.Next(3) + 4; // 4 to 6
						slot++;
					}
				}
				if (tile.TileType == ModContent.TileType<EarthenPlatingStorageTile>())
				{
					int slot = 0;
					Tile tile2 = Main.tile[chest.x, chest.y + 2];
					if (tile2.TileType == ModContent.TileType<VibrantBrickTile>() && tile.WallType == ModContent.WallType<VibrantWallWall>()) //locked chest
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<PerfectStar>());
						slot++;
						chest.item[slot].SetDefaults(ModContent.ItemType<MinersPickaxe>());
						chest.item[slot].stack = Main.rand.Next(11) + 20; // 20 to 30
						slot++;
						chest.item[slot].SetDefaults(ItemID.LifeCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.ManaCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.GoldCoin);
						chest.item[slot].stack = Main.rand.Next(3) + 3; // 3 to 5
						slot++;
					}
					else if (tile2.TileType == ModContent.TileType<EarthenPlatingTile>() && tile.TileFrameX < 36)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<VisionAmulet>());
						slot++;
						if(Main.rand.NextBool(2))
                        {
							chest.item[slot].SetDefaults(ModContent.ItemType<ManicMiner>());
							slot++;
						}
						else
						{
							chest.item[slot].SetDefaults(ItemID.BonePickaxe);
							slot++;
						}
						chest.item[slot].SetDefaults(ModContent.ItemType<MinersPickaxe>());
						chest.item[slot].stack = Main.rand.Next(11) + 20; // 20 to 30
						slot++;
						chest.item[slot].SetDefaults(ItemID.LifeCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.ManaCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.GoldCoin);
						chest.item[slot].stack = Main.rand.Next(3) + 1; // 1 to 3
						slot++;
					}
				}
				if (tile.TileType == ModContent.TileType<NaturePlatingCapsuleTile>())
				{
					int slot = 0;
					Tile tile2 = Main.tile[chest.x, chest.y + 2];
					if (tile.TileFrameX >= 36 && tile2.TileType == ModContent.TileType<NaturePlatingTile>() && tile.WallType == ModContent.WallType<NaturePlatingWallWall>() && Framing.GetTileSafely(chest.x -1, chest.y).TileType == ModContent.TileType<NaturePlatingCapsuleTile>()) //locked chest
					{
						if (worldName.Contains("Starbound") || worldName.Contains("starbound"))
						{
							chest.item[slot].SetDefaults(ModContent.ItemType<Earthshaker>());
							chest.item[slot].stack = 1;
							slot++;
						}
						else
						{
							chest.item[slot].SetDefaults(ModContent.ItemType<NatureHydroponics>());
							chest.item[slot].stack = 3;
							slot++;
						}
						chest.item[slot].SetDefaults(ItemID.HerbBag);
						chest.item[slot].stack = 10;
						slot++;
						chest.item[slot].SetDefaults(ModContent.ItemType<FoulConcoction>());
						chest.item[slot].stack = 10;
						slot++;
						chest.item[slot].SetDefaults(ItemID.LifeCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.ManaCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.GoldCoin);
						chest.item[slot].stack = Main.rand.Next(3) + 3; // 3 to 5
						slot++;
					}
				}
				if (tile.TileType == ModContent.TileType<PermafrostPlatingCapsuleTile>() && tile.WallType == ModContent.WallType<HardIceBrickWallWall>())
				{
					int slot = 0;
					chest.item[slot].SetDefaults(ModContent.ItemType<GlazeBow>()); //Will be replaced with Glaze Repeater
					slot++;
					chest.item[slot].SetDefaults(ModContent.ItemType<StrawberryIcecream>());
					chest.item[slot].stack = 10;
					slot++;
					chest.item[slot].SetDefaults(ItemID.LifeCrystal);
					slot++;
					chest.item[slot].SetDefaults(ItemID.ManaCrystal);
					slot++;
					chest.item[slot].SetDefaults(ItemID.GoldCoin);
					chest.item[slot].stack = Main.rand.Next(3) + 1; // 1 to 3
					slot++;
				}
				if (tile.TileType == ModContent.TileType<RuinedChestTile>())
				{
					int slot = 0;
					Tile tile2 = Main.tile[chest.x, chest.y + 2];
					if(tile2.TileType == ModContent.TileType<PyramidBrickTile>())
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<CoconutGun>());
						slot++;
						chest.item[slot].SetDefaults(ModContent.ItemType<CoconutMilk>());
						chest.item[slot].stack = 10; // 3 to 5
						slot++;
						chest.item[slot].SetDefaults(ItemID.LifeCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.ManaCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.GoldCoin);
						chest.item[slot].stack = Main.rand.Next(3) + 3; // 3 to 5
						slot++;
					}
					else if (tile2.TileType == ModContent.TileType<DullPlatingTile>())
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<BoneClapper>());
						slot++;
						chest.item[slot].SetDefaults(ModContent.ItemType<AvocadoSoup>());
						chest.item[slot].stack = 10;
						slot++;
						chest.item[slot].SetDefaults(ItemID.LifeCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.ManaCrystal);
						slot++;
						chest.item[slot].SetDefaults(ItemID.GoldCoin);
						chest.item[slot].stack = Main.rand.Next(5) + 6; // 6 to 10
						slot++;
					}
					else if(tile.TileFrameX < 36) //If the chest is not locked
                    {
						chest.item[slot].SetDefaults(ModContent.ItemType<WorldgenScanner>());
						slot++;
						chest.item[slot].SetDefaults(ItemID.Rope);
						chest.item[slot].stack = 100;
						slot++;
						chest.item[slot].SetDefaults(ItemID.Grenade);
						chest.item[slot].stack = 5;
						slot++;
						chest.item[slot].SetDefaults(ItemID.LesserHealingPotion);
						chest.item[slot].stack = 5;
						slot++;
						chest.item[slot].SetDefaults(ItemID.LesserManaPotion);
						chest.item[slot].stack = 10;
						slot++;
						chest.item[slot].SetDefaults(ItemID.CopperHammer);
						slot++;
					}
					else if(tile2.TileType == TileID.GrayBrick && tile.WallType == WallID.StoneSlab)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<AncientSteelBar>());
						chest.item[slot].stack = WorldGen.genRand.Next(6) + 7; //7-12
						slot++;
						chest.item[slot].SetDefaults(ItemID.EmptyBucket);
						chest.item[slot].stack = WorldGen.genRand.Next(3) + 2; //2-4
						slot++;
						chest.item[slot].SetDefaults(ItemID.CanOfWorms);
						chest.item[slot].stack = WorldGen.genRand.Next(3) + 2; //2-4
						slot++;
						chest.item[slot].SetDefaults(ItemID.GillsPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(3) + 2; //2-4
						slot++;
						chest.item[slot].SetDefaults(ModContent.ItemType<NightmarePotion>());
						chest.item[slot].stack = 1;
						slot++;
						chest.item[slot].SetDefaults(ItemID.RestorationPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(4) + 4; //4-7
						slot++;
						chest.item[slot].SetDefaults(ModContent.ItemType<FragmentOfTide>());
						chest.item[slot].stack = WorldGen.genRand.Next(3) + 2; //2-4
						slot++;
						chest.item[slot].SetDefaults(ModContent.ItemType<FragmentOfEvil>());
						chest.item[slot].stack = WorldGen.genRand.Next(3) + 3; //3-5
						slot++;
						chest.item[slot].SetDefaults(ItemID.SilverCoin);
						chest.item[slot].stack = WorldGen.genRand.Next(40, 91); //40-90
						slot++;
					}
					else if (tile2.TileType == TileID.Stone && tile.WallType == WallID.GrayBrick)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<AncientSteelBar>());
						chest.item[slot].stack = WorldGen.genRand.Next(7) + 8; //8-14
						slot++;
						chest.item[slot].SetDefaults(WorldGen.genRand.NextBool(3) ? ItemID.Glowstick : ItemID.Torch);
						chest.item[slot].stack = WorldGen.genRand.Next(41) + 40; //40-80
						slot++;
						chest.item[slot].SetDefaults(ItemID.MiningPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(3) + 2; //2-4
						slot++;
						chest.item[slot].SetDefaults(WorldGen.genRand.NextBool(3) ? ItemID.StickyBomb : ItemID.Bomb);
						chest.item[slot].stack = WorldGen.genRand.Next(6) + 6; //6-11
						slot++;
						chest.item[slot].SetDefaults(WorldGen.genRand.NextBool(3) ? ItemID.StickyDynamite : ItemID.Dynamite);
						chest.item[slot].stack = WorldGen.genRand.Next(3) + 3; //3-5
						slot++;
						chest.item[slot].SetDefaults(ItemID.RestorationPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 2; //2-3
						slot++;
						chest.item[slot].SetDefaults(WorldGen.genRand.NextBool(3) ? ModContent.ItemType<FragmentOfEarth>() : ModContent.ItemType<FragmentOfEvil>());
						chest.item[slot].stack = WorldGen.genRand.Next(3) + 4; //4-6
						slot++;
						if (WorldGen.genRand.NextBool(3))
						{
							chest.item[slot].SetDefaults(!WorldGen.genRand.NextBool(3) ? ModContent.ItemType<AncientSteelGreatPickaxe>() : ItemID.GoldPickaxe);
							chest.item[slot].stack = 1;
							slot++;
						}
						if (WorldGen.genRand.NextBool(6))
						{
							chest.item[slot].SetDefaults(ItemID.MiningHelmet);
							chest.item[slot].stack = 1;
							slot++;
						}
						if (WorldGen.genRand.NextBool(6))
						{
							chest.item[slot].SetDefaults(ItemID.MiningShirt);
							chest.item[slot].stack = 1;
							slot++;
						}
						if (WorldGen.genRand.NextBool(6))
						{
							chest.item[slot].SetDefaults(ItemID.MiningPants);
							chest.item[slot].stack = 1;
							slot++;
						}
					}
				}
				if (tile.TileType == ModContent.TileType<PyramidChestTile>())
				{
					int slot = 39;
					for(int i = 0; i < 39; i++)
					{
						if(chest.item[i].type == ItemID.None && i < slot)
						{
							slot = i;
						}
					}
				
					int rand = WorldGen.genRand.Next(12);
					if(rand == 0)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<Aten>());
						slot++;
					}
					if(rand == 1)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<EmeraldBracelet>());
						slot++;
					}
					if(rand == 2)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<ImperialPike>());
						slot++;
					}
					if(rand == 3)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<PharaohsCane>());
						slot++;
					}
					if(rand == 4)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<PitatiLongbow>());
						slot++;
					}
					if(rand == 5)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<RoyalMagnum>());
						slot++;
					}
					if(rand == 6)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<SandstoneEdge>());
						slot++;
					}
					if(rand == 7)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<SandstoneWarhammer>());
						slot++;
					}
					if(rand == 8)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<ShiftingSands>());
						slot++;
					}
					if(rand == 9)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<SunlightAmulet>());
						slot++;
					}
					if(rand == 10)
					{
						chest.item[slot].SetDefaults(ItemID.FlyingCarpet);
						slot++;
					}
					if(rand == 11)
					{
						chest.item[slot].SetDefaults(ItemID.SandstorminaBottle);
						slot++;
					}
					
					int second = WorldGen.genRand.Next(10);
					if(second == 0)
					{
						chest.item[slot].SetDefaults(848);
						slot++;
						chest.item[slot].SetDefaults(866);
						slot++;
					}
					if(second == 1)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<AnubisHat>());
						slot++;
					}
					if(second > 1)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<JuryRiggedDrill>());
						chest.item[slot].stack = WorldGen.genRand.Next(35) + 11;
						slot++;
					}
					
					int third = WorldGen.genRand.Next(12);
					if(third == 0)
					{
						chest.item[slot].SetDefaults(ItemID.MiningPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(third == 1)
					{
						chest.item[slot].SetDefaults(ItemID.SpelunkerPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(third == 2)
					{
						chest.item[slot].SetDefaults(ItemID.BuilderPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(third == 3)
					{
						chest.item[slot].SetDefaults(ItemID.ShinePotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(third == 4)
					{
						chest.item[slot].SetDefaults(ItemID.NightOwlPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(third == 5)
					{
						chest.item[slot].SetDefaults(ItemID.ArcheryPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(third == 6)
					{
						chest.item[slot].SetDefaults(ItemID.EndurancePotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(third == 7)
					{
						chest.item[slot].SetDefaults(ItemID.SummoningPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					
					
					int fourth = WorldGen.genRand.Next(12);
					if(fourth == 0)
					{
						chest.item[slot].SetDefaults(ItemID.WrathPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(fourth == 1)
					{
						chest.item[slot].SetDefaults(ItemID.HeartreachPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(fourth == 2)
					{
						chest.item[slot].SetDefaults(ItemID.RagePotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(fourth == 3)
					{
						chest.item[slot].SetDefaults(ItemID.TitanPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					if(fourth == 4)
					{
						chest.item[slot].SetDefaults(ItemID.TeleportationPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 1;
						slot++;
					}
					
					int fifth = WorldGen.genRand.Next(8);
					if(fifth == 0)
					{
						chest.item[slot].SetDefaults(ItemID.GoldBar);
						chest.item[slot].stack = WorldGen.genRand.Next(9) + 5;
						slot++;
					}
					if(fifth == 1)
					{
						chest.item[slot].SetDefaults(ItemID.PlatinumBar);
						chest.item[slot].stack = WorldGen.genRand.Next(9) + 5;
						slot++;
					}
					if(fifth == 2)
					{
						chest.item[slot].SetDefaults(ItemID.CrimtaneBar);
						chest.item[slot].stack = WorldGen.genRand.Next(9) + 5;
						slot++;
					}
					if(fifth == 3)
					{
						chest.item[slot].SetDefaults(ItemID.DemoniteBar);
						chest.item[slot].stack = WorldGen.genRand.Next(9) + 5;
						slot++;
					}
					
					int thirdLast = WorldGen.genRand.Next(4);
					if(thirdLast == 0)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<ExplosiveKnife>());
						chest.item[slot].stack = WorldGen.genRand.Next(41) + 20;
						slot++;
					}
					if(thirdLast == 1)
					{
						chest.item[slot].SetDefaults(ItemID.HellfireArrow);
						chest.item[slot].stack = WorldGen.genRand.Next(41) + 20;
						slot++;
					}
					if(thirdLast == 2)
					{
						chest.item[slot].SetDefaults(ItemID.AngelStatue);
						slot++;
					}
					
					
					int secLast = WorldGen.genRand.Next(2);
					if(secLast == 0)
					{
						chest.item[slot].SetDefaults(ItemID.RecallPotion);
						chest.item[slot].stack = WorldGen.genRand.Next(2) + 2;
						slot++;
					}
					
					int last = WorldGen.genRand.Next(3);
					if(last == 0)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<AncientGoldTorch>());
						chest.item[slot].stack = WorldGen.genRand.Next(20) + 15;
						slot++;
					}
					if(last == 1)
					{
						chest.item[slot].SetDefaults(ItemID.GoldCoin);
						chest.item[slot].stack = WorldGen.genRand.Next(3) + 2;
						slot++;
					}
					if(last == 2)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<RoyalGoldBrick>());
						chest.item[slot].stack = WorldGen.genRand.Next(51) + 50;
						slot++;
					}
				}
				if (tile.TileType == TileID.Containers || tile.TileType == TileID.Containers2)
				{
					int slot = 39;
					for (int i = 0; i < 39; i++)
					{
						if (chest.item[i].type == ItemID.None && i < slot)
						{
							slot = i;
						}
					}

					TileObjectData tileData = TileObjectData.GetTileData(tile);
					int style = TileObjectData.GetTileStyle(tile);
					Tile tile2 = Main.tile[chest.x, chest.y + 2];
					Tile tile3 = Main.tile[chest.x, chest.y + 5];
					if ((style == 13 && tile.TileType == TileID.Containers2) || (style >= 23 && style <= 27 && tile.TileType == TileID.Containers) && (tile3.TileType == ModContent.TileType<DullPlatingTile>() || tile3.TileType == ModContent.TileType<AvaritianPlatingTile>()))
                    {
						int importantItem = 0;
						int importantItem2 = 0;
						int consumable = 0;
						int consumableQuant = 10;
						if (style == 23)
                        {
							importantItem = ModContent.ItemType<TangleStaff>();
							importantItem2 = ModContent.ItemType<DissolvingNature>();
							consumable = ModContent.ItemType<CoconutMilk>();
						}
						if (style == 24)
						{
							importantItem = ModContent.ItemType<PathogenRegurgitator>();
							importantItem2 = ModContent.ItemType<DissolvingUmbra>();
							consumable = ModContent.ItemType<AlmondMilk>();
						}
						if (style == 25)
						{
							importantItem = ModContent.ItemType<RebarRifle>();
							importantItem2 = ModContent.ItemType<DissolvingUmbra>();
							consumable = ModContent.ItemType<AlmondMilk>();
						}
						if (style == 26)
						{
							importantItem = ModContent.ItemType<StarcallerStaff>();
							importantItem2 = ModContent.ItemType<DissolvingBrilliance>();
							consumable = ModContent.ItemType<DigitalCornSyrup>();
						}
						if (style == 27)
						{
							importantItem = ModContent.ItemType<Sawflake>();
							importantItem2 = ModContent.ItemType<DissolvingAurora>();
							consumable = ModContent.ItemType<StrawberryIcecream>();
							consumableQuant = 20;
						}
						if (style == 13)
						{
							importantItem = ModContent.ItemType<DuneSplicer>();
							importantItem2 = ModContent.ItemType<DissolvingEarth>();
							consumable = ModContent.ItemType<CursedCaviar>();
							consumableQuant = 15;
						}
						if (!chest.item[0].active)
						{
							if(importantItem != 0)
							{
								chest.item[slot].SetDefaults(importantItem);
								slot++;
							}
							if (importantItem2 != 0)
							{
								chest.item[slot].SetDefaults(importantItem2);
								slot++;
							}
							if (consumable != 0)
							{
								chest.item[slot].SetDefaults(consumable);
								chest.item[slot].stack = consumableQuant;
								slot++;
							}
						}
						if (!Main.rand.NextBool(4)) //Adds ammo or stars to chest
						{
							int amt = Main.rand.Next(151, 334); //(ushort)ModContent.TileType<PyramidBrickTile>() to 333
							int[] ammoItems = new int[] { ItemID.ChlorophyteArrow, ItemID.ChlorophyteBullet, ItemID.RocketIII };
							int rand = Main.rand.Next(ammoItems.Length);
							int thirdType = ammoItems[rand];
							chest.item[slot].SetDefaults(thirdType);
							chest.item[slot].stack = amt;
							slot++;
						}
						if (!Main.rand.NextBool(4)) //adds healing
						{
							int fourthType = ItemID.GreaterHealingPotion;
							int amt = Main.rand.Next(14, 21); //14 to 20
							chest.item[slot].SetDefaults(fourthType);
							chest.item[slot].stack = amt;
							slot++;
						}
						if (!Main.rand.NextBool(5)) //adds first potions 80%
						{
							int amt = Main.rand.Next(3) + 4; //4 to 6
							int[] potions1 = new int[] { ModContent.ItemType<SoulAccessPotion>(), ItemID.LifeforcePotion };
							int rand = Main.rand.Next(potions1.Length);
							int fifthType = potions1[rand];
							chest.item[slot].SetDefaults(fifthType);
							chest.item[slot].stack = amt;
							slot++;
						}
						if (Main.rand.NextBool(3)) //adds second potions 33%
						{
							int amt = 1;
							int[] potions2 = new int[] { ModContent.ItemType<BlightfulTonic>(), ModContent.ItemType<GlacialTonic>(), ModContent.ItemType<SeismicTonic>(), ModContent.ItemType<StarlightTonic>(), ModContent.ItemType<AbyssalTonic>(), ModContent.ItemType<DoubleVisionPotion>() };
							int rand = Main.rand.Next(potions2.Length);
							int fifthType = potions2[rand];
							chest.item[slot].SetDefaults(fifthType);
							chest.item[slot].stack = amt;
							slot++;
						}
						if (!Main.rand.NextBool(4)) //Adds torches
						{
							int amt = Main.rand.Next(15, 30); //15 to 29
							int sixthType = ItemID.Torch;
							chest.item[slot].SetDefaults(sixthType);
							chest.item[slot].stack = amt;
							slot++;
						}
					}
					int firstType = chest.item[0].type;
					if (style == 1 || firstType == ItemID.ShoeSpikes || firstType == ItemID.LavaCharm || firstType == ItemID.HermesBoots || firstType == ItemID.CloudinaBottle || firstType == ItemID.BandofRegeneration || firstType == ItemID.Extractinator || firstType == ItemID.FlareGun || firstType == ItemID.MagicMirror || firstType == ItemID.EnchantedBoomerang) //gold chest
					{
						if (WorldGen.genRand.NextBool(16))
						{
							chest.item[slot].SetDefaults(ModContent.ItemType<CrushingAmplifier>());
							slot++;
						}
					}
					if(WorldGen.genRand.NextBool(25))
					{
						if(WorldGen.genRand.NextBool(2))
						{
							chest.item[slot].SetDefaults(ModContent.ItemType<ShieldofDesecar>());
						}
						else
						{
							chest.item[slot].SetDefaults(ModContent.ItemType<ShieldofStekpla>());
						}
						slot++;
					}
					if(WorldGen.genRand.NextBool(5) && (chest.item[0].type == ItemID.ShoeSpikes || chest.item[0].type == ItemID.ClimbingClaws))
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<SpikedClub>());
						slot++;
					}
					if(WorldGen.genRand.NextBool(7) && chest.item[0].type == ItemID.HermesBoots)
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<WingedKnife>());
						slot++;
					}
					if(WorldGen.genRand.NextBool(2) && (chest.item[0].type == ItemID.Starfury || chest.item[0].type == ItemID.ShinyRedBalloon || chest.item[0].type == ItemID.CreativeWings)) //this should be fledgling wings now
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<TinyPlanet>());
						slot++;
					}
					if(firstType == ItemID.LihzahrdPowerCell)
					{
						int addItem = LihzahrdItems2[Main.rand.Next(LihzahrdItems2.Count)]; //this picks a random item
						if (LihzahrdItems.Count > 0) //this picks a random item that has not been picked yet
						{
							int rand = Main.rand.Next(LihzahrdItems.Count);
							addItem = LihzahrdItems[rand];
							LihzahrdItems.RemoveAt(rand);
						}
						chest.item[slot].SetDefaults(addItem); //add item to chest loot
						slot++;
						if(addItem == ModContent.ItemType<SupernovaScatter>())
						{
							chest.item[slot].SetDefaults(ModContent.ItemType<SolarBullet>()); //add item to chest loot
							chest.item[slot].stack = WorldGen.genRand.Next(400, 601); //add item to chest loot
							slot++;
						}
					}
				}
			}
		}

        /***
		/ This is where the old Mod. stuff was moved to. Basically, it has to belong in a ModSystem Now. I don't know which one to put it in yet, so it stays here for now.
		/ A lot of old parameters were also made into Static parameters. This shouldn't break anything, but is noted in case it does.
		/
		/
		/
		/
		*/
        public static void LoadUI()
		{
			if (!Main.dedServ)
			{
				VoidUI = new VoidUI();
				VoidUI.Activate();
				_VoidUserInterface = new UserInterface();
				_VoidUserInterface.SetState(VoidUI);

				_lastScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
				_lastViewSize = Main.ViewSize;
			}
		}
		private static UserInterface _VoidUserInterface;
		internal static VoidUI VoidUI;
		public static float lightingChange = 1f;
		private static Vector2 _lastScreenSize;
		private static Vector2 _lastViewSize;
		public override void PreUpdateEntities()
		{
			if (!Main.dedServ)
			{
				if (_lastScreenSize != new Vector2(Main.screenWidth, Main.screenHeight))
				{
					if (primitives != null)
						primitives.LoadContent(Main.graphics.GraphicsDevice);
					if (SOTSDetours.TargetProj != null)
						SOTSDetours.ResizeTargets();
				}
				_lastScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
				_lastViewSize = Main.ViewSize;
			}
			bool frozen = SOTSWorld.UpdateWhileFrozen();
			if (!frozen)
			{
				SOTSWorld.Update();
				PlayerCount = 0;
				if (Main.netMode != NetmodeID.SinglePlayer) //updating this on Dedicated Server just in case I accidentally forgot to make dust not spawn in multiplayer somewhere
				{
					for (int i = 0; i < Main.player.Length; i++)
					{
						Player player = Main.player[i];
						if (player.active)
						{
							PlayerCount++;
							SOTSPlayer.ModPlayer(player).FoamStuff();
						}
					}
				}
				else
				{
					PlayerCount = 1;
					SOTSPlayer.ModPlayer(Main.LocalPlayer).FoamStuff();
				}
			}
			if(Main.netMode != NetmodeID.MultiplayerClient)
				if(!NPC.AnyNPCs(ModContent.NPCType<Archaeologist>()))
				{
					NPC npc = NPC.NewNPCDirect(new EntitySource_SpawnNPC(""), 3200, 3200, ModContent.NPCType<Archaeologist>());
					npc.netUpdate = true;
					Archaeologist.locationTimer = 1000000;
				}
			if (Main.netMode != NetmodeID.Server)
			{
				NPCs.Town.VoidAnomaly.PrepareLocalPlayerShader();
				Vector2 position = NPCs.Town.VoidAnomaly.AnomalyShaderPosition;
				float progress = NPCs.Town.VoidAnomaly.AnomalyShaderProgress;
				float intesity = NPCs.Town.VoidAnomaly.AnomalyIntesity;
				if (position == Vector2.Zero)
				{
					if (Filters.Scene["AnomalyFilter"].IsActive())
					{
						Filters.Scene["AnomalyFilter"].GetShader().UseProgress(progress).UseTargetPosition(position).UseColor(new Vector3(0f, 0f, 0f)).UseIntensity(intesity);
					}
					if (Filters.Scene["AnomalyFilter"].IsActive())
					{
						Filters.Scene["AnomalyFilter"].Deactivate();
					}
				}
				else
				{
					if (!Filters.Scene["AnomalyFilter"].IsActive())
						Filters.Scene.Activate("AnomalyFilter", position).GetShader().UseColor(0, 0, 0).UseTargetPosition(position);
					if (Filters.Scene["AnomalyFilter"].IsActive())
					{
						Filters.Scene["AnomalyFilter"].GetShader().UseProgress(progress).UseTargetPosition(position).UseColor(new Vector3(0f, 0f, 0f)).UseIntensity(intesity);
					}
				}
			}
		}
        public override void PostUpdateProjectiles()
        {
			if (Main.netMode != NetmodeID.Server)
			{
				primitives.UpdateTrails();
			}
		}
		public override void UpdateUI(GameTime gameTime)
        {
			if (_VoidUserInterface != null)
			{
				_VoidUserInterface.Update(gameTime);
			}
		}
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (mouseTextIndex != -1) {
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"SOTS: Void Meter",
					delegate {
						if (VoidUI.visible) {
							_VoidUserInterface.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
		public override void ModifyLightingBrightness(ref float scale)
		{
			scale += lightingChange - 1;
			lightingChange = 1;
		}
		public static float LuxLightingFadeIn = 0;
		public static float PlanetariumLightingFadeIn = 0;
		public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
		{
			if (Main.gameMenu)
			{
				return;
			}
			var player = Main.LocalPlayer;
			SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
			if (sPlayer.zoneLux)
			{

			}
			else if (LuxLightingFadeIn > 0)
			{
				LuxLightingFadeIn -= 0.01f;
			}
			backgroundColor = Color.Lerp(backgroundColor, new Color(15, 0, 30), 0.96f * LuxLightingFadeIn);
			tileColor = Color.Lerp(tileColor, new Color(15, 0, 30), 0.96f * LuxLightingFadeIn);
			//Lighting.GlobalBrightness *= MathHelper.Lerp(1, 0, 0.96f * LuxLightingFadeIn);
			if (sPlayer.PlanetariumBiome)
			{
				if (PlanetariumLightingFadeIn < 1)
					PlanetariumLightingFadeIn += 0.01f;
			}
			else if (PlanetariumLightingFadeIn > 0)
			{
				PlanetariumLightingFadeIn -= 0.01f;
			}
			backgroundColor = Color.Lerp(backgroundColor, new Color(0, 0, 10), 0.9f * PlanetariumLightingFadeIn);
			tileColor = Color.Lerp(tileColor, new Color(0, 0, 10), 0.9f * PlanetariumLightingFadeIn);
			//Lighting.GlobalBrightness *= MathHelper.Lerp(1, 0, 0.9f * PlanetariumLightingFadeIn);
		}
		public override void AddRecipes()
		{
			Recipe.Create(ItemID.SlimeStaff, 1).AddIngredient<Wormwood>(30).AddTile(TileID.Anvils).Register();
			Recipe.Create(ItemID.TeleportationPotion, 1).AddIngredient(ItemID.BottledWater).AddIngredient<SkipSoul>(1).AddIngredient<SkipShard>(1).AddIngredient<FragmentOfOtherworld>(1).AddTile(TileID.Bottles).Register();
			ItemHelpers.InitializeWormholeRecipes();
		}
		public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.SB"), new int[]
			{
				ItemID.SilverBar,
				ItemID.TungstenBar
			});
			RecipeGroup.RegisterGroup("SOTS:SilverBar", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.EM"), new int[]
			{
				ItemID.TissueSample,
				ItemID.ShadowScale
			});
			RecipeGroup.RegisterGroup("SOTS:EvilMaterial", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.EB"), new int[]
			{
				ItemID.CrimtaneBar,
				ItemID.DemoniteBar
			});
			RecipeGroup.RegisterGroup("SOTS:EvilBar", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.GB"), new int[]
			{
				ItemID.GoldBar,
				ItemID.PlatinumBar
			});
			RecipeGroup.RegisterGroup("SOTS:GoldBar", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.GR"), new int[]
			{
				ItemID.RubyRobe,
				ItemID.AmethystRobe,
				ItemID.TopazRobe,
				ItemID.SapphireRobe,
				ItemID.EmeraldRobe,
				ItemID.DiamondRobe
			});
			RecipeGroup.RegisterGroup("SOTS:GemRobes", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.AG"), new int[]
			{
				ItemID.Diamond,
				ItemID.Ruby,
				ItemID.Amethyst,
				ItemID.Topaz,
				ItemID.Sapphire,
				ItemID.Emerald,
				ItemID.Amber
			});
			RecipeGroup.RegisterGroup("SOTS:AnyGem", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.PHO"), new int[]
			{
				ItemID.TungstenOre,
				ItemID.CopperOre,
				ItemID.TinOre,
				ItemID.IronOre,
				ItemID.LeadOre,
				ItemID.SilverOre,
				ItemID.GoldOre,
				ItemID.PlatinumOre
			});
			RecipeGroup.RegisterGroup("SOTS:PHMOre", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.T2D2A"), new int[]
			{
				ItemID.SquirePlating,
				ItemID.SquireGreatHelm,
				ItemID.SquireGreaves,
				ItemID.HuntressWig,
				ItemID.HuntressJerkin,
				ItemID.HuntressPants,
				ItemID.ApprenticeHat,
				ItemID.ApprenticeRobe,
				ItemID.ApprenticeTrousers,
				ItemID.MonkBrows,
				ItemID.MonkShirt,
				ItemID.MonkPants
			});
			RecipeGroup.RegisterGroup("SOTS:T2DD2Armor", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.T2DD2A"), new int[]
			{
				ItemID.SquireShield,
				ItemID.HuntressBuckler,
				ItemID.ApprenticeScarf,
				ItemID.MonkBelt
			});
			RecipeGroup.RegisterGroup("SOTS:T2DD2Accessory", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.DE"), new int[]
			{
				ModContent.ItemType<DissolvingAether>(),
				ModContent.ItemType<DissolvingNature>(),
				ModContent.ItemType<DissolvingEarth>(),
				ModContent.ItemType<DissolvingAurora>(),
				ModContent.ItemType<DissolvingDeluge>(),
				ModContent.ItemType<DissolvingUmbra>(),
				ModContent.ItemType<DissolvingBrilliance>(),
				ModContent.ItemType<DissolvingNether>()
			});
			RecipeGroup.RegisterGroup("SOTS:DissolvingElement", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.EF"), new int[]
			{
				ModContent.ItemType<FragmentOfOtherworld>(),
				ModContent.ItemType<FragmentOfNature>(),
				ModContent.ItemType<FragmentOfEarth>(),
				ModContent.ItemType<FragmentOfPermafrost>(),
				ModContent.ItemType<FragmentOfTide>(),
				ModContent.ItemType<FragmentOfEvil>(),
				ModContent.ItemType<FragmentOfChaos>(),
				ModContent.ItemType<FragmentOfInferno>()
			});
			RecipeGroup.RegisterGroup("SOTS:ElementalFragment", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.EP"), new int[]
			{
				ModContent.ItemType<OtherworldPlating>(),
				ModContent.ItemType<UltimatePlating>(),
				ModContent.ItemType<DullPlating>(),
				ModContent.ItemType<NaturePlating>(),
				ModContent.ItemType<EarthenPlating>(),
				ModContent.ItemType<PermafrostPlating>(),
				ModContent.ItemType<TidePlating>(),
				ModContent.ItemType<EvilPlating>(),
				ModContent.ItemType<ChaosPlating>(),
				ModContent.ItemType<InfernoPlating>()
			});
			RecipeGroup.RegisterGroup("SOTS:ElementalPlating", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.AS"), new int[]
			{
				ItemID.DaybloomSeeds,
				ItemID.MoonglowSeeds,
				ItemID.BlinkrootSeeds,
				ItemID.ShiverthornSeeds,
				ItemID.WaterleafSeeds,
				ItemID.FireblossomSeeds,
				ItemID.DeathweedSeeds

			});
			RecipeGroup.RegisterGroup("SOTS:AlchSeeds", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.SOTS.AddRecipeGroups.CC"), new int[]
			{
				ModContent.ItemType<CrushingTransformer>(),
				ModContent.ItemType<CrushingAmplifier>(),
				ModContent.ItemType<CrushingCapacitor>(),
				ModContent.ItemType<CrushingResistor>()
			});
			RecipeGroup.RegisterGroup("SOTS:CrushingComponents", group);
		}
	}
}