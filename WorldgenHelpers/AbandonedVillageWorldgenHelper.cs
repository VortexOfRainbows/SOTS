using Terraria.ID;
using System.Diagnostics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Items.Otherworld;
using SOTS.Items.Pyramid;
using SOTS.Items.ChestItems;
using System;
using SOTS.Items;
using SOTS.Items.Pyramid;
using SOTS.Items.Pyramid.PyramidWalls;
using SOTS.Items.Furniture.AncientGold;
using SOTS.Items.Tide;
using SOTS.Items.Permafrost;
using SOTS.Items.Secrets;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Items.Otherworld.Blocks;
using SOTS.Items.Earth;
using SOTS.Items.Nvidia;
using SOTS.Items.Chaos;
using SOTS.Items.Furniture.Earthen;
using SOTS.Items.Fragments;
using Microsoft.Xna.Framework;
using System.Linq;

namespace SOTS.WorldgenHelpers
{
	public class AbandonedVillageWorldgenHelper
	{
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
								tile.TileType = TileID.Dirt;
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
		public static void GenerateMineEntrance(int xPos, int yPos)
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
				{3,3,3,3,3,3,3,3,3,5,3,5,5,5,5,5,5,3,2,2,2,2,3,3,2,1,1,1,2,2,2,2,2,2,2,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,2,2,2,2,2,2,2,2,4,4,3,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,7,7,7,2,2,2,2,4,4,4,1,1,3,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,7,7,7,7,4,4,1,1,3,3,3,3,3,3},
				{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,7,7,7,7,7,7,7,3,3,3,3,3,3,3,3}
			};
			int PosX = xPos - _structure.GetLength(1) / 2;  //spawnX and spawnY is where you want the anchor to be when this generates
			int PosY = yPos - 3;
			GenHalfCircle(xPos, yPos - 2, 0, _structure.GetLength(1) / 2, 15);
			GenHalfCircle(xPos, yPos + 3, 1, _structure.GetLength(1) / 2, 30);

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
				{11,11,11,11,11,11,11,9,9,9,9,9,9,2,2,3,2,2,2,3,3,3,3,2,4,0,0,0,0,0,0,0,0,2,2,0},
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
								/*case 15: //Signs do not generate properly
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.TatteredWoodSign, true, true, -1, 0);
									}
									break;*/
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
		}
		public static void PlaceAbandonedVillage()
		{
			int center = Main.maxTilesX / 2;
			int leftTiles = 0;
			int rightTiles = 0;
			int foundEvilBiomeLeft = 0;
			int foundEvilBiomeRight = 0;
			Point rightSide = new Point();
			Point leftSide = new Point();
			int[] ValidEvilTiles = new int[]
			{
				TileID.CrimsonGrass,
				TileID.CorruptGrass
				//TileID.Ebonstone,
				//TileID.Crimstone
			};
			int[] InvalidTiles = new int[]
			{
				TileID.Cloud,
				TileID.RainCloud,
				TileID.Trees
			};
			for (int x = center; x < Main.maxTilesX - 200; x++)
			{
				rightTiles++;
				for (int y = 100; y < Main.worldSurface; y++)
				{
					Tile tile = Main.tile[x, y];
					if (tile.HasTile && Main.tileSolid[tile.TileType] && !InvalidTiles.Contains(tile.TileType))
					{
						if (ValidEvilTiles.Contains(tile.TileType) && IsLineSolid(x, y))
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
						if (ValidEvilTiles.Contains(tile.TileType) && IsLineSolid(x, y))
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
			int[] ValidEvilTiles = new int[]
			{
				TileID.CrimsonGrass,
				TileID.CorruptGrass,
				TileID.Ebonstone,
				TileID.Crimstone
			};
			int[] InvalidTiles = new int[]
			{
				TileID.Cloud,
				TileID.RainCloud,
				TileID.Trees
			};
			bool generating = true;
			int tileFoundCounter = 0;
			int totalCounter = 0;
			while(generating)
			{
				X += direction;
				for (int y = 100; y < Main.worldSurface; y++)
				{
					Tile tile = Main.tile[X, y];
					if (tile.HasTile && Main.tileSolid[tile.TileType] && !InvalidTiles.Contains(tile.TileType))
					{
						if (ValidEvilTiles.Contains(tile.TileType) && IsLineSolid(X, y))
						{
							tileFoundCounter++;
							if(tileFoundCounter == 40) //40 solid tiles from well
                            {
								GenerateMineEntrance(X, y);
								generating = false;
                            }
						}
						break;
					}
				}
				totalCounter++;
				if(totalCounter > 1000)
                {
					generating = false;
                }
            }
        }
	}
}