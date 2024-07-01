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
using SOTS.Items.Conduit;
using Terraria.GameContent;

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
		public static void GenerateDownwardEntrance(ref int x, ref int y, float rotation)
		{
            float nextPlatform = 10;
			int height = 30;
			float bonusDegreesLeft = Main.rand.NextFloat(360);
            float bonusDegreesRight = Main.rand.NextFloat(360);
            HashSet<Point> platformPoints = new HashSet<Point>();
            for (float j = 0; j < height; j += 0.25f)
            {
                bool generatePlatforms = false;
				nextPlatform -= 0.25f * Math.Abs((float)Math.Cos(MathHelper.ToRadians(rotation)));
                if (nextPlatform <= 0)
                {
                    generatePlatforms = true;
                    nextPlatform = WorldGen.genRand.Next(8, 15);
                }
                int left = -7 - Math.Abs((int)(2.5f * Math.Sin(MathHelper.ToRadians(bonusDegreesLeft))));
				int right = 7 + Math.Abs((int)(2.5f * Math.Sin(MathHelper.ToRadians(bonusDegreesRight))));
                int sootLeft = -8 - Math.Abs((int)(2.5f * Math.Sin(MathHelper.ToRadians(bonusDegreesRight))));
                int sootRight = 8 + Math.Abs((int)(2.5f * Math.Sin(MathHelper.ToRadians(bonusDegreesLeft))));
				for(int RunType = 0; RunType <= 2; RunType++)
                {
                    for (float i = left - 4; i <= right + 4; i += 0.25f)
                    {
                        Vector2 vPoint = new Vector2(i + 0.5f, j + 0.5f).RotatedBy(MathHelper.ToRadians(rotation));
                        Point rPoint = new Point(x + (int)(vPoint.X), y + (int)(vPoint.Y));
                        Tile tile = Framing.GetTileSafely(rPoint);
                        bool interior = false;
                        bool generateSoot = Math.Abs(i - sootLeft) < 1.5f || Math.Abs(i - sootRight) < 1.5f;
						bool generateSides = i >= left && i <= right && (Math.Abs(i - left) < 3.5f || Math.Abs(i - right) < 3.5f);
                        if ((i >= left && i <= right) || generateSoot)
                        {
                            interior = true;
                            if (RunType == 0)
                            {
                                Tile tileabove = Framing.GetTileSafely(rPoint.X, rPoint.Y - 1);
                                if (tile.WallType != WallID.RocksUnsafe1 && tile.WallType != WallID.StoneSlab && tile.WallType != WallID.GrayBrick && 
                                    tileabove.TileType != TileID.Containers && tileabove.TileType != TileID.Containers2 && 
                                    tileabove.TileType != TileID.FakeContainers && tileabove.TileType != TileID.FakeContainers2)
                                {
                                    tile.HasTile = false;
                                    tile.LiquidAmount = 0;
                                }
                            }
                        }
						if (RunType == 1)
                        {
							if((tile.WallType != WallID.RocksUnsafe1 && tile.WallType != WallID.StoneSlab && tile.WallType != WallID.GrayBrick)
								|| tile.TileType == ModContent.TileType<SootBlockTile>())
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
                            if (i == 0)
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
			foreach (Point p in platformPoints)
            {
                for (int i2 = -8; i2 <= 8; i2++)
                {
					Tile tile = Main.tile[p.X + i2, p.Y];
					if(tile.HasTile && tile.TileType == TileID.Rope)
					{
						tile.HasTile = false;
                        tile.LiquidAmount = 0;
                    }
                    WorldGen.PlaceTile(p.X + i2, p.Y, TileID.Platforms, false, false, -1, 43);
                }
            }
            SOTSWorldgenHelper.SmoothRegion(x, y + height / 2, 24, height);
            Vector2 vPointA = new Vector2(0, height + 1).RotatedBy(MathHelper.ToRadians(rotation));
			x = (int)(x + vPointA.X + 0.5f);
            y = (int)(y + vPointA.Y + 0.5f);
        }
		public static void GenerateDownwardPathCircle(int x, int y)
		{
			int size = 11;
			float wallSize = 6.5f;
			for(int i = -size; i < size; i++)
			{
				for(int j = -size; j < size; j++)
				{
					float radius = MathF.Sqrt(i * i + j * j);
                    Tile t = Framing.GetTileSafely(x + i, y + j);
                    if (radius <= wallSize + WorldGen.genRand.NextFloat(-0.5f, 0.5f))
                    {
						//WallID.stone and soot walls are used for the border of the generation
						if(t.WallType != WallID.Stone && t.WallType != ModContent.WallType<SootWallTile>())
                        {
                            if (t.HasTile) //)t.WallType != WallID.StoneSlab && t.WallType != WallID.GrayBrick && t.WallType != WallID.RocksUnsafe1)
                            {
                                Tile tileabove = Framing.GetTileSafely(x + i, y + j - 1);
                                if(tileabove.TileType != TileID.Containers && tileabove.TileType != TileID.Containers2 &&
                                    tileabove.TileType != TileID.FakeContainers && tileabove.TileType != TileID.FakeContainers2)
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
					else if(radius <= size + WorldGen.genRand.NextFloat(-0.5f, 0.8f))
					{
						bool generateStone = radius <= wallSize + 2 * WorldGen.genRand.NextFloat(0.7f, 1.4f);
						bool open = t.WallType == WallID.RocksUnsafe1 || t.WallType == WallID.GrayBrick || t.WallType == WallID.StoneSlab;
						bool isStone = t.TileType == TileID.GrayBrick || t.TileType == TileID.Stone || t.TileType == TileID.StoneSlab;
                        if (!generateStone && !open && !isStone)
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
				if(i != 0)
					GenerateDownwardPathCircle(x, y);
                GenerateDownwardEntrance(ref x, ref y, rotation);
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
                {11 ,0,18,12 ,0 ,0 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0, 0,20,20,20,21,21 ,0 ,0,13,13,13,13,22 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0,23,13,13,13,13 ,0 ,0,21,21,20,20,20, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0,12,17,11},
                {11 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0, 0,19,20,20,20 ,0 ,0 ,0,25,16,16,16,16 ,0,22 ,0 ,0 ,0 ,9 ,0 ,0 ,0,23 ,0,16,16,16,16 ,0 ,0 ,0 ,0,20,20,20,24, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12 ,0,11},
                {25 ,0 ,0,12 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0, 0,19 ,0,20,20,20 ,0 ,0,25,25,13,13,13,13,14,14,14,14,14,14,14,14,14,14,14,13,13,13,13,25 ,0 ,0 ,0,20,20,20 ,0,24, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,12 ,0,11},
                {25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,21,21,21,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,34,15,15,15,15,15,15,15,25,25,25,25 ,0 ,0,20,20,20,21,21,21 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,25},
                {25,25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,25,25},
                {25,25,25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20 ,0 ,0 ,0 ,0 ,0 ,0 ,0,26,26,26 ,0 ,0 ,0 ,0 ,0 ,0 ,0,25,25},
                {25,25,25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,27 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20 ,0 ,0 ,0 ,0 ,0 ,0 ,0,26,26,26 ,0 ,0 ,0 ,0 ,0 ,0,25,25,25},
                {25,25,25,25 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20 ,0 ,0 ,0 ,0 ,0 ,0 ,0,26,26,26 ,0 ,0 ,0 ,0 ,0 ,0,25,25,25},
                {25,25,25,25,25,25 ,0 ,0,28 ,0 ,0 ,0,29 ,0 ,0,30 ,0,31 ,0 ,0,20,20,20,25,25,25,25,25,25,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,25,25,25,25,25,25,20,20,20 ,0,32 ,0 ,0 ,0 ,0 ,0 ,0,33 ,0 ,0 ,0 ,0 ,0,25,25,25,25,25},
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
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingPlatformTile>();
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
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingPlatformTile>();
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EarthenPlatingPlatformTile>();
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
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingBedTile>(), true, true, -1, 0);
                                        tile.TileFrameX += 72;
                                    }
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
	}
}