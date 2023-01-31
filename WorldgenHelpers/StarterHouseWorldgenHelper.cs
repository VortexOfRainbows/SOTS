using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Items;	
using SOTS.Items.AbandonedVillage;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using SOTS.Items.Furniture.Nature;
using SOTS.Items.Fragments;

namespace SOTS.WorldgenHelpers
{
	public static class StarterHouseID
	{
		public static int Legacy0 = 0;
		public static int Legacy1 = 1;
		public static int Legacy2 = 2;
		public static int Legacy3 = 3;
		public static int Legacy4 = 4;
		public static int Legacy5 = 5;
		public static int Legacy6 = 6;
		public static int Legacy7 = 7;
		public static int Legacy8 = 8;
		public static int Legacy9 = 9;
		public static int Mushnib = 10;
		public static int Astrobit = 11;
		public static int Bloodmoon = 12;
		public static int Bandit = 13;
		public static int Boulder = 14;
		public static int Mine = 15;
		public static int Tavern = 16;
		public static int Chaos = 17;
		public static int Drowned = 18;
		public static int Comfy = 19;
		public static int Nature = 20;
	}
	public class StarterHouseWorldgenHelper
	{
		public static int RuinedChest => ModContent.TileType<RuinedChestTile>();
		public struct StarterHouse
		{
			public StarterHouse(int Type, double SuccessChance = 1.0)
			{
				this.Type = Type;
				this.SuccessChance = SuccessChance;
			}
			public double SuccessChance { get; }
			public int Type { get; }
		}
		public static int PickStarterHouseTypeUsingWorldName()
		{
			List<StarterHouse> houses = new List<StarterHouse>();
			string worldName = Main.worldName;
			if (worldName.Contains("Legacy") || worldName.Contains("legacy"))
			{
				if(worldName.Contains("0") || worldName.Contains("House") || worldName.Contains("house"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy0, 1));
				if (worldName.Contains("1") || worldName.Contains("Angel") || worldName.Contains("angel"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy1, 1));
				if (worldName.Contains("2") || worldName.Contains("Barrel") || worldName.Contains("barrel"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy2, 1));
				if (worldName.Contains("3") || worldName.Contains("Hotel") || worldName.Contains("hotel"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy3, 1));
				if (worldName.Contains("4") || worldName.Contains("Origin") || worldName.Contains("origin"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy4, 1));
				if (worldName.Contains("5") || worldName.Contains("Tower") || worldName.Contains("tower"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy5, 1));
				if (worldName.Contains("6") || worldName.Contains("Sword") || worldName.Contains("sword"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy6, 1));
				if (worldName.Contains("7") || worldName.Contains("Tree") || worldName.Contains("tree"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy7, 1));
				if (worldName.Contains("8") || worldName.Contains("Library") || worldName.Contains("library") || worldName.Contains("Librarian") || worldName.Contains("librarian"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy8, 1));
				if (worldName.Contains("9") || worldName.Contains("Crate") || worldName.Contains("crate"))
					houses.Add(new StarterHouse(StarterHouseID.Legacy9, 1));
				if (houses.Count <= 0)
				{
					houses = new List<StarterHouse>()
					{
						new StarterHouse(StarterHouseID.Legacy0, 1),
						new StarterHouse(StarterHouseID.Legacy1, 1),
						new StarterHouse(StarterHouseID.Legacy2, 1),
						new StarterHouse(StarterHouseID.Legacy3, 1),
						new StarterHouse(StarterHouseID.Legacy4, 1),
						new StarterHouse(StarterHouseID.Legacy5, 1),
						new StarterHouse(StarterHouseID.Legacy6, 1),
						new StarterHouse(StarterHouseID.Legacy7, 1),
						new StarterHouse(StarterHouseID.Legacy8, 1),
						new StarterHouse(StarterHouseID.Legacy9, 1),
						new StarterHouse(StarterHouseID.Mushnib, 1),
						new StarterHouse(StarterHouseID.Astrobit, 1)
					};
				}
			}
			if (worldName.Contains("Astro") || worldName.Contains("astro"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Astrobit, 1));
				houses.Add(new StarterHouse(StarterHouseID.Bloodmoon, 0.1));
			}
			if (worldName.Contains("Bandit") || worldName.Contains("bandit"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Bandit, 1));
			}
			if (worldName.Contains("Blood") || worldName.Contains("blood"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Bloodmoon, 1));
			}
			if (worldName.Contains("Boulder") || worldName.Contains("boulder"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Boulder, 1));
			}
			if (worldName.Contains("Comfy") || worldName.Contains("comfy"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Comfy, 1));
			}
			if (worldName.Contains("Chaos") || worldName.Contains("chaos"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Chaos, 1));
			}
			if (worldName.Contains("Moat") || worldName.Contains("moat") || worldName.Contains("Drown") || worldName.Contains("drown") || worldName.Contains("Crab") || worldName.Contains("crab") )
			{
				houses.Add(new StarterHouse(StarterHouseID.Drowned, 1));
			}
			if (worldName.Contains("Mine") || worldName.Contains("mine"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Mine, 1));
			}
			if (worldName.Contains("Mush") || worldName.Contains("mush"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Mushnib, 1));
			}
			if (worldName.Contains("Tavern") || worldName.Contains("tavern") || worldName.Contains("Bar") || worldName.Contains("bar"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Tavern, 1));
			}
			if (worldName.Contains("Starbound") || worldName.Contains("starbound"))
			{
				houses.Add(new StarterHouse(StarterHouseID.Nature, 1));
			}
            else
			{
				if (worldName.Contains("Nature") || worldName.Contains("nature") || worldName.Contains("Hydroponic") || worldName.Contains("hydroponic"))
				{
					houses.Add(new StarterHouse(StarterHouseID.Nature, 1));
				}
			}
			//The following is the default list if no codeword is in the world name.
			if (houses.Count <= 0)
            {
				houses = new List<StarterHouse>()
				{
					new StarterHouse(StarterHouseID.Legacy0, 0.05),
					new StarterHouse(StarterHouseID.Legacy1, 0.05),
					new StarterHouse(StarterHouseID.Legacy2, 0.05),
					new StarterHouse(StarterHouseID.Legacy3, 0.02), //really big + legacy
					new StarterHouse(StarterHouseID.Legacy4, 0.05),
					new StarterHouse(StarterHouseID.Legacy5, 0.05),
					new StarterHouse(StarterHouseID.Legacy6, 0.5), //contains the enchanted sword
					new StarterHouse(StarterHouseID.Legacy7, 0.05),
					new StarterHouse(StarterHouseID.Legacy8, 0.05),
					new StarterHouse(StarterHouseID.Legacy9, 0.05),
					new StarterHouse(StarterHouseID.Mushnib, 1),
					new StarterHouse(StarterHouseID.Astrobit, 1),
					//Following are the new stuff
					new StarterHouse(StarterHouseID.Bloodmoon, 1),
					new StarterHouse(StarterHouseID.Bandit, 1),
					new StarterHouse(StarterHouseID.Boulder, 1),
					new StarterHouse(StarterHouseID.Mine, 0.5), //rarer because of the explosives. It also gives Iron bars, Sapphire, and Crates. Enough to make hooks. Therefore it should be rare.
					new StarterHouse(StarterHouseID.Tavern, 1),
					new StarterHouse(StarterHouseID.Chaos, 0.7), //rarer because of the massive size and abundance of statues.
					new StarterHouse(StarterHouseID.Drowned, 1), 
					new StarterHouse(StarterHouseID.Comfy, 0.6), //rarer because BIG
					new StarterHouse(StarterHouseID.Nature, 0.4) //rarer because BIG
				};
			}
			return PickStarterHouseTypeUsingWeightings(houses);
        }
		public static int PickStarterHouseTypeUsingWeightings(List<StarterHouse> houses)
        {
			int selection = Main.rand.Next(houses.Count);
			StarterHouse currentHouse = houses[selection];
			while(currentHouse.SuccessChance < Main.rand.NextDouble() && houses.Count > 1)
            {
				houses.RemoveAt(selection);
				selection = Main.rand.Next(houses.Count);
				currentHouse = houses[selection];
			}
			return currentHouse.Type;
        }
		public static void GenerateStarterHouseFull()
		{
			int Type = PickStarterHouseTypeUsingWorldName();
			int spawnX = -1;
			int spawnY = -1;
			int randomOne = Main.rand.Next(2) * 2 - 1;
			int randAttempt = randomOne * Main.rand.Next(40, 81);
			for (int xCheck = Main.maxTilesX / 2 + randAttempt; ; xCheck = Main.maxTilesX / 2 + randAttempt)
			{
				randAttempt = randomOne * Main.rand.Next(40, 81);
				for (int ydown = 0; ; ydown++)
				{
					Tile tile = Framing.GetTileSafely(xCheck, ydown);
					if (tile.HasTile && Main.tileSolid[tile.TileType])
					{
						spawnY = ydown;
						break;
					}
				}
				if (spawnY != -1)
				{
					spawnX = xCheck;
					break;
				}
			}
			GenerateStarterHouse(spawnX, spawnY, Type);
		}
		public static void UseStarterHouseHalfCircle(int spawnX, int spawnY, int side = 0, int radius = 10, int radiusY = 10, int type = 0, int type2 = 0)
		{
			radius += 2;
			radiusY++;
			float scale = radiusY / (float)radius;
			float invertScale = (float)radius / radiusY;
			if (side == 0)
			{
				for (int x = -radius; x <= radius; x++)
				{
					for (float y = -radius - 1; y <= 0; y += invertScale)
					{
						int xPosition6 = spawnX + x;
						if (Math.Sqrt(x * x + (int)y * (int)y) <= radius + 0.5)
						{
							WorldGen.KillTile(xPosition6, spawnY + (int)(y * scale + 0.5f), false, false, false);
						}
					}
				}
			}
			else if (side == 1)
			{
				for (int x = -radius; x <= radius; x++)
				{
					for (float y = -1; y <= radius; y += invertScale)
					{
						if (Math.Sqrt(x * x + y * y) <= radius + 0.5)
						{
							int xPosition6 = spawnX + x;
							int yPosition6 = spawnY + (int)(y * scale + 0.5f);
							Tile tile = Framing.GetTileSafely(xPosition6, yPosition6);
							Tile tile2 = Framing.GetTileSafely(xPosition6, yPosition6 - 1);
							if (!tile.HasTile)
							{
								tile.TileType = (ushort)type;
								tile.HasTile = true;
							}
							if (!tile2.HasTile && tile.TileType == type)
							{
								tile.TileType = (ushort)type2;
							}
							//tile.HasTile;
						}
					}
				}
			}
			else if (side == 2)
			{
				for (int x = -radius; x <= radius; x++)
				{
					for (float y = -1; y <= radius; y += invertScale)
					{
						if (Math.Sqrt(x * x + y * y) <= radius + 0.5)
						{
							int xPosition6 = spawnX + x;
							int yPosition6 = spawnY + (int)(y * scale + 0.5f);
							Tile tile = Framing.GetTileSafely(xPosition6, yPosition6);
							Tile tile2 = Framing.GetTileSafely(xPosition6, yPosition6 - 1);
							if (!tile.HasTile)
							{
								tile.TileType = (ushort)type;
								tile.HasTile = true;
							}
							if (tile2.HasTile && tile.TileType == type)
							{
								if(WorldGen.genRand.NextBool(3))
									tile.TileType = (ushort)type2;
							}
							//tile.HasTile;
						}
					}
				}
			}
		}
		public static void GenerateStarterHouse(int spawnX, int spawnY, int type)
		{
			if (type == StarterHouseID.Legacy0)
				GenerateLegacy0StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Legacy1)
				GenerateLegacy1StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Legacy2)
				GenerateLegacy2StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Legacy3)
				GenerateLegacy3StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Legacy4)
				GenerateLegacy4StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Legacy5)
				GenerateLegacy5StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Legacy6)
				GenerateLegacy6StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Legacy7)
				GenerateLegacy7StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Legacy8)
				GenerateLegacy8StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Legacy9)
				GenerateLegacy9StarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Mushnib)
				GenerateMushnibStarterHouse(spawnX, spawnY, WorldGen.genRand.NextBool(5));
			if (type == StarterHouseID.Astrobit)
				GenerateAstrobitStarterHouse(spawnX, spawnY, WorldGen.genRand.NextBool(5));
			if (type == StarterHouseID.Bloodmoon)
				GenerateBloodMoonStarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Bandit)
				GenerateBanditStarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Boulder)
				GenerateBoulderStarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Mine)
				GenerateMineStarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Tavern)
				GenerateTavernStarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Chaos)
				GenerateChaosStarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Drowned)
				GenerateDrownedStarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Comfy)
				GenerateComfyStarterHouse(spawnX, spawnY);
			if (type == StarterHouseID.Nature)
				GenerateNatureStarterHouse(spawnX, spawnY);
		}
		public static void GenerateAstrobitStarterHouse(int spawnX, int spawnY, bool WaterBolt = false)
		{
			//i = vertical, j = horizontal
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,4,4,5,5,4,4,4,4,4,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,4,6,6,6,6,6,6,6,6,6,6,4,7,0,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,0,4,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4,4,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,4,4,4,4,6,8,8,8,8,6,6,6,6,8,8,8,8,6,6,4,9,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,3,4,6,6,6,8,8,8,10,8,4,4,8,8,8,8,8,8,4,4,4,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,0,3,4,6,6,8,8,8,10,0,0,0,12,9,13,0,13,14,8,8,12,9,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,5,4,4,4,6,6,8,8,8,10,13,0,0,0,0,13,13,13,0,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,3,4,6,6,6,6,6,8,8,8,0,0,0,0,0,13,0,13,0,13,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,1,0,4,4,6,6,6,6,6,6,8,8,4,17,0,0,11,0,0,13,0,13,0,0,15,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1},
				{4,4,4,6,6,6,6,6,6,6,6,8,8,4,4,19,19,19,19,20,20,20,20,19,19,8,8,5,4,21,22,0,18,0,5,4,4,4,4,4,4},
				{6,6,6,6,6,6,6,6,6,6,6,8,8,19,19,19,19,23,24,0,13,13,13,25,26,19,19,4,4,8,8,4,4,4,4,4,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,8,25,13,12,13,13,0,0,0,0,13,25,13,0,0,12,28,8,8,8,6,8,6,8,8,6,6,6,6},
				{6,6,6,6,6,6,6,6,4,6,6,8,4,25,29,29,29,13,0,0,0,0,0,25,0,29,30,29,0,26,19,19,4,4,19,19,8,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,4,8,4,25,20,20,20,0,13,0,0,0,13,25,13,20,20,20,13,0,12,0,28,13,25,8,8,6,6,6,6},
				{6,6,6,6,4,6,6,6,6,6,6,6,4,25,0,0,0,0,0,0,0,0,0,25,0,13,0,13,0,29,29,13,29,0,25,34,8,6,6,6,6},
				{6,6,6,4,6,6,6,6,6,6,8,6,8,25,0,33,0,0,13,0,27,0,0,25,5,0,0,13,13,20,20,20,20,13,25,8,8,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,8,8,19,19,19,19,19,19,19,19,4,19,4,4,19,35,0,0,0,13,0,13,13,25,8,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,8,8,8,8,8,8,8,8,8,8,8,8,8,6,19,36,35,0,13,13,0,0,0,25,8,6,6,6,6,6},
				{6,6,6,6,6,6,6,4,6,6,6,6,6,6,8,8,8,8,8,6,8,8,8,6,6,6,19,36,35,13,0,37,0,17,25,8,8,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4,8,8,8,8,8,8,8,8,6,6,8,19,19,19,4,19,4,4,19,19,8,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,8,8,8,8,8,6,6,8,8,8,8,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,8,8,8,6,8,8,8,6,8,8,6,8,6,6,6,6},
				{6,6,6,6,6,6,6,4,6,6,6,6,6,6,6,6,6,6,6,4,6,6,6,6,6,6,8,8,8,8,8,6,6,8,8,8,8,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,8,6,6,6,6,6,6,6}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 15;
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									if (confirmPlatforms == 1 && !WorldGen.genRand.NextBool(3))
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Saplings, true, true, -1, 0);
										WorldGen.GrowTree(k, l);
										WorldGen.GrowEpicTree(k, l);
									}
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 101, true, true, -1, 25);
									}
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 373;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 15:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 30);
									}
									break;
								case 17:
									tile.HasTile = true;
									tile.TileType = 73;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 18:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 215, true, true, -1, 0);
									}
									break;
								case 19:
									tile.HasTile = true;
									tile.TileType = 321;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 19);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 21:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 22:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 321;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 24:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 42, true, true, -1, 5);
									}
									break;
								case 25:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 26:
									tile.HasTile = true;
									tile.TileType = 321;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 27:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 104, true, true, -1, 6);
									}
									break;
								case 28:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 29:
									if (confirmPlatforms == 1)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Books, true, true, -1, Main.rand.Next(6));
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 30:
									if (confirmPlatforms == 1)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Books, true, true, -1, Main.rand.Next(6));
									if (WaterBolt)
										tile.TileFrameX = 90;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 33:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 79, true, true, -1, 24);
									}
									break;
								case 34:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 35:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 19);
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 36:
									tile.HasTile = true;
									tile.TileType = 321;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 37:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 38:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY + 14, 1, _structure.GetLength(1) / 2, 10);
			_structure = new int[,]  {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,1,1,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,3,2,1,1,2,0,0,0,0,0,0,4,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,0,0,0,0,2,3,3,3,1,1,1,1,1,1,1,4,4,1,8,8,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,3,3,3,3,3,1,3,1,1,1,1,1,3,8,8,8,0,0,0,0,0,0,0,0,0,0},
				{0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,3,3,3,3,3,3,3,3,1,1,1,1,3,3,8,8,8,8,8,8,8,8,8,8,8,8},
				{1,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,3,3,3,3,3,3,3,1,3,3,1,1,1,8,1,1,1,8,8,8,8,8,8,8,8},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,3,3,3,3,2,2,2,0,1,2,1,1,1,1,1,8,8,8,8,8,8,8},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,4,0,3,3,3,3,3,3,3,3,0,0,2,2,6,6,6,6,6,6,6,6,6,6,6,6},
				{0,0,0,0,0,0,0,0,0,0,6,6,0,1,1,1,3,3,3,3,3,3,3,3,3,1,1,1,1,6,6,6,6,6,6,6,6,6,6,6,6},
				{0,0,0,0,0,0,0,0,6,6,6,6,0,1,1,2,3,3,3,3,3,3,3,3,3,3,1,1,1,1,6,6,6,6,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,7,6,6,0,1,1,2,3,3,3,3,3,3,3,3,3,3,1,1,1,3,3,3,1,1,3,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,0,1,3,3,3,3,3,3,3,3,3,1,1,1,1,1,3,3,3,3,1,1,3,3,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,7,6,6,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,3,3,3,3,3,3,1,3,3,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,7,7,6,6,0,0,0,0,0,0,0,0,4,0,0,0,0,0,1,3,3,3,3,3,3,1,3,3,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,3,3,3,3,3,1,3,3,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,1,3,3,1,1,1,3,3,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,6,3,3,3,3,3,3,3,3,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6}
			};
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 8:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 68;
								break;
							case 2:
								tile.WallType = 5;
								break;
							case 3:
								tile.WallType = 147;
								break;
							case 4:
								tile.WallType = 66;
								break;
							case 6:
								tile.WallType = 2;
								break;
							case 7:
								tile.WallType = 59;
								break;
						}
					}
				}
			}
		}
		public static void GenerateMushnibStarterHouse(int spawnX, int spawnY, bool WaterBolt = false)
		{
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,3,3,3,3,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,3,3,3,3,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,3,3,3,3,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,3,3,3,3,2,0,4,4,0,0,0,0,0,0,0},
				{0,0,0,0,2,3,3,3,3,2,0,4,4,4,4,4,4,0,0,0},
				{0,0,0,0,2,3,3,3,3,2,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,3,3,3,3,2,4,5,4,4,4,4,5,0,0,0},
				{0,0,0,0,2,3,6,6,3,2,4,5,4,6,6,4,5,0,0,0},
				{0,0,0,0,2,3,6,6,3,2,4,5,4,6,6,4,5,0,0,0},
				{0,0,0,0,2,3,3,3,3,2,4,5,4,4,4,4,5,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,4,1,1,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{7,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
				{7,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,8,1,8,0},
				{7,0,9,9,9,8,8,8,4,8,9,8,4,9,8,4,8,9,8,0},
				{7,9,9,10,8,1,1,8,8,8,8,4,8,8,8,8,8,9,8,8},
				{7,7,10,10,10,10,8,8,8,8,8,8,8,8,0,0,0,7,7,7},
				{7,7,10,10,10,10,10,10,10,10,10,10,7,7,7,7,7,7,7,7}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 7;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 1:
								tile.WallType = WallID.MudstoneBrick;
								tile.WallColor = PaintID.GrayPaint;
								break;
							case 2:
								tile.WallType = WallID.Ebonwood;
								tile.WallColor = PaintID.BrownPaint;
								break;
							case 3:
								tile.WallType = 4;
								break;
							case 4:
								tile.WallType = 5;
								break;
							case 5:
								tile.WallType = WallID.PinkDungeonSlab;
								tile.WallColor = PaintID.GrayPaint;
								break;
							case 6:
								tile.WallType = WallID.MetalFence;
								break;
							case 7:
								tile.WallType = 59;
								break;
							case 8:
								tile.WallType = 1;
								break;
							case 9:
								tile.WallType = 16;
								break;
							case 10:
								tile.WallType = 2;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,2,3,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,2,2,3,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,1,2,2,2,2,3,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0,0},
				{0,0,4,5,2,2,2,2,2,2,5,4,0,0,0,0,0,0,0,0},
				{0,0,5,6,5,5,5,5,5,5,6,5,0,0,0,0,0,0,0,0},
				{0,0,7,6,7,7,7,7,7,7,6,7,0,0,0,0,0,0,0,0},
				{0,0,0,6,0,0,0,8,9,19,6,0,0,0,0,0,0,0,0,0},
				{0,0,0,6,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0},
				{0,0,0,6,0,11,0,0,0,0,6,12,12,12,0,0,0,0,0,0},
				{0,0,0,6,0,0,0,0,0,0,6,5,5,5,12,12,12,12,0,0},
				{0,0,0,6,13,0,0,0,10,0,6,5,5,5,5,5,5,5,5,0},
				{0,0,7,6,7,7,14,14,7,7,6,7,7,7,7,7,7,6,15,0},
				{0,0,0,6,0,0,0,0,0,0,6,0,16,9,18,9,0,6,0,0},
				{0,0,0,6,0,0,0,0,0,0,20,0,21,21,21,21,0,6,0,0},
				{0,0,0,0,0,11,0,0,0,0,20,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,20,0,0,0,0,0,0,0,0,0},
				{0,27,0,22,0,0,23,0,24,0,20,0,0,25,0,26,0,22,0,0},
				{28,28,2,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,2,28},
				{29,30,2,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,2,30},
				{30,2,2,0,20,0,0,20,0,0,20,0,0,20,0,0,20,0,0,30},
				{30,30,30,32,20,33,0,20,0,0,20,0,0,20,0,0,20,0,34,30},
				{29,30,30,30,30,30,34,20,0,0,20,0,0,20,35,36,30,30,30,30},
				{29,29,30,30,30,30,30,30,30,30,30,34,34,30,30,30,30,30,30,30},
				{29,29,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,29,30,30}
			};
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY - 2, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = TileID.BlueDynastyShingles;
									tile.TileColor = PaintID.GrayPaint;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = TileID.BlueDynastyShingles;
									tile.TileColor = PaintID.GrayPaint;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = TileID.RichMahogany;
									tile.TileColor = PaintID.BrownPaint;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									if (confirmPlatforms == 2)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Bottles, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 2)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Books, true, true, -1, Main.rand.Next(6));
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 19:
									if (confirmPlatforms == 2)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Books, true, true, -1, Main.rand.Next(6));
									if (WaterBolt)
										tile.TileFrameX = 90;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 101, true, true, -1, 5);
									}
									break;
								case 11:
									if (confirmPlatforms == 2)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Candles, true, true, -1, 1);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 13:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 376, true, true, -1, 0);
									}
									break;
								case 14:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = TileID.BlueDynastyShingles;
									tile.TileColor = PaintID.GrayPaint;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 16:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 4);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 18:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 3);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 21:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 1);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 22:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 13);
									}
									break;
								case 23:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 14);
									}
									break;
								case 24:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 15, true, true, -1, 17);
									}
									break;
								case 25:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 94, true, true, -1, 0);
									}
									break;
								case 26:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 27:
									tile.HasTile = true;
									tile.TileType = 3;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 28:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 29:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 30:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 31:
									tile.HasTile = true;
									tile.TileType = TileID.Mudstone;
									tile.TileColor = PaintID.GrayPaint;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 32:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 33:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 227, true, true, -1, 1);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 34:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 35:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 227, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 36:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY + 6, 1, _structure.GetLength(1) / 2, 6);
		}
		public static void GenerateLegacy0StarterHouse(int spawnX, int spawnY)
		{
			int[,] _structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,2,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,3,2,3,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,3,3,2,3,3,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,2,1,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,4,4,4,4,2,4,4,4,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,2,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,1,3,3,3,3,2,3,3,3,3,1,0,0,0,0,0,0,0},
				{0,0,0,0,1,3,3,3,3,2,3,3,3,3,1,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,2,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,4,4,4,4,0},
				{0,0,0,0,5,5,5,5,5,5,5,1,1,5,5,0,4,4,4,4,4,0},
				{0,0,0,0,5,3,3,3,3,3,5,1,1,5,5,0,4,4,4,4,4,0},
				{0,0,0,0,5,3,3,3,3,3,5,1,1,5,5,0,4,4,4,4,4,0},
				{0,0,0,0,5,5,5,5,5,5,5,1,1,5,5,0,4,4,4,4,4,0},
				{0,0,0,0,6,6,6,6,6,6,6,1,1,6,6,0,4,4,4,4,4,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,0,0,0,0,0,0,0,0,0},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,0,0,0,0,0}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 2;
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 4;
								break;
							case 2:
								tile.WallType = 78;
								break;
							case 3:
								tile.WallType = 21;
								break;
							case 4:
								tile.WallType = 27;
								break;
							case 5:
								tile.WallType = 142;
								break;
							case 6:
								tile.WallType = 5;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,2,2,2,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,2,2,3,2,2,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,2,2,3,3,0,2,2,1,1,0,0,0,0,0,0,0},
				{0,0,0,1,1,2,2,3,0,0,14,14,2,2,1,1,0,0,0,0,0,0},
				{0,0,1,1,2,2,3,5,6,7,4,14,8,2,2,1,1,0,0,0,0,0},
				{0,1,1,2,2,9,9,9,9,9,9,9,9,9,2,2,1,1,0,0,0,0},
				{0,0,0,2,2,3,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0},
				{0,0,0,2,3,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0},
				{0,0,0,10,0,0,0,0,0,0,0,0,0,0,0,10,0,0,0,0,0,0},
				{0,0,0,10,0,0,0,0,0,0,0,0,0,0,11,10,0,0,0,0,0,0},
				{0,0,0,2,0,12,0,0,0,0,0,0,0,9,9,2,0,1,0,13,0,1},
				{0,0,0,2,1,1,1,1,1,1,1,9,9,1,1,2,1,1,1,1,1,1},
				{0,0,0,2,2,2,2,2,2,2,2,9,9,2,2,2,2,2,2,2,2,2},
				{0,0,0,2,2,0,3,3,0,0,0,9,9,0,2,2,2,3,3,0,2,2},
				{0,0,0,2,15,0,0,0,0,0,0,9,9,0,0,2,3,0,0,0,0,2},
				{0,0,0,2,9,9,0,0,0,0,0,9,9,0,0,2,0,0,16,0,0,2},
				{0,0,0,0,0,0,0,0,0,0,0,9,9,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,9,9,0,0,0,0,0,0,0,0,0},
				{0,0,0,19,0,21,0,0,22,0,23,9,9,0,0,19,0,24,24,24,0,19},
				{0,25,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{25,26,26,1,1,1,1,27,27,27,1,1,27,27,27,1,27,27,1,1,27,27},
				{26,26,26,26,1,27,27,27,27,1,1,27,27,27,27,27,27,1,1,27,26,25}
			};
			UseStarterHouseHalfCircle(spawnX, spawnY, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 14:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 5:
								case 6:
								case 8:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									else if (confirmPlatforms == 1)
									{
										WorldGen.PlaceTile(k, l, TileID.Books, true, true, -1, Main.rand.Next(5));
										tile.Slope = 0;
										tile.IsHalfBlock = false;
									}
									break;
								case 7:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 33, true, true, -1, 1);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 54;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 319, true, true, -1, 0);
									}
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 79, true, true, -1, 0);
									}
									break;
								case 13:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 215, true, true, -1, 0);
									}
									break;
								case 15:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 103, true, true, -1, 2);
									}
									break;
								case 16:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 240, true, true, -1, 20);
									}
									break;
								case 19:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 21:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 21, true, true, -1, 5);
									}
									break;
								case 22:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 96, true, true, -1, 0);
									}
									break;
								case 23:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 172, true, true, -1, 0);
									}
									break;
								case 24:
									tile.HasTile = true;
									tile.TileType = 332;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 25:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 26:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 27:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY, 1, _structure.GetLength(1) / 2, 8);
			
		}
		public static void GenerateLegacy1StarterHouse(int spawnX, int spawnY)
		{
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,1,1,0,0,0,2,0,0,1,1,0,0,0},
				{0,0,1,1,0,0,0,0,2,0,0,0,1,0,0,0},
				{0,0,1,0,3,3,0,0,2,0,3,0,1,1,0,0},
				{0,1,1,3,3,3,3,0,2,3,3,3,3,1,1,0},
				{0,0,0,0,0,0,4,4,2,0,0,0,0,0,0,0},
				{5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 2;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 60;
								break;
							case 2:
								tile.WallType = 78;
								break;
							case 3:
								tile.WallType = 66;
								break;
							case 4:
								tile.WallType = 1;
								break;
							case 5:
								tile.WallType = 2;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,1,1,1,1,1,0,0,0,1,1,1,1,0,0},
				{0,1,1,1,1,0,0,0,0,0,0,0,2,0,0,0},
				{0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,14,14,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,9,14,0,0,0,0,0,0,3,0,0,0,0},
				{5,6,7,7,7,7,0,4,0,7,7,7,7,7,7,5},
				{8,8,6,7,7,7,7,7,6,6,6,7,7,8,8,8},
				{8,8,8,8,6,6,6,7,7,7,7,8,8,8,8,5}
			};
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 14:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 42, true, true, -1, 7);
									}
									break;
								case 3:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 105, true, true, -1, 1);
									}
									break;
								case 4:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 215, true, true, -1, 0);
									}
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY, 1, _structure.GetLength(1) / 2, 5);
		}
		public static void GenerateLegacy2StarterHouse(int spawnX, int spawnY)
		{
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0,0},
				{0,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,0,0},
				{0,0,0,4,4,2,2,4,2,2,4,2,2,4,4,0,0},
				{0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,0,0},
				{0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,0,0},
				{0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 2;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 4;
								break;
							case 2:
								tile.WallType = 21;
								break;
							case 3:
								tile.WallType = 27;
								break;
							case 4:
								tile.WallType = 1;
								break;
							case 5:
								tile.WallType = 2;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,2,2,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,2,0,0,2,1,1,0,0,0,0},
				{0,0,0,0,1,1,2,0,0,0,0,2,1,1,0,0,0},
				{0,0,0,1,1,2,0,0,0,0,0,0,2,1,1,0,0},
				{0,0,0,0,0,2,0,0,0,0,0,0,2,0,0,0,0},
				{0,0,0,0,0,3,0,0,0,0,0,0,3,0,0,0,0},
				{0,0,0,0,0,3,0,0,0,0,0,0,3,0,0,0,0},
				{0,0,0,0,0,2,0,0,0,0,0,0,2,0,0,0,0},
				{0,1,1,1,1,2,4,0,4,0,4,0,2,1,1,1,1},
				{0,2,2,2,2,2,5,5,5,5,5,5,2,2,2,2,2},
				{0,0,2,2,0,0,0,0,0,0,0,0,0,0,2,2,0},
				{6,0,2,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,16,16,16,0,16,16,0,0,0,0},
				{0,0,7,0,10,11,16,8,16,0,9,16,12,0,0,7,0},
				{13,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{14,14,1,1,15,1,1,15,15,15,15,1,1,1,15,14,14},
				{14,14,14,14,14,14,14,15,1,1,1,1,14,14,14,14,14}
			};
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 16:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 54;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 21, true, true, -1, 5);
									}
									break;
								case 5:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 55, true, true, -1, 3);
									}
									break;
								case 7:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 8:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Furnaces, true, true, -1, 0);
									}
									break;
								case 9:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 10:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 239, true, true, -1, 2);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Anvils, true, true, -1, 0); // WorldGen.SavedOreTiers.Iron == 6 ? 0 : 1);
									}
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 18, true, true, -1, 0);
									}
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY, 1, _structure.GetLength(1) / 2, 7);
			
		}
		public static void GenerateLegacy3StarterHouse(int spawnX, int spawnY)
		{
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,0,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,3,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,4,2,2,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,4,4,4,2,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,4,2,2,2,4,2,5,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,4,4,4,2,4,4,4,2,5,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,4,2,2,2,4,2,2,2,4,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,2,2,2,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,4,4,4,4,4,4,4,6,3,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,4,4,4,4,4,4,4,6,3,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,4,4,4,4,4,4,4,6,3,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,2,2,2,2,2,2,2,6,3,0,0,0,0,7,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,7,7,7,7,7,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,3,2,2,2,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,3,3,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,3,6,4,4,4,4,4,4,4,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,6,4,4,4,4,4,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,3,6,4,4,4,4,4,4,4,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,6,4,4,4,4,4,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,3,6,4,4,4,4,4,4,4,6,3,0,8,8,8,8,8,8,8,8,8,8,8,8,0,3,3,3,3,6,4,4,4,4,4,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,7,0,0,0,0,3,6,2,2,2,2,2,2,2,6,3,0,8,8,8,8,8,8,8,8,8,8,8,8,0,3,3,3,3,6,2,2,2,2,2,6,0,0,0,0,7,0,0,0},
				{0,0,0,0,0,0,7,7,7,7,7,3,3,3,3,3,3,3,3,3,3,3,0,9,9,9,9,9,8,9,9,9,9,8,8,0,3,3,3,3,3,3,3,3,3,3,3,7,7,7,7,7,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,0,0,0,0,3,3,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,5,3,3,3,2,2,2,2,2,2,2,0,2,2,2,2,2,2,2,2,2,2,2,2,3,2,3,3,3,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,6,4,4,4,4,4,6,3,3,6,4,4,4,4,4,4,4,4,6,3,3,3,3,3,3,6,4,4,4,4,4,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,6,4,4,4,4,4,6,3,3,6,4,4,4,4,4,4,4,4,6,3,3,3,3,3,3,6,4,4,4,4,4,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,6,2,2,2,2,2,6,3,3,6,2,2,2,2,2,2,2,2,6,3,3,3,3,3,3,6,2,2,2,2,2,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,8,3,3,3,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,3,3,3,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,8,3,3,3,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,3,3,3,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,8,8,8,8,0,0,0,0,0,0,0,0,8,8,8,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 2;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 60;
								break;
							case 2:
								tile.WallType = 1;
								break;
							case 3:
								tile.WallType = 4;
								break;
							case 4:
								tile.WallType = 21;
								break;
							case 5:
								tile.WallType = 27;
								break;
							case 6:
								tile.WallType = 78;
								break;
							case 7:
								tile.WallType = 106;
								break;
							case 8:
								tile.WallType = 5;
								break;
							case 9:
								tile.WallType = 115;
								break;
							case 10:
								tile.WallType = 2;
								break;
						}
					}
				}
			}
			_structure = new int[,]{
				{0,0,0,0,0,0,0,0,0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,3,2,2,2,2,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,0,0,0,0,0,2,2,5,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,1,2,2,2,2,4,0,0,0,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0},
				{3,2,2,2,2,5,2,0,0,8,5,5,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,9,7,7,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,2,2,2,10,5,5,5,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,9,11,9,7,7,0,0,0,0,0,0,0,0,0,0},
				{0,0,12,0,13,0,0,0,5,0,0,2,2,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,9,11,0,0,9,7,7,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,5,0,2,2,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,9,11,0,0,0,11,9,7,7,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,10,5,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,9,11,0,0,0,0,11,11,9,7,7,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,10,5,0,0,0,0,0,7,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,9,11,11,0,0,0,0,0,11,11,9,7,7,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,5,14,0,0,0,7,7,7,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,9,11,11,15,15,0,15,15,0,15,0,11,9,7,7,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,10,5,0,8,7,7,9,7,7,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,9,0,0,16,16,16,16,16,16,16,0,0,9,5,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,5,5,7,7,17,11,18,7,7,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,19,0,0,0,0,0,0,0,0,0,0,0,19,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,5,7,7,17,0,0,0,18,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,19,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,21,7,7,17,11,0,20,0,0,18,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,19,0,0,0,15,15,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,7,7,17,11,11,0,0,0,11,11,18,7,7,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,16,16,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,7,7,17,11,11,0,0,0,0,0,11,11,18,7,7,0,0,0,0,0,0,0,0,0,0,5,9,0,0,0,0,24,0,0,23,0,0,0,22,0,0,0,9,5,0,0},
				{0,0,0,0,0,0,0,0,7,7,9,0,11,15,15,15,0,15,0,15,0,0,9,7,7,0,0,0,0,0,0,0,0,0,5,9,9,16,16,16,9,9,9,9,9,9,9,9,9,9,9,9,5,0,0},
				{0,0,0,0,0,0,0,0,0,5,9,0,0,16,16,16,16,16,16,16,0,0,9,5,0,0,0,0,0,0,0,0,0,0,5,9,0,16,16,16,0,0,11,11,11,11,0,9,5,5,5,5,5,0,0},
				{0,0,0,0,0,0,0,0,0,0,19,0,0,0,0,0,0,0,0,0,0,0,19,0,0,0,0,0,0,0,0,0,0,0,0,19,0,16,16,16,0,0,0,0,11,11,11,19,0,0,0,25,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0,0,0,0,19,0,0,0,0,0,0,0,0,0,0,0,0,19,0,16,16,16,0,0,0,0,0,0,0,7,0,0,0,25,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,19,0,0,0,0,0,26,0,0,0,0,0,0,19,0,16,16,16,0,0,0,0,0,0,0,0,0,0,0,25,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,2,2,4,0,16,16,1,2,4,0,0,7,0,16,16,16,0,0,0,0,0,0,0,0,0,0,0,25,0,0,0},
				{0,0,0,0,0,5,9,0,0,0,27,0,0,0,28,0,24,0,0,23,0,0,9,2,2,2,2,2,1,2,2,2,2,4,5,9,0,16,16,16,24,0,0,23,0,0,0,29,0,0,0,9,5,0,0},
				{0,0,0,0,0,5,9,9,9,9,9,9,16,16,16,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,16,16,16,9,9,9,9,9,9,9,9,9,9,9,9,5,2,0},
				{0,0,0,0,0,5,5,5,5,5,9,0,16,16,16,0,0,0,11,11,11,11,25,11,11,0,0,0,0,0,0,11,11,11,11,11,0,16,16,16,0,0,0,0,0,0,0,9,5,5,5,5,5,2,2},
				{0,0,0,0,0,0,25,0,0,0,19,0,16,16,16,0,0,0,0,0,11,11,25,11,0,0,0,0,0,0,0,0,0,11,11,11,0,16,16,16,0,0,0,0,0,0,0,19,0,0,0,25,2,2,0},
				{0,0,0,0,0,0,25,0,0,0,7,0,16,16,16,0,0,0,0,0,0,0,25,0,0,0,0,0,0,0,0,0,0,0,30,30,0,16,16,16,0,31,0,30,30,30,0,7,0,0,0,25,0,0,0},
				{0,0,0,0,0,0,25,0,0,0,32,0,16,16,16,0,0,0,33,0,0,0,25,0,33,0,0,0,0,33,34,0,0,0,16,16,0,16,16,16,0,16,16,16,16,16,0,0,0,0,0,25,0,0,0},
				{0,0,0,0,0,0,25,0,0,0,0,0,16,16,16,0,0,0,0,0,0,0,25,0,0,0,0,0,0,0,0,0,0,0,0,0,0,16,16,16,0,0,0,0,0,0,0,0,0,0,0,25,0,0,0},
				{0,40,41,0,0,0,25,0,0,0,0,0,16,16,16,0,35,0,36,0,35,0,25,0,36,0,35,0,35,0,37,0,0,38,0,38,0,16,16,16,0,39,38,0,38,0,0,22,0,0,0,25,0,0,0},
				{43,43,43,40,41,0,25,0,0,44,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,44,0,0,25,0,0,42},
				{43,45,45,43,43,7,7,7,7,46,46,46,7,7,7,7,7,7,46,46,46,46,46,46,7,7,46,46,46,46,46,46,46,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,43,43},
				{45,45,45,45,46,7,7,7,7,7,7,46,46,46,46,46,7,7,7,7,46,46,46,46,46,46,7,7,7,7,7,7,7,46,46,46,46,46,46,7,7,7,7,7,7,7,7,7,7,7,7,7,7,45,45}
			};
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 11:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 15:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									else if (confirmPlatforms == 1)
									{
										WorldGen.PlaceTile(k, l, TileID.Books, true, true, -1, Main.rand.Next(5));
										tile.Slope = 0;
										tile.IsHalfBlock = false;
									}
									break;
								case 16:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 17:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 18:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 19:
									tile.HasTile = true;
									tile.TileType = 54;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 240, true, true, -1, 48);
									}
									break;
								case 21:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 32:
								case 22:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 23:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										int temp = Main.player[Main.myPlayer].direction;
										Main.player[Main.myPlayer].direction = -1;
										WorldGen.PlaceTile(k, l, 79, true, true, Main.myPlayer, 0);
										Main.player[Main.myPlayer].direction = temp;
									}
									break;
								case 24:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 18, true, true, -1, 0);
									}
									break;
								case 25:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 26:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 27:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 28:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 304, true, true, -1, 0);
									}
									break;
								case 29:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 30:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 31:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 103, true, true, -1, 2);
									}
									break;
								case 33:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 4);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 34:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 49, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 35:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 15, true, true, -1, 21);
									}
									break;
								case 36:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 16);
									}
									break;
								case 37:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 17);
									}
									break;
								case 38:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 21, true, true, -1, 5);
									}
									break;
								case 39:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 94, true, true, -1, 0);
									}
									break;
								case 40:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 41:
									tile.HasTile = true;
									tile.TileType = 73;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 42:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 254, true, true, -1, 4);
									}
									break;
								case 43:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 44:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 45:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 46:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY, 1, _structure.GetLength(1) / 2, 9);
			
		}
		public static void GenerateLegacy4StarterHouse(int spawnX, int spawnY)
		{
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
				{0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,2,2,0,0,3,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,2,2,2,2,3,2,2,2,2,2,2,0,0},
				{0,0,0,2,2,2,2,2,2,3,2,2,2,2,2,2,0,0},
				{4,4,4,2,2,2,2,2,5,3,5,2,2,2,2,2,0,0},
				{4,4,4,2,2,2,2,5,5,3,5,5,2,2,2,2,4,4},
				{4,4,4,4,4,4,4,5,5,3,5,5,4,4,4,6,6,6},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,6,6,6,6},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,6,6,6,6,6}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 10;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 106;
								break;
							case 2:
								tile.WallType = 4;
								break;
							case 3:
								tile.WallType = 78;
								break;
							case 4:
								tile.WallType = 2;
								break;
							case 5:
								tile.WallType = 1;
								break;
							case 6:
								tile.WallType = 59;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,1,1,0,0,0,0,0,1,1,1,1,1,1,1},
				{2,0,0,0,2,1,1,2,0,0,0,2,1,1,1,1,1,1,1},
				{1,1,3,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1},
				{1,1,3,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1},
				{1,1,3,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1},
				{1,1,3,1,1,1,1,1,1,3,4,4,1,1,1,1,1,1,1},
				{1,1,3,7,1,1,1,1,4,3,4,4,5,1,6,1,1,1,1},
				{1,8,8,8,8,9,9,8,8,8,8,8,8,8,8,8,8,8,1},
				{0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{-1,8,0,0,-2,-2,1,1,1,1,-2,-2,-2,-2,-2,0,0,8,-1},
				{-1,8,0,-2,-2,1,1,1,1,1,1,1,1,1,-2,-2,0,8,-1},
				{-1,8,0,1,1,11,11,1,1,1,1,1,1,1,1,1,0,8,-1},
				{-1,8,0,10,1,11,11,11,1,1,1,1,5,1,5,1,0,8,-1},
				{13,8,8,8,8,8,14,14,1,12,1,14,14,8,8,14,14,14,14},
				{15,15,14,14,14,14,8,8,8,8,14,14,14,8,8,8,14,14,13},
				{15,15,15,15,14,14,14,14,14,14,14,14,8,8,8,15,15,15,15}
			};
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case -2:
									tile.HasTile = true;
									tile.TileType = TileID.Cobweb;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 0:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 1:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 2:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Banners, true, true, -1, 0);
									}
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									else if (!tile.HasTile)
									{
										WorldGen.PlaceTile(k, l, TileID.MetalBars, true, true, -1, 0);// WorldGen.IronTierOre == 6 ? 2 : 3);
										tile.Slope = 0;
										tile.IsHalfBlock = false;
									}
									break;
								case 5:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.FishingCrate, true, true, -1, 0);
									}
									break;
								case 6:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 7:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Anvils, true, true, -1, 0);// WorldGen.IronTierOre == 6 ? 0 : 1);
									}
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 0);
									}
									break;
								case 10:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 21, true, true, -1, 5);
									}
									break;
								case 11:
									tile.HasTile = true;
									tile.TileType = 332;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 215, true, true, -1, 0);
									}
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			//UseStarterHouseHalfCircle(spawnX, spawnY, 1, _structure.GetLength(1) / 2, 15);
			
		}
		public static void GenerateLegacy5StarterHouse(int spawnX, int spawnY)
        {
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,0,0,3,0,0,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,2,2,3,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,2,2,0,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 6;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 5;
								break;
							case 2:
								tile.WallType = 4;
								break;
							case 3:
								tile.WallType = 78;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,2,2,2,0,0,0,2,2,2,1,0,0,2,2,2,0,0},
				{0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0},
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0},
				{0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0},
				{0,0,0,0,0,2,4,4,4,4,4,4,4,2,0,0,0,0,0},
				{0,0,0,0,0,2,4,5,5,5,5,5,4,2,0,0,0,0,0},
				{0,0,0,0,0,3,4,3,3,3,3,3,4,3,0,0,0,0,0},
				{0,0,0,0,0,2,4,0,0,0,0,0,4,2,0,0,0,0,0},
				{0,0,0,0,0,2,4,6,0,0,0,0,4,2,0,0,0,0,0},
				{0,0,0,0,0,3,0,0,0,0,0,0,0,3,0,0,0,0,0},
				{0,0,0,0,0,2,6,0,6,0,0,7,7,2,0,0,0,0,0},
				{0,0,0,0,0,2,8,8,8,8,8,8,8,2,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,9,0,0,0,0,0,0,0,10,0,0,0,0,0},
				{0,0,0,0,2,2,2,0,0,11,0,0,2,2,2,0,0,0,0},
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0},
				{0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0},
				{0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
				{12,12,13,2,2,2,2,2,2,2,2,2,2,2,2,2,2,12,12},
				{13,12,13,13,2,2,2,14,14,14,14,14,14,2,2,13,13,13,13},
				{13,13,13,13,13,14,14,14,2,2,2,14,14,14,13,13,13,13,13},
				{13,13,13,13,13,13,14,2,2,2,13,13,13,13,13,13,13,13,13},
				{13,13,15,15,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13}
			};
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 273;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 6:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 376, true, true, -1, 0);
									}
									break;
								case 7:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = true;
										tile.TileType = 332;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
									}
									break;
								case 8:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 15);
									}
									break;
								case 10:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 15);
									}
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 215, true, true, -1, 0);
									}
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY, 1, _structure.GetLength(1) / 2, 8);
			

		}
		public static void GenerateLegacy6StarterHouse(int spawnX, int spawnY)
		{
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,2,0,0,1,0,0,2,1,0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,3,0,0,2,0,2,2,0,1,2,2,1,1,0,0,2,0,0,0,1,0,1,0,1,0,2,0,0,0,0,0,0},
				{0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0},
				{4,4,0,0,0,0,0,0,0,0,5,5,5,5,5,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{4,4,0,4,4,4,0,0,0,5,5,5,5,5,5,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{4,4,4,6,5,5,5,5,7,7,7,5,5,5,5,3,8,5,7,7,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{4,4,4,4,4,5,5,5,7,7,7,7,7,7,7,8,8,7,7,7,7,7,8,8,8,8,8,5,5,5,5,5,5,5,5,5,5},
				{4,6,4,4,4,4,5,5,7,7,7,7,7,7,7,8,7,7,7,7,7,7,7,5,5,8,8,7,7,5,5,5,5,5,5,5,5},
				{4,4,4,4,4,6,5,5,0,7,7,7,7,7,7,7,7,8,8,5,5,7,5,5,5,8,5,5,7,7,5,5,5,5,5,5,5},
				{4,4,4,6,4,4,5,5,5,7,7,7,7,7,7,7,7,8,5,5,5,5,5,5,5,5,5,5,7,7,5,5,5,5,5,5,5},
				{4,4,6,6,6,6,4,5,5,5,5,0,0,7,7,7,7,8,7,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{5,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,0,7,7,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{5,5,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,9,5},
				{5,9,4,4,6,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{9,9,9,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{9,5,5,5,4,4,4,4,5,5,5,5,5,5,5,5,9,9,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 16;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 5;
								break;
							case 2:
								tile.WallType = 16;
								break;
							case 3:
								tile.WallType = 1;
								break;
							case 4:
								tile.WallType = 63;
								break;
							case 5:
								if (tile.WallType == 0)
									tile.WallType = WallID.DirtUnsafe;
								break;
							case 6:
								tile.WallType = 65;
								break;
							case 7:
								tile.WallType = 66;
								break;
							case 8:
								tile.WallType = 68;
								break;
							case 9:
								tile.WallType = 59;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,0,0,0,2,6,0,0,0,0,0},
				{0,0,0,0,0,0,5,2,2,5,0,0,0,0,0,0,0,0,0,0,0,0,4,0,5,5,0,0,9,9,2,2,0,0,0,0,0},
				{0,0,10,1,2,5,10,2,2,11,7,0,5,0,0,0,0,0,0,12,12,13,14,14,15,9,8,0,9,10,2,2,16,0,0,17,0},
				{1,1,2,2,2,13,14,14,14,14,14,14,14,15,5,5,5,6,13,14,14,14,14,14,14,14,14,14,14,14,2,2,14,14,18,19,17},
				{14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,10,10,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,20},
				{21,21,10,10,10,10,10,14,14,10,10,10,14,14,14,5,10,14,10,10,14,14,14,14,14,14,10,14,14,10,10,10,10,10,10,10,21},
				{21,21,21,21,10,10,10,10,10,14,14,14,14,10,10,10,5,14,10,10,14,14,10,10,10,14,14,10,10,10,10,10,10,21,21,21,21},
				{21,21,21,21,21,21,21,20,20,20,20,21,21,21,20,5,5,20,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21},
				{21,21,21,21,21,21,21,20,22,0,23,20,20,20,20,5,10,20,20,20,20,20,20,20,20,20,20,21,21,21,21,21,21,21,21,21,21},
				{21,21,21,21,21,21,21,20,22,0,0,22,22,23,20,0,0,20,20,0,0,0,0,0,0,0,23,20,20,21,21,21,21,21,21,21,21},
				{21,21,21,21,21,21,21,20,24,5,0,22,22,0,22,0,0,22,0,0,0,0,0,0,0,0,0,5,23,20,21,21,21,21,21,21,21},
				{21,21,21,21,21,21,21,21,20,25,5,0,0,0,22,0,0,0,0,0,0,0,0,0,0,0,0,5,5,23,20,21,21,21,21,21,21},
				{20,20,21,21,21,21,21,21,21,20,25,26,0,5,22,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,20,21,21,21,21,21,21},
				{21,21,21,21,21,21,21,21,21,21,20,20,20,24,22,10,10,0,0,0,0,0,0,0,0,0,0,0,0,5,20,21,21,21,21,21,21},
				{21,21,20,21,21,21,21,21,21,21,21,21,21,20,20,20,10,10,27,27,27,27,27,27,27,0,0,27,27,27,20,21,21,21,21,21,21},
				{21,21,21,21,21,21,21,21,21,21,21,21,21,20,21,20,21,20,20,20,24,24,29,29,29,28,0,29,29,30,20,21,21,21,21,21,21},
				{21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,20,20,20,20,20,20,20,20,20,21,21,21,21,21,21,21},
				{21,21,21,21,20,21,21,21,21,21,21,21,21,21,21,20,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21},
				{21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21}
			};
			UseStarterHouseHalfCircle(spawnX, spawnY - 1, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 57;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 57;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 57;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 4:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 2);
									}
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 7:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 4);
									}
									break;
								case 8:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 1);
									}
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 331;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									tile.HasTile = true;
									tile.TileType = 57;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 332;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 16:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 17:
									tile.HasTile = true;
									tile.TileType = 3;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 18:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 19:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 78, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 21:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 22:
									tile.HasTile = true;
									tile.TileType = 52;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 24:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 25:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 26:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 27:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 46;
										tile.LiquidType = 0;
									}
									break;
								case 28:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<EnchantedSwordShrineTile>(), true, true, -1, 0);
									}
									break;
								case 29:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 255;
										tile.LiquidType = 0;
									}
									break;
								case 30:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
		}
		public static void GenerateLegacy7StarterHouse(int spawnX, int spawnY)
		{
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 15;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 60;
								break;
							case 2:
								if (tile.WallType == 0)
									tile.WallType = 2;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,2,2,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,2,2,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,2,2,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,1,1,2,2,2,2,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,2,2,2,1,1,2,2,1,1,2,2,2,2,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,2,2,1,1,1,1,1,2,2,2,2,1,1,2,2,2,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,2,2,2,2,1,1,1,1,1,3,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,1,2,2,1,1,0,3,0,0,3,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,3,0,4,1,1,1,4,2,2,4,4,0,3,0,0,3,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,3,0,0,4,3,4,2,2,2,4,0,0,3,0,0,3,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,3,0,0,0,3,4,2,2,4,4,0,0,3,5,0,3,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,2,2,2,2,0,0,6,6,6,6,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,8,8,9,0,0,0,0,2,2,2,0,0,0,0,0,0,10,2,2,0,0,0,0,0,0},
				{11,12,12,12,12,12,8,8,13,8,8,12,12,12,2,2,2,2,14,14,12,12,2,2,12,2,2,12,12,12,12,8},
				{8,14,14,14,14,14,14,14,14,12,12,12,14,14,2,2,2,2,2,14,12,2,2,14,14,12,12,2,14,14,13,13},
				{13,13,12,12,14,14,14,12,12,12,12,12,12,12,2,2,2,2,2,2,2,2,12,12,12,12,12,13,13,13,13,13},
				{8,13,13,13,13,13,13,13,13,13,13,13,12,14,2,2,2,2,13,13,13,13,13,13,13,13,13,13,13,13,13,13},
				{13,13,13,13,13,13,13,13,13,13,13,13,13,2,2,2,2,2,14,14,14,13,13,13,13,13,13,13,13,13,13,13},
				{13,13,13,13,13,13,8,8,8,2,2,13,2,2,2,2,2,2,2,2,14,14,13,13,13,13,13,13,13,13,13,13},
				{13,13,13,13,13,13,8,4,8,8,13,2,2,2,13,2,2,2,13,2,2,14,13,13,13,13,13,13,13,13,13,13},
				{13,13,13,13,13,8,8,4,4,2,2,2,4,4,13,2,2,13,2,13,2,2,8,13,13,13,13,13,13,13,13,13},
				{13,13,13,13,8,15,0,0,0,8,8,0,0,0,2,4,2,13,2,13,0,0,8,13,13,13,13,13,13,13,13,13},
				{13,13,13,13,8,4,0,0,0,0,0,0,0,0,2,4,2,4,4,2,0,0,2,8,13,13,13,13,13,13,13,13},
				{13,13,13,13,8,16,0,0,0,0,0,0,0,0,0,4,2,4,0,0,0,0,0,8,13,13,13,13,13,13,13,13},
				{13,13,13,13,13,8,0,0,0,0,0,0,0,0,0,0,4,4,0,0,17,0,18,8,13,13,13,13,13,13,13,13},
				{13,13,13,13,13,8,16,0,0,0,0,0,0,0,0,0,0,0,19,0,8,8,8,13,13,13,13,13,13,13,13,13},
				{13,13,13,13,13,13,8,20,0,0,0,0,0,0,21,4,4,4,8,8,8,13,13,13,13,13,13,13,13,13,13,13},
				{13,13,13,13,13,13,8,8,8,21,0,0,20,0,8,8,4,18,8,13,13,13,13,13,13,13,13,13,13,13,13,13},
				{13,13,13,13,13,13,13,13,8,8,8,8,8,8,8,8,8,8,13,13,14,13,13,13,8,13,13,13,13,13,13,13}
			};
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY - 3, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 353;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 6:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 23);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 1);
									}
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 11:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 227, true, true, -1, 3);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 16:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 17:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 227, true, true, -1, 8);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 18:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 19:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 227, true, true, -1, 11);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 227, true, true, -1, 10);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 21:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 227, true, true, -1, 9);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			
		}
		public static void GenerateLegacy8StarterHouse(int spawnX, int spawnY)
		{
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,2,1,2,3,3,3,2,1,1,2,3,3,3,2,1,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,2,2,1,2,3,3,3,2,1,1,2,3,3,3,2,1,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,2,2,1,2,3,3,3,2,1,1,2,3,3,3,2,1,2,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,2,2,2,1,2,3,3,3,2,1,1,2,3,3,3,2,1,2,2,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,2,2,2,2,1,2,3,3,3,2,1,1,2,3,3,3,2,1,2,2,2,2,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,2,2,2,2,1,2,3,3,3,2,1,1,2,3,3,3,2,1,2,2,2,2,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,2,2,2,2,1,2,3,3,3,2,1,1,2,3,3,3,2,1,2,2,2,2,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,2,2,2,2,1,2,3,3,3,2,1,1,2,3,3,3,2,1,2,2,2,2,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,2,1,0,3,3,3,2,1,1,2,3,3,3,0,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,4,4,4,4,1,4,5,3,3,4,1,1,4,3,3,3,4,1,4,4,4,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,4,4,4,4,1,4,5,5,3,4,1,1,4,3,3,3,4,1,4,4,4,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,4,4,4,4,1,4,5,5,5,4,1,1,4,3,3,3,4,1,4,4,4,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,4,4,4,4,1,4,5,5,5,5,1,1,4,3,3,3,4,1,4,4,4,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,6,6,6,6,1,5,5,5,5,5,5,1,6,3,3,3,6,1,6,6,6,6,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,6,6,6,6,5,5,5,5,5,5,5,5,6,3,3,3,6,1,6,6,6,6,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,5,5,5,5,5,5,5,0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 4;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 78;
								break;
							case 2:
								tile.WallType = 4;
								break;
							case 3:
								tile.WallType = 21;
								break;
							case 4:
								tile.WallType = 27;
								break;
							case 5:
								tile.WallType = 5;
								break;
							case 6:
								tile.WallType = 1;
								break;
						}
					}
				}
			}
			_structure = new int[,]  {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,2,2,2,2,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,2,2,2,3,3,3,2,2,2,2,3,3,3,2,2,2,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,2,2,4,0,0,0,0,4,4,4,0,0,0,0,0,4,2,2,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,2,2,4,0,0,0,0,0,0,4,0,0,0,0,0,0,0,4,2,2,1,1,0,0,0,0,0,0},
				{0,0,0,0,1,1,2,2,4,4,5,0,0,0,0,0,5,5,0,0,0,0,0,5,4,4,2,2,1,1,0,0,0,0,0},
				{0,0,0,0,2,2,2,4,6,7,0,0,0,0,0,0,0,0,0,0,0,9,7,0,0,10,4,2,2,2,0,0,0,0,0},
				{0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,8,0,0,0,0,0,0,0,0,0,0,4,1,1,0,0,0,0,0},
				{0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0},
				{0,0,0,0,12,3,0,0,0,0,0,0,0,0,0,0,15,10,0,0,0,0,0,0,0,0,0,0,3,12,0,0,0,0,0},
				{0,0,0,0,16,3,0,0,11,0,13,0,0,0,14,0,17,0,0,0,0,11,0,0,0,11,0,0,3,16,0,0,0,0,0},
				{0,0,0,0,1,1,2,2,2,2,2,2,18,5,5,5,5,5,5,5,5,5,2,2,2,2,2,2,1,1,0,0,0,0,0},
				{0,0,0,0,2,2,2,4,4,0,0,4,0,18,0,0,0,0,0,0,0,0,19,0,4,4,4,2,2,2,0,0,0,0,0},
				{0,0,0,0,2,2,4,4,0,0,7,15,20,0,18,0,0,0,0,0,0,0,0,0,0,20,15,4,2,2,0,0,2,2,2},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,18,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,19,23,19},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,18,0,0,0,0,0,20,6,0,0,0,0,0,0,0,0,0,23,0},
				{0,0,0,0,0,21,0,0,0,0,0,0,0,0,0,4,4,18,0,0,0,0,0,0,0,0,0,0,22,0,0,0,0,23,0},
				{0,0,0,0,2,2,27,0,24,0,11,0,0,11,0,17,0,0,18,0,25,0,26,0,0,11,0,27,2,2,0,0,0,23,0},
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,23,0},
				{28,28,28,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{29,28,28,28,28,28,1,28,28,1,1,1,28,1,1,1,1,28,1,28,28,1,1,1,1,28,1,28,28,28,28,28,28,29,29},
				{29,29,29,29,29,29,29,29,1,1,1,28,28,28,1,1,28,28,28,28,28,28,1,1,28,1,1,1,1,29,29,29,29,29,29}
			};
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 54;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
								case 7:
								case 9:
								case 15:
								case 20:
									if (confirmPlatforms == 2)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Books, true, true, -1, Main.rand.Next(6));
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 245, true, true, -1, 3);
									}
									break;
								case 10:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 101, true, true, -1, 0);
									}
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 3;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 14:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 89, true, true, -1, 1);
									}
									break;
								case 16:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 78, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 17:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 18, true, true, -1, 0);
									}
									break;
								case 18:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 19:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 91, true, true, -1, 0);
									}
									break;
								case 21:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 13);
									}
									break;
								case 22:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 13);
									}
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = TileID.WoodenBeam;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 24:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 105, true, true, -1, 20);
									}
									break;
								case 25:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 15, true, true, -1, 0);
									}
									break;
								case 26:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 16);
									}
									break;
								case 27:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 28:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 29:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY + 1, 1, _structure.GetLength(1) / 2, 8);

			
		}
		public static void GenerateLegacy9StarterHouse(int spawnX, int spawnY)
		{
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,3,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,3,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,2,2,3,3,0,0,0,0,0,0},
				{0,0,0,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,3,3,3,2,2,3,3,3,0,0,0,0,0},
				{0,0,0,3,0,0,0,0,0,0,3,0,0,0,0,0,0,4,3,3,3,2,2,3,3,3,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,3,3,3,2,2,3,3,3,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,2,3,3,2,2,3,2,2,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,3,2,1,0,0,0,0,0},
				{0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,1,1,2,2,2,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 2;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 5;
								break;
							case 2:
								tile.WallType = 1;
								break;
							case 3:
								tile.WallType = 4;
								break;
							case 4:
								tile.WallType = 78;
								break;
							case 5:
								tile.WallType = 106;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,1,1,1,1,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,2,0,1,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,3,3,2,2,0,0,3,3,1,1,1,0,0},
				{0,0,3,3,3,3,3,3,3,3,3,3,0,0,1,1,1,3,3,3,2,0,0,0,3,3,3,1,1,1,0},
				{0,3,3,3,3,3,3,3,3,3,3,3,3,0,1,1,3,3,3,2,2,0,0,0,2,3,3,3,1,1,0},
				{0,3,3,3,4,2,2,2,0,4,3,3,3,0,0,0,3,3,2,0,0,0,0,0,2,2,3,3,0,0,0},
				{0,0,5,4,0,0,0,0,0,0,4,5,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,3,0,0,0},
				{0,0,5,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,3,0,0,0},
				{0,0,5,0,0,0,0,0,0,0,0,5,0,0,0,0,6,0,0,0,8,8,7,0,8,8,1,1,0,0,0},
				{0,0,5,0,8,8,0,0,0,0,0,5,0,0,0,0,1,1,0,0,0,0,0,0,0,8,1,1,0,0,0},
				{0,0,5,8,8,8,11,0,0,9,0,5,10,0,0,1,1,1,1,7,0,7,0,7,0,1,1,1,1,0,0},
				{12,12,12,12,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{1,12,12,1,1,1,12,1,12,12,1,12,1,1,12,1,1,12,12,12,1,12,1,12,1,12,12,1,12,13,13},
				{13,13,13,13,13,13,13,13,13,13,13,13,13,12,12,12,1,12,12,12,12,1,1,12,12,1,1,12,13,13,13}
			};
			//i = vertical, j = horizontal
			UseStarterHouseHalfCircle(spawnX, spawnY, 0, _structure.GetLength(1) / 2, _structure.GetLength(0));
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 91, true, true, -1, 0);
									}
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 7:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 376, true, true, -1, 0);
									}
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 332;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 17, true, true, -1, 0);
									}
									break;
								case 10:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 16, true, true, -1, 0);
									}
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY - 1, 1, _structure.GetLength(1) / 2, 10);
		}
		public static void GenerateBloodMoonStarterHouse(int spawnX, int spawnY)
        {
			int bannerColor = WorldGen.genRand.Next(4);
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,3,1,1,1,1,3,0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,3,1,1,1,1,1,1,1,1,3,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,3,1,1,1,1,1,1,1,1,3,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,3,4,4,0,4,3,3,3,1,1,1,4,1,1,1,1,3,3,4,0,0,0,4,3,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,4,4,1,3,3,4,4,3,3,3,3,1,1,4,4,4,0,0,0,3,3,3,4,4,4,3,3,1,1,1,1,4,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,3,4,4,4,4,1,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,4,4,4,3,0,0,0,1,4,4,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,1,4,0,0,0,0,0,0,0,4,4,4,3,0,0,0,1,1,0,0,0,2,2,2,2,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 11;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 147;
								break;
							case 2:
								tile.WallType = 145;
								break;
							case 3:
								tile.WallType = 5;
								break;
							case 4:
								tile.WallType = 1;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,4,0,0,4,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,4,2,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,6,7,7,7,8,8,8,8,8,9,9,0,0,0,0,0,2,0,5,0,0,2,0,0,0,0,0,9,9,8,8,8,7,7,7,8,8,8,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,7,7,8,8,8,8,8,8,8,0,0,9,9,8,8,7,7,7,7,7,10,7,8,8,8,9,9,0,0,8,8,8,8,8,7,8,8,8,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,11,2,4,0,0,0,4,2,11,0,0,0,4,8,8,8,8,7,7,7,7,7,8,8,8,0,0,0,4,11,2,4,0,0,0,4,2,11,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,0,0,12,0,4,2,0,0,0,0,0,0,2,4,4,4,4,4,14,14,0,2,0,0,0,0,0,0,2,4,0,13,0,0,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,2,0,0,0,4,4,0,2,14,14,4,4,14,14,14,14,2,0,0,0,0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,4,4,0,0,0,2,0,0,4,4,15,15,15,7,7,0,4,16,7,7,7,15,7,7,0,0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,10,7,7,19,4,4,10,7,8,8,8,8,8,15,15,15,15,7,10,14,14,7,7,15,15,15,7,7,20,0,4,7,8,8,8,7,7,7,7,8,0,0,0,0,0,0,0,0},
				{0,17,0,18,0,0,0,10,7,7,8,8,7,7,7,8,8,8,8,8,8,8,8,8,8,8,7,7,7,7,7,8,8,8,8,7,7,0,10,7,8,8,8,8,7,7,7,8,8,21,0,22,0,0,23,0},
				{24,24,24,24,24,7,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,7,7,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,24,24,24,24,24},
				{24,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,8,8,8,8,8,8,8,8,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,24},
				{24,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,8,8,2,4,4,2,8,8,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,7,25,25,25,24},
				{24,25,25,25,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,15,15,8,8,2,0,0,2,8,8,15,15,7,7,7,7,7,7,7,7,7,7,7,7,25,25,25,7,7,7,25,25,25,24},
				{24,25,25,25,25,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,15,15,8,8,2,26,0,2,8,8,15,15,7,7,7,7,7,25,25,7,7,7,7,7,7,25,7,7,7,7,25,25,25,24},
				{24,25,25,25,25,25,25,7,7,25,25,25,7,7,7,7,7,7,7,25,7,7,15,15,15,15,15,15,15,15,15,15,15,15,7,7,7,25,25,25,25,25,25,25,7,7,7,25,7,7,7,7,25,25,25,24},
				{24,25,25,25,25,25,25,25,25,25,25,7,7,7,7,7,25,25,25,25,7,7,15,15,15,15,15,15,15,15,15,15,15,15,7,7,25,25,25,25,25,25,25,7,7,7,7,25,25,7,7,7,25,25,25,24},
				{24,25,25,25,25,25,25,25,25,7,7,7,7,7,25,25,25,25,25,25,25,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,25,25,25,25,25,7,7,7,7,7,7,25,25,25,7,25,25,25,25,24},
				{24,25,25,25,25,25,25,25,25,7,7,7,7,25,25,25,25,25,25,25,25,25,7,7,7,7,7,7,25,25,7,7,7,7,7,25,25,25,25,7,7,7,7,7,7,7,25,25,25,25,25,25,25,25,25,24},
				{25,25,25,25,25,25,25,25,25,25,25,7,7,25,25,25,25,25,25,25,25,25,25,25,7,7,7,25,25,25,25,25,25,25,25,25,25,25,25,25,25,7,7,7,25,25,25,25,25,25,25,25,25,25,25,25},
				{25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25}
			};
			//i = vertical, j = horizontal
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 241, true, true, -1, 2);
									}
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Campfire, true, true, -1, 0);
									}
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.BloodMoonMonolith, true, true, -1, 0);
									}
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 43);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 91, true, true, -1, bannerColor);
									}
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 240, true, true, -1, 17);
									}
									break;
								case 13:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 240, true, true, -1, 16);
									}
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 331;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 273;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 16:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 17:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 3);
									}
									break;
								case 18:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 1);
									}
									break;
								case 19:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 20:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 21:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 4);
									}
									break;
								case 22:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 2);
									}
									break;
								case 23:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 5);
									}
									break;
								case 24:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 25:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 26:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
							}
						}
					}
				}
			}
		}
		public static void GenerateBanditStarterHouse(int spawnX, int spawnY)
		{
			int bannerColor = WorldGen.genRand.Next(4);
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,1,1,1,1,1,1,1,1,1,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,4,4,4,4,4,4,4,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
				{3,3,5,5,5,5,5,5,5,5,5,5,5,3,4,4,4,4,4,4,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
				{3,3,5,5,5,6,6,5,5,5,5,6,6,5,4,4,4,4,4,4,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
				{3,5,5,5,6,6,5,5,5,5,5,5,6,5,4,4,4,4,4,4,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
				{3,5,5,6,6,6,6,5,5,5,5,5,5,5,4,4,4,4,4,4,4,4,4,4,3,3,7,3,3,3,3,3,3,3,3,3,3,7,3},
				{3,5,5,6,6,6,6,5,5,5,5,5,5,5,4,4,4,4,4,4,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
				{3,5,6,6,6,5,5,5,5,5,5,5,5,5,4,4,4,4,4,4,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,4,4,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,7,3,3,4,4,4,4,4,4,4,3,7,5,5,5,5,5,5,5,5,5,5,5,5,3,3},
				{3,3,3,3,3,3,3,3,3,3,7,3,3,3,3,4,4,4,4,4,4,4,4,4,4,5,5,5,5,5,6,6,6,6,5,5,5,3,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,4,4,4,4,4,4,4,4,4,5,5,5,5,5,5,6,6,5,5,5,5,3,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,4,4,4,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,7,3},
				{3,7,3,3,3,3,3,3,3,3,3,3,7,3,3,4,4,4,4,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,6,6,3,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,4,4,4,4,4,4,4,4,4,5,5,5,5,5,5,5,6,6,6,6,6,5,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,6,6,5,6,5,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,7,3,3,3,3,3,3,3,3,3,3,7,3,3,3}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 17;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = WallID.None;
								break;
							case 1:
								tile.WallType = 4;
								break;
							case 2:
								tile.WallType = 106;
								break;
							//case 3:
							//	tile.WallType = 2;
							//	break;
							case 4:
								tile.WallType = 27;
								break;
							case 5:
								tile.WallType = 1;
								break;
							case 6:
								tile.WallType = 5;
								break;
							//case 7:
								//tile.WallType = 59;
								//break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,2,0,0,0,0,0,0,0,2,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,4,4,4,4,4,4,5,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,5,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,2,0,0,0,0,5,0,0,2,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,5,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,5,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,5,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,5,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,6,0,0,6,0,0,6,0,0,0,0,3,5,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,1,1,1,4,4,4,4,4,4,5,1,1,1,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{8,9,9,8,8,8,9,9,9,8,8,8,8,1,1,1,0,0,0,0,0,5,0,1,1,1,10,10,10,10,10,10,10,10,10,10,10,10,10},
				{8,9,9,9,8,9,9,9,8,8,8,8,8,9,0,0,0,0,0,0,5,0,0,0,1,1,10,10,10,10,10,10,10,10,10,10,10,10,10},
				{8,8,0,0,0,0,0,0,0,0,0,2,0,9,0,0,0,0,0,5,0,0,11,0,1,1,10,10,10,10,10,10,10,10,10,10,10,10,10},
				{8,8,0,0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,5,0,0,0,0,0,1,1,10,10,10,10,10,10,10,10,10,10,10,10,10},
				{8,8,0,0,0,0,0,0,0,12,0,0,0,0,0,0,0,5,0,0,0,0,0,0,1,1,10,10,10,10,10,10,10,10,10,10,10,10,10},
				{8,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,11,0,1,1,10,0,0,10,10,10,10,10,10,10,10,10,10},
				{9,9,0,13,15,0,13,17,17,17,0,0,0,14,0,5,0,0,0,0,0,0,0,0,1,1,10,0,16,10,10,10,10,10,10,10,10,10,10},
				{9,9,9,9,8,8,8,8,9,9,9,8,8,8,1,1,18,4,4,4,4,4,4,1,1,1,9,9,8,8,8,9,9,9,9,9,8,8,8},
				{8,9,9,8,8,8,8,8,9,9,8,8,8,8,1,1,0,18,0,0,0,0,0,1,1,1,9,8,8,8,8,9,9,9,9,8,8,8,8},
				{10,10,10,10,10,10,10,10,10,10,10,10,10,1,1,0,0,0,18,0,0,0,0,0,0,9,0,0,0,2,0,0,0,0,0,0,0,8,8},
				{10,10,10,10,10,10,10,10,10,10,10,10,10,1,1,0,0,0,0,18,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,8,8},
				{10,10,10,10,10,10,10,10,10,10,10,10,10,1,1,0,0,0,0,0,18,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,9},
				{10,10,10,10,10,10,10,10,10,10,10,10,10,1,1,0,0,19,0,0,0,18,0,0,0,0,0,0,0,0,0,21,21,17,17,17,0,9,9},
				{10,10,10,10,10,10,10,10,10,10,10,10,10,1,1,13,0,0,0,0,0,0,18,0,0,20,0,22,17,17,21,21,21,17,17,17,13,9,9},
				{10,10,10,10,10,10,10,10,10,10,10,10,10,1,1,9,9,8,8,8,8,8,9,9,9,9,8,8,8,8,8,8,9,9,9,9,9,9,9},
				{10,10,10,10,10,10,10,10,10,10,10,10,10,1,1,9,9,9,8,8,8,9,9,9,9,9,9,9,8,8,8,8,8,9,9,9,9,9,9}
			};
			//i = vertical, j = horizontal
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 91, true, true, -1, bannerColor);
									}
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 6:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Tombstones, true, true, -1, 2);
										//Main.tile[k, l].TileFrameX
										//TETrainingDummy.Hook_AfterPlacement(k, l);
									}
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 240, true, true, -1, 43);
									}
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 240, true, true, -1, 45);
									}
									break;
								case 13:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 93, true, true, -1, 0);
									}
									break;
								case 14:
								case 20:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 15:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 16:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Boulder, true, true, -1, 0);
									}
									break;
								case 17:
									tile.HasTile = true;
									tile.TileType = 332;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 18:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 19:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 240, true, true, -1, 44);
									}
									break;
								case 21:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 239, true, true, -1, 6);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 22:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.RedWire = true;
										Framing.GetTileSafely(k, l - 1).RedWire = true;
										Framing.GetTileSafely(k, l - 2).RedWire = true;
										Framing.GetTileSafely(k, l - 3).RedWire = true;
										Framing.GetTileSafely(k, l - 4).RedWire = true;
										Framing.GetTileSafely(k, l - 5).RedWire = true;
										Framing.GetTileSafely(k, l - 6).RedWire = true;
										Framing.GetTileSafely(k, l - 5).HasActuator = true;
										Framing.GetTileSafely(k, l - 6).HasActuator = true;
										Framing.GetTileSafely(k + 1, l - 5).RedWire = true;
										Framing.GetTileSafely(k + 1, l - 6).RedWire = true;
										Framing.GetTileSafely(k + 1, l - 5).HasActuator = true;
										Framing.GetTileSafely(k + 1, l - 6).HasActuator = true;
									}
									WorldGen.PlaceTile(k, l, TileID.PressurePlates, true, true, -1, 2);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
		}
		public static void GenerateBoulderStarterHouse(int spawnX, int spawnY)
		{
			int bannerColor = WorldGen.genRand.Next(4);
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,2,3,3,3,3,3,3,3,3,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,2,3,3,3,3,3,3,3,3,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,2,3,3,3,3,3,3,3,3,3,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,2,4,4,4,4,4,4,4,4,4,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{5,5,5,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,5,5,5,5,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,7,7,7,7,1,1,1,1,7,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,7,7,0,0,0,6,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 5;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 1;
								break;
							case 2:
								tile.WallType = 4;
								break;
							case 3:
								tile.WallType = 21;
								break;
							case 4:
								tile.WallType = 27;
								break;
							case 5:
								tile.WallType = 106;
								break;
							case 7:
								tile.WallType = 5;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,0,2,0,0,0,0,0,0,2,4,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,4,4,4,5,5,4,6,0,3,0,0,4,4,5,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,5,4,4,5,4,5,4,4,7,0,4,5,5,5,5,5,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,8,1,1,8,0,9,10,0,0,0,0,9,0,8,1,1,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,0,0,9,10,0,0,0,0,9,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,0,0,9,10,0,0,0,0,9,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,9,10,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,9,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,12,0,16,16,9,14,0,17,6,0,9,0,0,0,13,0,0,15,0,0,0,0,0,0,0,0,0,0,0,11,0,11,0,0},
				{18,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,4,4,0,0,0,0,0,0,0,5,5,5,18,18,18,18},
				{20,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,5,4,5,0,0,19,0,0,5,5,5,5,20,20,20,20},
				{20,20,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,20,5,5,5,5,5,5,4,4,5,5,5,20,20,20,20,20},
				{20,20,20,20,4,4,4,4,4,4,4,4,4,4,4,4,4,4,20,20,20,20,5,5,5,5,5,4,4,4,5,20,20,20,20,20,20},
				{20,20,20,20,20,20,4,4,4,4,4,4,4,4,4,4,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20}
			};
			//i = vertical, j = horizontal
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 3:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Boulder, true, true, -1, 0);
									}
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 7:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Platforms, true, true, -1, 43);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Banners, true, true, -1, bannerColor);
									}
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 214;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.FishingCrate, true, true, -1, 0);
									}
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 13:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 14:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 15:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 96, true, true, -1, 0);
									}
									break;
								case 16:
									tile.HasTile = true;
									tile.TileType = 331;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 17:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 18:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 19:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Campfire, true, true, -1, 0);
									}
									break;
								case 20:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY + 5, 1, _structure.GetLength(1) / 2, 6);
		}
		public static void GenerateMineStarterHouse(int spawnX, int spawnY)
        {
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,1,1,1,1,1,2,2,2,2,2,1,1,1,1,1,2,2,2,2,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,0,4,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 18;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 4;
								break;
							case 2:
								tile.WallType = 21;
								break;
							case 3:
								tile.WallType = 27;
								break;
							case 5:
								tile.WallType = 63;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,3,0,0,3,3,3,1,1,1,3,3,0,2,3,3,3,1,1,1,3,3,3,0,0,0,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,3,1,1,1,0,0,0,2,0,3,3,1,1,1,3,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,4,0,0,0,0,2,0,0,0,0,4,0,0,5,5,0,0,0,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,4,0,0,0,0,2,0,0,0,0,4,0,5,5,5,5,5,0,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,0,0,7,7,0,0,0,4,0,0,7,7,2,0,9,9,0,4,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,7,7,7,7,7,8,0,4,0,7,7,7,2,9,9,9,9,4,0,6,0,6,0,6,0,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,10,1,1,1,1,1,1,1,11,11,4,11,11,1,1,1,1,1,11,11,4,11,11,1,1,1,1,1,1,1,12,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,10,3,1,1,1,1,1,1,1,0,0,4,0,0,1,1,1,1,1,3,0,4,0,0,1,1,1,1,1,1,1,3,12,0,0,0,0,0,0},
				{0,0,0,0,0,0,10,3,3,3,2,3,3,3,3,0,0,0,4,0,0,3,3,2,3,3,3,0,4,0,0,0,3,3,3,3,2,3,3,3,12,0,0,0,0,0},
				{0,0,0,0,0,10,0,0,3,3,2,3,3,0,0,0,0,0,4,0,0,0,3,2,3,3,0,0,4,0,0,0,0,0,3,3,2,3,3,3,0,12,0,0,0,0},
				{0,0,0,0,10,0,0,0,0,3,2,3,0,0,0,0,0,0,4,0,0,0,0,2,3,0,0,0,4,0,0,0,0,0,0,3,2,3,3,0,0,0,12,0,0,0},
				{0,0,0,10,0,0,0,0,0,0,2,0,0,0,0,0,0,0,4,0,0,0,0,2,0,0,0,0,4,0,0,0,0,0,0,0,2,3,0,0,0,0,0,12,0,0},
				{0,0,10,0,0,0,0,0,0,0,2,0,13,14,15,0,0,0,4,0,0,0,0,2,0,0,0,0,4,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,12,0},
				{0,10,0,0,0,0,0,0,0,1,1,1,15,15,15,0,0,0,4,0,0,0,0,2,0,0,0,0,4,0,0,0,0,15,15,1,1,1,15,15,15,15,15,15,15,15},
				{15,15,15,15,15,15,15,15,15,1,1,1,16,15,0,0,0,0,4,0,0,0,0,2,0,0,0,0,4,0,0,0,15,15,16,1,1,1,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,16,16,16,16,16,15,0,0,0,0,4,0,0,0,0,2,0,0,0,0,4,17,18,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,16,16,16,16,15,20,0,0,0,0,4,0,0,0,0,2,0,0,0,17,21,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,16,16,16,16,15,0,0,0,0,0,4,0,0,0,1,1,1,0,22,19,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,16,16,16,16,15,0,0,0,0,0,4,0,0,0,1,1,1,15,15,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,16,16,16,15,15,0,0,0,0,0,4,0,0,0,1,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,16,16,15,15,0,0,0,0,0,0,4,0,0,0,1,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,16,16,15,0,0,0,0,0,0,0,4,0,0,0,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,15,15,15,0,0,0,0,0,0,0,4,0,0,18,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,19,19,19,0,0,0,0,0,0,0,0,0,4,0,17,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,19,19,19,19,23,0,0,0,0,0,0,0,4,0,0,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,19,19,19,24,24,25,17,0,0,0,0,0,4,15,15,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,19,19,19,24,24,24,24,25,22,0,0,0,4,0,15,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,19,19,19,19,24,24,24,19,19,21,17,18,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,19,19,19,19,19,19,24,19,19,19,19,19,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,19,19,19,19,24,24,24,24,19,19,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,19,19,19,19,24,24,24,24,24,19,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,19,19,19,19,24,24,24,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16},
				{16,16,16,16,16,16,16,16,16,16,19,19,19,19,19,19,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 213;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 331;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									if (confirmPlatforms >= 1)
									{
										if (confirmPlatforms == 1)
											tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.FishingCrate, true, true, -1, 0);
									}
									break;
								case 7:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 239, true, true, -1, 2);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 0);
									}
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 332;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 3;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 16:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 17:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.ExposedGems, true, true, -1, 2);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 18:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 19:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 21:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 22:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.LandMine, true, true, -1, 0);
									tile.TileColor = PaintID.GrayPaint;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 63;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 24:
									tile.HasTile = true;
									tile.TileType = 63;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 25:
									tile.HasTile = true;
									tile.TileType = 63;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
		}
		public static void GenerateTavernStarterHouse(int spawnX, int spawnY)
		{
			int bannerColor = WorldGen.genRand.Next(4);
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,2,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,2,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,1,1,2,3,4,4,4,4,4,3,3,3,4,4,4,4,4,3,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,3,4,4,4,4,4,3,3,3,4,4,4,4,4,3,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,3,5,5,5,5,5,3,3,3,5,5,5,5,5,3,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,6,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,7,7,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,7,7,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,7,7,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,7,7,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,7,7,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,7,7,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,9,9,9,9,8,8,9,9,9,9,8,8,8,8,8,8,9,9,8,8,8,8,8,8,8,8,9,8,8,8,8,8,8},
				{8,9,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,9,8,8,8,8,8,8,8,8,8,8,9,8,8,8,8,8,8,8,8,8,8,8,8,8,9,8,8,8,8}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 6;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 4;
								break;
							case 2:
								tile.WallType = 1;
								break;
							case 3:
								tile.WallType = 78;
								break;
							case 4:
								tile.WallType = 21;
								break;
							case 5:
								tile.WallType = 147;
								break;
							case 6:
								tile.WallType = 27;
								break;
							case 7:
								tile.WallType = 5;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0},
				{0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0},
				{0,0,0,0,0,3,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,3,0,0,0,0,0},
				{0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0},
				{0,0,0,0,2,2,2,4,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,5,4,2,2,2,0,0,0,0},
				{0,0,0,0,6,3,6,4,5,4,0,7,8,9,10,10,10,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,8,8,2,4,5,4,6,3,6,0,0,0,0},
				{0,0,0,0,0,3,0,4,5,4,7,8,9,10,10,10,10,10,6,3,6,0,0,0,0,0,0,11,0,10,10,10,12,8,13,4,5,4,0,3,0,0,0,0,0},
				{0,0,0,0,0,3,0,4,5,4,8,9,10,10,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,10,10,10,12,8,4,5,4,0,3,0,0,0,0,0},
				{0,0,0,0,2,2,2,4,5,4,9,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,10,10,10,12,4,5,4,2,2,2,0,0,0,0},
				{0,0,0,0,2,2,2,4,5,4,14,14,14,15,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,10,10,4,5,4,2,2,2,0,0,0,0},
				{0,0,0,0,6,3,6,4,5,4,16,16,16,16,16,0,0,0,17,3,17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,4,6,3,6,0,0,0,0},
				{0,0,0,0,0,3,0,0,0,0,0,0,19,19,14,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0},
				{0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,23,0,0,0,0,0,0,15,0,0,0,0,0,0,0,0,3,0,0,0,0,0},
				{0,0,0,0,0,3,0,0,18,0,20,0,0,21,0,22,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,18,0,0,3,0,0,0,0,0},
				{0,0,0,0,0,3,0,4,5,4,2,2,2,2,2,2,2,2,2,2,26,0,30,0,25,0,24,0,30,0,25,0,24,0,0,4,5,4,0,3,0,0,0,0,0},
				{27,28,28,28,28,28,28,28,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,28,28,28,28,28,28,28,27},
				{29,28,28,28,28,28,28,28,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,28,28,28,28,28,28,29,29},
				{29,29,29,28,28,28,28,28,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,28,28,28,28,29,29,29,29},
				{29,29,29,29,29,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,29,29,29,29,29,29},
				{29,29,29,29,29,29,29,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,29,29,29,29,29,29,29,29},
				{29,29,29,29,29,29,29,29,29,29,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,28,29,29,29,29,29,29,29,29,29,29,29},
				{29,29,29,29,29,29,29,29,29,29,29,29,29,29,28,28,28,28,28,28,28,28,28,28,28,28,28,28,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29}
			};
			//i = vertical, j = horizontal
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 39;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 273;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Banners, true, true, -1, bannerColor);
									}
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 34, true, true, -1, 2);
									}
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 14:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 4);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 520, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 16:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 17:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 4, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 18:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 19:
									tile.HasTile = true;
									tile.TileType = 332;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 0);
									}
									break;
								case 21:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 17);
									}
									break;
								case 22:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 15, true, true, -1, 21);
									}
									break;
								case 23:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 520, true, true, -1, 1);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 24:
								case 30:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Chairs, true, true, -1, 0);
										if (_structure[i, j] == 30)
                                        {
											Framing.GetTileSafely(k, l).TileFrameX += 18;
											Framing.GetTileSafely(k, l - 1).TileFrameX += 18;
										}
									}
									break;
								case 25:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 16);
									}
									break;
								case 26:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 27:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 28:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 29:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY + 5, 1, _structure.GetLength(1) / 2, 8);
		}
		public static void GenerateChaosStarterHouse(int spawnX, int spawnY)
		{
			int bannerColor = WorldGen.genRand.Next(4);
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,3,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,2,4,4,2,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,2,4,4,2,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,4,4,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,4,4,4,4,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,4,4,4,4,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,2,4,4,4,4,2,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,2,4,4,4,4,2,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,2,4,4,4,4,2,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,2,2,2,2,4,4,4,4,2,2,2,2,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,4,4,2,2,4,4,4,4,2,2,4,4,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,4,4,2,2,4,4,4,4,2,2,4,4,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,4,4,2,2,4,4,4,4,2,2,4,4,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,4,4,2,2,4,4,4,4,2,2,4,4,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,4,4,2,2,4,4,4,4,2,2,4,4,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,4,4,2,2,4,4,4,4,2,2,4,4,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,4,4,2,2,4,4,4,4,2,2,4,4,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,2,2,4,4,2,2,4,4,4,4,2,2,4,4,2,2,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,1,1,1,2,2,1,1,2,2,1,1,1,1,2,2,1,1,2,2,1,1,1,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,1,1,1,2,2,1,1,2,2,1,1,1,1,2,2,1,1,2,2,1,1,1,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,7,7,7,7,7,7,7,7,7,7,0,2,3,1,1,1,2,2,1,1,2,2,1,1,1,1,2,2,1,1,2,2,1,1,1,3,2,0,7,7,7,7,7,7,7,7,7,7,7,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 8;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 5;
								break;
							case 2:
								tile.WallType = 147;
								break;
							case 3:
								tile.WallType = 1;
								break;
							case 4:
								tile.WallType = 21;
								break;
							case 5:
								tile.WallType = 4;
								break;
							case 6:
								tile.WallType = 27;
								break;
							case 7:
								tile.WallType = 245;
								break;
							/*case 8:
								tile.WallType = 2;
								break;*/
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY - 2, 0, _structure.GetLength(1) / 2 + 1, 15);
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,3,3,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,3,3,3,3,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,4,4,4,4,4,4,4,4,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,5,5,5,5,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,5,5,5,5,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,5,5,5,5,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,6,7,7,7,7,6,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,0,0,0,7,7,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,0,0,0,0,7,7,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,6,0,0,0,7,6,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,9,0,4,4,6,7,10,0,7,6,4,4,0,9,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,5,5,6,11,12,12,11,6,5,5,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,5,5,6,11,7,7,11,6,5,5,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,5,5,6,11,7,7,11,6,5,5,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,6,11,7,7,7,0,0,7,7,7,11,6,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,9,0,4,4,6,11,7,0,0,0,0,0,7,7,11,6,4,4,0,9,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,4,4,6,11,0,0,0,0,0,0,0,7,11,6,4,4,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,6,6,6,11,0,0,0,0,0,0,0,0,11,6,6,6,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,11,7,7,13,0,0,0,0,0,0,0,0,13,7,7,11,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,6,11,7,0,13,0,0,0,0,0,0,0,0,13,0,7,11,6,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,0,0,9,0,4,4,6,11,0,0,13,0,0,0,0,0,0,0,0,13,0,0,11,6,4,4,0,9,0,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,4,4,6,11,11,11,11,11,12,12,12,12,12,12,11,11,11,11,11,6,4,4,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,6,6,6,11,11,11,11,11,0,0,0,0,0,0,11,11,11,11,11,6,6,6,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,4,4,5,5,5,11,7,7,13,7,7,0,0,0,0,0,0,0,0,0,0,0,7,13,7,7,11,5,5,5,4,4,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,14,0,4,4,6,11,7,0,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,7,7,11,6,4,4,0,14,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,4,4,6,11,0,0,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,0,7,11,6,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,4,5,5,6,11,0,0,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,0,0,11,6,5,5,4,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,4,5,5,6,11,11,11,11,11,12,12,12,12,12,12,12,12,12,12,12,12,11,11,11,11,11,6,5,5,4,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,9,0,0,0,0,0,0,0,0,4,5,5,6,11,11,11,11,11,0,0,0,0,0,0,0,0,0,0,0,0,11,11,11,11,11,6,5,5,4,0,0,0,0,0,0,0,0,0,9,0,0},
				{0,4,4,4,0,0,0,0,0,0,0,0,4,4,6,11,15,0,13,15,0,0,0,0,0,0,0,0,0,0,0,0,15,13,7,15,11,6,4,4,0,0,0,0,0,0,0,0,0,4,4,4,0},
				{0,11,11,11,0,0,0,0,0,0,0,0,4,4,6,11,15,0,13,15,0,0,0,0,0,0,0,0,0,0,0,0,15,13,7,15,11,6,4,4,0,0,0,0,0,0,0,0,0,11,11,11,0},
				{0,14,13,14,0,0,0,0,0,0,0,4,5,5,6,11,15,0,13,15,0,0,0,0,0,0,0,0,0,0,0,0,15,13,0,15,11,6,5,5,4,0,0,0,0,0,0,0,0,14,13,14,0},
				{0,0,13,0,0,0,0,0,0,0,0,4,5,5,6,11,15,0,13,0,0,0,0,0,0,0,0,0,0,0,0,0,15,13,0,15,11,6,5,5,4,0,0,0,0,0,0,0,0,0,13,0,0},
				{0,0,13,0,0,0,0,0,0,0,0,4,5,5,6,11,15,0,13,0,0,0,0,0,0,16,0,0,0,0,0,0,15,13,0,15,11,6,5,5,4,0,0,0,0,0,0,0,0,0,13,0,0},
				{0,0,13,0,0,0,0,0,0,0,0,0,0,0,0,0,15,0,13,0,0,18,18,4,6,6,6,6,4,0,0,0,15,13,0,15,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,0,0},
				{0,0,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,0,18,18,4,4,6,6,6,6,4,4,0,0,15,13,18,18,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,0,0},
				{19,0,13,20,0,0,21,0,0,0,0,0,17,0,0,18,18,18,13,18,18,4,4,4,6,6,6,6,4,4,4,18,18,13,18,18,18,0,0,17,0,0,0,19,0,0,0,20,0,0,13,0,0},
				{22,22,22,22,22,22,22,22,22,22,22,4,5,5,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,4,5,5,4,22,22,22,22,22,22,22,22,22,22,22,22},
				{23,23,23,23,23,23,23,23,23,23,23,4,5,5,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,4,5,5,4,23,23,23,23,23,23,23,23,23,23,23,23},
				{23,23,23,23,23,23,23,23,23,23,23,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,23,23,23,23,23,23,23,23,23,23,23,23},
				{23,23,23,23,23,23,23,23,23,23,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,23,23,23,23,23,23,23,23,23,23,23},
				{23,23,23,23,23,23,23,23,23,23,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,5,5,5,5,23,23,23,23,23,23,23,23,23,23,23},
				{23,23,23,23,23,23,23,23,23,23,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,23,23,23,23,23,23,23,23,23,23,23},
				{23,23,23,23,23,23,23,23,23,23,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,23,23,23,23,23,23,23,23,23,23,23},
				{23,23,23,23,23,23,23,23,23,23,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,23,23,23,23,23,23,23,23,23,23,23},
				{23,23,23,23,23,23,23,23,23,23,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,23,23,23,23,23,23,23,23,23,23,23}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 105, true, true, -1, 239);
									}
									break;
								case 2:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 105, true, true, -1, 74);
									}
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 332;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 273;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 54;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Campfire, true, true, -1, 0);
									}
									break;
								case 10:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 5);
									}
									break;
								case 11:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 14:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Banners, true, true, -1, bannerColor);
									}
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 214;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 16:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 0);
									}
									break;
								case 17:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 8);
									}
									break;
								case 18:
									tile.HasTile = true;
									tile.TileType = 331;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 19:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 4);
									}
									break;
								case 20:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 1);
									}
									break;
								case 21:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 2);
									}
									break;
								case 22:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY + 9, 1, _structure.GetLength(1) / 2, 12);
		}
		public static void GenerateDrownedStarterHouse(int spawnX, int spawnY)
		{
			int bannerColor = WorldGen.genRand.Next(4);
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,2,2,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,2,2,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,4,1,1,1,1,1,1,1,1,1,1,1,1,4,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,4,5,5,5,5,5,5,5,5,5,5,5,5,4,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,4,5,5,5,5,5,5,5,5,5,5,5,5,4,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,4,4,4,4,4,4,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,4,4,4,4,4,4,0,6,6,0,0,0,0,0,0,6,6,0,0,0,0,0,0,6,6,0,4,4,4,4,4,4,0,0,0,0,0,0,0},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,8},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,7,7,8,8,8,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,7,8,8,8,8,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,7,8,8,8,8,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,8,8,8,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,8,8,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,7,7,6,6,8,7,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,7,8,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,7,8,8,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,6,6,7,7,7,8,8,8,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,8,7,7,6,6,7,7,8,8,7,7,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,8,7,7,6,6,7,8,8,7,7,7,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,8,7,7,6,6,8,8,7,7,7,7,6,6,7,7,7,7,7,7,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,8,8,8,8,8,8,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,8,8,8,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
				{7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 17;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 27;
								break;
							case 2:
								tile.WallType = 21;
								break;
							case 3:
								tile.WallType = 147;
								break;
							case 4:
								tile.WallType = 5;
								break;
							case 5:
								tile.WallType = 4;
								break;
							case 6:
								tile.WallType = 78;
								break;
							case 7:
								if(tile.WallType == 0)
									tile.WallType = 2;
								break;
							case 8:
								if (tile.WallType == 0)
									tile.WallType = 59;
								break;
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY - 2, 0, _structure.GetLength(1) / 2 + 1, 15);
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,1,0,0,0,0,1,1,1,1,0,0,0,0,1,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,1,1,1,1,1,1,2,2,1,1,1,1,1,1,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,3,3,3,3,3,3,3,3,3,3,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,3,3,3,3,3,3,3,3,3,0,0,0},
				{0,0,0,3,3,3,3,3,3,3,3,3,3,3,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,3,3,3,3,3,3,3,3,3,3,3,0,0,0},
				{0,0,0,4,5,5,4,0,0,6,6,6,6,1,2,2,1,6,6,0,0,5,0,0,5,6,6,6,6,1,2,2,1,6,6,0,0,0,0,4,5,5,4,0,0,0},
				{0,0,0,0,5,5,0,0,0,0,0,6,6,1,2,2,1,6,0,0,0,5,0,0,5,0,6,6,6,1,2,2,1,6,0,0,0,0,0,0,5,5,0,0,0,0},
				{0,0,0,0,5,5,0,0,0,0,0,0,0,1,2,2,1,0,0,0,0,5,7,0,5,0,0,0,6,1,2,2,1,0,0,0,0,0,0,0,5,5,0,0,0,0},
				{0,0,0,0,5,5,0,0,0,0,0,0,0,0,0,0,0,0,9,9,10,5,11,11,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,0,0,0,0},
				{0,0,0,0,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,15,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,0,0,0,0},
				{0,0,0,0,5,5,0,0,0,0,0,0,0,0,8,0,0,13,0,14,0,5,18,0,5,0,16,0,0,0,0,12,0,0,17,0,17,0,0,0,5,5,0,0,0,0},
				{0,0,19,1,1,1,1,11,11,11,11,11,11,1,2,2,1,3,3,3,3,3,3,3,3,3,3,3,3,1,2,2,1,11,11,11,11,11,11,1,1,1,1,20,0,0},
				{0,19,0,1,2,2,1,23,23,23,23,23,23,1,2,2,1,3,3,3,3,3,3,3,3,3,3,3,3,1,2,2,1,23,23,23,23,23,23,1,2,2,1,0,20,0},
				{21,21,21,21,22,22,21,23,23,23,23,23,23,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,23,23,23,23,23,23,21,22,22,21,21,21,21},
				{22,22,22,22,22,22,21,25,25,25,25,25,25,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,25,25,25,25,25,25,21,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,21,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,25,25,25,25,25,25,21,26,26,24,24,24,24,24,24,24,24,24,24,24,24,24,24,26,26,21,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,25,25,25,25,25,25,21,26,26,22,22,22,24,24,24,24,24,24,24,24,22,22,22,26,26,21,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,25,25,25,25,25,25,25,26,26,22,22,22,22,22,22,26,26,22,22,22,22,22,22,26,26,25,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,25,25,25,25,25,25,25,25,26,21,22,22,22,22,22,26,26,22,22,22,22,22,21,26,25,25,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,25,25,25,25,25,25,25,25,25,21,21,21,22,22,22,26,26,22,22,22,21,21,21,25,25,25,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,25,25,25,25,25,25,25,25,25,25,25,21,21,21,21,26,26,21,21,21,21,25,25,25,25,25,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,9,9,9,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,9,9,9,9,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,9,9,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,21,21,9,9,9,9,9,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,9,9,9,25,9,9,21,21,22,22,22,22,22,22},
				{22,22,22,22,22,22,22,21,21,21,9,9,9,9,25,25,0,0,25,25,25,25,0,0,25,9,9,25,25,25,25,25,9,9,9,9,21,21,21,22,22,22,22,22,22,22},
				{22,22,22,22,22,22,22,22,22,21,21,21,21,9,9,9,17,0,25,25,9,9,27,0,9,9,9,9,25,25,25,9,9,21,21,21,21,22,22,22,22,22,22,22,22,22},
				{22,22,22,22,22,22,22,22,22,22,22,22,21,21,26,26,21,21,21,21,21,21,26,26,21,21,21,21,21,21,26,26,21,21,22,22,22,22,22,22,22,22,22,22,22,22},
				{22,22,22,22,22,22,22,22,22,22,22,22,22,22,26,26,22,22,22,22,22,22,26,26,22,22,22,22,22,22,26,26,22,22,22,22,22,22,22,22,22,22,22,22,22,22},
				{22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 273;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Banners, true, true, -1, bannerColor);
									}
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 5);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 15);
									}
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 331;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 15);
									}
									break;
								case 13:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Chairs, true, true, -1, 0);
										Framing.GetTileSafely(k, l - 1).TileFrameX += 18;
										Framing.GetTileSafely(k, l).TileFrameX += 18;
									}
									break;
								case 14:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 0);
									}
									break;
								case 15:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 103, true, true, -1, 2);
									}
									break;
								case 16:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 96, true, true, -1, 0);
									}
									break;
								case 17:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.FishingCrate, true, true, -1, 0);
									}
									break;
								case 18:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 18, true, true, -1, 0);
									}
									break;
								case 19:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 20:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 21:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 22:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 24:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 23:
								case 25:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 255;
										tile.LiquidType = 0;
									}
									break;
								case 26:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 27:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 0);
									}
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY + 17, 1, _structure.GetLength(1) / 2, 7);
		}
		public static void GenerateComfyStarterHouse(int spawnX, int spawnY)
        {
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,2,2,2,2,2,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,2,2,2,2,2,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,0,4,4,4,4,4,4,4,4,4,4,4,3,3,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,0,4,4,4,2,2,2,2,2,4,4,4,3,3,1,1,1,2,2,2,2,2,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,4,4,4,2,2,2,2,2,4,4,4,3,3,1,1,1,2,2,2,2,1,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,4,4,4,4,4,4,4,4,4,4,4,3,3,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,4,4,4,4,4,4,4,4,4,4,4,3,3,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0},
				{5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
				{5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - _structure.GetLength(0) + 5;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 4;
								break;
							case 2:
								tile.WallType = 21;
								break;
							case 3:
								tile.WallType = 78;
								break;
							case 4:
								tile.WallType = 27;
								break;
							case 5:
								if(tile.WallType == 0)
									tile.WallType = 2;
								break;
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY - 1, 0, _structure.GetLength(1) / 2 + 1, 12);
			_structure = new int[,]  {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,1,1,6,6,0,0,0,0,0,6,6,6,6,1,1,5,4},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,7,6,0,0,0,0,0,0,0,6,6,6,7,0,5,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,7,0,0,0,0,0,0,0,0,0,0,6,7,0,5,0},
				{0,0,8,0,0,0,0,0,0,0,0,0,0,0,9,0,0,12,0,0,0,10,0,0,13,0,5,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,5,0},
				{0,2,2,2,0,0,0,11,0,0,0,0,15,2,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,0,14,0,0,0,0,0,0,0,0,0,1,1,5,0},
				{0,3,3,3,16,16,16,16,16,16,16,15,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,3,16,16,16,16,16,16,15,1,1,3,3},
				{0,3,3,3,0,0,0,0,0,0,15,0,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,3,0,0,0,0,0,15,6,1,1,3,3},
				{0,4,5,4,0,0,0,0,0,15,0,0,0,4,1,1,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15,6,6,1,1,5,4},
				{0,0,5,0,0,0,0,0,15,0,0,0,0,0,1,1,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15,6,6,6,7,0,5,0},
				{0,0,5,0,0,0,0,15,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15,6,6,6,0,7,0,5,0},
				{0,0,5,0,0,0,15,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15,0,0,0,0,0,1,1,5,0},
				{0,0,5,0,0,15,0,0,0,0,0,0,0,0,17,0,0,0,18,0,0,0,25,0,20,0,19,0,0,0,0,0,0,15,0,21,0,0,21,0,1,1,5,0},
				{0,0,5,0,15,0,0,0,22,0,0,0,15,3,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3},
				{23,23,23,23,23,23,23,23,23,23,23,23,3,3,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3},
				{23,23,23,23,23,23,23,23,23,23,23,23,23,23,1,1,23,23,23,23,23,23,23,23,23,23,23,1,1,23,23,23,23,23,23,23,23,23,23,23,1,1,23,23},
				{24,24,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23},
				{24,24,24,24,24,24,24,24,24,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,24,24,24,23,23,23,23,23,23,23,23,23,23,24},
				{24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,23,23,23,23,23,23,24,24,24,24,24,24,24,24,24,24,23,23,23,23,23,23,24}
			};
			//i = vertical, j = horizontal
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 91, true, true, -1, 2);
									}
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 51;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 54;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 215, true, true, -1, 0);
									}
									break;
								case 9:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 21, true, true, -1, 5);
									}
									break;
								case 10:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 17, true, true, -1, 0);
									}
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 0);
									}
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 18, true, true, -1, 0);
									}
									break;
								case 13:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 16, true, true, -1, 0);
									}
									break;
								case 14:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 79, true, true, -1, 0);
									}
									break;
								case 15:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 16:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 17:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 10, true, true, -1, 0);
									}
									break;
								case 18:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 96, true, true, -1, 0);
									}
									break;
								case 19:
								case 25:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 15, true, true, -1, 0);
										if (_structure[i, j] == 25)
                                        {
											tile.TileFrameX += 18;
											Framing.GetTileSafely(k, l - 1).TileFrameX += 18;
										}
									}
									break;
								case 20:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 0);
									}
									break;
								case 21:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 21, true, true, -1, 0);
									}
									break;
								case 22:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 89, true, true, -1, 0);
									}
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 24:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
			UseStarterHouseHalfCircle(spawnX, spawnY + 5, 1, _structure.GetLength(1) / 2, 9);
		}
		public static void GenerateNatureStarterHouse(int spawnX, int spawnY)
		{
			int bannerColor = WorldGen.genRand.Next(4);
			int bannerColor2 = (bannerColor + 1) % 4;
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,0,0,1,3,4,4,4,3,1,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,1,1,1,1,1,3,4,4,4,3,1,1,1,1,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,1,1,1,1,1,3,4,4,4,3,1,1,1,1,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,3,4,4,4,3,1,1,1,1,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,0,5,5,0,0,0,0,0,0},
				{0,5,5,5,5,5,0,0,0,0,0,0,5,5,5,0,0,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,0,5,5,5,5,0,0,0,0},
				{5,5,5,5,5,5,0,0,0,0,5,5,5,5,5,5,0,1,1,1,0,1,3,3,3,3,3,1,1,1,1,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,0,5,0,0,5,5,5,5,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,3,3,3,3,3,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,6,6,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,3,3,3,3,3,1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,6,6,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,3,3,3,3,3,1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,6,6,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,6,6,6,6,6,1,1,1,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,3,3,3,3,3,1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1,6,6,1,1,1,1,1,1,1,1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1},
				{6,6,6,6,6,6,6,1,1,1,6,6,6,6,6,6,6,1,1,6,6,1,3,3,3,3,3,1,1,1,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{6,6,6,6,6,6,6,1,1,1,3,3,3,3,3,3,3,3,3,6,6,1,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,1,1,1,3,3,3,3,3,3,3,3,3,6,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,6,1,1,3,3,3,3,3,3,3,3,3,6,6,1,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,6,1,1,3,3,3,3,3,3,3,3,3,1,1,1,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,6,1,1,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,6,1,1,3,3,3,3,3,3,3,3,3,1,1,1,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,7,7,7,7,7,7,1,1,1,1,1},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,1,6}
			};
			int PosX = spawnX - _structure.GetLength(1) / 2;
			int PosY = spawnY - 6;
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = (ushort)ModContent.WallType<NaturePlatingWallWall>();
								break;
							case 2:
								tile.WallType = 4;
								break;
							case 3:
								tile.WallType = (ushort)ModContent.WallType<NaturePlatingPanelWallWall>();
								break;
							case 4:
								tile.WallType = 21;
								break;
							case 5:
								tile.WallType = 68;
								break;
							case 6:
								tile.WallType = 2;
								break;
							case 7:
								tile.WallType = (ushort)ModContent.WallType<NatureWallWall>();
								break;
						}
					}
				}
			}
			PosY -= 22;
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,1,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,2,2,2,2,2,2,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,2,2,2,2,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,2,2,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,2,2,2,0,1,1,2,2,2,2,1,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,1,2,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,3,0,0,0,1,1,1,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,4,4,4,1,1,1,0,0,0,0,0,2,2,2,2,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,5,5,5,5,5,5,5,0,0,0,4,4,4,4,4,4,4,6,6,6,4,4,4,4,4,4,4,0,0,0,0,0,2,2,2,2,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,5,5,5,5,5,5,5,0,0,0,4,4,4,4,4,4,4,6,6,6,4,4,4,4,4,4,4,0,0,0,0,0,0,2,2,2,0,0,0,0,0,7,7,7,7,4,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,8,9,0,0,0,9,8,0,0,0,4,4,4,4,4,4,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,2,2,2,0,0,0,0,0,4,4,4,7,4,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,9,0,10,0,9,0,0,0,0,0,0,11,0,0,36,13,0,0,0,13,0,0,0,11,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,4,7,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,9,0,0,0,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,4,7,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,9,0,0,0,9,0,0,0,0,0,0,0,0,12,14,0,0,0,0,0,0,12,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,0,0,0,0,1,4,7,4,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,9,0,0,0,9,0,0,0,0,0,0,17,4,4,4,0,0,0,0,0,4,4,4,18,0,0,0,0,0,0,0,2,2,2,2,2,0,0,0,1,1,1,4,7,4,1,1,1,1,0,0,0,0},
				{0,0,0,0,15,0,0,9,20,0,20,9,0,0,20,16,0,17,4,4,4,4,21,21,21,21,21,4,4,4,4,18,0,0,0,19,0,2,2,2,2,2,2,0,20,1,1,1,1,4,7,4,1,1,1,1,0,0,0,20},
				{22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,23,4,4,4,4,4,4,0,0,0,0,0,4,4,4,4,4,4,22,22,22,22,2,2,2,2,2,2,2,22,22,24,4,4,4,7,4,24,24,24,22,22,22,22,22},
				{24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,23,4,4,4,4,4,7,0,0,0,0,0,7,4,4,4,4,4,24,24,24,24,2,2,2,2,2,2,2,24,24,24,4,4,4,7,4,24,24,24,24,24,24,24,24},
				{24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,23,23,23,23,23,4,4,21,21,21,21,21,4,4,23,23,23,23,24,24,24,24,24,2,2,24,2,2,24,24,24,24,23,23,4,7,4,24,24,24,24,24,24,24,24},
				{24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,23,23,23,23,23,4,4,0,0,0,0,0,4,4,23,23,23,23,24,24,2,2,2,2,2,24,2,2,2,2,24,24,23,23,4,7,4,24,23,23,23,23,23,23,23},
				{24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,23,23,23,23,23,4,7,0,0,0,0,0,7,4,23,23,23,23,24,24,24,24,24,2,2,24,24,24,24,2,2,24,23,23,4,7,4,23,23,23,23,4,4,4,23},
				{24,24,24,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,4,4,21,21,21,21,21,4,4,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,4,7,4,4,4,4,4,4,4,4,23},
				{24,24,23,23,23,23,23,4,7,7,7,7,7,7,7,7,7,7,7,7,4,4,0,0,0,0,0,4,4,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,4,23},
				{24,23,23,23,23,23,4,4,7,4,4,4,4,4,4,4,4,4,4,4,4,7,0,0,0,0,0,7,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,7,4,4},
				{24,23,23,23,23,23,4,4,7,4,4,4,4,4,4,4,4,4,4,4,4,38,21,21,21,21,21,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,4,4,7,4,4},
				{24,23,23,23,23,23,23,4,7,4,37,11,0,0,0,0,0,11,0,4,4,4,0,0,0,0,0,4,4,4,0,25,0,4,0,0,0,0,0,0,0,0,0,0,0,0,6,6,0,0,0,0,0,0,0,0,4,7,4,23},
				{24,23,23,23,23,23,23,4,7,4,13,0,0,0,0,0,0,0,0,4,4,7,0,0,0,0,0,7,4,4,0,0,0,4,13,0,0,0,0,0,0,0,0,27,0,0,6,6,0,0,0,0,0,0,0,0,4,7,4,23},
				{24,23,23,23,23,23,23,4,7,4,0,0,0,0,0,0,0,0,0,4,4,4,21,21,21,21,21,4,4,4,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,6,6,0,0,0,0,0,0,0,0,4,7,4,23},
				{24,23,23,23,23,23,23,4,7,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,26,0,0,28,0,29,0,30,0,6,6,0,0,0,0,0,0,0,0,4,7,4,23},
				{24,23,23,23,23,23,23,4,7,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,18,21,21,21,21,21,4,4,4,4,4,4,4,4,13,0,0,0,0,0,0,13,4,7,4,23},
				{24,23,23,23,23,23,23,4,7,4,0,32,0,0,0,0,33,0,0,0,12,0,0,0,0,0,0,0,12,0,0,0,0,0,0,18,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,4,7,4,23},
				{24,23,23,23,23,23,4,4,7,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,18,0,0,0,0,0,18,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,7,4,23},
				{24,23,23,23,23,23,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,18,0,0,0,0,0,18,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,7,4,23},
				{24,24,23,23,23,23,23,4,4,4,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,4,4,4,4,4,4,18,0,0,0,0,0,18,0,34,0,34,0,35,0,0,12,0,0,0,0,31,0,0,0,4,7,4,23},
				{24,24,24,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,7,4,4},
				{23,23,24,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,4,4,4,23}
			};
			//i = vertical, j = horizontal
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 36:
									tile.HasTile = false;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									if (confirmPlatforms == 0)
										for (int a = 0; a < 6; a++)
										{
											WorldGen.PlaceWire4(k + a, l);
										}
									break;
								case 38:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<NaturePlatingTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									if (confirmPlatforms == 1)
									{
										int b = 0;
										while (!Framing.GetTileSafely(k, l + b).YellowWire)
										{
											WorldGen.PlaceWire4(k, l + b);
											b--;
										}
									}
									break;
								case 37:
									tile.HasTile = false;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									if (confirmPlatforms == 0)
										for (int a = 0; a < 46; a++)
										{
											WorldGen.PlaceWire4(k + a, l);
										}
									break;
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 191;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 192;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<NaturePlatingTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 30;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 54;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<DissolvingNatureTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 91, true, true, -1, bannerColor);
									}
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 240, true, true, -1, 17);
									}
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 91, true, true, -1, bannerColor2);
									}
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<NaturePlatingBlastDoorTileClosed>(), true, true, -1, 0);
									}
									break;
								case 13:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									if (confirmPlatforms == 1)
									{
										WorldGen.PlaceTile(k, l, ModContent.TileType<NaturePlatingTorchTile>(), true, true, -1, 0);
										tile.TileFrameX += 66;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										int c = 0;
										while (!Framing.GetTileSafely(k, l + c).YellowWire)
										{
											WorldGen.PlaceWire4(k, l + c);
											c--;
										}
									}
									break;
								case 14:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 135, true, true, -1, 2);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 85, true, true, -1, 2);
									}
									break;
								case 16:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 0);
									}
									break;
								case 17:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<NaturePlatingPlatformTile>(), true, true, -1, 0);
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 18:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<NaturePlatingPlatformTile>(), true, true, -1, 0);
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 19:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 89, true, true, -1, 0);
									}
									break;
								case 20:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 227, true, true, -1, 4);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 21:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<NaturePlatingPlatformTile>(), true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 22:
									tile.HasTile = true;
									tile.TileType = 2;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 24:
									tile.HasTile = true;
									tile.TileType = 0;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 25:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<NaturePlatingChandelierTile>(), true, true, -1, 0);
										tile.TileFrameX += 54;
										Framing.GetTileSafely(k - 1, l).TileFrameX += 54;
										Framing.GetTileSafely(k + 1, l).TileFrameX += 54;
										Framing.GetTileSafely(k - 1, l + 1).TileFrameX += 54;
										Framing.GetTileSafely(k, l + 1).TileFrameX += 54;
										Framing.GetTileSafely(k + 1, l + 1).TileFrameX += 54;
										Framing.GetTileSafely(k - 1, l + 2).TileFrameX += 54;
										Framing.GetTileSafely(k, l + 2).TileFrameX += 54;
										Framing.GetTileSafely(k + 1, l + 2).TileFrameX += 54;
									}
									break;
								case 26:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<NaturePlatingBookcaseTile>(), true, true, -1, 0);
									}
									break;
								case 27:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Bottles, true, true, -1, 1);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 28:
								case 30:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<NaturePlatingChairTile>(), true, true, -1, 0);
										if (_structure[i, j] == 28)
										{
											tile.TileFrameX += 18;
											Framing.GetTileSafely(k, l - 1).TileFrameX += 18;
										}
									}
									break;
								case 29:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<NaturePlatingTableTile>(), true, true, -1, 0);
									}
									break;
								case 31:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<Hydroponics>(), true, true, -1, 0);
										Hydroponics.PlaceInWorldStatic(k, l, null);
									}
									break;
								case 32:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<NaturePlatingBedTile>(), true, true, -1, 0);
										tile.TileFrameX += 72;
										Framing.GetTileSafely(k - 1, l - 1).TileFrameX += 72;
										Framing.GetTileSafely(k, l - 1).TileFrameX += 72;
										Framing.GetTileSafely(k + 1, l - 1).TileFrameX += 72;
										Framing.GetTileSafely(k + 2, l - 1).TileFrameX += 72;
										Framing.GetTileSafely(k - 1, l).TileFrameX += 72;
										Framing.GetTileSafely(k + 1, l).TileFrameX += 72;
										Framing.GetTileSafely(k + 2, l).TileFrameX += 72;
									}
									break;
								case 33:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<NaturePlatingDresserTile>(), true, true, -1, 0);
									}
									break;
								case 34:
								case 35:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<NaturePlatingCapsuleTile>(), true, true, -1, _structure[i, j] == 35 ? 1 : 0);
									}
									break;
							}
						}
					}
				}
			}
		}
	}
}