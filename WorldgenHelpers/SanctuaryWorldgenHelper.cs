using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Items.Invidia;

namespace SOTS.WorldgenHelpers
{
    public static class SanctuaryWorldgenHelper
    {
        public static FastNoiseLite genNoise = null;
        public static void SetNoise()
        {
            if (genNoise == null)
            {
                genNoise = new FastNoiseLite();
                genNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
                genNoise.SetFractalType(FastNoiseLite.FractalType.PingPong);
                genNoise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
                genNoise.SetSeed(WorldGen.genRand.Next(500, 1500));
            }
        }
        public static int Ceiling => Main.UnderworldLayer;
        public static int Bottom => Main.maxTilesY - 1;
        public static int UnderworldHeight => Main.UnderworldLayer + 65;
        public static int SideOfWorld => Main.maxTilesX * 21 / 24;
        public static void PrepareUnderworldArea(int x, int y, int endX, int endY, int style = 0, int heightCutoff = 0)
        {
            ushort Evostone = (ushort)ModContent.TileType<EvostoneTile>();
            float height = endY - y;
            float length = endX - x;
            int rightSide = endX;
            int bottom = Bottom;
            if (style < 0)
            {
                x += (int)length / 2;
                if (style == -2)
                {
                    bottom -= (int)(height / 2);
                }
            }
            if (style > 0)
            {
                rightSide -= (int)length / 2;
                if(style == 2)
                {
                    bottom -= (int)(height / 2);
                }
            }

            for (int i = x; i <= rightSide; i++)
            {
                float percentX = 1f - (endX - i) / length;
                float sinX = MathF.Sin(percentX * MathF.PI) + .1f;
                float randHeightMult = WorldGen.genRand.NextFloat(1 - 0.04f * sinX, 1 + 0.04f * sinX);
                int h = WorldGen.genRand.Next(3);
                for (int j = y; j <= bottom; j++)
                {
                    float percentY = 1f - (endY - j) / height;
                    float distX = MathF.Abs(percentX - .5f) * 2f;
                    float distY = MathF.Abs(percentY - .5f) * 2f;
                    float d = distX * distX + distY * distY; //Don't have to take sqrt because we are comparing it to 1
                    Tile t = Main.tile[i, j];
                    if (d < 1f && style == 0)
                    {
                        t.ClearEverything();
                    }
                    if (d > 0.9f * randHeightMult && j > UnderworldHeight + h + heightCutoff)
                    {
                        if(t.TileType != TileID.Hellstone || !t.HasTile)
                        {
                            t.HasTile = true;
                            t.TileType = Evostone;
                            t.LiquidAmount = 0;
                            //t.Slope = 0;
                            //t.IsHalfBlock = false;
                        }
                    }
                }
            }
        }
        public static void GenerateSanctuary()
        {
            int size = 5;
            int spread = 70;
            int left = SideOfWorld - size * spread;
            int right = SideOfWorld + size * spread;
            int outcropSize = 180;
            PrepareUnderworldArea(left - 2, Ceiling + 5, right + 2, Bottom - 65, 0);
            left -= 10;
            right += 10;
            PrepareUnderworldArea(left - outcropSize, Ceiling + 20, left, Bottom - 4, -1);
            PrepareUnderworldArea(right, Ceiling + 20, right + outcropSize, Bottom - 4, 1);

            GeneratePillar(SideOfWorld + size * spread, UnderworldHeight);
            GeneratePillar(SideOfWorld - size * spread, UnderworldHeight);

            GenerateRectangle(SideOfWorld - size * spread - 5, Ceiling, SideOfWorld + size * spread + 5, Ceiling + 15);

            for(int i = -1; i <= 1; i ++)
            {
                int x = SideOfWorld + size * spread * i * 3 / 5;
                int centerPillarSize = i == 0 ? 60 : WorldGen.genRand.Next(40, 51);
                int heightOffset = i == 0 ? 25 : WorldGen.genRand.Next(15, 26);
                PrepareUnderworldArea(x - centerPillarSize - outcropSize, Ceiling + 60, x - centerPillarSize, Bottom - 4, -2, heightOffset - 2);
                PrepareUnderworldArea(x + centerPillarSize, Ceiling + 60, x + outcropSize + centerPillarSize, Bottom - 4, 2, heightOffset - 2);
                GenerateRectangle(x - centerPillarSize, UnderworldHeight + heightOffset, x + centerPillarSize, Bottom, 0);
            }

            left -= outcropSize / 2;
            right += outcropSize / 2;
            SOTSWorldgenHelper.SmoothRegion(left / 2 + right / 2, Ceiling / 2 + Bottom / 2, right - left, Bottom - Ceiling, ModContent.TileType<EvostoneTile>());
        }
        public static void GeneratePillar(int i, int j)
        {
            GenerateRectangle(i - 10, j - 30 - 1, i + 10, j + 1, -1);
            GenerateRectangle(i - 10, Ceiling, i + 10, j - 30);
            GenerateRectangle(i - 10, j, i + 10, Bottom);
        }
        public static void GenerateRectangle(int x, int y, int endX, int endY, int style = 0)
        {
            ushort Evostone = (ushort)ModContent.TileType<EvostoneBrickTile>();
            ushort EvostoneWall = (ushort)ModContent.WallType<EvostoneBrickWallTile>();
            for (int i = x; i <= endX; i++)
            {
                for (int j = y; j <= endY; j++)
                {
                    Tile t = Main.tile[i, j];
                    t.ClearTile();
                    if (style == 0)
                    {
                        t.TileType = Evostone;
                        t.HasTile = true;
                        WorldGen.TileFrame(i, j);
                    }
                    if (i > x && i < endX && j > y && j < endY)
                    {
                        t.WallType = EvostoneWall;
                    }
                }
            }
        }
    }
}