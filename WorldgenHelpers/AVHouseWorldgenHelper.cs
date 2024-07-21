using SOTS.Items.AbandonedVillage;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SOTS.WorldgenHelpers
{
    public static class AVHouseWorldgenHelper
    {
        public static void GenerateHouse1(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX, posY, 1, 10, 10);
            int[,] _structure = {
                {0,0,0,0,0,1,1,1,0,0,0,0},
                {0,0,0,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,2,2,2,2,1,1,1,0,0},
                {0,0,0,2,2,2,2,1,1,1,0,0},
                {0,0,0,2,2,2,2,1,1,1,1,0},
                {0,0,0,2,2,2,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1}
            };
            int PosX = posX - 6;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 8;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                            case 2:
                                tile.WallType = (ushort)ModContent.WallType<CharredWoodWallTile>();
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,1,1,2,3,0,0,0},
                {0,0,4,5,5,5,5,2,2,3,0,0},
                {0,6,2,2,2,2,2,2,2,2,3,0},
                {2,2,2,2,2,2,2,2,2,2,2,2},
                {0,0,2,0,0,0,0,0,0,2,0,0},
                {0,0,0,0,0,0,0,0,0,2,0,0},
                {0,0,2,0,0,0,0,0,0,2,0,0},
                {0,0,2,0,7,0,0,0,5,2,5,0},
                {0,2,2,2,2,2,2,5,5,5,5,5}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 6:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 7:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 21, true, true, -1, 5);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateHouse2(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX, posY, 1, 20, 10);
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,2,2,0,0,1,1,1,1,0,2,2,0,0,0,0,0,0,0},
                {0,0,0,0,0,2,2,2,1,1,1,1,1,2,2,2,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,2,0,0,0,1,0,0,0,0,0,1,0,0,0,0},
                {0,0,0,0,0,1,1,1,0,1,1,1,1,0,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,0,1,1,1,0,1,1,1,1,0,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,0,0,1,1,0,1,1,1,1,0,1,1,1,1,1,0,0,0,0},
                {0,0,0,2,0,0,1,1,0,1,1,1,1,0,1,1,1,1,1,0,0,0,0},
                {0,0,0,2,2,1,1,1,0,1,1,1,1,0,1,1,1,1,1,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,0,0,1,1,1,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,2,2,2,2,0,0,0,0,2,2,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,2,0,2,2,2,0,0,2,2,2,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,1,1,1,1,1,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,1,1,1,1,1,0,1,1,0,0,1,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,1,1,1,1,0,2,2,1,1,1,1,1,0,0,0,0,0,0,0},
                {0,0,0,0,1,1,1,1,0,2,2,2,1,1,1,1,1,0,0,0,0,0,0},
                {0,0,2,2,2,2,1,1,0,2,2,2,2,1,1,1,1,1,1,0,2,0,0},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
            };
            int PosX = posX - 11;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 24;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
                {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
                {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
                {0,0,0,2,1,1,0,0,0,0,0,0,0,0,0,0,1,1,3,0,0,0,0},
                {0,0,2,1,1,4,4,0,0,0,0,0,0,0,4,4,1,1,1,3,0,0,0},
                {0,2,1,1,1,4,4,4,0,0,5,0,4,4,4,4,1,1,1,1,3,0,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                {0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                {0,0,4,4,0,0,0,0,1,0,6,0,0,1,0,0,0,0,0,1,0,0,0},
                {0,0,1,4,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                {0,0,1,4,4,0,0,0,1,0,0,0,0,1,7,0,0,0,0,1,0,0,0},
                {0,0,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,0,0,0},
                {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
                {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
                {0,0,0,2,1,1,0,0,0,0,0,0,0,0,0,8,1,1,3,0,0,0,0},
                {0,0,2,1,1,1,4,4,9,0,0,0,0,0,4,4,1,1,1,3,0,0,0},
                {0,2,1,1,1,1,4,4,4,4,4,9,0,4,4,4,1,1,1,1,3,0,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                {0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                {0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,1,4,4,0,0,1,0,0,0,0,0,0,0,0,0},
                {0,0,9,4,4,9,0,0,1,4,4,4,0,1,0,0,0,0,0,1,4,9,0},
                {8,4,4,4,4,4,4,1,1,1,1,1,1,1,1,1,1,1,1,1,4,4,4}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 6:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 245, true, true, -1, 20);
                                    }
                                    break;
                                case 7:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 94, true, true, -1, 0);
                                    }
                                    break;
                                case 8:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 9:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateHouse3(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX, posY, 1, 15, 10);
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,0,1,1,1,1,0,0,0},
                {0,0,0,0,1,1,1,1,1,1,0,1,1,1,0,0,0,0},
                {0,0,0,2,1,1,1,1,1,1,0,1,1,0,0,0,0,0},
                {0,0,2,2,2,1,1,1,1,1,0,1,1,1,2,2,0,0},
                {0,2,2,2,2,2,1,1,1,1,0,1,1,2,2,2,2,0},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
            };
            int PosX = posX - 9;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 8;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
                {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,0,2,2,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,2,2,2,2,2,2,0,0,0,1,0,0,0,3,2,2,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateHouse4(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX, posY, 1, 15, 10);
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0},
                {0,0,0,0,0,0,0,0,0,2,2,2,2,2,0,0,0},
                {0,0,0,0,0,0,0,0,0,2,2,0,0,2,0,0,0},
                {0,0,0,0,0,0,0,0,0,2,0,0,0,2,1,0,0},
                {0,0,0,0,0,0,0,0,0,2,0,0,2,2,1,0,0},
                {0,0,0,0,0,0,0,0,0,2,2,2,2,1,1,0,0},
                {0,1,1,1,0,0,0,0,0,2,2,2,1,1,1,1,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };
            int PosX = posX - 8;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 12;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                            case 2:
                                tile.WallType = (ushort)ModContent.WallType<CharredWoodWallTile>();
                                break;
                        }
                    }
                }
            } 
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,0,0},
                {0,0,0,0,0,0,0,0,1,0,0,2,2,2,1,0,0},
                {0,0,0,0,0,0,0,0,1,0,2,2,2,2,1,0,0},
                {0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1},
                {0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0},
                {0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0},
                {0,0,0,0,0,0,0,0,1,0,0,0,0,0,2,0,0},
                {0,0,0,0,0,0,0,0,1,0,0,0,0,0,2,0,0},
                {0,0,0,0,0,0,0,0,1,0,0,0,0,0,2,0,0},
                {0,2,2,3,0,0,0,0,1,0,0,0,2,2,2,2,0},
                {2,2,2,1,1,1,1,1,1,1,1,1,1,2,2,2,2}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateHouse5(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX, posY, 1, 15, 10);
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,1,1,0,1,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,1,1,1,0,0,1,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,1,1,1,0,1,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,2,1,1,1,1,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,2,2,1,1,2,2,2,2,0,0,0,0,0},
                {0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0},
                {0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0},
                {0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0},
                {0,0,0,0,0,0,2,1,1,1,1,2,2,2,0,0,0,0,0},
                {0,0,0,0,0,0,0,1,1,1,1,0,2,0,0,0,0,0,0},
                {0,0,0,2,2,2,2,1,1,0,0,0,2,0,2,2,0,0,0},
                {0,0,0,1,1,2,2,0,0,0,0,0,2,0,2,2,0,0,0},
                {0,0,0,0,1,2,2,0,0,0,0,0,2,0,2,2,0,0,0},
                {0,0,0,0,1,2,2,0,0,0,0,0,2,0,2,2,2,0,0},
                {0,0,0,1,1,2,2,0,0,1,0,0,2,0,2,1,1,0,0},
                {0,0,1,0,2,2,2,0,1,1,1,0,2,0,1,1,1,1,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };
            int PosX = posX - 9;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 17;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                            case 2:
                                tile.WallType = (ushort)ModContent.WallType<CharredWoodWallTile>();
                                break;
                        }
                    }
                }
            }
            _structure = new int[,]  {
                {0,0,0,0,0,0,0,0,0,1,2,2,3,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,2,2,3,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,1,2,2,2,3,0,0,0,0},
                {0,0,0,4,2,1,1,0,0,1,1,1,2,2,2,2,3,0,0},
                {0,0,0,2,2,2,1,0,0,0,2,2,2,2,2,2,2,0,0},
                {0,0,0,0,0,2,0,0,0,0,0,0,0,0,2,0,0,0,0},
                {0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,2,0,0,0,0,0,0,0,0,2,0,0,0,0},
                {0,0,0,0,0,2,0,5,0,0,0,0,0,0,2,0,0,0,0},
                {0,0,2,2,2,2,2,2,1,1,1,1,2,2,2,2,2,2,0},
                {0,0,0,0,0,0,2,0,0,0,0,0,0,2,0,0,0,0,0},
                {0,0,0,0,0,0,2,0,0,0,0,0,0,2,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0},
                {0,0,6,6,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0},
                {0,1,1,1,1,0,0,0,0,0,0,0,0,2,0,0,1,1,0},
                {1,1,1,1,1,2,2,2,1,1,1,2,2,2,2,1,1,1,1}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 6:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateHouse6(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX, posY, 1, 10, 10);
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,2,2,2,1,1,0,0,0},
                {0,0,0,0,0,0,0,0,1,2,2,1,1,0,0,0},
                {0,0,0,0,0,2,2,2,1,1,1,1,1,0,0,0},
                {0,0,1,2,2,2,2,2,1,1,1,1,1,0,0,0},
                {0,0,0,0,2,2,2,2,1,1,1,1,1,0,0,0},
                {0,0,0,0,2,2,2,2,0,0,1,1,0,0,0,0},
                {0,0,0,0,2,2,1,1,0,1,1,1,1,0,0,0},
                {0,0,0,2,2,1,1,1,0,1,1,1,1,0,0,0},
                {0,0,2,2,2,1,1,1,0,1,1,1,1,0,0,0},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
            };
            int PosX = posX - 7;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 12;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                        }
                    }
                }
            }
            _structure = new int[,]{
                {0,0,0,0,0,0,0,0,0,1,2,2,2,3,0,0},
                {0,0,0,0,0,0,0,0,0,4,2,2,2,2,3,0},
                {0,0,0,0,0,0,0,0,4,4,2,2,2,2,2,2},
                {0,0,0,0,0,0,0,4,4,0,0,0,0,2,0,0},
                {0,0,0,0,5,4,4,4,2,0,0,0,0,2,0,0},
                {0,0,4,4,4,4,4,0,2,0,0,0,0,0,0,0},
                {2,2,2,2,2,4,0,0,2,0,0,0,0,0,0,0},
                {0,0,2,0,0,0,0,0,2,0,0,0,0,2,0,0},
                {0,0,2,0,0,0,0,0,2,2,2,2,2,2,0,0},
                {0,0,2,0,0,0,0,0,2,0,0,0,0,2,0,0},
                {0,0,2,0,0,0,0,0,2,0,0,0,0,2,0,0},
                {0,0,2,4,4,0,0,0,2,0,0,0,0,2,0,0},
                {0,4,4,4,4,4,4,2,2,2,2,2,2,2,0,0}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateHouse7(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX, posY, 1, 14, 10);
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
                {0,0,2,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
                {0,0,2,1,1,1,1,1,2,2,0,0,0,0,0,0,0,0},
                {0,0,2,1,1,1,1,2,2,2,0,0,0,0,0,0,0,0},
                {0,0,0,1,1,1,2,2,2,2,1,1,1,1,1,0,0,0},
                {0,0,0,1,1,1,2,1,0,0,1,1,1,1,0,0,0,0},
                {0,0,0,0,1,1,1,0,0,0,1,1,0,0,0,0,0,0},
                {0,0,0,0,1,1,1,0,0,0,0,1,0,0,0,0,0,0},
                {0,0,0,1,1,2,1,1,0,0,0,1,1,2,2,2,0,0},
                {0,0,0,1,2,2,2,1,0,0,1,1,2,2,2,2,0,0},
                {0,0,0,2,2,2,2,1,1,0,2,2,2,2,2,0,0,0},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
            };
            int PosX = posX - 8;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 14;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,1,2,2,3,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0,0},
                {2,2,2,2,2,2,2,2,2,2,4,0,0,0,0,0,0,0},
                {0,0,2,0,0,0,0,0,2,2,2,2,4,0,0,0,0,0},
                {0,0,5,0,0,0,0,0,2,2,2,2,2,2,4,0,0,0},
                {0,0,5,0,0,0,0,0,5,2,2,2,2,2,2,2,2,0},
                {0,0,5,0,0,0,0,0,5,5,2,2,2,2,2,2,0,0},
                {0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0},
                {0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0},
                {0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,5,0},
                {0,0,6,5,5,6,6,0,0,0,0,6,6,5,5,5,5,0},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 5:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 6:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateHouse8(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX, posY, 1, 10, 10);
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,0,0,0,2,1,1,1,0,0,0,0,0},
                {0,0,0,0,2,2,1,1,1,0,0,0,0},
                {0,0,0,0,2,2,1,1,1,1,0,0,0},
                {0,0,0,0,2,1,1,1,1,1,0,0,0},
                {0,0,0,0,2,1,1,1,1,1,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,2,2,2,2,2,2,2,2,0,0},
                {0,0,0,2,2,2,2,2,2,2,2,0,0},
                {0,0,0,2,2,2,2,2,2,2,2,0,0},
                {0,0,0,2,2,2,2,2,2,2,2,0,0},
                {0,0,0,2,2,2,2,2,2,2,1,0,0},
                {0,0,0,2,2,2,2,2,2,1,1,1,0},
                {0,0,1,1,1,1,1,1,1,1,1,1,1}
            };
            int PosX = posX - 7;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 17;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                            case 2:
                                tile.WallType = (ushort)ModContent.WallType<CharredWoodWallTile>();
                                break;
                        }
                    }
                }
            }
           _structure = new int[,] {
                {0,0,0,0,1,1,0,0,0,0,0,0,0},
                {0,0,2,2,2,1,1,0,0,0,0,0,0},
                {0,0,0,0,2,1,1,1,0,0,0,0,0},
                {0,0,0,0,2,1,1,1,0,0,0,0,0},
                {0,2,2,2,2,1,1,0,0,0,0,0,0},
                {0,0,0,2,0,1,1,0,0,0,2,0,0},
                {0,0,0,2,0,0,1,0,0,0,2,0,0},
                {0,0,0,2,0,0,0,0,1,1,2,0,0},
                {0,0,0,2,0,3,1,1,1,1,2,0,0},
                {0,0,0,2,1,1,1,1,1,1,2,0,0},
                {2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,2,0,0,0,0,0,0,0,0,2,0},
                {0,0,2,0,0,0,0,0,4,0,0,2,0},
                {0,0,2,0,0,0,0,0,0,0,0,2,0},
                {0,0,2,0,0,0,0,0,0,0,0,0,0},
                {0,0,2,0,0,0,0,0,0,0,0,0,0},
                {0,0,2,0,5,0,0,0,0,0,0,0,0},
                {0,0,2,2,2,2,2,2,2,1,1,1,1}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 4:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 240, true, true, -1, 43);
                                    }
                                    break;
                                case 5:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 21, true, true, -1, 5);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateHouse9(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX + 1, posY, 1, 16, 10);
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,0,0,0},
                {0,0,0,1,1,1,1,1,0,0,0,1,1,1,1,1,0,0,1,1,0,0,0},
                {0,0,0,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0},
                {0,0,0,1,1,1,2,2,2,0,0,1,1,1,2,2,1,1,1,1,0,0,0},
                {0,0,0,1,1,2,2,2,2,2,1,1,1,2,2,2,2,2,1,1,0,0,0},
                {0,0,0,1,1,1,1,1,0,1,1,1,1,1,1,1,2,2,1,1,0,0,0},
                {0,0,0,1,1,1,1,1,0,1,1,1,1,1,2,2,2,2,2,1,1,0,0},
                {0,0,0,1,1,1,1,1,0,1,1,1,0,0,0,0,2,2,1,1,1,0,0},
                {0,0,0,2,2,1,1,1,0,1,1,1,0,0,0,0,0,0,1,1,1,0,0},
                {0,0,0,1,2,2,1,1,0,1,1,1,1,0,0,0,0,1,1,1,1,0,0},
                {0,0,0,1,1,2,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,0,0},
                {0,0,0,1,1,0,0,0,1,1,1,1,1,1,0,1,1,1,1,1,1,0,0},
                {0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,0,0},
                {0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,0,0},
                {0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,1,1,1,1,0,2,0,0},
                {0,0,0,0,2,0,0,0,1,1,1,1,1,1,0,1,1,1,2,0,2,0,0},
                {0,0,0,2,2,2,0,0,1,1,1,1,1,1,0,1,1,2,2,2,2,0,2},
                {0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
            };
            int PosX = posX - 11;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 18;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0},
                {1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1},
                {1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1},
                {1,1,0,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1},
                {1,1,0,1,1,1,1,3,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1},
                {1,1,0,1,1,1,3,3,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1},
                {0,0,0,0,1,1,0,0,0,0,0,0,0,3,3,3,3,3,0,0,0,0,0},
                {1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1},
                {1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,0,4,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,0,3,4,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1},
                {1,1,0,3,3,3,5,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1},
                {1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,6,1,1,0,1,1,7,1,3,0,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,3,0,3,1},
                {1,1,1,1,1,8,1,1,0,1,1,1,1,1,0,1,1,1,3,3,3,3,1},
                {1,8,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
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
                                        WorldGen.PlaceTile(k, l, 245, true, true, -1, 2);
                                    }
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 6:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 240, true, true, -1, 41);
                                    }
                                    break;
                                case 7:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, 240, true, true, -1, 44);
                                    }
                                    break;
                                case 8:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateHouse10(int posX, int posY)
        {
            AbandonedVillageWorldgenHelper.GenHalfCircle(posX, posY, 1, 11, 10);
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,1,1,1,1,0,0,0,0,0,0,0,0},
                {0,0,0,1,1,1,1,0,0,0,0,0,0,0,0},
                {0,0,0,1,1,1,1,0,0,0,0,0,0,0,0},
                {0,0,2,2,1,1,1,0,2,0,0,0,0,0,0},
                {0,2,2,2,2,2,1,0,2,2,2,0,0,0,0},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,0}
            };
            int PosX = posX - 7;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 10;
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
                                tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
                                break;
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,1,2,2,2,1,0,0,0,0,0},
                {0,0,0,0,2,2,2,2,2,2,2,0,0,0,0},
                {0,0,1,2,2,2,2,2,2,2,2,2,1,0,0},
                {0,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {0,0,2,0,0,0,0,2,0,0,0,0,2,0,0},
                {0,0,2,0,0,0,0,2,0,0,0,0,2,0,0},
                {0,0,2,0,0,0,0,2,0,0,0,0,0,0,0},
                {0,0,3,0,0,0,0,2,3,3,0,0,0,0,0},
                {0,3,3,3,3,0,0,2,3,3,3,0,0,0,0},
                {3,3,2,2,2,2,2,2,2,2,3,3,3,3,0}
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 2:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                            }
                        }
                    }
                }
            }

        }
    }
}