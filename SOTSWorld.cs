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
using Terraria.World.Generation;
using SOTS.Items.Flails;
using SOTS.Items.Secrets;
using SOTS.Items.Tools;
using SOTS.Items.GhostTown;
using SOTS.Items.Otherworld.Blocks;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Items.Pyramid.AncientGold;
using SOTS.Items.Earth;

namespace SOTS
{
    public class SOTSWorld : ModWorld
	{
		public static int SecretFoundMusicTimer = 0;
        public static int planetarium = 0;
		public static int pyramidBiome = 0;
		public static int geodeBiome = 0;
		public static bool downedBoss2 = false;

		public static bool downedPinky = false;
		public static bool downedCurse = false;
		
		public static bool downedAmalgamation = false;
		public static bool downedCelestial = false;
		public static bool downedSubspace = false;
		public static bool downedAdvisor = false;

		#region unused
		public static bool downedEntity = false;
		public static bool downedAntilion = false;
		public static bool downedChess = false;
		
		public static bool challengeDecay = false;
		public static bool challengeLock = false;
		public static bool challengePermanence = false;
		
		public static bool challengeIce = false;
		public static bool challengeGlass = false;
		public static bool challengeIcarus = false;
        #endregion
        public override void Initialize()
		{
			downedPinky = false;
			downedAdvisor = false;
			downedCurse = false;
			downedEntity = false;
			downedBoss2 = false;
			downedAntilion = false;
			downedAmalgamation = false;
			downedChess = false;
			
			downedCelestial = false;
			downedSubspace = false;
			
			challengeDecay = false;
			challengeLock = false;
			challengePermanence = false;
			
			challengeIce = false;
			challengeGlass = false;
			challengeIcarus = false;
		}
		public override TagCompound Save() {
			var downed = new List<string>();
			if (downedPinky) {
				downed.Add("pinky");
			}
			if (downedAdvisor) {
				downed.Add("advisor");
			}
			if (downedCurse) {
				downed.Add("curse");
			}
			if (downedEntity) {
				downed.Add("entity");
			}
			if (downedAntilion) {
				downed.Add("antilion");
			}
			if (downedAmalgamation) {
				downed.Add("amalgamation");
			}
			if (downedChess) {
				downed.Add("chess");
			}
			if (downedCelestial) {
				downed.Add("celestial");
			}
			if (downedSubspace) {
				downed.Add("subspace");
			}
			if (downedBoss2)
			{
				downed.Add("boss2");
			}

			var challenge = new List<string>();
			if (challengeDecay) {
				challenge.Add("decay");
			}
			if (challengeLock) {
				challenge.Add("lock");
			}
			if (challengePermanence) {
				challenge.Add("permanence");
			}
			if (challengeIce) {
				challenge.Add("ice");
			}
			if (challengeGlass) {
				challenge.Add("glass");
			}
			if (challengeIcarus) {
				challenge.Add("icarus");
			}
			return new TagCompound {
				{"downed", downed},
				{"challenge", challenge},
			};
		}
		public override void Load(TagCompound tag) {
			var downed = tag.GetList<string>("downed");
			downedPinky = downed.Contains("pinky");
			downedAdvisor = downed.Contains("advisor");
			downedCurse = downed.Contains("curse");
			downedEntity = downed.Contains("entity");
			downedAntilion = downed.Contains("antilion");
			downedAmalgamation = downed.Contains("amalgamation");
			downedChess = downed.Contains("chess");
			downedCelestial = downed.Contains("celestial");
			downedSubspace = downed.Contains("subspace");
			downedBoss2 = downed.Contains("boss2");

			var challenge = tag.GetList<string>("challenge");
			challengeDecay = challenge.Contains("decay");
			challengeLock = challenge.Contains("lock");
			challengePermanence = challenge.Contains("permanence");
			challengeIce = challenge.Contains("ice");
			challengeGlass = challenge.Contains("glass");
			challengeIcarus = challenge.Contains("icarus");
		}
		public override void LoadLegacy(BinaryReader reader) {
			int loadVersion = reader.ReadInt32();
			if (loadVersion == 0) {
				BitsByte flags = reader.ReadByte();
				downedPinky = flags[0];
				downedAdvisor = flags[1];
				downedEntity = flags[2];
				downedAntilion = flags[3];
				downedAmalgamation = flags[4];
				downedChess = flags[5];
				downedCurse = flags[6];
				downedCelestial = flags[7];
				
				BitsByte flags3 = reader.ReadByte();
				downedSubspace = flags3[0];
				downedBoss2 = flags3[1];

				BitsByte flags2 = reader.ReadByte();
				challengeDecay = flags2[0];
				challengeLock = flags2[1];
				challengePermanence = flags2[2];
				challengeIce = flags2[3];
				challengeGlass = flags2[4];
				challengeIcarus = flags2[5];
			}
		}
		public override void NetSend(BinaryWriter writer) {
			BitsByte flags = new BitsByte();
			flags[0] = downedPinky;
			flags[1] = downedAdvisor;
			flags[2] = downedEntity;
			flags[3] = downedAntilion;
			flags[4] = downedAmalgamation;
			flags[5] = downedChess;
			flags[6] = downedCurse;
			flags[7] = downedCelestial;
			writer.Write(flags);
			
			BitsByte flags3 = new BitsByte();
			flags3[0] = downedSubspace;
			flags3[1] = downedBoss2;
			writer.Write(flags3);
			
			BitsByte flags2 = new BitsByte();
			flags2[0] = challengeDecay;
			flags2[1] = challengeLock;
			flags2[2] = challengePermanence;
			flags2[3] = challengeIce;
			flags2[4] = challengeGlass;
			flags2[5] = challengeIcarus;
			writer.Write(flags2);
		}
		public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			downedPinky = flags[0];
			downedAdvisor = flags[1];
			downedEntity = flags[2];
			downedAntilion = flags[3];
			downedAmalgamation = flags[4];
			downedChess = flags[5];
			downedCurse = flags[6];
			downedCelestial = flags[7];
			
			BitsByte flags3 = reader.ReadByte();
			downedSubspace = flags3[0];
			downedBoss2 = flags3[1];

			BitsByte flags2 = reader.ReadByte();
			challengeDecay = flags2[0];
			challengeLock = flags2[1];
			challengePermanence = flags2[2];
			challengeIce = flags2[3];
			challengeGlass = flags2[4];
			challengeIcarus = flags2[5];
		}
		public override void PostUpdate()
		{
			if(NPC.downedBoss2 && !downedBoss2)
			{
				downedBoss2 = true;
				//Main.NewText("The pyramid's curse weakens", 155, 115, 0);
			}
		}
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndexOres = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
			int genIndexGeodes = tasks.FindIndex(genpass => genpass.Name.Equals("Lakes"));
			int genIndexGems = tasks.FindIndex(genpass => genpass.Name.Equals("Random Gems"));
            int genIndexEnd = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));

			tasks.Insert(genIndexOres, new PassLegacy("SOTSOres", GenSOTSOres));
			tasks.Insert(genIndexGeodes + 1, new PassLegacy("SOTSOres", GenSOTSGeodes));
			tasks.Insert(genIndexGems + 1, new PassLegacy("ModdedSOTSStructures", delegate (GenerationProgress progress)
			{
				progress.Message = "Generating Surface Structures";
				SOTSWorldgenHelper.GenerateStarterHouseFull(mod, Main.rand.Next(12));

				int iceY = -1;
				int iceX = -1;
				int totalChecks = 0;
				for (int xCheck = Main.rand.Next(Main.maxTilesX); xCheck != -1; xCheck = Main.rand.Next(Main.maxTilesX))
				{
					for (int ydown = 0; ydown != -1; ydown++)
					{
						Tile tile = Framing.GetTileSafely(xCheck, ydown);
						bool allValid = totalChecks > 100 || (SOTSWorldgenHelper.TrueTileSolid(xCheck + 1, ydown) && SOTSWorldgenHelper.TrueTileSolid(xCheck + 2, ydown) && SOTSWorldgenHelper.TrueTileSolid(xCheck - 1, ydown) && SOTSWorldgenHelper.TrueTileSolid(xCheck - 2, ydown));
						if (tile.active() && tile.type == TileID.SnowBlock)
						{
							iceY = ydown;
							break;
						}
						else if (tile.active())
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
					if (direction == -1)
					{
						fromBorder = Main.maxTilesX - fromBorder;
					}
					for (int j = 0; j < Main.maxTilesY; j++)
					{
						Tile tile = Framing.GetTileSafely(fromBorder, j);
						if (tile.liquidType() == 0 && tile.liquid > 1)
						{
							SOTSWorldgenHelper.GenerateCoconutIsland(mod, fromBorder, j, direction);
							coconutGenerated = true;
							break;
						}
					}
				}

			}));
			tasks.Insert(genIndexEnd + 4, new PassLegacy("genIndexModPlanetarium", delegate (GenerationProgress progress)
			{
				progress.Message = "Generating Sky Artifacts";
				int dungeonSide = -1; // -1 = dungeon on left, 1 = dungeon on right
				if (Main.dungeonX > (int)(Main.maxTilesX / 2))
				{
					dungeonSide = 1;
				}
				SOTSWorldgenHelper.FindAndGenerateDamocles(dungeonSide);
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
							if (tile.active() && (tile.type == TileID.SnowBlock || tile.type == TileID.IceBlock))
							{
								validLocation = true;
								break;
							}
							else if (tile.active() && Main.tileSolid[tile.type])
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
							if (SOTSWorldgenHelper.GeneratePlanetariumFull(mod, pX, yLocation, force))
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
							if (tile.active() && (tile.type == TileID.SnowBlock || tile.type == TileID.IceBlock))
							{
								validLocation = true;
								break;
							}
							else if(tile.active() && Main.tileSolid[tile.type])
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
							if (SOTSWorldgenHelper.GeneratePlanetariumFull(mod, pX, 140, force))
							{
								break;
							}
						}
					}
				}

				bool hasDoneEvil = false;
				int overrideCounter = 0;
				bool hasDoneJungle = false;
				int xCord = Main.rand.Next(240, Main.maxTilesX - 240);
				for (; xCord != -1; xCord = Main.rand.Next(240, Main.maxTilesX - 240))
				{
					overrideCounter++;
					if (hasDoneEvil && hasDoneJungle)
                    {
						xCord = -1;
						return;
                    }
					for (int ydown = 0; ydown != -1; ydown++)
					{
						Tile tile = Framing.GetTileSafely(xCord, ydown);
						if (tile.active() && Main.tileSolid[tile.type])
						{
							if(tile.type == TileID.JungleGrass || tile.type == TileID.JunglePlants || tile.type == TileID.JunglePlants2 || overrideCounter > 100)
                            {
								int y = 140 + Main.rand.Next(50);
								if(!hasDoneJungle)
								{
									hasDoneJungle = SOTSWorldgenHelper.GenerateBiomeChestIslands(xCord, y, 3, mod);
								}
								break;
							}
							if (tile.type == TileID.Crimstone || tile.type == TileID.FleshGrass || tile.type == TileID.Crimsand || overrideCounter > 100)
							{
								int y = 140 + Main.rand.Next(50);
								if (!hasDoneEvil)
								{
									hasDoneEvil = SOTSWorldgenHelper.GenerateBiomeChestIslands(xCord, y, 0, mod);
								}
								break;
							}
							if (tile.type == TileID.Ebonstone || tile.type == TileID.CorruptGrass || tile.type == TileID.Ebonsand || overrideCounter > 100)
							{
								int y = 140 + Main.rand.Next(50);
								if (!hasDoneEvil)
								{
									hasDoneEvil = SOTSWorldgenHelper.GenerateBiomeChestIslands(xCord, y, 1, mod);
								}
								break;
							}
							break;
						}
					}
				}
			}));
			tasks.Insert(genIndexEnd + 5, new PassLegacy("genIndexModPyramid", delegate (GenerationProgress progress)
			{
				progress.Message = "Generating A Pyramid";
				PyramidWorldgenHelper.GenerateSOTSPyramid(mod);
				for (int k = 100; k < Main.maxTilesX - 100; k++)
				{
					for (int l = (int)WorldGen.rockLayerLow - 20; l < Main.maxTilesY - 220; l++)
					{
						if (Main.tile[k, l].wall == ModContent.WallType<VibrantWallWall>())
						{
							Main.tile[k, l].liquidType(0);
							Main.tile[k, l].liquid = 0;
							if (Main.tile[k, l].type == ModContent.TileType<VibrantOreTile>() && Main.tile[k, l].active() && Main.tile[k, l].slope() == 0 && !Main.tile[k, l].halfBrick())
							{
								bool oneSideIsClear = !Main.tile[k - 1, l].active() || !Main.tile[k + 1, l].active() || !Main.tile[k, l + 1].active() || !Main.tile[k, l - 1].active();
								if(oneSideIsClear)
									for (int i = 0; i < 3; i++)
										if(SOTSTile.GenerateVibrantCrystal(k, l)) break;
							}
						}
					}
				}
			}));
		}
		private void GenSOTSOres(GenerationProgress progress)
		{
			progress.Message = "Generating SOTS Ores";
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
		private void GenSOTSGeodes(GenerationProgress progress)
		{
			progress.Message = "Generating Fancy Geodes";
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
				bool valid = tile.type == TileID.Stone || tile.type == TileID.Dirt;
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
		public override void TileCountsAvailable(int[] tileCounts)
		{
			planetarium = tileCounts[ModContent.TileType<DullPlatingTile>()] + tileCounts[ModContent.TileType<AvaritianPlatingTile>()];  
			//geodeBiome = tileCounts[mod.TileType("GeodeBlock")];
			pyramidBiome = tileCounts[ModContent.TileType<SarcophagusTile>()] + tileCounts[ModContent.TileType<RefractingCrystalBlockTile>()] + tileCounts[ModContent.TileType<AcediaGatewayTile>()];  
		}
        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            base.ModifyHardmodeTasks(list);
        }
        public override void PostWorldGen()
		{
			// Iterate chests
			List<int> starItemPool2 = new List<int>() { ModContent.ItemType<SkywareBattery>(), ModContent.ItemType<Poyoyo>(), ModContent.ItemType<SupernovaHammer>(), ModContent.ItemType<StarshotCrossbow>(), ModContent.ItemType<LashesOfLightning>(), ModContent.ItemType<Starbelt>(), ModContent.ItemType<TwilightAssassinsCirclet>() };
			List<int> lightItemPool2 = new List<int>() { ModContent.ItemType<HardlightQuiver>(), ModContent.ItemType<CodeCorrupter>(), ModContent.ItemType<PlatformGenerator>(), ModContent.ItemType<Calculator>(), ModContent.ItemType<TwilightAssassinsLeggings>(), ModContent.ItemType<TwilightFishingPole>(), ModContent.ItemType<ChainedPlasma>(), ModContent.ItemType<OtherworldlySpiritStaff>() };
			List<int> fireItemPool2 = new List<int>() { ModContent.ItemType<BlinkPack>(), ModContent.ItemType<FlareDetonator>(), ModContent.ItemType<VibrancyModule>(), ModContent.ItemType<CataclysmMusketPouch>(), ModContent.ItemType<TerminatorAcorns>(), ModContent.ItemType<TwilightAssassinsChestplate>(), ModContent.ItemType<InfernoHook>() };

			List<int> starItemPool = new List<int>() { ModContent.ItemType<SkywareBattery>(), ModContent.ItemType<Poyoyo>(), ModContent.ItemType<SupernovaHammer>(), ModContent.ItemType<StarshotCrossbow>(),ModContent.ItemType<LashesOfLightning>(), ModContent.ItemType<Starbelt>(), ModContent.ItemType<TwilightAssassinsCirclet>() };
			List<int> lightItemPool = new List<int>() { ModContent.ItemType<HardlightQuiver>(), ModContent.ItemType<CodeCorrupter>(), ModContent.ItemType<PlatformGenerator>(), ModContent.ItemType<Calculator>(), ModContent.ItemType<TwilightAssassinsLeggings>(), ModContent.ItemType<TwilightFishingPole>(), ModContent.ItemType<ChainedPlasma>(), ModContent.ItemType<OtherworldlySpiritStaff>() };
			List<int> fireItemPool = new List<int>() { ModContent.ItemType<BlinkPack>(), ModContent.ItemType<FlareDetonator>(), ModContent.ItemType<VibrancyModule>(), ModContent.ItemType<CataclysmMusketPouch>(), ModContent.ItemType<TerminatorAcorns>(), ModContent.ItemType<TwilightAssassinsChestplate>(), ModContent.ItemType<InfernoHook>() };
			foreach (Chest chest in Main.chest.Where(c => c != null))
			{
				// Get a chest
				var tile = Main.tile[chest.x, chest.y]; // the chest tile 
				if (tile.type == ModContent.TileType<LockedStrangeChest>() || tile.type == ModContent.TileType<LockedSkywareChest>() || tile.type == ModContent.TileType<LockedMeteoriteChest>())
				{
					int type = tile.type == ModContent.TileType<LockedStrangeChest>() ? 0 : tile.type == ModContent.TileType<LockedSkywareChest>() ? 1 : 2;
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
				if (tile.type == ModContent.TileType<RuinedChestTile>())
				{
					int slot = 0;
					Tile tile2 = Main.tile[chest.x, chest.y + 2];
					if (tile2.type == ModContent.TileType<CharredWoodTile>() && tile.wall == ModContent.WallType<HardIceBrickWallWall>())
					{
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
					else
                    {
						chest.item[slot].SetDefaults(ModContent.ItemType<WorldgenScanner>());
						slot++;
					}
				}
				if (tile.type == ModContent.TileType<PyramidChestTile>())
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
				if (tile.type == TileID.Containers)
				{
					int slot = 39;
					for(int i = 0; i < 39; i++)
					{
						if(chest.item[i].type == ItemID.None && i < slot)
						{
							slot = i;
						}
					}

					TileObjectData tileData = TileObjectData.GetTileData(tile);
					int style = TileObjectData.GetTileStyle(tile);
					Tile tile2 = Main.tile[chest.x, chest.y + 2];
					Tile tile3 = Main.tile[chest.x, chest.y + 5];
					if (style == 31 && tile2.type == (ushort)ModContent.TileType<PyramidBrickTile>()) //Coconut Chest
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
					if (style == 17 && tile2.type == (ushort)ModContent.TileType<DullPlatingTile>()) //Damocles Chest
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
					if (style >= 23 && style <= 27 && (tile3.type == ModContent.TileType<DullPlatingTile>() || tile3.type == ModContent.TileType<AvaritianPlatingTile>()))
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
							importantItem2 = ModContent.ItemType<DissolvingDeluge>();
							consumable = ModContent.ItemType<AlmondMilk>();
						}
						if (style == 25)
						{
							importantItem = ModContent.ItemType<RebarRifle>();
							importantItem2 = ModContent.ItemType<DissolvingEarth>();
							consumable = ModContent.ItemType<AlmondMilk>();
						}
						if (style == 26)
						{
							importantItem = ModContent.ItemType<StarcallerStaff>();
							importantItem2 = ModContent.ItemType<DissolvingAether>();
							consumable = ModContent.ItemType<DigitalCornSyrup>();
						}
						if (style == 27)
						{
							importantItem = ModContent.ItemType<Sawflake>();
							importantItem2 = ModContent.ItemType<DissolvingAurora>();
							consumable = ModContent.ItemType<StrawberryIcecream>();
							consumableQuant = 20;
						}
						if(!chest.item[0].active)
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
					if(WorldGen.genRand.NextBool(2) && (chest.item[0].type == ItemID.Starfury || chest.item[0].type == ItemID.ShinyRedBalloon || chest.item[0].type == ItemID.LuckyHorseshoe))
					{
						chest.item[slot].SetDefaults(ModContent.ItemType<TinyPlanet>());
						slot++;
					}
				}
			}
		}	
	}
}