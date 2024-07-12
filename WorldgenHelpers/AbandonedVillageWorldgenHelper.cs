using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Items.AbandonedVillage;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Items.Fragments;
using SOTS.Items.Furniture.Earthen;
using SOTS.Items.Furniture.Functional;
using Terraria.DataStructures;

namespace SOTS.WorldgenHelpers
{
	public static class AbandonedVillageWorldgenHelper
    {
        [Obsolete]
        private static void GenerateOldMineEntrance(int xPos, int yPos)
        {
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,1,1,2,2,2,2},
                {3,3,1,4,4,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,4,4,4,4,4,4,2,0},
                {3,3,3,1,4,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,1,1,2,2,2,2,3,0},
                {3,3,3,1,4,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,2,3,3,3,3,3,3},
                {3,3,3,1,4,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,3,3,3,3,3,3,3},
                {3,3,3,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,1,2,3,3,3,3,3,3,3},
                {3,3,3,3,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,4,1,2,3,3,3,3,3,3,3},
                {3,3,3,3,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,2,3,3,3,3,3,3,3},
                {3,3,3,3,3,2,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,2,3,3,3,3,3,3,3},
                {3,3,3,3,3,3,2,5,5,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,3,3,3,3,3,3,3},
                {3,3,3,3,3,3,2,5,5,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,3,3,3,3,3,3,3},
                {3,3,3,3,3,3,3,5,5,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,4,4,4,4,4,3,3,3,3},
                {3,3,3,3,3,3,3,5,5,6,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,2,2,1,3,3,3},
                {3,3,3,3,3,3,3,5,5,5,6,5,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,2,2,3,3,3},
                {3,3,3,3,3,3,3,3,5,5,5,5,6,5,5,0,0,0,0,0,0,0,1,2,1,2,1,1,4,4,4,1,2,4,3,3},
                {3,3,3,3,3,3,3,3,5,5,5,5,5,5,5,5,5,2,2,2,2,2,2,2,1,2,1,1,1,4,4,2,2,1,3,3},
                {3,3,3,3,3,3,3,3,3,5,3,5,5,5,5,5,5,5,2,2,2,2,3,3,2,1,1,1,2,2,2,2,2,2,2,3},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,3,3,3,3,2,2,2,2,2,2,2,2,4,4,3,3},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,3,7,7,7,2,2,2,2,4,4,4,1,1,3,3},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,7,7,7,7,4,4,1,1,3,3,3,3,3,3},
                {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,7,7,7,7,7,7,7,3,3,3,3,3,3,3,3}
            };
            int PosX = xPos - _structure.GetLength(1) / 2;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = yPos - 3;
            GenHalfCircle(xPos, yPos - 2, 0, _structure.GetLength(1) / 2, 15);
            GenHalfCircle(xPos, yPos + 3, 1, _structure.GetLength(1) / 2 - 1, 30);
            int placeX = -1;
            int placeY = -1;
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
                            case 4:
                                tile.WallType = 147;
                                break;
                            case 5:
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                            case 6:
                                tile.WallType = (ushort)ModContent.WallType<CharredWoodWallTile>();
                                break;
                            case 7:
                                tile.WallType = 59;
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {2,2,2,2,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,0,0,0,0,0},
                {2,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,3,3,3,3,4,4,4,4,4,4,3,3,3,4,4,4,4,4,4,4},
                {2,4,4,4,4,4,4,6,0,0,0,0,0,0,0,0,0,3,3,3,3,4,4,4,4,4,4,3,4,4,4,4,4,4,4,4},
                {2,2,2,2,2,2,2,0,6,0,0,0,0,0,0,0,0,0,0,7,0,8,0,0,0,0,4,4,2,2,2,3,3,3,9,9},
                {9,9,2,2,3,2,2,0,0,6,0,0,0,0,0,0,0,0,0,0,0,8,0,0,0,0,0,10,2,2,2,3,3,9,9,11},
                {9,9,9,2,3,3,2,0,0,0,6,0,0,0,0,0,0,0,0,0,0,8,0,0,0,0,0,0,3,2,2,3,9,9,11,11},
                {11,11,9,9,3,2,2,0,0,0,0,6,0,0,0,0,0,0,0,0,0,8,0,0,0,0,0,0,2,2,2,3,9,11,11,11},
                {11,11,11,9,2,2,2,0,0,0,0,0,6,0,0,0,0,0,0,0,0,8,0,0,0,0,0,0,3,3,2,9,9,11,11,11},
                {11,11,11,9,9,2,2,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,2,9,11,11,11,11},
                {11,11,11,11,9,2,3,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,12,0,3,3,3,2,9,11,11,11,11},
                {11,11,11,11,9,2,3,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,3,3,3,3,2,2,9,11,11,11,11},
                {11,11,11,11,9,9,3,0,0,13,0,0,0,0,0,0,6,0,0,0,0,0,0,0,4,3,4,4,4,4,4,9,9,11,11,11},
                {11,11,11,11,11,9,9,14,0,13,0,0,0,0,0,0,0,6,0,0,0,0,0,0,4,4,4,4,4,4,4,2,2,9,9,11},
                {11,11,11,11,11,9,9,14,0,13,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,3,3,3,3,3,3,2,2,9,9},
                {11,11,11,11,11,9,9,14,14,13,0,13,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,7,0,0,4,4,2,2,9},
                {11,11,11,11,11,9,9,14,14,13,14,13,0,14,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,4,2,9},
                {11,11,11,11,11,9,14,14,14,13,14,13,0,14,14,14,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,4,2,9},
                {11,11,11,11,11,9,9,14,14,13,13,13,13,13,14,14,14,2,16,0,0,0,6,0,0,0,0,0,0,0,0,0,0,4,2,4},
                {11,11,11,11,11,11,11,9,9,9,9,13,14,14,14,14,2,2,2,2,2,2,4,4,5,5,5,5,5,5,5,5,5,4,3,2},
                {11,11,11,11,11,11,11,9,9,9,9,9,9,2,2,3,2,2,2,3,3,3,3,2,4,0,0,0,0,15,0,0,0,2,2,0},
                {11,11,11,11,11,11,11,11,11,11,11,9,9,9,3,3,3,3,2,2,2,3,4,4,4,4,0,0,0,0,0,0,0,3,0,0},
                {11,11,11,11,11,11,11,11,11,11,11,9,9,9,9,9,9,3,3,3,2,2,2,4,4,4,4,2,0,0,0,0,0,0,0,0},
                {11,11,11,11,11,11,11,11,11,11,11,11,11,11,9,9,9,9,9,9,2,2,2,2,3,3,2,0,0,0,0,0,0,0,0,0}
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
                                        WorldGen.PlaceTile(k, l, TileID.ArrowSign, true, true, -1, 1);
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
                                    tile.TileType = 1;
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
                                    WorldGen.PlaceTile(k, l, 19, true, true, -1, 43);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 6:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, 19, true, true, -1, 43);
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 7:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 42, true, true, -1, 2);
                                    }
                                    break;
                                case 8:
                                    tile.HasTile = true;
                                    tile.TileType = 214;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 9:
                                    tile.HasTile = true;
                                    tile.TileType = 0;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 10:
                                    tile.HasTile = true;
                                    tile.TileType = 1;
                                    tile.Slope = (SlopeType)4;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 12:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 13:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 14:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 15:
                                    if (confirmPlatforms == 1)
                                    {
                                        placeX = k;
                                        placeY = l;
                                    }
                                    break;
                                case 16:
                                    tile.HasTile = true;
                                    tile.TileType = 38;
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                            }
                        }
                    }
                }
            }
            GenerateDownwardPath(placeX, placeY);
        }
        public static void InitializeValidTileLists()
		{
			if (ValidGrassTiles != null && ValidStoneTiles != null && InvalidTiles != null)
				return;
            ValidGrassTiles = new List<int>()
            {
                TileID.CrimsonGrass,
                TileID.CorruptGrass
            };
            ValidStoneTiles = new List<int>()
            {
                TileID.CrimsonGrass,
                TileID.CorruptGrass,
                TileID.Ebonstone,
                TileID.Crimstone
            };
            InvalidTiles = new List<int>()
            {
                TileID.Cloud,
                TileID.RainCloud,
                TileID.Trees
            };
            Mod AVALON;
            bool avalon = ModLoader.TryGetMod("Avalon", out AVALON);
            if (avalon)
            {
                if (AVALON.TryFind("Ickgrass", out ModTile grossGrass))
                {
                    ValidGrassTiles.Add(grossGrass.Type);
                    ValidStoneTiles.Add(grossGrass.Type);
                }
                if (AVALON.TryFind("Chunkstone", out ModTile grossStone))
                {
                    ValidStoneTiles.Add(grossStone.Type);
                }
            }
        }
		private static List<int> ValidGrassTiles = null;
        private static List<int> ValidStoneTiles = null;
        private static List<int> InvalidTiles = null;
        public static void GenHalfCircle(int spawnX, int spawnY, int side = 0, int radius = 10, int radiusY = 10)
		{
			//radius++;
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
							WorldGen.KillWall(xPosition6, spawnY + (int)(y * scale + 0.5f));
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
								tile.TileType = TileID.Dirt;//WorldGen.crimson ? TileID.Crimstone : TileID.Ebonstone;
								tile.HasTile = true;
							}
							if (!tile2.HasTile && tile.TileType == TileID.Dirt)
							{
								tile.TileType = WorldGen.crimson ? TileID.CrimsonGrass : TileID.CorruptGrass;
							}
							//tile.HasTile;
						}
					}
				}
			}
		}
		public static bool IsLineSolid(int x, int y, int totalY = 9, int neededY = 7)
        {
			int foundSolid = 0;
			for(int i = 0; i < totalY; i++)
            {
				Tile tile = Main.tile[x, y + i];
				if(tile.HasTile && Main.tileSolid[tile.TileType])
                {
					foundSolid++;
                }
            }
			return foundSolid >= neededY;
        }
		public static void GenerateAbandonedVillageWell(int xPos, int yPos)
        {
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,2,2,2,2,2,2,2,2,2,0,0},
				{0,0,0,2,2,2,2,0,0,0,0,0,2,2,2,2,2,2,2,2,2,0,0},
				{0,0,0,3,2,2,2,0,0,0,0,0,2,2,2,2,2,2,2,2,2,0,0},
				{0,4,3,3,2,2,2,4,4,0,0,0,2,2,3,3,2,2,2,2,2,0,0},
				{0,3,3,2,2,2,2,2,2,0,0,0,2,2,3,3,2,2,2,2,2,0,0},
				{0,3,3,2,2,2,2,2,2,4,4,4,2,2,2,2,2,2,2,2,4,0,0},
				{0,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0},
				{0,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,2,0},
				{0,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,3,3,3,3,2,0},
				{0,2,2,2,2,2,2,3,3,2,2,2,2,2,3,3,3,3,3,3,3,2,0},
				{0,4,4,4,2,2,3,3,3,3,3,3,3,3,3,3,2,2,2,2,2,2,0},
				{0,0,0,0,0,2,2,2,3,3,3,2,4,4,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,4,2,2,2,2,2,2,5,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,4,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,4,5,5,5,5,5,5,5,5,5,4,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,4,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,4,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,5,5,5,5,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,4,5,5,4,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			int PosX = xPos - 5;  //spawnX and spawnY is where you want the anchor to be when this generates
			int PosY = yPos - 10;
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
								tile.WallType = 6;
								break;
							case 2:
								tile.WallType = 5;
								break;
							case 3:
								tile.WallType = WallID.StoneSlab;
								break;
							case 4:
								tile.WallType = 2;
								break;
							case 5:
								tile.WallType = 1;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,3,3,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,3,4,18,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,1,3,3,18,18,3,3,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,5,18,18,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,5,18,18,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,5,18,18,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,18,18,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,18,18,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,7,7,18,18,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,7,8,8,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,7,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,7,6,9,9,7,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,7,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,7,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,7,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,7,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,7,9,9,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,7,9,9,6,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,7,7,9,9,7,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,7,9,9,7,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,6,6,9,9,6,7,0,0,0,6,6,6,6,6,6,6,6,7,7,6,0},
				{0,0,6,6,9,9,6,6,0,0,0,6,10,10,10,18,18,11,6,7,6,6,0},
				{0,0,6,6,9,9,6,6,0,0,0,6,6,18,18,18,18,18,18,18,6,7,0},
				{6,6,10,10,9,9,6,6,6,6,7,6,6,18,18,18,18,18,18,18,6,6,0},
				{6,10,6,9,9,9,6,6,6,6,7,6,7,18,18,18,18,18,18,18,6,6,0},
				{10,13,9,9,9,9,9,9,6,6,7,7,6,18,12,18,18,18,18,18,10,6,0},
				{10,9,9,9,9,9,9,9,6,6,7,6,10,6,6,6,14,18,18,15,10,10,0},
				{10,9,9,9,9,9,9,9,6,10,10,18,18,18,18,6,6,18,18,6,10,10,6},
				{10,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,10,6},
				{6,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,6},
				{6,16,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,7},
				{6,6,7,6,10,17,17,9,9,9,9,9,9,9,17,10,10,10,6,6,6,6,6},
				{6,7,6,7,6,6,10,10,10,9,9,9,10,10,10,6,6,10,10,6,6,7,7},
				{0,0,0,0,0,7,7,7,7,9,9,9,7,7,7,7,0,0,0,0,0,0,0},
				{0,0,0,0,7,7,7,7,9,9,9,9,9,9,7,7,6,0,0,0,0,0,0},
				{0,0,0,0,7,6,9,9,9,9,19,9,9,9,9,7,7,0,0,0,0,0,0},
				{0,0,0,0,6,6,9,9,9,9,9,9,9,9,7,6,7,0,0,0,0,0,0},
				{0,0,0,0,7,6,7,9,9,9,9,9,9,9,7,7,7,0,0,0,0,0,0},
				{0,0,0,0,7,6,7,7,9,9,9,9,9,9,7,7,0,0,0,0,0,0,0},
				{0,0,0,0,0,7,7,7,7,9,9,9,9,7,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,7,7,6,6,9,9,7,6,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,7,7,6,7,7,7,7,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,7,7,7,7,7,7,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,7,7,7,0,0,0,0,0,0,0,0,0,0}
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
								case 18:
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
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 39;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 39;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 39;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 124;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 8;
										tile.LiquidType = 0;
									}
									break;
								case 9:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 255;
										tile.LiquidType = 0;
									}
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 273;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 1);
									}
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 273;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 273;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 16:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 17:
									tile.HasTile = true;
									tile.TileType = 273;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 19:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.Painting3X3, true, true, -1, 16);
									}
									break;
							}
						}
					}
				}
			}
		}
		public static void PlaceAbandonedVillage()
		{
			InitializeValidTileLists();

            int center = Main.maxTilesX / 2;
			int leftTiles = 0;
			int rightTiles = 0;
			int foundEvilBiomeLeft = 0;
			int foundEvilBiomeRight = 0;
			Point rightSide = new Point();
			Point leftSide = new Point();

			for (int x = center; x < Main.maxTilesX - 200; x++)
			{
				rightTiles++;
				for (int y = 100; y < Main.worldSurface; y++)
				{
					Tile tile = Main.tile[x, y];
					if (tile.HasTile && Main.tileSolid[tile.TileType] && !InvalidTiles.Contains(tile.TileType))
					{
						if (ValidGrassTiles.Contains(tile.TileType) && IsLineSolid(x, y))
						{
							if (foundEvilBiomeRight < 30)
							{
								foundEvilBiomeRight++;
							}
							else
							{
								rightSide = new Point(x, y);
								x = Main.maxTilesX;
							}
						}
						break;
					}
				}
			}
			for (int x = center; x > 200; x--)
            {
				leftTiles++;
				for (int y = 100; y < Main.worldSurface; y++)
                {
					Tile tile = Main.tile[x, y];
					if (tile.HasTile && Main.tileSolid[tile.TileType] && !InvalidTiles.Contains(tile.TileType))
					{
						if (ValidGrassTiles.Contains(tile.TileType) && IsLineSolid(x, y))
						{
							if(foundEvilBiomeLeft < 30)
							{
								foundEvilBiomeLeft++;
							}
							else
                            {
								leftSide = new Point(x, y);
								x = 0;
                            }								
                        }
						break;
                    }
                }
            }
			if (rightTiles > leftTiles)
            {
                GenerateAbandonedVillageWell(leftSide.X, leftSide.Y);
				ContinueGeneration(leftSide.X, -1);
			}
			else
            {
                GenerateAbandonedVillageWell(rightSide.X, rightSide.Y);
				ContinueGeneration(rightSide.X, 1);
			}
		}
		public static void ContinueGeneration(int X, int direction = 1)
		{
			bool generating = true;
			int tileFoundCounter = 0;
			int totalCounter = 0;
            int bestFlatness = 0;
            int bestLocationX = 0;
            int bestLocationY = 0;
            while (generating)
			{
				X += direction;
                for (int y = 200; y < Main.worldSurface; y++)
                {
                    Tile tile = Main.tile[X, y];
                    if (tile.HasTile && Main.tileSolid[tile.TileType] && !InvalidTiles.Contains(tile.TileType))
                    {
                        if (ValidStoneTiles.Contains(tile.TileType) && IsLineSolid(X, y))
                        {
                            tileFoundCounter++;
                            int f = AreaFlatness(X, y);
                            if(bestFlatness < f)
                            {
                                bestFlatness = f;
                                bestLocationX = X;
                                bestLocationY = y;
                            }
                        }
                        break;
                    }
                }
				totalCounter++;
				if(totalCounter > 1000 || !WorldGen.InWorld(X, 100, 50))
                {
					generating = false;
                }
            }

            if(!generating && bestLocationX != 0)
            {
                GenerateNewMineEntrance(bestLocationX, bestLocationY + 2);
            }
        }
        public static bool TileIsNotContainer(Tile tile)
        {
            return tile.TileType != TileID.Containers && tile.TileType != TileID.Containers2 && tile.TileType != TileID.FakeContainers && tile.TileType != TileID.FakeContainers2;
        }
        public static void GenerateTunnel(ref int x, ref int y, float rotation, int width = 7, int size = 30, bool doRopesPlatforms = true)
		{
            int sootSize = width + 1;
            float nextPlatform = 10;
			int height = size;
			float bonusDegreesLeft = Main.rand.NextFloat(360);
            float bonusDegreesRight = Main.rand.NextFloat(360);
            HashSet<Point> platformPoints = new HashSet<Point>();
            for (float j = 0; j < height; j += 0.25f)
            {
                bool generatePlatforms = false;
				nextPlatform -= 0.25f * Math.Abs((float)Math.Cos(MathHelper.ToRadians(rotation)));
                if (nextPlatform <= 0 && doRopesPlatforms)
                {
                    generatePlatforms = true;
                    nextPlatform = WorldGen.genRand.Next(8, 15);
                }
                int left = -width - Math.Abs((int)(2.5f * Math.Sin(MathHelper.ToRadians(bonusDegreesLeft))));
				int right = width + Math.Abs((int)(2.5f * Math.Sin(MathHelper.ToRadians(bonusDegreesRight))));
                int sootLeft = -sootSize - Math.Abs((int)(2.5f * Math.Sin(MathHelper.ToRadians(bonusDegreesRight))));
                int sootRight = sootSize + Math.Abs((int)(2.5f * Math.Sin(MathHelper.ToRadians(bonusDegreesLeft))));
				for(int RunType = 0; RunType <= 2; RunType++)
                {
                    for (float i = left - 4; i <= right + 4; i += 0.25f)
                    {
                        Vector2 vPoint = new Vector2(i + 0.5f, j + 0.5f).RotatedBy(MathHelper.ToRadians(rotation));
                        Point rPoint = new Point(x + (int)(vPoint.X), y + (int)(vPoint.Y));
                        Tile tile = Framing.GetTileSafely(rPoint);
                        bool interior = false;
                        bool generateSoot = Math.Abs(i - sootLeft) < 1.75f || Math.Abs(i - sootRight) < 1.75f;
						bool generateSides = i >= left && i <= right && (Math.Abs(i - left) < 3.75f || Math.Abs(i - right) < 3.75f);
                        bool validWall = tile.WallType != WallID.RocksUnsafe1 && tile.WallType != WallID.StoneSlab && tile.WallType != WallID.GrayBrick /*&& tile.WallType != WallID.Stone*/ &&
                            tile.WallType != ModContent.WallType<EarthenPlatingBeamWall>() && tile.WallType != ModContent.WallType<EarthenPlatingPanelWallWall>() && tile.WallType != ModContent.WallType<EarthenPlatingWallWall>();
                        if ((i >= left && i <= right) || generateSoot)
                        {
                            interior = true;
                            if (RunType == 0)
                            {
                                Tile tileabove = Framing.GetTileSafely(rPoint.X, rPoint.Y - 1);
                                if (validWall && TileIsNotContainer(tile) && TileIsNotContainer(tileabove))
                                {
                                    tile.HasTile = false;
                                    tile.LiquidAmount = 0;
                                }
                            }
                        }
						if (RunType == 1)
                        {
							if(validWall || tile.TileType == ModContent.TileType<SootBlockTile>())
                            {
                                if (generateSides)
                                {
                                    ushort type = TileID.GrayBrick;
                                    if (WorldGen.genRand.NextBool(2))
                                    {
                                        type = TileID.Stone;
                                    }
                                    else if (WorldGen.genRand.NextBool(3))
                                    {
                                        type = TileID.StoneSlab;
                                    }
                                    tile.TileType = (ushort)type;
                                    tile.HasTile = true;
                                    tile.WallType = WallID.Stone;
                                }
                                else if (generateSoot)
                                {
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.HasTile = true;
                                    tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                }
                            }
                        }
                        else if (interior && RunType == 2 && !generateSoot)
                        {
                            ushort type = WallID.RocksUnsafe1;
                            if (WorldGen.genRand.NextBool(7))
                            {
                                type = WallID.GrayBrick;
                            }
                            else if (WorldGen.genRand.NextBool(10))
                                type = WallID.StoneSlab;
							if(!generateSides)
								tile.WallType = (ushort)type;
                            if (i == 0 && doRopesPlatforms)
                            {
                                if (generatePlatforms)
                                {
									if(!platformPoints.Contains(rPoint))
										platformPoints.Add(rPoint); 
									generatePlatforms = false;
                                }
								else if(Math.Abs(rotation) < 5)
									WorldGen.PlaceTile(rPoint.X, rPoint.Y, TileID.Rope);
                            }
                        }
                    }
                }
				bonusDegreesLeft += (float)Math.Pow(WorldGen.genRand.NextFloat(), 2) * 32 * 0.5f;
                bonusDegreesRight += (float)Math.Pow(WorldGen.genRand.NextFloat(), 2) * 32 * 0.5f;
            }
            if(doRopesPlatforms)
            {
                foreach (Point p in platformPoints)
                {
                    for (int i2 = -8; i2 <= 8; i2++)
                    {
                        Tile tile = Main.tile[p.X + i2, p.Y];
                        if (tile.HasTile && tile.TileType == TileID.Rope)
                        {
                            tile.HasTile = false;
                            tile.LiquidAmount = 0;
                        }
                        WorldGen.PlaceTile(p.X + i2, p.Y, TileID.Platforms, false, false, -1, 43);
                    }
                }
            }
            SOTSWorldgenHelper.SmoothRegion(x, y + height / 2, 24, height);
            Vector2 vPointA = new Vector2(0, height + 1).RotatedBy(MathHelper.ToRadians(rotation));
			x = (int)(x + vPointA.X + 0.5f);
            y = (int)(y + vPointA.Y + 0.5f);
        }
		public static void GenerateCaveCircle(int x, int y, float xMult = 1f, float yMult = 1f, int outlineSize = 11, float wallSize = 6.5f, float stoneSize = 2)
        {
            for (int i = -outlineSize; i < outlineSize; i++)
			{
				for(int j = -outlineSize; j < outlineSize; j++)
                {
                    float radius = MathF.Sqrt(i * i + j * j);
                    int i2 = (int)(i * xMult);
                    int j2 = (int)(j * yMult);
                    Tile t = Framing.GetTileSafely(x + i2, y + j2);
                    if (radius <= wallSize + WorldGen.genRand.NextFloat(-0.5f, 0.5f))
                    {
						//WallID.stone and soot walls are used for the border of the generation
						if(t.WallType != WallID.Stone && t.WallType != ModContent.WallType<SootWallTile>() 
                            && t.WallType != ModContent.WallType<EarthenPlatingBeamWall>() && t.WallType != ModContent.WallType<EarthenPlatingPanelWallWall>() && t.WallType != ModContent.WallType<EarthenPlatingWallWall>())
                        {
                            if (t.HasTile && t.TileType != TileID.Platforms && t.TileType != TileID.Rope) //)t.WallType != WallID.StoneSlab && t.WallType != WallID.GrayBrick && t.WallType != WallID.RocksUnsafe1)
                            {
                                Tile tileabove = Framing.GetTileSafely(x + i2, y + j2 - 1);
                                if(TileIsNotContainer(t) && TileIsNotContainer(tileabove))
                                {
                                    t.HasTile = false;
                                    t.LiquidAmount = 0;
                                }
                            }
                            ushort type = WallID.RocksUnsafe1;
                            if (WorldGen.genRand.NextBool(7))
                            {
                                type = WallID.GrayBrick;
                            }
                            else if (WorldGen.genRand.NextBool(10))
                                type = WallID.StoneSlab;
                            t.WallType = type;
                        }
                    }
					else if(radius <= outlineSize + WorldGen.genRand.NextFloat(-0.5f, 0.8f))
					{
						bool generateStone = radius <= wallSize + stoneSize + WorldGen.genRand.NextFloat(-0.6f, 0.8f);
						bool open = t.WallType == WallID.RocksUnsafe1 || t.WallType == WallID.GrayBrick || t.WallType == WallID.StoneSlab
                            || t.WallType == ModContent.WallType<EarthenPlatingBeamWall>() || t.WallType == ModContent.WallType<EarthenPlatingPanelWallWall>() || t.WallType == ModContent.WallType<EarthenPlatingWallWall>();
						bool isStone = t.TileType == TileID.GrayBrick || t.TileType == TileID.Stone || t.TileType == TileID.StoneSlab;
                        if (!generateStone && !open && !isStone && t.TileType != ModContent.TileType<EarthenPlatingTile>() && t.TileType != ModContent.TileType<EarthenPlatingPlatformTile>())
                        {
                            t.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                            t.HasTile = true;
                            t.WallType = (ushort)ModContent.WallType<SootWallTile>();
                        }
						else if(generateStone && !open)
                        {
                            ushort type = TileID.GrayBrick;
                            if (WorldGen.genRand.NextBool(2))
                            {
                                type = TileID.Stone;
                            }
                            else if (WorldGen.genRand.NextBool(3))
                            {
                                type = TileID.StoneSlab;
                            }
                            t.TileType = (ushort)type;
                            t.HasTile = true;
							t.WallType = WallID.Stone;
                        }
                    }
				}
            }
            SOTSWorldgenHelper.SmoothRegion(x, y, outlineSize * 2 + 1, outlineSize * 2 + 1);
        }
		public static void GenerateDownwardPath(int x, int y)
		{
			float rotation = 0;
			for(int i = 0; i < 15; i++)
            {
                int previousX = x;
                int previousY = y;
                //after 6 tunnels, the earthen layer should start setting in

                //at the 10nth tunnell intersection, there will be a split. One side leads to the GULA portal. The other side leads to a crimson/corruption boss arena.
                //Which has some orbs and a pylon for calling down the ancient Earthen Construct

                if (previousY > Main.rockLayer + 601 && i > 12) //At least 12 layers should be built
					break; //after this is the earthen layer

                //bottom of the earthen layer will be the Gula Layer
                GenerateTunnel(ref x, ref y, rotation);
                if (i != 0)
                    GenerateCaveCircle(previousX, previousY, 1, 1, 12, 5.5f, 2);
                if (i == 13)
                    rotation = 0;
                else
				    rotation = WorldGen.genRand.NextFloat(-45, 45);
            }
        }
		public static void GenerateNewMineEntrance(int x, int y)
		{
            int endX = 0;
            int endY = 0;
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,5,4,4,4,4,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,5,0,0,0,5,5,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,5,5,5,5,5,5,4,4,4,4,4,0,4,4,4,4,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,4,4,0,0,0,4,4,4,4,4,4,4,0,4,4,4,5,5,5,5,5,5,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,4,5,5,5,5,5,5,4,4,4,4,4,4,4,4,4,4,0,4,4,4,4,0,0,0,0},
                {0,0,0,0,0,4,4,4,0,4,4,4,4,4,4,4,4,0,4,4,4,4,5,5,5,5,5,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,4,5,5,5,5,5,5,5,4,4,4,4,4,4,4,4,4,0,4,4,4,4,0,0,0,0},
                {0,0,0,0,4,4,4,4,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,5,5,5,4,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,4,4,5,5,5,5,5,4,4,4,4,0,4,4,4,4,4,0,4,4,4,4,0,0,0,0},
                {0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,4,4,4,4,4,4,4,4,4,4,4,0,4,4,4,4,4,0,4,4,4,4,4,0,0,0},
                {0,4,4,4,4,4,4,4,4,4,4,4,4,7,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,2,2,2,2,2,2,2,2,2,2,2,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,7,4,4,4,0,4,4,4,4,4,4,4,0},
                {0,4,4,4,4,4,4,4,4,4,4,4,4,7,9,9,9,9,9,9,0,0,0,10,10,10,10,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,10,10,10,10,0,0,0,9,9,9,9,9,9,7,4,4,4,4,4,4,4,4,4,4,4,0},
                {0,10,4,4,4,4,4,4,4,4,4,4,4,7,9,9,9,9,9,9,0,0,0,10,10,10,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,10,10,10,10,0,0,0,9,9,9,9,9,9,7,4,4,4,4,4,4,4,4,4,0,10,0},
                {0,10,10,10,4,4,4,4,4,4,4,4,4,7,9,9,9,9,9,9,0,0,0,10,10,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,10,10,10,0,0,0,9,9,9,9,9,9,7,4,4,4,4,4,4,4,4,4,10,10,0},
                {0,10,10,10,10,10,10,10,10,4,4,4,4,7,9,9,9,9,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,10,0,0,0,9,9,9,9,9,9,7,4,4,4,4,4,10,10,10,10,10,10,0},
                {0,0,10,10,10,10,7,8,8,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,8,8,7,10,10,10,0,0},
                {0,0,0,10,10,10,7,9,9,9,9,9,9,9,9,9,9,9,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,7,10,10,10,0,0},
                {0,0,0,10,10,10,7,9,9,9,9,9,9,9,9,9,9,9,9,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,7,10,10,0,0,0},
                {0,0,0,0,0,10,7,8,8,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,8,8,7,10,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
            int PosX = x - 36; //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = y - 16;
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
                                tile.WallType = (ushort)ModContent.WallType<CharredWoodWallTile>();
                                break;
                            case 2:
                                tile.WallType = 5;
                                break;
                            case 3:
                                tile.WallType = 147;
                                break;
                            case 4:
                                tile.WallType = 274;
                                break;
                            case 5:
                                tile.WallType = 261;
                                break;
                            case 6:
                                tile.WallType = 21;
                                break;
                            case 7:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingBeamWall>();
                                break;
                            case 8:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingWallWall>();
                                break;
                            case 9:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingPanelWallWall>();
                                break;
                            case 10:
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                        }
                    }
                }
            }
			_structure = new int[,] {
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,3 ,3 ,3 ,2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,3 ,3 ,3 ,3 ,2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,3 ,3 ,4 ,5 ,3 ,3 ,2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,3 ,3 ,0 ,0 ,0 ,5 ,3 ,3 ,3 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,6 ,3 ,3 ,3 ,3 ,4 ,0 ,0 ,0 ,0 ,5 ,3 ,3 ,2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,6 ,3 ,3 ,4 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,5 ,3 ,3 ,2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,3 ,3 ,3 ,4 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,3 ,3 ,2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,3 ,3 ,3 ,3 ,3 ,6 ,6 ,6 ,6 ,6 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,2 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,8 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,0,10,10,10,10,10,10,10,10,10,10,10,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,10,10,10,10,11,11,11,11,11,11,11,11,11,11,11,11,11,10,10,10 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0,10,10,10,10,11,11,11,11,11,11,11,11,11,10,10,10,10,10,10,10,15,15,15,15},
                {15,15,15,10,10,10,11,11,11,11,11,11,11,11,11,11,12,12,12,12,12,11,11,11,10,10,10,13,13,13,13,13,14,14,14,14,14,14,14,14,14,13,13,13,13,13,10,10,10,11,11,11,11,11,11,11,12,12,12,12,11,11,11,11,11,11,11,10,10,15,15,15},
                {15,15,10,10,11,11,11,11,12,12,11,11,12,12,12,12,12,12,12,12,12,12,12,11,11,11,11,16,16,16,16,16 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0,16,16,16,16,16,11,11,11,11,11,11,12,12,12,12,12,12,12,12,12,12,11,12,12,12,11,11,10,10,15,15},
                {15,10,10,11,11,11,11,12,12,12,12,12,12,12,12,12,12,12,12,17 ,0,11,11,11,11,11,11,13,13,13,13,13 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0,13,13,13,13,13,11,11,11,11,11,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,11,11,10,10,15},
                {10,10,11,11,12,12,12,12,12,12,12,17 ,0 ,0 ,0 ,0,18,12,12 ,0 ,0 ,0,11,11,11,11 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,0 ,0 ,0 ,0,11,11,11,11 ,0 ,0 ,0,18,12,12 ,0 ,0,12,12,12,12,12,12,12,12,11,10,10},
                {11,11,11,12,12,12,12,12,12,12,17 ,0 ,0 ,0 ,0 ,0 ,0,12,17 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12,12 ,0 ,0 ,0,12,12,12 ,0,12,12,12,12,11,11},
                {11,11,12,12,12 ,0 ,0,18,12,12 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12,17 ,0 ,0 ,0,12,12,17 ,0 ,0 ,0,12,12,11,11},
                {11,12,12,12,17 ,0 ,0 ,0,12,17 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12 ,0 ,0 ,0 ,0,18,12 ,0 ,0 ,0 ,0,12,12,12,11},
                {11,12,12,12 ,0 ,0 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0,12 ,0 ,0 ,0 ,0,18,12,12,11},
                {11 ,0,18,12 ,0 ,0 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,19,20,20,20,21,21 ,0 ,0,13,13,13,13,22 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0,23,13,13,13,13 ,0 ,0,21,21,20,20,20,24 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0,12,17,11},
                {11 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,19, 0,20,20,20 ,0 ,0 ,0,25,16,16,16,16 ,0,22 ,0 ,0 ,0 ,9 ,0 ,0 ,0,23 ,0,16,16,16,16 ,0 ,0 ,0 ,0,20,20,20, 0,24 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12 ,0,11},
                {25 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,19, 0, 0,20,20,20 ,0 ,0,25,25,13,13,13,13,14,14,14,14,14,14,14,14,14,14,14,13,13,13,13,25 ,0 ,0 ,0,20,20,20 ,0, 0,24 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12 ,0,11},
                {25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,21,21,21,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,34,15,15,15,15,15,15,15,25,25,25,25 ,0 ,0,20,20,20,21,21,21 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,25},
                {25,25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,25,25},
                {25,25,25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20 ,0 ,0 ,0 ,0 ,0 ,0 ,0,26,26,26 ,0 ,0 ,0 ,0 ,0 ,0 ,0,25,25},
                {25,25,25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,27 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20 ,0 ,0 ,0 ,0 ,0 ,0 ,0,26,26,26 ,0 ,0 ,0 ,0 ,0 ,0,25,25,25},
                {25,25,25,25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20,35,35,35,35 ,0 ,0 ,0,26,26,26 ,0 ,0 ,0 ,0 ,0 ,0,25,25,25},
                {25,25,25,25,25,25 ,0 ,0,28 ,0 ,0 ,0,29 ,0 ,0,30 ,0,31 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20,35,32,35,35 ,0 ,0 ,0 ,0,33 ,0 ,0 ,0 ,0 ,0,25,25,25,25,25},
                {25,25,25,25,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,25,25,25},
                {25,25,25,25,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,25,25,25},
                {25,25,25,25,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,25,25,25}
            };
            //i = vertical, j = horizontal
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
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)3;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)4;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 6:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 7:
                                    tile.HasTile = true;
                                    tile.TileType = 124;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 8:
                                    if (confirmPlatforms == 2)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 42, true, true, -1, 2);
                                    }
                                    break;
                                case 9:
                                    tile.HasTile = true;
                                    tile.TileType = 213;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 10:
                                    tile.HasTile = true;
                                    tile.TileType = WorldGen.crimson ? TileID.CrimsonGrass : TileID.CorruptGrass;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 11:
                                    tile.HasTile = true;
                                    tile.TileType = 0;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 12:
                                    tile.HasTile = true;
                                    tile.TileType = 1;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 13:
                                    tile.HasTile = true;
                                    tile.TileType = 38;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 14:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, 19, true, true, -1, 43);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 15:
                                    break;
                                case 16:
                                    tile.HasTile = true;
                                    tile.TileType = 273;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 17:
                                    tile.HasTile = true;
                                    tile.TileType = 1;
                                    tile.Slope = (SlopeType)3;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 18:
                                    tile.HasTile = true;
                                    tile.TileType = 1;
                                    tile.Slope = (SlopeType)4;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 19:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>(), true, true, -1, 43);
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 20:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 21:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>(), true, true, -1, 43);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 22:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, 19, true, true, -1, 43);
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 23:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, 19, true, true, -1, 43);
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 24:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>(), true, true, -1, 43);
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 25:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 26:
                                    tile.HasTile = true;
                                    tile.TileType = WorldGen.crimson ? TileID.Crimtane : TileID.Demonite;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 27:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingBulbTile>(), true, true, -1, 0);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 28:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingStorageTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 29:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingSofaTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 30:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingChairTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 31:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingTableTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 32:
                                case 35:
                                    if (confirmPlatforms == 1 && _structure[i, j] == 32)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingBedTile>(), true, true, -1, 0);
                                    }
                                    else if(_structure[i, j] == 35)
                                    {
                                        if (confirmPlatforms == 0)
                                        {
                                            tile.HasTile = false;
                                            tile.IsHalfBlock = false;
                                            tile.Slope = 0;
                                        }
                                    }
                                    if (confirmPlatforms == 2)
                                        tile.TileFrameX += 72;
                                    break;
                                case 33:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<MineralariumTile>(), true, true, -1, 0);
                                        ModTileEntity.PlaceEntityNet(k - 3, l, ModContent.TileEntityType<MineralariumTE>());
                                        NetMessage.SendTileSquare(Main.myPlayer, k, l, 7);
                                    }
                                    break;
                                case 34:
                                    endX = k;
                                    endY = l;
                                    break;
                            }
                        }
                    }
                }
            }
            GenerateDownwardPath(endX, endY);
        }
        public static int FindHeightAverage(int x)
        {
            int avgY = 0;
            for(int i = -30; i < 30; i++)
            {
                for (int j = 200; j < Main.worldSurface; j++)
                {
                    Tile tile = Main.tile[x + i, j];
                    if (tile.HasTile && Main.tileSolid[tile.TileType] && !InvalidTiles.Contains(tile.TileType))
                    {
                        avgY += j;
                        break;
                    }
                }
            }
            avgY = avgY / 61;
            return avgY;
        }
        public static int AreaFlatness(int x, int y)
        {
            int tileThere = 0;
            for (int i = -30; i < 30; i++)
            {
                for (int j = -3; j <= 2; j++)
                {
                    Tile tile = Main.tile[x + i, y + j];
                    if (tile.HasTile && Main.tileSolid[tile.TileType] && !InvalidTiles.Contains(tile.TileType))
                    {
                        if(j > -3)
                            tileThere++;
                        break;
                    }
                }
            }
            return tileThere;
        }
        public static void GenerateBeam(int posX, int posY, int width = 13)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int x = posX + i;
                    int y = posY + j;
                    Tile t = Framing.GetTileSafely(x, y);
                    t.ClearTile();
                    WorldGen.PlaceTile(x, y, (ushort)ModContent.TileType<EarthenPlatingTile>());
                }
            }
        }
        public static void GenerateWalls(int posX, int posY, int width, int minHeight, int height)
        {
            int full = WorldGen.genRand.Next(2);
            for (int i = 0; i < 2; i++)
            {
                int randU = height;
                if (i != full && minHeight < height)
                    randU = WorldGen.genRand.Next(minHeight, height);
                int rand = WorldGen.genRand.Next(2);
                int startDir = rand * 2 - 1;
                int startSide = rand * height;
                int endY = 0;
                for (int j = 0; j <= randU; j++)
                {
                    int x = posX + i * width;
                    int y = posY + j * startDir - startSide;
                    Tile t = Framing.GetTileSafely(x, y);
                    t.WallType = WallID.None;
                    WorldGen.PlaceWall(x, y, (ushort)ModContent.WallType<EarthenPlatingBeamWall>());
                    endY = j;
                }
                if (full != i)
                    for (int k = 1; k < width; k++)
                    {
                        for (int l = 0; l <= randU; l++)
                        {
                            int x = posX + k;
                            int y = posY + (endY - l) * startDir - startSide;
                            Tile t = Framing.GetTileSafely(x, y);
                            t.WallType = WallID.None;
                            ushort type = (ushort)ModContent.WallType<EarthenPlatingWallWall>();
                            if (l != 0)
                            {
                                type = (ushort)ModContent.WallType<EarthenPlatingPanelWallWall>();
                            }
                            WorldGen.PlaceWall(x, y, type);
                            if (l != 0)
                            {
                                if(WorldGen.genRand.NextBool(12))
                                {
                                    TryPlacingTorch(x, y, WorldGen.genRand.Next(2, 5));
                                }
                            }

                        }
                    }
            }
        }
        public static Point16[] GenerateMineShaft(int posX, int posY, int dir = 1)
        {
            Point16[] edges = new Point16[4];

            int platW = 12;
            if (dir == -1)
            {
                posX -= platW;
            }
            int biggerPlatform = WorldGen.genRand.Next(2);
            int bonusPlatSize = WorldGen.genRand.Next(7);
            int offset = WorldGen.genRand.Next(bonusPlatSize);

            int startX = posX - (biggerPlatform == 1 ? offset : 0);
            int width1 = platW + (biggerPlatform == 1 ? bonusPlatSize : 0);
            GenerateBeam(startX, posY, width1);
            edges[0] = new Point16(startX, posY + 1);
            edges[1] = new Point16(startX + width1 - 1, posY + 1);
            if (dir == -1)
            {
                Point16 temp = edges[0];
                edges[0] = edges[1];
                edges[1] = temp;
            }
            int total = 0;
            for (int i = 0; i < width1; i++)
            {
                for (int j = 6; j < 15; j++)
                {
                    Tile t = Framing.GetTileSafely(startX + i, posY - j);
                    if(t.HasTile)
                    {
                        total += j;
                        break;
                    }
                }
            }
            total /= width1;
            int sepVert = total + WorldGen.genRand.Next(3);
            int nextX = posX - (biggerPlatform != 1 ? offset : 0);
            int nextWidth1 = platW + (biggerPlatform != 1 ? bonusPlatSize : 0);
            for (int i = 0; i < Math.Max(width1, nextWidth1); i++)
            {
                for(int j = 0; j < sepVert - 2; j++)
                {
                    int x = Math.Min(nextX, startX) + i;
                    int y = posY - 1 - j;
                    Tile tile = Framing.GetTileSafely(x, y);
                    tile.HasTile = false;
                }
            }
            startX = nextX;
            width1 = nextWidth1;
            GenerateBeam(startX, posY - sepVert, width1);
            edges[2] = new Point16(startX, posY - sepVert);
            edges[3] = new Point16(startX + width1 - 1, posY - sepVert);
            if (dir == -1)
            {
                Point16 temp = edges[2];
                edges[2] = edges[3];
                edges[3] = temp;
            }

            int wallStuffOffset = WorldGen.genRand.Next(3);
            int width = WorldGen.genRand.Next(3, 6);
            int height = sepVert - 3;
            GenerateWalls(posX + wallStuffOffset, posY - 1, width, WorldGen.genRand.Next(1, 4), height);

            int w = WorldGen.genRand.Next(3, 6);
            int startPos = platW - w - 1 - WorldGen.genRand.Next(3);
            GenerateWalls(posX + startPos, posY - 1, w, WorldGen.genRand.Next(1, 4), height);

            return edges;
        }
        public static void ClearLine(Point16 start, Point16 end, int dir = 1)
        {
            for(float x = 0; x <= 1f; x += 0.2f)
            {
                Vector2 middle = Vector2.Lerp(start.ToVector2(), end.ToVector2(), x) + Vector2.One * 0.5f;
                Tile t = Framing.GetTileSafely((int)middle.X, (int)middle.Y);
                if (t.TileType != ModContent.TileType<EarthenPlatingTile>())
                {
                    for(int j = -4; j <= 4; j++)
                    {
                        t = Framing.GetTileSafely((int)middle.X, (int)middle.Y + j * dir);
                        if (!t.HasTile || j == 0)
                        {
                            t.ClearTile();
                            t.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                            t.HasTile = true;
                        }
                        if(j < 0)
                        {
                            t.ClearTile();
                        }
                    }
                }
            }
        }
        public static void GenerateEntireShaft(int posX, int posY, int size = 15, int dir = 1, int floorNum = 0)
        {
            int stairWellLocation = Math.Max(WorldGen.genRand.Next(size), WorldGen.genRand.Next(size)); //Use max function to bias the stairs towards spawing farther away. They can still spawn close, but will attempt not to.
            int x2 = posX;
            int y2 = posY;
            for(int i = 0; i < size; i++)
            {
                GenerateTunnel(ref x2, ref y2, -90 * dir, width: 8, size: 20);
            }
            posX += 3 * dir;
            posY += 3;
            Point16 previousPointTop = new Point16(0, 0);
            Point16 previousPointBot = new Point16(0, 0);
            for (int i = 0; i < size; i++)
            {
                Point16[] edges = GenerateMineShaft(posX + i * 20 * dir, posY + WorldGen.genRand.Next(4), dir);
                for(int j = 0; j < 4; j++)
                {
                    if (j == 3)
                    {
                        previousPointTop = edges[j];
                    }
                    if(j == 2)
                    {
                        if (previousPointTop.X != 0)
                        {
                            Vector2 middle = Vector2.Lerp(edges[j].ToVector2(), previousPointTop.ToVector2(), 0.5f) + Vector2.One * 0.5f;
                            ClearLine(new Point16((int)middle.X, (int)middle.Y), previousPointTop, -1);
                            ClearLine(new Point16((int)middle.X, (int)middle.Y), edges[j], -1);
                        }
                    }
                    if (j == 1)
                    {
                        previousPointBot = edges[j];
                    }
                    if(j == 0)
                    {
                        if (previousPointBot.X != 0)
                        {
                            Vector2 middle = Vector2.Lerp(edges[j].ToVector2(), previousPointBot.ToVector2(), 0.5f) + Vector2.One * 0.5f;
                            ClearLine(new Point16((int)middle.X, (int)middle.Y), previousPointBot, 1);
                            ClearLine(new Point16((int)middle.X, (int)middle.Y), edges[j], 1);
                        }
                    }
                }
                if(i == stairWellLocation && floorNum > 0)
                {
                    Point16 center = new Point16((edges[0].X + edges[1].X) / 2, edges[0].Y - 1);
                    GenerateStairs(center.X, center.Y, floorNum - 1);
                }
            }
            GenerateCaveCircle(x2, y2);
        }
        public static void TryPlacingTorch(int posX, int posY, int padding)
        {
            for(int i = -padding; i <= padding; i++)
            {
                for(int j = -padding; j <= padding; j++)
                {
                    Tile t = Framing.GetTileSafely(posX + i, posY + j);
                    if(t.HasTile && (t.TileType == ModContent.TileType<EarthenPlatingTorchTile>() || Math.Abs(i) <= 1 || Math.Abs(j) <= 1))
                    {
                        return;
                    }
                }
            }
            Tile t2 = Framing.GetTileSafely(posX, posY);
            t2.ClearTile();
            WorldGen.PlaceTile(posX, posY, ModContent.TileType<EarthenPlatingTorchTile>());
        }
        public static void GenerateStairs(int posX, int posY, int floorNum)
        {
            int x1 = posX;
            int y1 = posY + 1;
            GenerateTunnel(ref x1, ref y1, 0, 9, 22, doRopesPlatforms: false);
            bool reflected = WorldGen.genRand.NextBool(2);
            int[,] _structure = {
                {0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0},
                {0,0,0,0,0,2,3,3,3,3,3,3,0,0,0,0,0},
                {4,4,4,3,3,3,2,3,3,3,3,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,2,3,3,3,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,3,2,3,3,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,3,3,2,3,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,3,3,3,2,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,3,3,3,3,2,3,3,4,4,4},
                {0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0},
                {0,0,0,0,0,2,3,3,3,3,3,3,0,0,0,0,0},
                {4,4,4,3,3,3,2,3,3,3,3,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,2,3,3,3,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,3,2,3,3,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,3,3,2,3,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,3,3,3,2,3,3,3,4,4,4},
                {4,4,4,3,3,3,3,3,3,3,3,2,3,3,4,4,4},
                {0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0},
                {0,0,0,0,0,3,3,3,3,3,3,3,0,0,0,0,0}
            };
            int PosX = posX - 8;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 0;
            for (int i = 0; i < _structure.GetLength(0); i++)
            {
                for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
                {
                    int k = PosX + j * (reflected ? -1 : 1) + (reflected ? 16 : 0);
                    int l = PosY + i;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (_structure[i, j])
                        {
                            case 0:
                                tile.HasTile = true;
                                tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                tile.Slope = 0;
                                tile.IsHalfBlock = false;
                                break;
                            case 1:
                                tile.HasTile = false;
                                WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>());
                                tile.Slope = 0;
                                tile.IsHalfBlock = false;
                                break;
                            case 2:
                                tile.HasTile = false;
                                WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>());
                                tile.Slope = (SlopeType)(reflected ? 2 : 1);
                                tile.IsHalfBlock = false;
                                break;
                            case 3:
                                tile.ClearTile();
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,2,1,3,3,3,3,3,3,3,3,3,3,3,1,2,0},
                {0,2,1,2,2,2,2,2,2,2,2,2,2,2,1,2,0},
                {0,2,1,3,3,3,3,3,3,3,3,3,3,3,1,2,0},
                {0,2,1,2,0,0,0,0,0,0,0,0,0,2,1,2,0},
                {0,2,1,2,0,0,0,0,0,0,0,0,0,2,1,2,0},
                {0,2,1,2,0,0,0,0,0,0,0,0,0,2,1,2,0},
                {0,2,1,2,0,0,0,0,0,0,0,0,0,2,1,2,0},
                {0,2,1,3,3,3,3,3,3,3,3,3,3,3,1,2,0},
                {0,0,1,2,2,2,2,2,2,2,2,2,2,2,1,0,0},
                {0,0,1,2,2,2,2,2,2,2,2,2,2,2,1,0,0},
                {0,2,1,3,3,3,3,3,3,3,3,3,3,3,1,2,0},
                {0,2,1,2,0,0,0,0,0,0,0,0,0,2,1,2,0},
                {0,2,1,2,0,0,0,0,0,0,0,0,0,2,1,2,0},
                {0,2,1,2,0,0,0,0,0,0,0,0,0,2,1,2,0},
                {0,2,1,2,0,0,0,0,0,0,0,0,0,2,1,2,0},
                {0,2,1,3,3,3,3,3,3,3,3,3,3,3,1,2,0},
                {0,2,1,2,2,2,2,2,2,2,2,2,2,2,1,2,0},
                {0,2,1,3,3,3,3,3,3,3,3,3,3,3,1,2,0}
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
                            case 1:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingBeamWall>();
                                break;
                            case 2:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingPanelWallWall>();
                                break;
                            case 3:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingWallWall>();
                                break;
                        }
                    }
                }
            }
            GenerateCaveCircle(x1, y1 + 5, yMult: 0.6f, outlineSize: 22, wallSize: 14f, stoneSize: 3);
            if (floorNum >= 0)
            {
                int mainSide = WorldGen.genRand.Next(2) * 2 - 1;
                GenerateEntireShaft(x1 + 9 * mainSide, y1 + 5, 5, mainSide, floorNum);
                if(WorldGen.genRand.NextBool(3))
                {
                    GenerateEntireShaft(x1 - 9 * mainSide, y1 + 5, 3, -mainSide, -1);
                }
            }
        }
        public static void GenerateUndergoundEntrance(int posX, int posY)
        {
            GenerateEntireShaft(posX + 22, posY - 12, 3, 1, -1);
            GenerateEntireShaft(posX - 22, posY - 12, 3, -1, -1);
            GenerateCaveCircle(posX, posY - 13, 1, 1, 30, 19f, 5);
            int[,] _structure = {
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1,-1, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 2, 3, 1, 1, 0, 0, 0, 1, 1, 4, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 3, 1, 1, 0, 0, 0, 1, 1, 4, 2},
                {-1, 1, 3, 1, 0, 0, 0, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 0, 0, 0, 1, 4, 1,-1},
                {-1, 1, 1, 2, 0, 0, 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0, 2, 1, 1,-1},
                {-1,-1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1,-1,-1},
                {-1,-1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1,-1,-1},
                {-1,-1,-1, 1, 0, 0, 0, 1, 1, 1, 1, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7, 1, 1, 1, 5, 1, 1, 1, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 1, 1, 1, 1, 0, 0, 0, 1,-1,-1,-1},
                {-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1},
                {-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1},
                {-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1},
                {-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1},
                {-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1},
                {-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1},
                {-1,-1,-1,-1, 1, 1, 1, 1, 8, 1, 8, 1, 8, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 9, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1},
                {-0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0},
                {-0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {-1, 0,-1, 0,-1, 0, 0, 0, 0, 0, 1, 1,10, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,-1, 0,-1, 0,-1},
                {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1},
                {-1, 0,-1, 0,-1, 0,-1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,-1, 0,-1, 0,-1, 0,-1},
                {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,12, 1,13, 1,15, 1,12, 1, 1, 1, 1, 1, 1, 1, 1, 1,12, 1,14, 1,11, 1,12, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1},
                {-1, 0,-1, 0,-1, 0,-1, 0,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1, 0,-1, 0,-1, 0,-1, 0,-1},
                {1, 0,-1, 0,-1, 0,-1, 0,-1, 0,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1, 0,-1, 0,-1, 0,-1, 0,-1, 0,-1}
            };
            int PosX = posX - 25; //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 31;
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
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
                                    if (confirmPlatforms == 0)
                                    {
                                        tile.HasTile = false;
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>(), true, true, -1, 0);
                                        tile.Slope = (SlopeType)0;
                                        tile.IsHalfBlock = false;
                                    }
                                    break;
                                case 3:
                                    if (confirmPlatforms == 0)
                                    {
                                        tile.HasTile = false;
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>(), true, true, -1, 0);
                                        tile.Slope = (SlopeType)1;
                                        tile.IsHalfBlock = false;
                                    }
                                    break;
                                case 4:
                                    if (confirmPlatforms == 0)
                                    {
                                        tile.HasTile = false;
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>(), true, true, -1, 0);
                                        tile.Slope = (SlopeType)2;
                                        tile.IsHalfBlock = false;
                                    }
                                    break;
                                case 5:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingBookcaseTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 6:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingTorchTile>(), true, true, -1, 0);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 7:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingBulbTile>(), true, true, -1, 0);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 8:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 21, true, true, -1, 5);
                                    }
                                    break;
                                case 9:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingStorageTile>(), true, true, -1, 1);
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
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingClockTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 12:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingLampTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 13:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 283, true, true, -1, 0);
                                    }
                                    break;
                                case 14:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingSofaTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 15:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingWorkBenchTile>(), true, true, -1, 0);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,0,0},
                {0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,3,2,2,2,2,2,2,2,2,2,2,2,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,2,2,2,2,2,2,2,2,2,2,2,3,1,0,0,0,0},
                {0,0,0,0,1,3,1,1,1,1,1,1,1,1,1,1,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,1,1,1,1,1,1,1,1,1,1,3,1,0,0,0,0},
                {0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0},
                {0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0},
                {0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0},
                {0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0},
                {0,1,1,1,1,3,1,0,0,0,0,0,0,0,0,0,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,0,0,0,0,0,0,0,0,0,1,3,1,1,1,1,0},
                {0,2,2,2,1,3,1,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,1,3,2,2,2,2,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
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
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingPanelWallWall>();
                                break;
                            case 2:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingWallWall>();
                                break;
                            case 3:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingBeamWall>();
                                break;
                        }
                    }
                }
            }
            GenerateStairs(posX, posY, 5);
        }
        public static void GeneratePortalBossRoom(int posX, int posY, int dir = 1)
        {
            int[,] _structure = {
                {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0},
                {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,14,14,14, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,14,14,14, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,14,14,14, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,14, 4,14, 1, 1},
                {3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 0, 0, 0, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 2, 2, 2, 2, 2, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 0, 2, 2, 2, 2, 2, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 0, 2, 2, 2, 2, 2, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1, 0, 2, 2, 2, 2, 2, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1,10, 0, 2, 2, 2, 2, 2, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1,10, 0, 0, 2, 2, 2, 2, 2, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1,10, 0, 0, 0, 2, 2, 2, 2, 2, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1,10, 0, 0, 6, 0, 2, 2, 2, 2, 2, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1,10, 0, 0, 6, 1, 0, 2, 2, 2, 2, 2, 0},
                {0, 0, 0, 0, 0, 0, 0, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 6, 1, 1,10, 0, 0, 6, 1, 1, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 6, 1, 1,10, 0, 0, 6, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 1, 1,10, 0, 0, 6, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 8, 0, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 0, 0, 6, 1, 1, 1, 1,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 8, 0, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 0, 0, 6, 1, 1, 1, 1, 1,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 9, 1, 1, 8, 0, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1,10, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 1, 1, 1, 1, 1, 1,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 0, 9, 1, 1, 8, 0, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1,10, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 1, 1, 1, 1, 1, 1,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 0, 0, 9, 1, 1, 8, 0, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 0, 6, 1, 1,10, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 1, 1, 1, 1, 1, 1, 1,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 8, 0, 0, 9, 1, 1, 0, 2, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 2, 0, 1, 1,10, 0, 0, 6, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1,10, 0, 0, 6, 1, 1, 1, 1, 1, 1, 1,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 8, 0, 0, 9, 1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 1,10, 0, 0, 6, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1,10, 0, 0, 6, 1, 1, 1, 1, 1, 1, 1, 1,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 8, 0, 0, 9, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0,10, 0, 0, 6, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1,10, 0, 0, 6, 1, 1, 1, 1, 1, 1, 1, 1,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 8, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 6, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1,10, 0, 0, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 8, 0, 0, 2, 0, 0,12,12,12,12,12, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,12,12,12,12,12, 0, 0, 2, 0, 0, 6, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1,10, 0, 0, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 8, 0, 2, 0,12,12, 1, 1, 1,12,12, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,12,12, 1, 1, 1,12,12, 0, 2, 0, 6, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0,10, 0, 0, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 0, 2, 0,12, 1, 1, 1, 1, 1,12, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,12, 1, 1, 1, 1, 1,12, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 0, 2, 0,12, 1, 1, 1, 1, 1,12, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,12, 1, 1, 1, 1, 1,12, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 0, 2, 0,12, 1, 1, 1, 1, 1,12, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,12, 1, 1, 1, 1, 1,12, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18, 1, 1, 1, 1, 1, 0, 2, 0,12,12, 1, 1, 1,12,12, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,12,12, 1, 1, 1,12,12, 0, 2, 0, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18, 1, 1, 1, 0, 2, 0, 0,12,12,12,12,12, 0, 0, 0,13, 1, 1, 1, 1, 1, 1, 1, 1,15,15,15,15,15,15,15,15,15,15,15, 1, 1, 1, 1, 1, 1, 1, 1,13, 0, 0, 0,12,12,12,12,12, 0, 0, 2, 0, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1,18, 0, 2, 2, 2, 2, 2, 0,18,18, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18, 1, 1, 0, 2, 2, 0, 0, 0,12, 0, 0, 0, 2, 0, 0, 0,13, 1, 1, 1, 1, 1, 1,15,15,15,15,15,11,15,15,15,15,15, 1, 1, 1, 1, 1, 1,13, 0, 0, 0, 2, 0, 0, 0,12, 0, 0, 0, 2, 2, 0,18,18, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1,18,18, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18,18, 1, 0, 2, 2, 2, 2, 2,12, 2, 2, 2, 2, 0, 0, 0, 0, 0,13, 1, 1, 1, 1,12,12,12,12,12,12,12,12,12,12,12, 1, 1, 1, 1,13, 0, 0, 0, 0, 0, 2, 2, 2, 2,12, 2, 2, 2, 2, 2, 0,18,18,18,18, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1,18,18,18,18, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18, 1,18, 0, 2, 2, 2, 2, 2, 0, 1,18,18,18,18,18, 1, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18, 0, 2, 2,12,12,12,12,12,12,12, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,12, 0,12, 0,12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,12,12,12,12,12,12,12, 2, 2, 0,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2,12, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,12, 0,12, 0,12, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,12, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12, 0,12, 0,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0}
            };
            int offset = dir == -1 ? _structure.GetLength(1) - 1 : 0;
            posX -= offset;
            int PosX = posX; 
            int PosY = posY - 35;
            int height = _structure.GetLength(0);
            GenerateRectangle(posX, posY - height/2 - 5, _structure.GetLength(1), height);
            for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
            {
                for (int i = _structure.GetLength(0) - 1; i >= 0; i--)
                {
                    for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
                    {
                        int k = PosX + j * dir + offset;
                        int l = PosY + i;
                        if (WorldGen.InWorld(k, l, 30))
                        {
                            Tile tile = Framing.GetTileSafely(k, l);
                            switch (_structure[i, j])
                            {
                                case 0:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 14:
                                    if (confirmPlatforms == 0)
                                    {
                                        tile.ClearTile();
                                    }
                                    break;
                                case 18:
                                    ushort type = TileID.GrayBrick;
                                    if (WorldGen.genRand.NextBool(2))
                                    {
                                        type = TileID.Stone;
                                    }
                                    else if (WorldGen.genRand.NextBool(3))
                                    {
                                        type = TileID.StoneSlab;
                                    }
                                    tile.HasTile = false;
                                    tile.WallType = WallID.Stone;
                                    WorldGen.PlaceTile(k, l, type, true, true, -1, 0);
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<GulaPlatingTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    if (confirmPlatforms == 0)
                                    {
                                        tile.ClearTile();
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingPlatformTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 4:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingBookcaseTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 5:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<GulaPlatingTile>();
                                    tile.Slope = dir == -1 ? (SlopeType)1 : (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 6:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                    tile.Slope = dir == -1 ? (SlopeType)4 : (SlopeType)3;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 7:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<GulaPlatingTile>();
                                    tile.Slope = dir == -1 ? (SlopeType)2 : (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 8:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                    tile.Slope = dir == -1 ? (SlopeType)3 : (SlopeType)4;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 9:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                    tile.Slope = dir == -1 ? (SlopeType)2 : (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 10:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                    tile.Slope = dir == -1 ? (SlopeType)1 : (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 11:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<GulaGatewayTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 12:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<GulaPortalPlatingTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 13:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 15:
                                    if (confirmPlatforms == 0)
                                    {
                                        tile.ClearTile();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            int w = _structure.GetLength(1);
            int h = _structure.GetLength(0);
            SOTSWorldgenHelper.SmoothRegion(PosX + w / 2, PosY + h / 2, w, h, TileID.Stone);
            SOTSWorldgenHelper.SmoothRegion(PosX + w / 2, PosY + h / 2, w, h, TileID.StoneSlab);
            SOTSWorldgenHelper.SmoothRegion(PosX + w / 2, PosY + h / 2, w, h, TileID.GrayBrick);
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,1,2,2,2,2,2,2,2,1,2,2,2,2,2,1,2,2,2,2,2,2,2,1,2,2,2,2,2,1,2,2,2,2,2,2,2,1,2,2,2,2,2,1,2,2,2,2,2,2,2,1,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,1},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,0,0,0,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,0,0,0,0,0,0,0,0,0,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,0,0,0,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1},
                {1,3,0,0,0,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,0,0,0,0,0,0,0,0,0,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,1,3,0,0,0,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,1,3,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,2,2,2,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,3,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,3,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,3,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,3,4,4,4,4,4,4,4,4,4,1,3,2,2,2,2,2,2,2,2,2,3,1,4,4,4,4,4,4,4,4,4,3,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,3,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,3,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,4,1,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,4,4,4,4,4,4,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,4,4,4,4,4,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,4,4,4,2,4,4,4,4,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,4,4,4,4,2,4,4,4,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,4,2,4,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,2,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,2,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4}
            };
            for (int i = 0; i < _structure.GetLength(0); i++)
            {
                for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
                {
                    int k = PosX + j * dir + offset;
                    int l = PosY + i;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (_structure[i, j])
                        {
                            case 0:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingWallWall>();
                                break;
                            case 1:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingPanelWallWall>();
                                break;
                            case 2:
                                tile.WallType = (ushort)ModContent.WallType<GulaPlatingWallWall>();
                                break;
                            case 3:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingBeamWall>();
                                break;
                        }
                    }
                }
            }
        }
        public static void GenerateRectangle(int posX, int posY, int width, int height)
        {
            int padding = 3;
            int x2 = posX - 1 + padding;
            int y2 = posY + height / 2;
            int x3 = x2;
            height /= 2;
            GenerateTunnel(ref x2, ref y2, -90, height, width + 1 - padding * 2, false);
            x2 -= padding;
            y2 -= height;
            int y3 = y2;
            GenerateTunnel(ref x2, ref y2, 0, 5, height * 2 - 1, false);
            GenerateCaveCircle(x2 - 2, y2, 1f, .4f, 13, 0);
            GenerateCaveCircle(x2 - 2, y2 - height * 2, 1f, .4f, 13, 0);
            GenerateTunnel(ref x3, ref y3, 0, 5, height * 2 - 1, false);
            GenerateCaveCircle(x3 + 4, y3, 1f, .4f, 13, 0);
            GenerateCaveCircle(x3 + 4, y3 - height * 2, 1f, .4f, 13, 0);
        }
        public static void GenerateVaultRoom(int posX, int posY, int dir = 1)
        {
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,7,7,7,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,0,0,0,0,0,0,0,0,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,7,7,7,7,7,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,7,7,0,2,2,2,2,2,2,0,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,7,7,7,7,7,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,7,7,0,2,2,2,2,2,2,0,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,2,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,7,7,7,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,0,0,0,0,0,0,0,0,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0,0,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,2,2,2,2,2,2,2,0,1,1,1,1,0,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,2,2,2,2,2,2,2,2,0,1,1,1,1,0,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {1,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,4,0,2,2,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,2,2,0,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {1,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,2,2,0,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,6,7,0,0,0,0,0,0,0,0,7,7,7,7,0,0,0,0,0,0,0,0,7,7,7,0,2,0},
                {0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,6,7,7,0,2,2,2,2,2,2,0,7,7,7,7,0,2,2,2,2,2,2,0,7,7,7,0,2,0},
                {0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,6,7,7,7,0,2,2,2,2,2,2,0,7,7,7,7,0,2,2,2,2,2,2,0,7,7,7,0,2,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,7,7,7,7,0,0,0,0,0,0,0,0,7,7,7,7,0,0,0,0,0,0,0,0,7,7,7,0,0,0}
            };
            int offset = dir == -1 ? _structure.GetLength(1) - 1 : 0;
            posX -= offset;
            int height = _structure.GetLength(0);
            int PosX = posX - 0;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 18;
            GenerateRectangle(PosX + 1, PosY, _structure.GetLength(1), height);
            for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
            {
                for (int i = 0; i < _structure.GetLength(0); i++)
                {
                    for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
                    {
                        int k = PosX + j * dir + offset;
                        int l = PosY + i;
                        if (WorldGen.InWorld(k, l, 30))
                        {
                            Tile tile = Framing.GetTileSafely(k, l);
                            switch (_structure[i, j])
                            {
                                case 0:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<GulaPlatingTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    if (confirmPlatforms == 0)
                                        tile.ClearTile();
                                    else
                                    {
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                    tile.Slope = dir == -1 ? (SlopeType)1 : (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingBlastDoorTileClosed>(), true, true, -1, 0);
                                    }
                                    break;
                                case 6:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingTile>();
                                    tile.Slope = dir == -1 ? (SlopeType)4 : (SlopeType)3;
                                    tile.IsHalfBlock = false;
                                    break;
                            }
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,1,2,2,2,2,2,2,1,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,1},
                {1,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,1,2,2,2,2,2,2,1,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,1},
                {0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
                {3,2,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,2,3},
                {3,2,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,2,3},
                {3,2,3,3,3,3,3,4,0,0,0,0,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,0,0,0,0,0,0,0,0,0,0,4,3,3,3,3,3,3,3,3,4,0,0,0,0,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,2,3},
                {3,2,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,2,3},
                {3,2,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,2,3},
                {3,2,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,0,0,0,0,0,0,0,0,0,0,4,3,3,3,3,3,3,3,3,4,0,0,0,0,4,3,3,3,3,3,3,4,0,0,0,0,4,3,3,3,3,3,2,3},
                {3,2,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,2,3},
                {3,2,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,2,3},
                {3,2,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,0,0,0,0,4,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3},
                {3,2,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,3,3,3,2,2,2,2,2,2,2,1,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,3},
                {4,3,4,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,3,3,2,2,2,2,2,2,2,2,1,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,3},
                {4,2,4,3,3,3,3,4,0,0,0,0,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,4,0,0,0,0,0,0,0,0,0,0,4,3,3,3,3,3,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3},
                {4,2,4,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,3,2,2,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,2,3},
                {4,2,4,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,4,2,2,2,2,2,2,2,2,2,2,4,3,3,3,2,2,3,3,3,4,2,2,2,2,4,3,3,3,3,3,3,4,2,2,2,2,4,3,3,3,3,3,2,3},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
                {1,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,1,2,2,2,2,1,2,2,2,2,2,2,1,2,2,2,1,2,1},
                {1,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,1,2,2,2,2,2,2,1,2,2,2,2,1,2,2,2,2,2,2,1,2,2,2,1,2,1},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
            for (int i = 0; i < _structure.GetLength(0); i++)
            {
                for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
                {
                    int k = PosX + j * dir + offset;
                    int l = PosY + i;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (_structure[i, j])
                        {
                            case 0:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingWallWall>();
                                break;
                            case 1:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingPanelWallWall>();
                                break;
                            case 2:
                                tile.WallType = (ushort)ModContent.WallType<GulaPlatingWallWall>();
                                break;
                            case 4:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingBeamWall>();
                                break;
                        }
                    }
                }
            }
        }
    }
}