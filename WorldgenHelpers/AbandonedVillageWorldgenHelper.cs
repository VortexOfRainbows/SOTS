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
using SOTS.Items;
using SOTS.Items.Earth;
using SOTS.Items.Permafrost;
using System.Linq;
using Terraria.WorldBuilding;
using SOTS.Items.Pyramid;
using SOTS.Items.Invidia;
using SOTS.Items.Gems;
using SOTS.Items.Planetarium.Blocks;
using Terraria.GameContent.Generation;

namespace SOTS.WorldgenHelpers
{
	public static class AbandonedVillageWorldgenHelper
    {
        public static FastNoiseLite genNoise = null;
        public class CorruptionRectangle()
        {
            public Rectangle rect;
            public int AverageHeight;
        }
        private static List<CorruptionRectangle> Corruptions = new List<CorruptionRectangle>();
        private static bool GulaLayer;
        private static Rectangle OuterRect;
        private static Rectangle AVRect;
        private static Rectangle AVSweepRect;
		private static HashSet<int> ValidGrassTiles = null;
        private static HashSet<int> ValidStoneTiles = null;
        private static HashSet<int> InvalidTiles = null;
        public static void SetNoise()
        {
            if (genNoise == null)
            {
                genNoise = new FastNoiseLite();
                genNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
                genNoise.SetFractalType(FastNoiseLite.FractalType.PingPong);
                genNoise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
                genNoise.SetSeed(WorldGen.genRand.Next(500, 1500));
            }
        }
        public static void InitializeValidTileLists()
		{
            SetNoise();
            if(Corruptions == null)
                Corruptions = new List<CorruptionRectangle>();
            //if (ValidGrassTiles != null && ValidStoneTiles != null && InvalidTiles != null)
            //	return;
            ValidGrassTiles = new HashSet<int>()
            {
                TileID.CrimsonGrass,
                TileID.CorruptGrass,
            };
            ValidStoneTiles = new HashSet<int>()
            {
                TileID.CrimsonGrass,
                TileID.CorruptGrass,
                TileID.Ebonstone,
                TileID.Crimstone,
                TileID.Crimsand,
                TileID.Ebonsand,
            };
            InvalidTiles = new HashSet<int>()
            {
                TileID.Cloud,
                TileID.RainCloud,
                TileID.Trees,
                TileID.LivingWood,
                TileID.LeafBlock,
                ModContent.TileType<DullPlatingTile>(),
                ModContent.TileType<AvaritianPlatingTile>(),
                ModContent.TileType<PortalPlatingTile>(),
                TileID.SnowBrick,
                TileID.IceBrick,
                TileID.EbonstoneBrick,
                TileID.CrimstoneBrick,
                TileID.GoldBrick,
                TileID.CobaltBrick,
                TileID.Sunplate,
                TileID.BlueDungeonBrick,
                TileID.GreenDungeonBrick,
                TileID.PinkDungeonBrick,
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
        public static void GenHalfCircle(int spawnX, int spawnY, int side = 0, int radius = 10, int radiusY = 10)
		{
			radiusY++;
			float scale = radiusY / (float)radius;
			float invertScale = (float)radius / radiusY;
            if (side == 1)
			{
				for (int x = -radius; x <= radius; x++)
                {
                    for (float y = -radius * 0.75f; y <= radius; y += invertScale)
                    {
                        float length = MathF.Sqrt(x * x + y * y);
                        float sootY = (radius + y / 0.75f);
                        if (length <= radius + 0.5)
						{
							int i = spawnX + x;
							int j = spawnY + (int)(y * scale + 0.5f) + (int)(radiusY * 0.75f);
							Tile t = Framing.GetTileSafely(i, j);
							Tile tUp = Framing.GetTileSafely(i, j - 1);
                            Tile tDown = Framing.GetTileSafely(i, j + 1);
                            bool sootRange = MathF.Sqrt(x * x + sootY * sootY) < radius * 0.8f;
                            if ((!t.HasTile || sootRange || (!Main.tileSolid[t.TileType] && !Main.tileAxe[t.TileType])) && t.WallType != WallID.LivingWood && !Main.wallHouse[t.WallType])
							{
                                int type = TileID.Dirt;
                                if (t.WallType == WallID.EbonstoneUnsafe)
                                {
                                    type = TileID.Ebonstone;
                                }
                                else if(t.WallType == WallID.CrimstoneUnsafe)
                                {
                                    type = TileID.Crimstone;
                                }
                                else if(sootRange && (sootY > radiusY * 0.15f || radiusY < 30))
                                {
                                    type = ModContent.TileType<SootBlockTile>();
                                }
                                t.ClearTile();
                                WorldGen.PlaceTile(i, j, type);
                                tDown.Slope = SlopeType.Solid;
							}
							if (!tUp.HasTile && t.TileType == TileID.Dirt)
                            {
                                WorldGen.PlaceTile(i, j, WorldGen.crimson ? TileID.CrimsonGrass : TileID.CorruptGrass);
							}
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
            GenerateAbandonedMinesInsideTheCorruptionRectangle();
            return;
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
			float bonusDegreesLeft = WorldGen.genRand.NextFloat(360);
            float bonusDegreesRight = WorldGen.genRand.NextFloat(360);
            HashSet<Point> platformPoints = new HashSet<Point>();
            for (float j = 0; j <= height; j += 0.25f)
            {
                bool generatePlatforms = false;
				nextPlatform -= 0.25f * Math.Abs((float)Math.Cos(MathHelper.ToRadians(rotation)));
                if (nextPlatform <= 0 && doRopesPlatforms)
                {
                    generatePlatforms = true;
                    nextPlatform = WorldGen.genRand.Next(8, 15);
                }
                int left = -width - Math.Abs((int)(2.6f * Math.Sin(MathHelper.ToRadians(bonusDegreesLeft))));
				int right = width + Math.Abs((int)(2.6f * Math.Sin(MathHelper.ToRadians(bonusDegreesRight))));
                int sootLeft = -sootSize - Math.Abs((int)(2.6f * Math.Sin(MathHelper.ToRadians(bonusDegreesRight))));
                int sootRight = sootSize + Math.Abs((int)(2.6f * Math.Sin(MathHelper.ToRadians(bonusDegreesLeft))));
				for(int RunType = 0; RunType <= 2; RunType++)
                {
                    for (float i = left - 4; i <= right + 4; i += 0.25f)
                    {
                        Vector2 vPoint = new Vector2(i, j).RotatedBy(MathHelper.ToRadians(rotation));
                        Point rPoint = new Point(x + (int)(vPoint.X), y + (int)(vPoint.Y + 0.75f));
                        Tile tile = Framing.GetTileSafely(rPoint);
                        bool interior = false;
                        bool generateSoot = Math.Abs(i - .5f - sootLeft) < 2.25f || Math.Abs(i + .5f - sootRight) <= 2.25f;
						bool generateSides = i >= left + 1 && i <= right && (Math.Abs(i - left) < 3.75f || Math.Abs(i - right) < 3.75f);
                        bool validWall = tile.WallType != WallID.RocksUnsafe1 && tile.WallType != WallID.StoneSlab && tile.WallType != WallID.GrayBrick /*&& tile.WallType != WallID.Stone*/ &&
                            tile.WallType != ModContent.WallType<EarthenPlatingBeamWall>() && tile.WallType != ModContent.WallType<EarthenPlatingPanelWallWall>() && tile.WallType != ModContent.WallType<EarthenPlatingWallWall>();
                        if ((i >= left + 1 && i <= right) || generateSoot) //remove tiles
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
						if (RunType == 1) //Place stone blocks on sides
                        {
							if(validWall || (tile.TileType == ModContent.TileType<SootBlockTile>() && tile.HasTile))
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
                        else if (interior && RunType == 2) //Place stone walls behind
                        {
                            if(!generateSoot)
                            {
                                ushort type = WallID.RocksUnsafe1;
                                if (WorldGen.genRand.NextBool(7))
                                {
                                    type = WallID.GrayBrick;
                                }
                                else if (WorldGen.genRand.NextBool(10))
                                    type = WallID.StoneSlab;
                                if (!generateSides)
                                    tile.WallType = (ushort)type;
                            }
                            if (i == 0 && doRopesPlatforms)
                            {
                                if (generatePlatforms)
                                {
									if(!platformPoints.Contains(rPoint))
										platformPoints.Add(rPoint); 
									generatePlatforms = false;
                                }
								else if(Math.Abs(rotation) < 0.5f)
                                {
                                    //Tile tileAbove = Framing.GetTileSafely(rPoint.X, rPoint.Y - 1);
                                    //Tile tileAbove2 = Framing.GetTileSafely(rPoint.X, rPoint.Y - 2);
                                    int type = TileID.Chain;
                                    //if(tileAbove.TileType == TileID.Chain || tileAbove2.TileType == TileID.Chain)
                                    //{
                                    //    type = TileID.Chain;
                                    //}
                                    WorldGen.PlaceTile(rPoint.X, rPoint.Y, type);
                                }
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
                        if (tile.HasTile && (tile.TileType == TileID.Rope || tile.TileType == TileID.Chain))
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
                            && t.WallType != ModContent.WallType<EarthenPlatingBeamWall>() && t.WallType != ModContent.WallType<EarthenPlatingPanelWallWall>() 
                            && t.WallType != ModContent.WallType<EarthenPlatingWallWall>() && t.WallType != ModContent.WallType<UnsafeGulaPlatingWall>() && t.WallType != ModContent.WallType<GulaPlatingWallWall>())
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
                            || t.WallType == ModContent.WallType<EarthenPlatingBeamWall>() || t.WallType == ModContent.WallType<EarthenPlatingPanelWallWall>() || t.WallType == ModContent.WallType<EarthenPlatingWallWall>() 
                            || t.WallType == ModContent.WallType<UnsafeGulaPlatingWall>() || t.WallType == ModContent.WallType<GulaPlatingWallWall>();
						bool isStone = t.HasTile && (t.TileType == TileID.GrayBrick || 
                            t.TileType == TileID.Stone || 
                            t.TileType == TileID.StoneSlab || 
                            t.TileType == ModContent.TileType<GulaPlatingTile>());
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
            SOTSWorldgenHelper.SmoothRegion(x, y, (int)((outlineSize * 2 + 1) * xMult), (int)((outlineSize * 2 + 1) * yMult));
        }
		public static void GenerateDownwardPath(int x, int y)
		{
            int total = 0;
            int size;
            Vector2 destination = AVRect.Top();
			float rotation = 0;
            bool HitDesiredLocation = false;
			while(!HitDesiredLocation)
            {
                int previousX = x;
                int previousY = y;

                Vector2 toDest = destination - new Vector2(x, y);
                float distanceFrom = toDest.Length();
                size = (int)Math.Min(30, Math.Max(distanceFrom - 18, 0));
                float desiredRotation = toDest.ToRotation() * 180 / MathHelper.Pi - 90;

                GenerateTunnel(ref x, ref y, rotation, size: size);
                if (total != 0)
                    GenerateCaveCircle(previousX, previousY, 1, 1, 12, 5.5f, 2);

                float percent = MathF.Max(1 - (total * size / distanceFrom) * 1.25f, 0);
                rotation = desiredRotation + WorldGen.genRand.NextFloat(-50, 50) * percent;

                if (size <= 5 || total > 100)
                {
                    HitDesiredLocation = true;
                }
                if(size > 6 || (Main.rockLayer < y && size > 3))
                {
                    //PrepareUnderground(new Rectangle(x - 35, y - 45, 70, 90), 15);
                }
                total++;
            }
        }
		public static void GenerateNewMineEntrance(int x, int y)
		{
            GenHalfCircle(x, y, 1, 50, 50);
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
                {0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,5,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,5,0,0,0,5,5,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,5,5,5,5,5,5,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0},
                {0,0,0,0,0,4,4,0,0,0,4,4,4,4,4,4,4,4,4,4,4,5,5,5,5,5,5,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,4,5,5,5,5,5,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0},
                {0,0,0,0,0,4,4,4,0,4,4,4,4,4,4,4,4,4,4,4,4,4,5,5,5,5,5,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,4,5,5,5,5,5,5,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0},
                {0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,5,5,5,4,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,4,4,5,5,5,5,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0},
                {0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,3,3,3,2,6,6,6,6,6,6,6,6,6,2,3,3,3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0},
                {0,4,4,4,4,4,4,4,4,4,4,4,4,7,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,2,2,2,2,2,2,2,2,2,2,2,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,7,4,4,4,4,4,4,4,4,4,4,4,0},
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
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,1 ,2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,3 ,3 ,3 ,2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,1 ,3 ,3 ,3 ,3 ,2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,1 ,3 ,3 ,4 ,5 ,3 ,3 ,2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,1 ,3 ,3 ,0 ,0 ,0 ,5 ,3 ,3 ,3,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,6 ,3 ,3 ,3 ,3 ,4 ,0 ,0 ,0 ,0 ,5 ,3 ,3 ,2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,6 ,3 ,3 ,4 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,5 ,3 ,3 ,2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,3 ,3 ,3 ,4 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,3 ,3 ,2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,1 ,3 ,3 ,3 ,3 ,3 ,6 ,6 ,6 ,6 ,6 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,1 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,8 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15},
                {15,15,15,15,15,15,15,15,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,15,15 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0 ,0 ,0 ,9 ,0 ,0 ,0 ,0 ,0 ,7 ,0 ,7 ,0 ,0 ,0,15,15,15,10,10,10,10,10,10,10,10,10,10,10,15,15,15,15,15,15,15,15,15,15},
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
                                    tile.TileType = TileID.Chain;
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
                                    if(tile.TileType == TileID.Crimstone || tile.TileType == TileID.Ebonstone)
                                    {
                                        tile.ClearTile();
                                    }
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
        public static int AreaFlatness(int x, int y, int size)
        {
            int tileThere = 0;
            for (int i = -size; i <= size; i++)
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
            int h = 2;
            ushort type = !GulaLayer ? (ushort)ModContent.TileType<EarthenPlatingTile>() : (ushort)ModContent.TileType<GulaPlatingTile>();
            if(GulaLayer)
            {
                h = 3;
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = GulaLayer ? -1 : 0; j < h; j++)
                {
                    int x = posX + i;
                    int y = posY + j;
                    Tile t = Framing.GetTileSafely(x, y);
                    t.ClearTile();
                    if(!GulaLayer || (i != 0 && j != -1 && j != 2 && i != width - 1))
                        WorldGen.PlaceTile(x, y, type);
                    else
                        WorldGen.PlaceTile(x, y, (ushort)ModContent.TileType<EarthenPlatingTile>());
                }
            }
        }
        public static void GenerateWalls(int posX, int posY, int width, int minHeight, int height)
        {
            ushort typeFill = !GulaLayer ? (ushort)ModContent.WallType<EarthenPlatingPanelWallWall>() : (ushort)ModContent.WallType<GulaPlatingWallWall>();
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
                                type = typeFill;
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
            if (GulaLayer)
                posY += 4;
            int biggerPlatform = WorldGen.genRand.Next(2);
            int bonusPlatSize = WorldGen.genRand.Next(7);
            int offset = WorldGen.genRand.Next(bonusPlatSize);
            int startX = posX - (biggerPlatform == 1 ? offset : 0);
            int width1 = platW + (biggerPlatform == 1 ? bonusPlatSize : 0);
            width1 += GulaLayer ? 2 : 0;
            startX -= GulaLayer ? 1 : 0;
            GenerateBeam(startX, posY + (GulaLayer ? 1 : 0), width1);
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
                for (int j = 6; j < 15 + (GulaLayer ? 8 : 0); j++)
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
            nextWidth1 += GulaLayer ? 2 : 0;
            nextX -= GulaLayer ? 1 : 0;
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
            GenerateBeam(startX, posY - sepVert - (GulaLayer ? 1 : 0), width1);
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
            //foreach(Point16 pt in edges)
            //{
            //    Main.tile[pt.X, pt.Y].WallType = WallID.AdamantiteBeam;
            //}
            return edges;
        }
        public static void ClearLine(Point16 start, Point16 end, int dir = 1)
        {
            for(float x = 0; x <= 1f; x += 0.1f)
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
            bool finalFloor = floorNum == 0 || floorNum == -2;
            GulaLayer = finalFloor;
            int tunnelSize = 8;
            if (GulaLayer)
            {
                tunnelSize = 12;
            }
            int separation = GulaLayer ? 24 : 20;
            int stairWellLocation = Math.Max(WorldGen.genRand.Next(size), WorldGen.genRand.Next(size)); //Use max function to bias the stairs towards spawing farther away. They can still spawn close, but will attempt not to.
            int x2 = posX;
            int y2 = posY;
            for(int i = 0; i < size; i++)
            {
                GenerateTunnel(ref x2, ref y2, -90 * dir, width: tunnelSize, size: separation);
            }
            posX += 3 * dir;
            posY += 4;
            Point16 previousPointTop = new Point16(0, 0);
            Point16 previousPointBot = new Point16(0, 0);
            for (int i = 0; i < size; i++)
            {
                Point16[] edges = GenerateMineShaft(posX + i * separation * dir, posY + WorldGen.genRand.Next(4), dir);
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
                    if(WorldGen.GetWorldSize() == 0 && (floorNum == 3 || floorNum == 5))
                    {
                        GenerateStairs(center.X, center.Y, floorNum - 2);
                    }
                    else
                        GenerateStairs(center.X, center.Y, floorNum - 1);
                }
            }
            int sizeBonus = GulaLayer ? 3 : 0;
            GenerateCaveCircle(x2, y2, 1, 1, 12 + sizeBonus * 2, 6.5f + sizeBonus, 3);
            if(GulaLayer)
            {
                if(floorNum == -2)
                {
                    GenerateVaultRoom(x2, y2 + 8, dir);
                }
                else
                {
                    GeneratePortalBossRoom(x2, y2 + 8, dir);
                }
            }
            if(floorNum == 3)
            {
                GenerateMineralariumRoom(x2, y2 + 4, dir);
            }
            if(floorNum == 5)
            {
                GenerateNewRubyGemStructure(x2 - 2 * dir, y2 - 20);
            }
            GulaLayer = false;
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
            for(int cPlatforms = 0; cPlatforms < 2; cPlatforms++)
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
                                    if (cPlatforms == 0)
                                    {
                                        tile.ClearTile();
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                    }
                                    else
                                    {
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>(), true);
                                        tile.Slope = 0;
                                    }
                                    break;
                                case 2:
                                    if (cPlatforms == 0)
                                    {
                                        tile.ClearTile();
                                    }
                                    else
                                    {
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>());
                                        tile.Slope = (SlopeType)(reflected ? 2 : 1);
                                        tile.IsHalfBlock = false;
                                    }
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
            int sizeBonus = floorNum == 0 ? 5 : 0;
            int down = floorNum == 0 ? 9 : 5;
            GenerateCaveCircle(x1, y1 + down, yMult: (floorNum == 0 ? 0.6f : 0.6f), outlineSize: 22 + sizeBonus, wallSize: 14f + sizeBonus, stoneSize: 3);
            if (floorNum >= 0)
            {
                int offset = 9;
                if (floorNum == 0)
                    offset = 11;
                int attempts = 100;
                while(true)
                {
                    attempts--;
                    int mainSide = WorldGen.genRand.Next(2) * 2 - 1;

                    //Try to recenter the generation
                    int middle = AVRect.Center.X;
                    bool left = x1 < middle;
                    bool overrideWithCenteringDirection = floorNum <= 1 || WorldGen.genRand.NextBool(floorNum);
                    if(overrideWithCenteringDirection)
                    {
                        mainSide = left ? 1 : -1;
                    }

                    int width = WorldGen.genRand.Next(5, 8);
                    if((floorNum == 5 && attempts > 92) || floorNum == 0)
                    {
                        width = attempts - 85;
                        width = Math.Max(width, 2);
                    }
                    int wMultWidth = GulaLayer ? 24 : 20;

                    int startX = x1 + offset * mainSide;
                    int startY = y1 + down;

                    bool isMainSideWithinBounds = attempts <= 0 || AVRect.Contains(startX + width * mainSide * wMultWidth, startY);
                    if(floorNum == 0)
                    {
                        int sizeOfBossRoom = 175; //Not the actual exact size
                        if (WorldGen.GetWorldSize() == 0)
                            sizeOfBossRoom = 180;
                        isMainSideWithinBounds = attempts <= 0 || OuterRect.Contains(startX + (width * wMultWidth + sizeOfBossRoom) * mainSide, startY);
                    }
                    if (!isMainSideWithinBounds)
                        continue;
                    if (floorNum == 0)
                        width += 1;
                    if (floorNum == 0 || WorldGen.genRand.NextBool(3))
                    {
                        int width2 = WorldGen.genRand.Next(3, 6);
                        if (WorldGen.GetWorldSize() == 0)
                            width2--;
                        if (floorNum == 0)
                        {
                            int sizeOfLootRoom = 100; //Not the actual exact size
                            if (WorldGen.GetWorldSize() == 0)
                                sizeOfLootRoom = 120;
                            while(OuterRect.Contains(startX + (width2 * wMultWidth + sizeOfLootRoom) * -mainSide, startY))
                                width2 += 1;
                        }
                        GenerateEntireShaft(x1 - offset * mainSide, y1 + down, width2, -mainSide, floorNum == 0 ? -2 : -1);
                    }
                    GenerateEntireShaft(startX, startY, width, mainSide, floorNum);
                    break;
                }
            }
        }
        public static void GenerateUndergoundEntrance(int posX, int posY)
        {
            GenerateEntireShaft(posX + 22, posY - 12, WorldGen.genRand.Next(3, 7), 1, -1);
            GenerateEntireShaft(posX - 22, posY - 12, WorldGen.genRand.Next(3, 7), -1, -1);
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
            GenerateStairs(posX, posY, 7);
        }
        public static void GeneratePortalBossRoom(int posX, int posY, int dir = 1)
        {
            int[,] _structure = {
                {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                {0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 0},
                {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,14,14,14, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,14,14,14, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,14,14,14, 1, 1},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,14, 4,14, 1, 1},
                {3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 0, 0, 0, 0, 0, 0, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 2, 2, 2, 2, 2, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6, 0, 2, 2, 2, 2, 2, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15, 0, 2, 2, 2, 2, 2, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15, 0, 2, 2, 2, 2, 2, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15,10, 0, 2, 2, 2, 2, 2, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15,10, 0, 0, 2, 2, 2, 2, 2, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15,10, 0, 0, 0, 2, 2, 2, 2, 2, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15,10, 0, 0, 6, 0, 2, 2, 2, 2, 2, 0},
                {1, 1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15,10, 0, 0, 6,15, 0, 2, 2, 2, 2, 2, 0},
                {0, 0, 0, 0, 0, 0, 0, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 6,15,15,10, 0, 0, 6,15,15, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 6,15,15,10, 0, 0, 6,15,15,15, 0, 2, 2, 2, 2, 2, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6,15,15,10, 0, 0, 6,15,15,15,15, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15, 8, 0, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15,15,15,15,10, 0, 0, 6,15,15,15,15,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15,15, 8, 0, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15,15,15,10, 0, 0, 6,15,15,15,15,15,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 9,15,15, 8, 0, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15,10, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6,15,15,15,15,15,15,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 0, 9,15,15, 8, 0, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15,10, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6,15,15,15,15,15,15,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 0, 0, 9,15,15, 8, 0, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 0, 6,15,15,10, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6,15,15,15,15,15,15,15,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0, 8, 0, 0, 9,15,15, 0, 2, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 2, 0,15,15,10, 0, 0, 6, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,10, 0, 0, 6,15,15,15,15,15,15,15,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15, 8, 0, 0, 9,15, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 5, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0,15,10, 0, 0, 6,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,10, 0, 0, 6,15,15,15,15,15,15,15,15,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15,15, 8, 0, 0, 9, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0,10, 0, 0, 6,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,10, 0, 0, 6,15,15,15,15,15,15,15,15,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15,15,15, 8, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 6,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,10, 0, 0, 6,15,15,15,15,15,15,15,15,15,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15,15,15,15, 8, 0, 0, 2, 0, 0,12,12,12,12,12, 0, 0,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15, 0, 0,12,12,12,12,12, 0, 0, 2, 0, 0, 6,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,10, 0, 0, 6,15,15,15,15,15,15,15,15,15,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15,15,15,15,15, 8, 0, 2, 0,12,12,15,15,15,12,12,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,12,12,15,15,15,12,12, 0, 2, 0, 6,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,10, 0, 0, 6,15,15,15,15,15,15,15,15,15,15,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15, 0, 2, 0,12,15,15,15,15,15,12,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,12,15,15,15,15,15,12, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 6,15,15,15,15,15,15,15,15,15,15,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15, 0, 2, 0,12,15,15,15,15,15,12,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,12,15,15,15,15,15,12, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 6,15,15,15,15,15,15,15,15,15,15,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15, 0, 2, 0,12,15,15,15,15,15,12,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,12,15,15,15,15,15,12, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 6,15,15,15,15,15,15,15,15,15,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,15,15,15,15,15, 0, 2, 0,12,12,15,15,15,12,12,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,12,12,15,15,15,12,12, 0, 2, 0,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15,15,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,15,15,15, 0, 2, 0, 0,12,12,12,12,12, 0, 0, 0,13,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,13, 0, 0, 0,12,12,12,12,12, 0, 0, 2, 0,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,18, 0, 2, 2, 2, 2, 2, 0,18,18,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18,15,15, 0, 2, 2, 0, 0, 0,12, 0, 0, 0, 2, 0, 0, 0,13,15,15,15,15,15,15,15,15,15,15,15,11,15,15,15,15,15,15,15,15,15,15,15,13, 0, 0, 0, 2, 0, 0, 0,12, 0, 0, 0, 2, 2, 0,18,18,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,15,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,15,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,18,18,15,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
                {0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,15, 0, 2, 2, 2, 2, 2,12, 2, 2, 2, 2, 0, 0, 0, 0, 0,13,15,15,15,15,12,12,12,12,12,12,12,12,12,12,12,15,15,15,15,13, 0, 0, 0, 0, 0, 2, 2, 2, 2,12, 2, 2, 2, 2, 2, 0,18,18,18,18,15,15, 0, 2, 2, 2, 2, 2, 0,15,18,18,18,18,15,15, 0, 2, 2, 2, 2, 2, 0,15,15,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,15,18, 0, 2, 2, 2, 2, 2, 0,15,18,18,18,18,18,15, 0, 2, 2, 2, 2, 2, 0,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18, 0, 2, 2, 2, 2, 2, 0},
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
            GenerateRectangle(posX, posY - height/2 - 6, _structure.GetLength(1), height + 2);
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
                            case 4:
                                if(tile.WallType == WallID.Stone || tile.WallType == ModContent.WallType<SootWallTile>())
                                {
                                    ushort type = WallID.RocksUnsafe1;
                                    if (WorldGen.genRand.NextBool(7))
                                    {
                                        type = WallID.GrayBrick;
                                    }
                                    else if (WorldGen.genRand.NextBool(10))
                                        type = WallID.StoneSlab;
                                    tile.WallType = (ushort)type;
                                }
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
            //Generate top and bottom
            GenerateTunnel(ref x2, ref y2, -90, height + 1, width + 1 - padding * 2, false);

            //Generate right side
            x2 -= 1;
            y2 -= height;
            int y3 = y2;
            GenerateTunnel(ref x2, ref y2, 0, 5, height * 2 - 1, false);
            GenerateCaveCircle(x2 - 3, y2, 1f, .4f, 13, 0);
            GenerateCaveCircle(x2 - 3, y2 - height * 2 + 1, 1f, .4f, 13, 0);

            //Generate left side
            GenerateTunnel(ref x3, ref y3, 0, 5, height * 2 - 1, false);
            GenerateCaveCircle(x3 + 4, y3, 1f, .4f, 13, 0);
            GenerateCaveCircle(x3 + 4, y3 - height * 2 + 1, 1f, .4f, 13, 0);
        }
        public static void GenerateVaultRoom(int posX, int posY, int dir = 1)
        {
            List<Point16> lootPlacements = new List<Point16>();
            int[,] _structure = {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,7,7,7,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,0,0,0,0,0,0,0,0,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,7,7,7,7,7,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,7,7,0,2,2,2,2,2,2,0,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,7,7,7,7,7,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,0,7,7,7,7,7,7,0,2,2,2,2,2,2,0,7,7,7,7,0,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,2,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,7,7,7,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,0,0,0,0,0,0,0,0,7,7,7,7,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0,0,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
                {0,2,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,2,2,2,2,2,2,2,0,1,1,1,1,0,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,0,1,1,1,1,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,2,2,2,2,2,2,2,2,0,1,1,1,1,0,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {1,1,1,1,1,1,1,3,3,3,3,3,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,4,0,2,2,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,2,2,0,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
                {1,5,1,1,1,1,1,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,2,2,0,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,2,0},
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
            GenerateRectangle(PosX, PosY - 1, _structure.GetLength(1), height);
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
                                case 8:
                                    if (confirmPlatforms == 0)
                                        tile.ClearTile();
                                    else
                                        lootPlacements.Add(new Point16(k, l));
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
            foreach (Point16 p16 in lootPlacements)
            {
                GenerateVaultRoomLoot(p16.X, p16.Y, 59, dir);
            }
        }
        private static void GenerateVaultRoomLoot(int posX, int posY, int width, int dir = 1)
        {
            int exitAfterFailure = 1000;
            bool placedChest = false;
            while(!placedChest)
            {
                int randomX = posX + WorldGen.genRand.Next(width) * dir;
                WorldGen.PlaceTile(randomX, posY, ModContent.TileType<GulaVaultTile>(), style: 3);
                if (Main.tile[randomX, posY].TileType == ModContent.TileType<GulaVaultTile>()) 
                    placedChest = true;
                if (exitAfterFailure-- < 0) 
                    break;
            }
            for(int j = 0; j < 2; j++)
            {
                int placeBars = 4 - j * 2;
                while (placeBars > 0)
                {
                    int rY = posY - j;
                    int randomX = posX + WorldGen.genRand.Next(width) * dir;
                    WorldGen.PlaceTile(randomX, rY, ModContent.TileType<TheBars>(), style: 11);
                    if(WorldGen.genRand.NextBool())
                        WorldGen.PlaceTile(randomX + 1, rY, ModContent.TileType<TheBars>(), style: 11);
                    else if (!Main.tile[randomX + 1, rY].HasTile && Main.tile[randomX + 1, rY + 1].HasTile && Main.tile[randomX + 1, rY + 1].Slope == SlopeType.Solid)
                        WorldGen.PlaceTile(randomX + 1, rY, TileID.SilverCoinPile);
                    if (WorldGen.genRand.NextBool())
                        WorldGen.PlaceTile(randomX - 1, rY, ModContent.TileType<TheBars>(), style: 11);
                    else if (!Main.tile[randomX - 1, rY].HasTile && Main.tile[randomX - 1, rY + 1].HasTile && Main.tile[randomX - 1, rY + 1].Slope == SlopeType.Solid)
                        WorldGen.PlaceTile(randomX - 1, rY, TileID.SilverCoinPile);
                    if (Main.tile[randomX, rY].TileType == ModContent.TileType<TheBars>() ||
                        Main.tile[randomX, rY + 1].TileType == ModContent.TileType<TheBars>() ||
                        Main.tile[randomX, rY - 1].TileType == ModContent.TileType<TheBars>())
                        placeBars--;
                    if (exitAfterFailure-- < 0)
                        break;
                }
            }
            int placeBooks = 10;
            while (placeBooks > 0)
            {
                int randomX = posX + WorldGen.genRand.Next(width) * dir;
                WorldGen.PlaceTile(randomX, posY, TileID.Books);
                if (Main.tile[randomX, posY].TileType == TileID.Books)
                    placeBooks--;
                if (exitAfterFailure-- < 0)
                    break;
            }
            int placeRandomFurnishings = 30;
            while (placeRandomFurnishings > 0)
            {
                int randomX = posX + WorldGen.genRand.Next(width) * dir;
                int style = 1;
                if (WorldGen.genRand.NextBool(4))
                    style = 2;
                else if (WorldGen.genRand.NextBool(3))
                    style = 8;
                WorldGen.PlaceTile(randomX, posY, 13, style: style);
                placeRandomFurnishings--;
                if (exitAfterFailure-- < 0)
                    break;
            }
        }
        public static void GenerateMineralariumRoom(int posX, int posY, int dir = 1)
        {
            int[,] _structure = {
                {0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0},
                {0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0},
                {8,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,8},
                {8,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,8},
                {8,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,8},
                {8,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,8},
                {8,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,8},
                {8,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,8},
                {8,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,8},
                {8,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,8},
                {8,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,8,8},
                {0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0},
                {0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
                {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
                {0,0,0,0,0,0,0,0,1,1,1,3,3,3,1,1,1,1,1,4,4,4,1,1,1,1,1,5,5,5,1,1,1,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,1,1,1,3,3,3,1,1,1,1,1,4,4,4,1,1,1,1,1,5,5,5,1,1,1,0,0,0,0,0,0,0,0},
                {0,0,1,1,1,1,1,1,1,1,1,3,3,3,1,1,1,1,1,4,4,4,1,1,1,1,1,5,5,5,1,1,1,1,1,1,1,1,1,0,0},
                {0,0,1,1,1,1,1,1,1,1,1,1,6,1,1,1,1,1,1,1,6,1,1,1,1,1,1,1,6,1,1,1,1,1,1,1,1,1,1,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,7,7,7,7,7,0,0,0,0,0,7,7,7,7,7,0,0,0,0,0,0,0,7,7,7,7,7,0,0,0,0,0,7,7,7,7,7,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
            int PosX = posX - (dir == -1 ? _structure.GetLength(1) - 1 : 0);  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = posY - 11;
            GenerateRectangle(PosX, PosY - 3, _structure.GetLength(1), _structure.GetLength(0) + 4);
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
                                        tile.ClearTile();
                                    else
                                    {
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingPlatformTile>());
                                    }
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<FrigidIceTileSafe>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = 22;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<VibrantOreTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 6:
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
                                case 7:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<DissolvingEarthTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                            }
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2},
                {2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2},
                {2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2},
                {2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2},
                {2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2},
                {2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2},
                {2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2},
                {2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2},
                {2,3,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,3,2},
                {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                {2,2,2,2,2,3,1,1,1,1,1,1,1,1,1,1,3,2,3,1,1,1,3,2,3,1,1,1,1,1,1,1,1,1,1,3,2,2,2,2,2},
                {2,2,1,1,1,3,2,2,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,3,1,1,1,2,2},
                {2,2,1,1,1,3,2,2,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,3,1,1,1,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2},
                {2,2,1,1,1,1,1,3,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,3,1,1,1,1,1,2,2},
                {2,2,1,1,1,1,1,3,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,3,1,1,1,1,1,2,2},
                {2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2},
                {2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2},
                {2,2,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,2,2},
                {2,2,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
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
                            case 0:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingWallWall>();
                                break;
                            case 1:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingPanelWallWall>();
                                break;
                            case 3:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingBeamWall>();
                                break;
                            case 4:
                                tile.WallType = (ushort)ModContent.WallType<EarthWallWall>();
                                break;
                        }
                    }
                }
            }
        }
        public static void DesignateAVRectangle(int x, int y, int w, int h)
        {
            bool validLocation = false;
            float attempts = 0;
            while(!validLocation)
            {
                Vector2 random = WorldGen.genRand.NextVector2Circular(1, 1) * attempts;
                if (random.Y > 0)
                    random.Y *= 0.75f;
                else
                    random.Y *= 0.5f;
                x = (int)(x + random.X);
                y = (int)(y + random.Y);
                x = Math.Clamp(x, 240 + w / 2, Main.maxTilesX - 240 - w / 2);
                y = Math.Clamp(y, 240 + h / 2, Main.maxTilesY - 240 - h / 2);
                int padding = 40;
                OuterRect = new Rectangle(x - padding - w / 2, y - padding - h / 2, w + padding * 2, h + padding * 2);
                AVRect = new Rectangle(x - w/2, y - h / 2, w, h);
                attempts++;
                validLocation = ValidLocation(OuterRect, (int)attempts);
                if (attempts > 1000)
                {
                    validLocation = true;
                }
            }
            AVSweepRect = new Rectangle(OuterRect.X - 70, OuterRect.Y - 40, OuterRect.Width + 140, OuterRect.Height + 70);
            PrepareUnderground(AVSweepRect, 50, 0.2f);
            GenerateUndergoundEntrance(x, y - h / 2 + 10);
        }
        public static bool ValidLocation(Rectangle rect, int attempts)
        {
            bool InvalidType(Tile tile)
            {
                int type = tile.TileType;
                return type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick
                    || type == ModContent.TileType<PyramidBrickTile>() || type == ModContent.TileType<PyramidSlabTile>() || type == TileID.LihzahrdBrick || (type == TileID.JungleGrass && attempts < 500);
            }
            if (GenVars.shimmerPosition.ToPoint().ToVector2().Distance(rect.Center.ToVector2()) < rect.Width * 1.4f)
            {
                return false;
            }
            if(rect.Top < Main.rockLayer - 50 || rect.Bottom > Main.UnderworldLayer - 100)
            {
                return false;
            }
            for (int i = rect.Left; i <= rect.Right; i++)
            {
                int pX = i;
                int pY = rect.Y;
                Tile t = Main.tile[pX, pY];
                if (InvalidType(t))
                    return false;
                pY = rect.Y + rect.Height / 2;
                t = Main.tile[pX, pY];
                if (InvalidType(t))
                    return false;
                pY = rect.Y + rect.Height;
                t = Main.tile[pX, pY];
                if (InvalidType(t))
                    return false;
            }
            for (int j = rect.Y; j <= rect.Y + rect.Height; j++)
            {
                int pX = rect.X;
                int pY = j;
                Tile t = Main.tile[pX, pY];
                if (InvalidType(t))
                    return false;
                pX = rect.X + rect.Width / 2;
                t = Main.tile[pX, pY];
                if (InvalidType(t))
                    return false;
                pX = rect.X + rect.Width;
                t = Main.tile[pX, pY];
                if (InvalidType(t))
                    return false;
            }
            return true;
        }
        public static void DesignateDesiredEvilBiome()
        {
            Corruptions = new List<CorruptionRectangle>();
            InitializeValidTileLists();
            int totalEvils = 0;
            while (OutlineOneEvilBiome()) totalEvils++;
            FlattenEvilBiome(BestEvilBiome());
        }
        public static bool OutlineOneEvilBiome()
        {
            int evilStart = -1;
            int evilLength = 0;
            int evilEnd = -1;
            int allowance = 25; //How many blocks away from the crimson is still considered part of the crimson rectangle
            int start = 200;
            if (Corruptions.Count > 0)
            {
                start = Corruptions.Last().rect.Right;
            }
            int defaultDepth = (WorldGen.GetWorldSize() + 2) * 75;
            int startDepth = defaultDepth;
            int overrideDepth = int.MaxValue;
            int totalTiles = 0;
            int totalHeight = 0;
            for (int i = start; i < Main.maxTilesX - 200; i++)
            {
                int tilesDeep = 0;
                for (int j = startDepth; j < 800; j++)
                {
                    Tile t = Framing.GetTileSafely(i, j);
                    int type = t.TileType;
                    if (t.HasTile && Main.tileSolid[type])
                    {
                        if (ValidGrassTiles.Contains(type) || ValidStoneTiles.Contains(type))
                        {
                            totalHeight += j;
                            totalTiles++;
                            if (evilStart == -1)
                                evilStart = i;
                            else if (i - evilStart - evilLength < allowance)
                            {
                                evilLength = i - evilStart;
                            }
                            if (overrideDepth > j - 20)
                            {
                                overrideDepth = j - 20;
                                startDepth = overrideDepth;
                                if (startDepth > 500)
                                {
                                    startDepth = 500;
                                }
                                if (startDepth < defaultDepth)
                                {
                                    startDepth = defaultDepth;
                                }
                            }
                            break;
                        }
                        else if (evilStart != -1 && evilEnd == -1 && !(i - evilStart - evilLength < allowance))
                        {
                            evilEnd = i;
                            evilLength = evilEnd - evilStart;
                            break;
                        }
                        if(tilesDeep > 15)
                        {
                            break;
                        }
                        else
                        {
                            tilesDeep++;
                        }
                    }
                }
                if (evilEnd != -1)
                    break;
            }
            if (evilStart == -1 || totalTiles == 0)
                return false;
            Rectangle rect = new Rectangle(evilStart - allowance, startDepth, evilLength + allowance * 2, 200);
            CorruptionRectangle cR = new CorruptionRectangle();
            cR.rect = rect;
            cR.AverageHeight = totalHeight / totalTiles;
            //for (int i = rect.Left; i <= rect.Right; i++)
            //{
            //    int pX = i;
            //    int pY = rect.Y;
            //    Tile t = Main.tile[pX, pY];
            //    t.WallType = WallID.StoneSlab;
            //    pY = rect.Y + rect.Height;
            //    t = Main.tile[pX, pY];
            //    t.WallType = WallID.StoneSlab;
            //    t = Main.tile[pX, cR.AverageHeight];
            //    t.WallType = WallID.RubyGemspark;
            //}
            //for (int j = rect.Y; j <= rect.Y + rect.Height; j++)
            //{
            //    int pX = rect.X;
            //    int pY = j;
            //    Tile t = Main.tile[pX, pY];
            //    t.WallType = WallID.StoneSlab;
            //    pX = rect.X + rect.Width;
            //    t = Main.tile[pX, pY];
            //    t.WallType = WallID.StoneSlab;
            //}
            Corruptions.Add(cR);
            return true;
        }
        public static int BestEvilBiome()
        {
            Vector2 dungeon = new Vector2(GenVars.dungeonX, GenVars.dungeonY);
            Vector2 pyramid = PyramidWorldgenHelper.placementLocation;
            float bestL = -1;
            int best = -1;
            for (int i = 0; i < Corruptions.Count; i++)
            {
                Vector2 c = Corruptions[i].rect.Center.ToVector2();
                float toDung = Vector2.Distance(dungeon, c);
                float toPyra = Vector2.Distance(pyramid, c);
                float toShimmer = MathF.Abs((float)GenVars.shimmerPosition.X - c.X);
                float farthestWins = Math.Min(Math.Min(toDung, toPyra), toShimmer);
                if(farthestWins > bestL)
                {
                    bestL = farthestWins;
                    best = i;
                }
            }
            return best;
        }
        public static void FlattenEvilBiome(int oneToFlatten)
        {
            //Flattens the evil biome by shifting tiles that are above the avg down and tiles below the avg up.
            CorruptionRectangle cR = Corruptions[oneToFlatten];
            int avg = cR.AverageHeight;
            for(int i = cR.rect.Left; i < cR.rect.Right; i++)
            {
                float percent = (i - cR.rect.Left) / (float)cR.rect.Width;
                float flattenAmount = MathF.Pow(MathF.Abs(MathF.Sin(percent * MathHelper.Pi)), 0.25f);
                for (int j = cR.rect.Top; j < cR.rect.Bottom; j++)
                {
                    int diff = avg - j;
                    Tile t = Framing.GetTileSafely(i, j);
                    int type = t.TileType;
                    bool WallToMoveDown = t.WallType == WallID.CrimstoneUnsafe || t.WallType == WallID.EbonstoneUnsafe;
                    if (((t.HasTile && Main.tileSolid[type]) || WallToMoveDown) && !(InvalidTiles.Contains(t.TileType) || t.WallType == WallID.LivingWoodUnsafe || t.WallType == WallID.LivingWoodUnsafe))
                    {
                        int shift = (int)Math.Abs(diff * flattenAmount * 0.9f);
                        List<Tile> tiles = new List<Tile>();
                        if(diff > 0 && shift > 0)
                        {
                            for (int k = j; k < j + shift; k++)
                            {
                                tiles.Add(Main.tile[i, k]);
                            }
                            int n = 0;
                            for (int k = j + shift; k < j + shift + tiles.Count; k++)
                            {
                                Tile copyFrom = tiles[n];
                                Tile s = Main.tile[i, k];
                                //s.ClearEverything();
                                s.HasTile = copyFrom.HasTile ? true : s.HasTile;
                                s.Slope = 0;
                                s.IsHalfBlock = false;
                                s.TileType = copyFrom.TileType;
                                s.WallType = copyFrom.WallType != 0 ? copyFrom.WallType : s.WallType;
                                //if (!Main.tile[i, k - 1].HasTile || !Main.tileSolid[Main.tile[i, k - 1].TileType])
                                //    s.WallType = WallID.None;
                                n++;
                            }
                            for (int k = j; k < j + shift; k++)
                            {
                                Tile s = Main.tile[i, k + 1];
                                s.WallType = WallID.None;
                                Main.tile[i, k].ClearEverything();
                            }
                        }
                        else if(diff < 0 && shift > 0)
                        {
                            for (int k = j; k < j + shift; k++)
                            {
                                tiles.Add(Main.tile[i, k]);
                            }
                            int n = 0;
                            for (int k = j - shift; k < j - shift + tiles.Count; k++)
                            {
                                Tile copyFrom = tiles[n];
                                Tile s = Main.tile[i, k];
                                //s.ClearEverything();
                                s.HasTile = copyFrom.HasTile ? true : s.HasTile;
                                s.Slope = 0;
                                s.IsHalfBlock = false;
                                s.TileType = copyFrom.TileType;
                                s.WallType = copyFrom.WallType != 0 ? copyFrom.WallType : s.WallType;
                                n++;
                            }
                            for (int k = j; k < j + shift; k++)
                            {
                                if (ValidGrassTiles.Contains(Main.tile[i, k].TileType) || Main.tile[i, k].TileType == TileID.Grass)
                                    Main.tile[i, k].ResetToType(TileID.Dirt);
                                else if (Main.tile[i, k].HasTile && Main.tile[i, k - 1].HasTile)
                                {
                                    Tile s = Main.tile[i, k];
                                    s.Slope = 0;
                                }
                                if (!Main.tile[i, k].HasTile && !(Main.tile[i, k].WallType == WallID.CrimstoneUnsafe || Main.tile[i, k].WallType == WallID.EbonstoneUnsafe))
                                    Main.tile[i, k].ResetToType(Main.tile[i, k - 1].TileType);
                            }
                        }
                        break;
                    }
                }
            }
            SOTSWorldgenHelper.SmoothRegion(cR.rect.Center.X, cR.rect.Center.Y, cR.rect.Width, cR.rect.Height);
        }
        public static void GenerateNewRubyGemStructure(int spawnX, int spawnY)
        {
            ushort orbType = TileID.ShadowOrbs;
            bool crimson = WorldGen.crimson;
            Mod AVALON;
            bool avalon = ModLoader.TryGetMod("Avalon", out AVALON);
            if (avalon)
            {
                if (AVALON.TryFind("SnotOrb", out ModTile grossOrb))
                {
                    orbType = grossOrb.Type;
                }
            }
            Point16 pos = Point16.Zero;
            int[,] _structure = {
                { 0, 0, 0, 0, 0,30,30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,30,30,30, 0, 0, 0, 0, 0, 0},
                { 0, 2, 3, 2, 0,30,30, 0, 2, 2, 3, 3, 3, 2, 2, 2, 2, 3, 3, 3, 2, 3, 3, 0,30,30,30, 0, 2, 3, 2, 3, 0},
                { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 4, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,30},
                {30, 1, 1, 5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 6, 7, 1, 1, 1, 1, 1, 1, 1, 1, 6, 8, 1,30},
                { 0, 3, 3, 9, 9, 3, 3, 0,10, 1, 1, 1, 1, 1, 1, 4, 1, 1, 0, 0, 0, 0, 7, 7,11, 1, 1, 6, 7, 0, 0, 0, 0},
                { 0, 3, 2, 2, 2, 2, 3, 0, 1,10, 1, 1, 1, 1, 1, 4, 1, 1, 0, 2, 3, 0, 7, 8, 1, 1,12, 7, 7, 0, 2, 2, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0,13,13, 1, 1, 1, 1, 1, 4, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,14, 1, 4, 1, 1, 1, 7, 7, 1, 1, 1,16, 1, 1, 1, 1, 1, 7, 7,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 7, 7, 1, 1, 1, 1, 1, 1, 1, 1,17, 7, 7,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 7,11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7, 7,30},
                {30, 1,15, 1, 1,18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 7,17, 1, 1, 1, 1,16, 1, 1, 1, 1, 7, 7,30},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,10, 1, 1, 1, 4, 1, 1, 1,19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,19, 7,30},
                { 0, 3, 2, 3, 2, 2, 2, 2, 3, 3, 0, 1,10, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,11,30},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,13,13, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1,16, 1, 1, 1, 1, 1, 1, 1,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,30},
                {30, 1, 1, 1,20,21,22, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 1,30},
                {30, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 7, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7, 8,30},
                {30, 1,23, 1, 1,24, 1,23, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 7, 7, 1,12, 6, 1,25, 1, 1,26, 1, 7, 7,30},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,13,13,13,13,13, 0, 0, 0, 0,27,28,28,28,28,28,27, 0, 0, 0, 0},
                { 0, 3, 3, 2, 2, 3, 3, 3, 3, 2, 2, 2, 0, 1, 1, 4, 1, 1, 0, 3, 3, 0,27,28,28,28,28,28,27, 0, 3, 2, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,34, 1, 1, 0, 0, 0, 0,27,27,27,27,27,27,27, 0, 0, 0, 0}
            };
            int PosX = spawnX - 15;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = spawnY - 25;
            GenerateRectangle(PosX - 1, PosY - 2, _structure.GetLength(1) + 2, _structure.GetLength(0) + 3);
            for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
            {
                for (int i = _structure.GetLength(0) - 1; i >= 0; i--)
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
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EvostoneTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 3:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EvostoneBrickTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 34:
                                    tile.HasTile = true;
                                    tile.TileType = 214;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    pos = new Point16(k, l);
                                    break;
                                case 4:
                                    tile.HasTile = true;
                                    tile.TileType = 214;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 5:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<GemChestTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 6:
                                    tile.HasTile = true;
                                    tile.TileType = 54;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = true;
                                    break;
                                case 7:
                                    tile.HasTile = true;
                                    tile.TileType = 54;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 8:
                                    tile.HasTile = true;
                                    tile.TileType = 54;
                                    tile.Slope = (SlopeType)1;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 9:
                                    tile.HasTile = true;
                                    tile.TileType = 266;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 10:
                                    if (confirmPlatforms == 0)
                                        tile.ClearTile();
                                    else
                                    {
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>());
                                        tile.Slope = (SlopeType)1;
                                        tile.IsHalfBlock = false;
                                    }
                                    break;
                                case 11:
                                    tile.HasTile = true;
                                    tile.TileType = 54;
                                    tile.Slope = (SlopeType)3;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 12:
                                    tile.HasTile = true;
                                    tile.TileType = 54;
                                    tile.Slope = (SlopeType)2;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 13:
                                    if (confirmPlatforms == 0)
                                        tile.ClearTile();
                                    else
                                    {
                                        WorldGen.PlaceTile(k, l, (ushort)ModContent.TileType<EarthenPlatingPlatformTile>());
                                        tile.Slope = (SlopeType)0;
                                        tile.IsHalfBlock = false;
                                    }
                                    break;
                                case 14:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<SOTSGemLockTiles>(), true, true, -1, 0);
                                    }
                                    break;
                                case 15:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingBookcaseTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 16:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        tile.TileType = orbType;
                                        tile.HasTile = true;
                                        tile.TileFrameX = 0;
                                        Framing.GetTileSafely(k, l + 1).TileFrameX = 0;
                                        Framing.GetTileSafely(k + 1, l + 1).TileFrameX = 0;
                                        Framing.GetTileSafely(k + 1, l).TileFrameX = 0;
                                        tile.TileFrameY = 0;
                                        Framing.GetTileSafely(k, l + 1).TileFrameY = 0;
                                        Framing.GetTileSafely(k + 1, l + 1).TileFrameY = 0;
                                        Framing.GetTileSafely(k + 1, l).TileFrameY = 0;
                                        Main.tile[k, l + 1].TileType = orbType;
                                        Main.tile[k + 1, l + 1].TileType = orbType;
                                        Main.tile[k + 1, l].TileType = orbType;
                                        Framing.GetTileSafely(k, l + 1).HasTile = true;
                                        Framing.GetTileSafely(k + 1, l + 1).HasTile = true;
                                        Framing.GetTileSafely(k + 1, l).HasTile = true;
                                        Framing.GetTileSafely(k, l + 1).TileFrameY += 18;
                                        Framing.GetTileSafely(k + 1, l + 1).TileFrameX += 18;
                                        Framing.GetTileSafely(k + 1, l + 1).TileFrameY += 18;
                                        Framing.GetTileSafely(k + 1, l).TileFrameX += 18;
                                        if (crimson)
                                        {
                                            tile.TileFrameX += 36;
                                            Framing.GetTileSafely(k, l + 1).TileFrameX += 36;
                                            Framing.GetTileSafely(k + 1, l + 1).TileFrameX += 36;
                                            Framing.GetTileSafely(k + 1, l).TileFrameX += 36;
                                        }
                                    }
                                    break;
                                case 17:
                                    if (confirmPlatforms == 1)
                                    {
                                        WorldGen.PlaceTile(k, l, TileID.ExposedGems, true, true, -1, 4);
                                    }
                                    else
                                    {
                                        tile.ClearTile();
                                    }
                                    break;
                                case 18:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingSofaTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 19:
                                    tile.HasTile = true;
                                    tile.TileType = 54;
                                    tile.Slope = (SlopeType)4;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 20:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, 50, true, true, -1, 0);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 21:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, 49, true, true, -1, 0);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 22:
                                    if (confirmPlatforms == 0)
                                        tile.HasTile = false;
                                    WorldGen.PlaceTile(k, l, 50, true, true, -1, 1);
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 23:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingSinkTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 24:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<EarthenPlatingTableTile>(), true, true, -1, 0);
                                    }
                                    break;
                                case 25:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, TileID.DemonAltar, true, true, -1, crimson ? 1 : 0);
                                    }
                                    break;
                                case 26:
                                    if (confirmPlatforms == 1)
                                    {
                                        tile.HasTile = false;
                                        tile.Slope = 0;
                                        tile.IsHalfBlock = false;
                                        WorldGen.PlaceTile(k, l, ModContent.TileType<RuinedChestTile>(), true, true, -1, 1);
                                    }
                                    break;
                                case 27:
                                    tile.HasTile = true;
                                    tile.TileType = (ushort)ModContent.TileType<EvilPlatingTile>();
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                                case 28:
                                    tile.HasTile = true;
                                    tile.TileType = crimson ? TileID.Crimstone : TileID.Ebonstone;
                                    tile.Slope = 0;
                                    tile.IsHalfBlock = false;
                                    break;
                            }
                        }
                    }
                }
            }
            _structure = new int[,] {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1},
                {3,1,1,1,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,3},
                {3,4,4,4,3,4,4,3,4,4,4,4,1,4,4,4,4,4,1,4,4,4,4,3,4,4,4,3,4,4,4,4,3},
                {3,1,1,4,3,4,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,3,4,4,4,3,4,4,4,1,3},
                {3,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,1,4,3,4,1,1,1,3},
                {3,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,1,1,3,1,1,1,1,3},
                {3,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,1,1,1,3,1,1,1,1,3},
                {3,4,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,3,4,1,1,3,1,1,4,4,3},
                {3,4,4,4,3,4,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,4,4,3,4,4,1,3,1,4,4,4,3},
                {3,4,4,4,3,4,4,3,1,1,1,1,1,1,1,1,1,1,1,1,1,4,4,3,4,4,4,3,4,4,4,4,3},
                {1,1,4,5,5,5,5,5,5,5,5,5,5,5,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,1,1,1,5,1,1,1,1,1,1,1,2,5,2,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1},
                {3,1,1,1,1,1,1,1,1,1,1,2,2,5,2,2,1,1,3,1,1,3,4,6,6,6,6,6,4,3,1,1,3},
                {3,4,4,4,4,4,4,3,1,1,1,2,2,5,2,2,1,1,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {3,4,4,4,4,4,4,3,1,1,1,2,2,2,2,2,1,1,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {3,4,4,4,4,4,4,3,4,1,1,1,2,2,2,1,1,1,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {3,4,4,4,4,4,4,3,4,4,1,1,1,1,1,1,1,1,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {3,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,4,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {3,4,4,4,4,4,4,4,4,4,3,1,1,1,1,1,1,4,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {3,4,4,4,4,4,4,4,4,4,3,4,1,1,1,1,1,4,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {3,4,4,4,4,4,4,4,4,4,3,4,1,1,1,1,4,4,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {3,4,4,4,4,4,4,4,4,4,3,4,4,1,4,4,4,4,3,4,4,3,4,6,6,6,6,6,4,3,4,4,3},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,3,1,1,3,1,1,1,1,1,1,1,3,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
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
                            case 0:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingWallWall>();
                                break;
                            case 2:
                                tile.WallType = (ushort)ModContent.WallType<EvostoneBrickWallTile>();
                                break;
                            case 3:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingBeamWall>();
                                break;
                            case 4:
                                tile.WallType = (ushort)ModContent.WallType<EarthenPlatingPanelWallWall>();
                                break;
                            case 5:
                                tile.WallType = 164;
                                break;
                            case 6:
                                tile.WallType = crimson ? WallID.CrimstoneUnsafe : WallID.EbonstoneUnsafe;
                                break;
                        }
                    }
                }
            }

            int x2 = pos.X;
            int y2 = pos.Y + 1;
            GenerateTunnel(ref x2, ref y2, 0, 7, 15);
        }
        public static void GenerateAbandonedMinesInsideTheCorruptionRectangle()
        {
            int bestC = BestEvilBiome();
            CorruptionRectangle cR = Corruptions[bestC];
            Vector2 bottomOfCr = new Vector2(cR.rect.Center.X, (float)Main.rockLayer + 200);
            int size = 320;
            if (WorldGen.GetWorldSize() == 0)
                size = 240;

            PrepareUnderground(cR.rect, 60, 0.15f);
            DesignateAVRectangle((int)bottomOfCr.X, (int)bottomOfCr.Y, size + 80, size);

            Point16 placement = new Point16();
            for (int attempts = 0; attempts < 150; attempts++)
            {
                float aboveGroundLocation = MathHelper.Lerp(AVRect.Center.X, cR.rect.Center.X, WorldGen.genRand.NextFloat());
                int i = (int)(aboveGroundLocation + 0.5f);
                if(!cR.rect.Contains(i, cR.rect.Center.Y))
                {
                    break;
                }
                bool foundLocation = false;
                for(int j = cR.rect.Top; j < cR.rect.Bottom; j++)
                {
                    Tile t = Main.tile[i, j];
                    Tile tAbove = Main.tile[i, j - 1];
                    int type = t.TileType;
                    if (tAbove.WallType != 0)
                        break;
                    if (t.HasTile && Main.tileSolid[type])
                    {
                        placement = new Point16(i, j);
                        if (ValidGrassTiles.Contains(type) || ValidStoneTiles.Contains(type))
                        {
                            foundLocation = true;
                            break;
                        }
                    }
                }
                if (foundLocation)
                    break;
            }

            GenerateNewMineEntrance(placement.X, placement.Y);
            PlaceStructuresInAV(bestC, placement.X);
            AbandonedVillageTileCleanup(bestC);
        }
        public static void PlaceStructuresInAV(int evilBiome, int centerStructureX)
        {
            List<int> HouseTypes = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int TotalHouseTypes = HouseTypes.Count;
            CorruptionRectangle cR = Corruptions[evilBiome];
            bool rightSide = cR.rect.Center.X > Main.maxTilesX / 2;
            int wellSpot = rightSide ? cR.rect.Left + 70 : cR.rect.Right - 70;
            Point16 placement = new Point16(-1, -1);
            for (int attempts = 0; attempts < 100; attempts++)
            {
                int i = wellSpot;
                if (!cR.rect.Contains(i, cR.rect.Center.Y))
                {
                    break;
                }
                bool foundLocation = false;
                for (int j = cR.rect.Top; j < cR.rect.Bottom; j++)
                {
                    Tile t = Main.tile[i, j];
                    Tile tAbove = Main.tile[i, j - 1];
                    int type = t.TileType;
                    if (tAbove.WallType != 0)
                        break;
                    if (t.HasTile && Main.tileSolid[type] && MathF.Abs(centerStructureX - i) > 40)
                    {
                        placement = new Point16(i, j);
                        if (ValidGrassTiles.Contains(type) || ValidStoneTiles.Contains(type))
                        {
                            foundLocation = true;
                            break;
                        }
                    }
                }
                if (foundLocation)
                    break;
                wellSpot += rightSide ? 1 : -1;
            }
            GenerateAbandonedVillageWell(placement.X, placement.Y);
            int possibleBonusDistance = (int)Math.Sqrt(cR.rect.Width) / 2;
            int baseChance = 16 + possibleBonusDistance;
            int chance = baseChance;
            int baseDistanceFromEachOther = 12;
            for (int i = cR.rect.Left + 20; i < cR.rect.Right - 20; i++)
            {
                for (int j = cR.rect.Top; j < cR.rect.Bottom; j++)
                {
                    Tile t = Main.tile[i, j];
                    Tile tAbove = Main.tile[i, j - 1];
                    int type = t.TileType;
                    if (t.HasTile && Main.tileSolid[type] && !InvalidTiles.Contains(type))
                    {
                        bool isValidForPlacement = (ValidStoneTiles.Contains(type) || WorldGen.genRand.NextBool(5)) && MathF.Abs(centerStructureX - i) > 38 && Math.Abs(wellSpot - i) > 14 && tAbove.WallType == 0;
                        if (isValidForPlacement)
                        {
                            if(WorldGen.genRand.NextBool(chance) && AreaFlatness(i, j, 2) > 3)
                            {
                                int houseType = WorldGen.genRand.Next(TotalHouseTypes);
                                if(HouseTypes.Count <= 0)
                                {
                                    HouseTypes = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                                }
                                if (HouseTypes.Count > 0)
                                {
                                    int index = WorldGen.genRand.Next(HouseTypes.Count);
                                    houseType = HouseTypes[index];
                                    HouseTypes.RemoveAt(index);
                                }
                                int padding = (int)(baseDistanceFromEachOther + AVHouseWorldgenHelper.GenerateHouse(i, j, houseType) / 1.8f + WorldGen.genRand.Next(possibleBonusDistance));
                                i += padding;
                                chance = possibleBonusDistance;
                            }
                            else
                            {
                                if(chance > 3)
                                    chance--;
                            }    
                        }
                        break;
                    }
                    if(tAbove.WallType == WallID.EbonstoneUnsafe || tAbove.WallType == WallID.CrimstoneUnsafe)
                    {
                        break;
                    }
                }
            }
        }
        public static void AbandonedVillageTileCleanup(int evilBiome)
        {
            //int biomeType = !WorldGen.crimson ? BiomeConversionID.Corruption : BiomeConversionID.Crimson;
            int extraVerticalRangeToRemoveTrees = 10;
            CorruptionRectangle cR = Corruptions[evilBiome];
            for (int passNum = 0; passNum <= 3; passNum++)
            {
                for (int i = cR.rect.Left; i < cR.rect.Right; i++)
                {
                    for (int j = cR.rect.Top - extraVerticalRangeToRemoveTrees; j < cR.rect.Bottom; j++)
                    {
                        Tile t = Main.tile[i, j];
                        int type = t.TileType;
                        if (passNum == 0)
                        {
                            if (t.HasTile)
                            {
                                if (type == TileID.Trees || type == TileID.Pots || type == ModContent.TileType<AVPots>())
                                {
                                    t.ClearTile();
                                }
                                if (t.Slope == SlopeType.SlopeUpLeft || t.Slope == SlopeType.SlopeUpRight || t.IsHalfBlock)
                                {
                                    Tile down = Main.tile[i, j + 1];
                                    if (down.HasTile && Main.tileSolid[down.TileType])
                                    {
                                        t.Slope = 0;
                                        t.IsHalfBlock = false;
                                    }
                                }
                            }
                        }
                        else if (passNum == 1)
                        {
                            WorldGen.GrowTree(i, j);
                        }
                        else if (passNum == 2)
                        {
                            TryPlacingAmbientTiles(i, j);
                        }
                        else if (passNum == 3)
                        {
                            bool isGrass = type == TileID.CorruptGrass || type == TileID.CrimsonGrass;
                            bool isStone = ValidStoneTiles.Contains(type) && !isGrass;
                            if ((isGrass || (isStone && WorldGen.genRand.NextBool(20))) && t.HasTile && t.Slope == 0 && !t.IsHalfBlock && !Main.tile[i, j - 1].HasTile)
                            {
                                int plantType = TileID.CorruptPlants;
                                if (type == TileID.CrimsonGrass)
                                {
                                    plantType = TileID.CrimsonPlants;
                                }
                                if (WorldGen.genRand.NextBool(20) || isStone)
                                {
                                    plantType = TileID.MatureHerbs;
                                    if (WorldGen.genRand.NextBool(2) && !isStone)
                                    {
                                        plantType = ModContent.TileType<PeanutBushTile>();
                                    }
                                }
                                if (plantType == ModContent.TileType<PeanutBushTile>())
                                {
                                    Framing.GetTileSafely(i, j - 1).HasTile = false;
                                    WorldGen.PlaceTile(i, j - 1, plantType, false, true, -1, WorldGen.genRand.Next(3));
                                    if (Main.tile[i, j - 1].TileType == ModContent.TileType<PeanutBushTile>())
                                        for (int attempts = 0; attempts < 25; attempts++)
                                        {
                                            PeanutBushTile.AttemptToGrowPeanuts(i, j - 1);
                                        }
                                }
                                else
                                {
                                    WorldGen.PlaceTile(i, j - 1, plantType, false, false, -1, plantType == TileID.MatureHerbs ? 3 : 0);
                                }
                            }
                        }
                    }
                    if (passNum == 1)
                    {
                        if (WorldGen.genRand.NextBool(3))
                        {
                            i++;
                        }
                        if (WorldGen.genRand.NextBool(4))
                        {
                            i++;
                        }
                    }
                }
            }

            Rectangle rect = AVSweepRect;
            for (int i = rect.Left; i < rect.Right; i++)
            {
                for (int j = rect.Top; j < rect.Bottom; j++)
                {
                    TryPlacingAmbientTiles(i, j);
                }
            }
        }
        public static void TryPlacingAmbientTiles(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Tile tileRight = Framing.GetTileSafely(i + 1, j);
            int firstType = tile.TileType;
            int secondType = tileRight.TileType;
            if (!tile.HasTile)
                return;
            if ((firstType == TileID.Ebonstone || secondType == TileID.Crimstone) && !WorldGen.genRand.NextBool(4))
                return;
            if (firstType == ModContent.TileType<GulaPortalPlatingTile>() || secondType == ModContent.TileType<GulaPortalPlatingTile>())
                return;
            if (WorldGen.genRand.NextBool(4) && !Main.tile[i, j - 1].HasTile && !Main.tile[i + 1, j - 1].HasTile && !Main.tile[i, j - 2].HasTile && !Main.tile[i + 1, j - 2].HasTile)
            {
                Tile tileBottom = Framing.GetTileSafely(i, j + 1);
                Tile tileBottomLeft = Framing.GetTileSafely(i + 1, j + 1);
                Main.tile[i, j - 1].ClearTile();
                Main.tile[i + 1, j - 1].ClearTile();
                Main.tile[i, j - 2].ClearTile();
                Main.tile[i + 1, j - 2].ClearTile();
                List<int> validPotTypes = new List<int>();
                if (tileBottom.TileType == ModContent.TileType<GulaPlatingTile>() || tileBottomLeft.TileType == ModContent.TileType<GulaPlatingTile>())
                {
                    validPotTypes.Add(3);
                    validPotTypes.Add(4);
                    validPotTypes.Add(5);
                }
                else
                {
                    if (firstType == ModContent.TileType<EarthenPlatingTile>() || secondType == ModContent.TileType<EarthenPlatingTile>() ||
                        firstType == ModContent.TileType<EarthenPlatingPlatformTile>() || secondType == ModContent.TileType<EarthenPlatingPlatformTile>())
                    {
                        validPotTypes.Add(0);
                        validPotTypes.Add(1);
                        validPotTypes.Add(2);
                    }
                }
                if (firstType == ModContent.TileType<SootBlockTile>() || firstType == ModContent.TileType<CharredWoodTile>()
                     || secondType == ModContent.TileType<SootBlockTile>() || secondType == ModContent.TileType<CharredWoodTile>() || validPotTypes.Count <= 0)
                {
                    validPotTypes.Add(6);
                    validPotTypes.Add(7);
                    validPotTypes.Add(8);
                }
                if (validPotTypes.Count > 0)
                {
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVPots>(), true, true, -1, validPotTypes[WorldGen.genRand.Next(validPotTypes.Count)]); //pots
                }
            }
            else if(!WorldGen.genRand.NextBool(3))
            {
                bool hasPlaced = false;
                if (WorldGen.genRand.NextBool(3))
                {
                    if ((firstType == ModContent.TileType<CharredWoodTile>() && !WorldGen.genRand.NextBool(4)) || WorldGen.genRand.NextBool(30) || (firstType == ModContent.TileType<SootBlockTile>() && WorldGen.genRand.NextBool(5)))
                    {
                        WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVAmbientTile1x2>(), true, true, -1, WorldGen.genRand.Next(4));
                        hasPlaced = true;
                    }
                    else if ((ValidGrassTiles.Contains(firstType) && !WorldGen.genRand.NextBool(5)) || WorldGen.genRand.NextBool(20) || (ValidStoneTiles.Contains(firstType) && WorldGen.genRand.NextBool(4)))
                    {
                        WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVAmbientTile1x1>(), true, true, -1, WorldGen.genRand.NextFromList(4, 5, 6, 7, 8, 9, 10));
                        hasPlaced = true;
                    }
                    else if (firstType == ModContent.TileType<SootBlockTile>() || WorldGen.genRand.NextBool(9) || (firstType == ModContent.TileType<CharredWoodTile>() && WorldGen.genRand.NextBool(3))
                        || ((ValidGrassTiles.Contains(firstType) || ValidStoneTiles.Contains(firstType)) && WorldGen.genRand.NextBool(6)))
                    {
                        WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVAmbientTile1x1>(), true, true, -1, WorldGen.genRand.NextFromList(0, 1, 2, 3, 11, 12));
                        hasPlaced = true;
                    }

                }
                if (WorldGen.genRand.NextBool(3) && !hasPlaced)
                {
                    Tile third = Framing.GetTileSafely(i + 2, j);
                    bool canDoThird = third.HasTile;
                    if (canDoThird && WorldGen.genRand.NextBool(3) && tileRight.HasTile)
                    {
                        bool tryingToDoTall = WorldGen.genRand.NextBool(3);
                        bool correctTile = firstType == ModContent.TileType<SootBlockTile>() || secondType == ModContent.TileType<SootBlockTile>() || secondType == ModContent.TileType<SootBlockTile>() || third.TileType == ModContent.TileType<SootBlockTile>() || WorldGen.genRand.NextBool(25) ||
                            ((tryingToDoTall || WorldGen.genRand.NextBool(5)) && firstType == ModContent.TileType<EarthenPlatingTile>() && secondType == ModContent.TileType<EarthenPlatingTile>() && third.TileType == ModContent.TileType<EarthenPlatingTile>());
                        if (correctTile)
                        {
                            if (tryingToDoTall)
                            {
                                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVAmbientTile3x2>(), true, true, -1, WorldGen.genRand.Next(3));
                                hasPlaced = true;
                            }
                            else
                            {
                                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVAmbientTile3x1>(), true, true, -1, WorldGen.genRand.Next(2));
                                hasPlaced = true;
                            }
                        }
                        if(!hasPlaced)
                        {
                            if(ValidGrassTiles.Contains(firstType) || ValidGrassTiles.Contains(secondType) || ValidGrassTiles.Contains(third.TileType)
                                || WorldGen.genRand.NextBool(40))
                            {
                                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVAmbientTile3x2>(), true, true, -1, 3);
                                hasPlaced = true;
                            }
                        }
                    }
                    if (tileRight.HasTile && !hasPlaced)
                    {
                        bool correctTile = firstType == ModContent.TileType<SootBlockTile>() || secondType == ModContent.TileType<SootBlockTile>() || WorldGen.genRand.NextBool(15) ||
                            (WorldGen.genRand.NextBool(5) && firstType == ModContent.TileType<EarthenPlatingTile>() && secondType == ModContent.TileType<EarthenPlatingTile>());
                        if (correctTile)
                        {
                            WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVAmbientTile2x1>(), true, true, -1, WorldGen.genRand.Next(4));
                            hasPlaced = true;
                        }
                    }
                }
                if (WorldGen.genRand.NextBool(3) && !hasPlaced)
                {
                    if (tileRight.HasTile)
                    {
                        bool correctTile = firstType == ModContent.TileType<SootBlockTile>() || secondType == ModContent.TileType<SootBlockTile>() || WorldGen.genRand.NextBool(15) ||
                            (WorldGen.genRand.NextBool(5) && firstType == ModContent.TileType<EarthenPlatingTile>() && secondType == ModContent.TileType<EarthenPlatingTile>());
                        if (correctTile)
                        {
                            WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVAmbientTile2x1>(), true, true, -1, WorldGen.genRand.Next(4));
                            hasPlaced = true;
                        }
                        else
                        {
                            if (ValidGrassTiles.Contains(firstType) || ValidGrassTiles.Contains(secondType) || ((ValidStoneTiles.Contains(firstType) || ValidStoneTiles.Contains(secondType)) && WorldGen.genRand.NextBool(30))
                                || WorldGen.genRand.NextBool(36))
                            {
                                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<AVAmbientTile2x2>(), true, true, -1, WorldGen.genRand.Next(4));
                                hasPlaced = true;
                            }
                        }
                    }
                }
            }
        }
        public static void PrepareUnderground(Rectangle rect, float paddingZone = 50, float noiseWormMult = 0.2f)
        {
            SetNoise();
            int biomeType = !WorldGen.crimson ? BiomeConversionID.Corruption : BiomeConversionID.Crimson;
            for(int i = rect.Left; i < rect.Right; i++)
            {
                for (int j = rect.Top; j < rect.Bottom; j++)
                {
                    float fromLeft = MathF.Abs(i - rect.Left);
                    float fromRight = MathF.Abs(i - rect.Right);
                    float fromTop = MathF.Abs(j - rect.Top);
                    float fromBottom = MathF.Abs(j - rect.Bottom);
                    float percent = 1;
                    if (fromLeft < paddingZone || fromRight < paddingZone ||
                       fromTop < paddingZone || fromBottom < paddingZone)
                    {
                        float smallest = MathF.Min(MathF.Min(fromLeft, fromRight), MathF.Min(fromTop, fromBottom));
                        percent = smallest / paddingZone;
                    }
                    float noise = genNoise.GetNoise(i * 2, j * 2, 0);
                    Tile t = Main.tile[i, j];
                    bool validForSootConversion = t.TileType == TileID.Dirt || t.TileType == TileID.Stone || t.TileType == TileID.Mud || t.TileType == TileID.MushroomGrass
                        || t.TileType == TileID.Sand || t.TileType == TileID.HardenedSand || t.TileType == TileID.Sandstone || t.TileType == TileID.IceBlock || t.TileType == TileID.SnowBlock;
                    bool noiseWorm = noise > -noiseWormMult * percent && noise < noiseWormMult * percent;
                    if (validForSootConversion && noiseWorm && t.HasTile)
                    {
                        t.TileType = (ushort)ModContent.TileType<SootBlockTile>();
                        //t.WallType = (ushort)ModContent.WallType<SootWallTile>();
                    }
                    else if (noise >= -percent && noise <= percent)
                    {
                        WorldGen.Convert(i, j, biomeType, 0);
                    }
                }
            }
        }
    }
}