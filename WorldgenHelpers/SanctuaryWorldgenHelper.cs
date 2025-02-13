using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Items.Invidia;
using System.Linq;
using SOTS.Items.Invidia.MoonShard;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static SOTS.NPCs.AbandonedVillage.Famished;

namespace SOTS.WorldgenHelpers
{
    public static class SanctuaryWorldgenHelper
    {
        public static Texture2D pillarTexture;
        public static void DrawPillars()
        {
            if (pillarTexture == null)
                pillarTexture = ModContent.Request<Texture2D>("SOTS/Items/Invidia/SanctuaryPillar", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 zero = Main.drawToScreen ? zero = Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
            //Main.spriteBatch.Draw(pillarTexture, drawPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            int sizeX = pillarTexture.Width / 16;
            int sizeY = pillarTexture.Height / 16;
            int endY = Bottom - 40;
            Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) / 2;
            for(int side = -3; side <= 3; side += 2)
            {
                int size = 119;
                if (side == -3 || side == 3)
                    size = 113;
                int posX = SideOfWorld - sizeX / 2 + size * side;
                Main.NewText(MathF.Abs(screenCenter.X - posX * 16));
                if(MathF.Abs(screenCenter.X - posX * 16) < Main.screenWidth / 2)
                {
                    for (int posY = Ceiling - 15; posY < endY; posY += 4)
                    {
                        if (MathF.Abs(screenCenter.Y - posY * 16) < Main.screenHeight / 2)
                        {
                            for (int y = 0; y < sizeY; ++y)
                            {
                                for (int x = 0; x < sizeX; ++x)
                                {
                                    int i = posX + x;
                                    int j = posY + y;
                                    Vector2 drawPos = new Vector2(i, j) * 16 + zero - Main.screenPosition;
                                    Vector3[] slices = new Vector3[9];
                                    Lighting.GetColor9Slice(i, j, ref slices);
                                    Vector3 vector = Lighting.GetColor(i, j).ToVector3();
                                    Vector3 tileLight;
                                    Vector2 position;
                                    Color color = new Color();
                                    Rectangle value = new Rectangle();
                                    for (int a = 0; a < 9; a++)
                                    {
                                        value.X = 0;
                                        value.Y = 0;
                                        value.Width = 4;
                                        value.Height = 4;
                                        switch (a)
                                        {
                                            case 1:
                                                value.Width = 8;
                                                value.X = 4;
                                                break;
                                            case 2:
                                                value.X = 12;
                                                break;
                                            case 3:
                                                value.Height = 8;
                                                value.Y = 4;
                                                break;
                                            case 4:
                                                value.Width = 8;
                                                value.Height = 8;
                                                value.X = 4;
                                                value.Y = 4;
                                                break;
                                            case 5:
                                                value.X = 12;
                                                value.Y = 4;
                                                value.Height = 8;
                                                break;
                                            case 6:
                                                value.Y = 12;
                                                break;
                                            case 7:
                                                value.Width = 8;
                                                value.Height = 4;
                                                value.X = 4;
                                                value.Y = 12;
                                                break;
                                            case 8:
                                                value.X = 12;
                                                value.Y = 12;
                                                break;
                                        }
                                        //value.Y += glowOffset.Y;
                                        position.X = drawPos.X + value.X;
                                        position.Y = drawPos.Y + value.Y;
                                        value.X += x * 16;
                                        value.Y += y * 16;
                                        tileLight.X = (slices[a].X + vector.X) * 0.5f;
                                        tileLight.Y = (slices[a].Y + vector.Y) * 0.5f;
                                        tileLight.Z = (slices[a].Z + vector.Z) * 0.5f;
                                        int num = (int)(tileLight.X * 255f);
                                        int num2 = (int)(tileLight.Y * 255f);
                                        int num3 = (int)(tileLight.Z * 255f);
                                        if (num > 255)
                                            num = 255;
                                        if (num2 > 255)
                                            num2 = 255;
                                        if (num3 > 255)
                                            num3 = 255;
                                        num3 <<= 16;
                                        num2 <<= 8;
                                        color.PackedValue = (uint)(num | num2 | num3) | 0xFF000000u;
                                        Main.spriteBatch.Draw(pillarTexture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static FastNoiseLite genNoise = null;
        public static void SetNoise()
        {
            genNoise = new FastNoiseLite();
            genNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            genNoise.SetFractalType(FastNoiseLite.FractalType.PingPong);
            genNoise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
            genNoise.SetSeed(WorldGen.genRand.Next(500, 1500));
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
            int spread = 80;
            int left = SideOfWorld - size * spread;
            int right = SideOfWorld + size * spread;
            int outcropSize = 180;
            GenerateRectangle(left, Ceiling + 5, right, Bottom - 50, -1);
            PrepareUnderworldArea(left - 2, Ceiling + 5, right + 2, Bottom - 65, 0);
            left -= 10;
            right += 10;
            PrepareUnderworldArea(left - outcropSize, Ceiling + 20, left, Bottom - 4, -1);
            PrepareUnderworldArea(right, Ceiling + 20, right + outcropSize, Bottom - 4, 1);

            GeneratePillar(SideOfWorld + size * spread, UnderworldHeight);
            GeneratePillar(SideOfWorld - size * spread, UnderworldHeight);

            GenerateRectangle(SideOfWorld - size * spread - 5, Ceiling - 15, SideOfWorld + size * spread + 5, Ceiling + 5);

            for(int i = -3; i <= 3; i ++)
            {
                int x = SideOfWorld + i * 115;
                int centerPillarSize = i == 0 ? 60 : WorldGen.genRand.Next(44, 49);
                int heightOffset = i == 0 ? 25 : WorldGen.genRand.Next(15, 21);
                if(i % 2 == 0)
                {
                    PrepareUnderworldArea(x - centerPillarSize - outcropSize, Ceiling + 60, x - centerPillarSize, Bottom - 4, -2, heightOffset - 2);
                    PrepareUnderworldArea(x + centerPillarSize, Ceiling + 60, x + outcropSize + centerPillarSize, Bottom - 4, 2, heightOffset - 2);
                    GenerateRectangle(x - centerPillarSize, UnderworldHeight + heightOffset, x + centerPillarSize, Bottom, 3);
                }
                else
                {
                    int y = UnderworldHeight;
                    if (MathF.Abs(i) == 1)
                    {
                        y += WorldGen.genRand.Next(19, 23);
                        x += i * 4;
                    }
                    else
                    {
                        y += WorldGen.genRand.Next(6, 10);
                        x -= i * 2;
                    }
                    //GenerateRectangle(x - 20, Ceiling, x + 20, Bottom, 1);
                    GeneratePlatform(x, y, 0);
                    if (MathF.Abs(i) == 1)
                        GeneratePlatform(x, UnderworldHeight - WorldGen.genRand.Next(23, 28), 0);
                }
            }

            GenerateRectangle(left, UnderworldHeight + 37, right, Bottom, 2);
            GenerateRectangle(right - 150, UnderworldHeight + 25, right, Bottom, 2);
            GenerateRectangle(left, UnderworldHeight + 25, left + 150, Bottom, 2);

            left -= outcropSize / 2;
            right += outcropSize / 2;
            CleanUp(left, right, Ceiling - 30, Bottom);
            SOTSWorldgenHelper.SmoothRegion(left / 2 + right / 2, Ceiling / 2 + Bottom / 2, right - left, Bottom - Ceiling, ModContent.TileType<EvostoneTile>());

        }
        public static void GeneratePillar(int i, int j)
        {
            GenerateRectangle(i - 10, j - 30 - 1, i + 10, j + 1, 1);
            GenerateRectangle(i - 10, Ceiling - 15, i + 10, j - 30);
            GenerateRectangle(i - 10, j, i + 10, Bottom);
        }
        public static void GenerateRectangle(int x, int y, int endX, int endY, int style = 0)
        {
            bool topLayer = style == 3;
            if (style == 3)
                style = 0;
            ushort Invidia = (ushort)ModContent.TileType<InvidiaPlatingTile>();
            ushort Evostone = (ushort)ModContent.TileType<EvostoneBrickTile>();
            ushort EvostoneWall = (ushort)ModContent.WallType<EvostoneBrickWallTile>();
            for (int i = x; i <= endX; i++)
            {
                for (int j = y; j <= endY; j++)
                {
                    Tile t = Main.tile[i, j];
                    if(style == 2)
                    {
                        if(!t.HasTile)
                        {
                            t.LiquidType = LiquidID.Lava;
                            t.LiquidAmount = 255;
                        }
                    }
                    else
                    {
                        if (style == 0 || style == -1)
                        {
                            t.ClearTile();
                            if (style == 0)
                            {
                                if(topLayer && j <= y && i != x && i != endX)
                                    t.TileType = Invidia;
                                else
                                    t.TileType = Evostone;
                                t.HasTile = true;
                                t.LiquidAmount = 0;
                            }
                            else
                            {
                                t.WallType = WallID.None;
                                continue;
                            }
                        }
                        if (i > x && i < endX && j > y && j < endY)
                        {
                            t.WallType = EvostoneWall;
                        }
                    }
                }
            }
        }
        public static void GeneratePlatform(int x, int y, int style = 0)
        {
            ushort Invidia = (ushort)ModContent.TileType<InvidiaPlatingTile>();
            ushort Evostone = (ushort)ModContent.TileType<EvostoneBrickTile>();
            ushort Grass = (ushort)ModContent.TileType<OvergrownEvostoneBrickTile>();
            int[,] _structure;
            if (style == 0)
            {
                _structure = new int[,] {
                    {0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,3,3,3,1,1},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,3,3,3,1,1,1,3},
                    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,1,1,3,3,3,1,1,1,1,1,3},
                    {1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,0,3,3,3,1,1,1,3},
                    {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,0,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0}
                };
            }
            else
            {
                _structure = new int[,] {
                    {0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,3,3,3,1,1},
                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,3,3,3,3,1,1,1,3},
                    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,1,1,3,3,3,1,1,1,1,1,3},
                    {1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,0,3,3,3,1,1,1,3},
                    {1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,1,1},
                    {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,0,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0}
                };
            }
            int len = _structure.GetLength(1) - 1;
            int PosX = x - len;  //spawnX and spawnY is where you want the anchor to be when this generates
            int PosY = y;
            for (int i = 0; i < _structure.GetLength(0); i++)
            {
                for (int j = len; j >= 0; j--)
                {
                    int k = PosX + j;
                    int k2 = PosX - j + len * 2;
                    int l = PosY + i;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        Tile tile2 = Framing.GetTileSafely(k2, l);
                        switch (_structure[i, j])
                        {
                            case 0:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Evostone;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = false;
                                break;
                            case 2:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Grass;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = false;
                                break;
                            case 3:
                                tile.HasTile = tile2.HasTile = true;
                                tile.TileType = tile2.TileType = Invidia;
                                tile.Slope = tile2.Slope = 0;
                                tile.IsHalfBlock = tile2.IsHalfBlock = false;
                                break;
                        }
                    }
                }
            }
        }
        public static void CleanUp(int left, int right, int top, int bottom)
        {
            ushort Evostone = (ushort)ModContent.TileType<EvostoneTile>();
            ushort Rune = (ushort)ModContent.TileType<RunicEvostoneTile>();
            ushort EvostoneBrick = (ushort)ModContent.TileType<EvostoneBrickTile>();
            ushort RuneBrick = (ushort)ModContent.TileType<RunicEvostoneBrickTile>();
            SetNoise();
            float paddingZone = 30f;
            float noiseWormMult = 0.25f;
            int gridRate = 15;
            for(int pass = 0; pass <= 2; pass++)
            {
                for (int i = left; i <= right; i++)
                {
                    for (int j = top; j <= bottom; j++)
                    {
                        float fromLeft = MathF.Abs(i - left);
                        float fromRight = MathF.Abs(i - right);
                        float fromTop = MathF.Abs(j - top);
                        float fromBottom = MathF.Abs(j - bottom);
                        float percent = 1;
                        if (fromLeft < paddingZone || fromRight < paddingZone ||
                           fromTop < paddingZone || fromBottom < paddingZone)
                        {
                            float smallest = MathF.Min(MathF.Min(fromLeft, fromRight), MathF.Min(fromTop, fromBottom));
                            percent = smallest / paddingZone;
                        }
                        Tile t = Main.tile[i, j];
                        if (pass == 2 || pass == 1)
                        {
                            if(i % gridRate == 0 && j % gridRate == 0)
                            {
                                if(pass == 2)
                                {
                                    float noise = genNoise.GetNoise(i * 4f, j * 4f, 0);
                                    bool noiseWorm = noise > -noiseWormMult * percent && noise < noiseWormMult * percent;
                                    int edges = 0;
                                    if (Main.tile[i, j - 1].TileType == RuneBrick || Main.tile[i, j - 1].TileType == Rune)
                                        edges++;
                                    if (Main.tile[i, j + 1].TileType == RuneBrick || Main.tile[i, j + 1].TileType == Rune)
                                        edges++;
                                    if (Main.tile[i - 1, j].TileType == RuneBrick || Main.tile[i - 1, j].TileType == Rune)
                                        edges++;
                                    if (Main.tile[i + 1, j].TileType == RuneBrick || Main.tile[i + 1, j].TileType == Rune)
                                        edges++;
                                    if (noiseWorm || edges == 1)// && !WorldGen.genRand.NextBool(8))
                                    {
                                        if (t.TileType == RuneBrick || t.TileType == Rune)
                                            PlaceCircle(i, j, WorldGen.genRand.Next(3, 6));
                                    }
                                }
                                if(WorldGen.genRand.NextBool(3) && pass == 1)
                                {
                                    KillLine(i, j, 1, 0);
                                    KillLine(i, j, -1, 0);
                                    KillLine(i, j, 0, 1);
                                    KillLine(i, j, 0, -1);
                                }
                            }
                        }
                        if(pass == 0)
                        {
                            if (i % gridRate == 0 || j % gridRate == 0)
                            {
                                if (t.TileType == EvostoneBrick)
                                    t.TileType = RuneBrick;
                                if (t.TileType == Evostone)
                                    t.TileType = Rune;
                            }
                        }
                        WorldGen.TileFrame(i, j);
                    }
                }
            }
        }
        public static void PlaceCircle(int x, int y, int r)
        {
            ushort Evostone = (ushort)ModContent.TileType<EvostoneTile>();
            ushort Rune = (ushort)ModContent.TileType<RunicEvostoneTile>();
            ushort EvostoneBrick = (ushort)ModContent.TileType<EvostoneBrickTile>();
            ushort RuneBrick = (ushort)ModContent.TileType<RunicEvostoneBrickTile>();
            for (int i = x - r; i <= x + r; i++)
            {
                for(int j = y - r; j <= y + r; j++)
                {
                    float dist = MathF.Sqrt((i - x) * (i - x) + (j - y) * (j - y));
                    Tile t = Main.tile[i, j];
                    if(dist <= r + 0.45f && dist >= r - 0.45f)
                    {
                        if (t.HasTile)
                        {
                            if (t.TileType == EvostoneBrick)
                                t.TileType = RuneBrick;
                            if (t.TileType == Evostone)
                                t.TileType = Rune;
                        }
                    }
                    else if (t.HasTile)
                    {
                        if (t.TileType == RuneBrick)
                            t.TileType = EvostoneBrick;
                        if (t.TileType == Rune)
                            t.TileType = Evostone;
                    }
                }
            }
        }
        public static void KillLine(int x, int y, int dirX = 1, int dirY = 0)
        {
            ushort Evostone = (ushort)ModContent.TileType<EvostoneTile>();
            ushort Rune = (ushort)ModContent.TileType<RunicEvostoneTile>();
            ushort EvostoneBrick = (ushort)ModContent.TileType<EvostoneBrickTile>();
            ushort RuneBrick = (ushort)ModContent.TileType<RunicEvostoneBrickTile>();
            Tile t = Main.tile[x, y];
            while (t.HasTile && WorldGen.InWorld(x, y))
            {
                t = Main.tile[x, y];
                if (t.TileType == RuneBrick)
                    t.TileType = EvostoneBrick;
                if (t.TileType == Rune)
                    t.TileType = Evostone;
                x += dirX;
                y += dirY;
                Tile perpendicularT1 = Main.tile[x + dirY, y + dirX];
                Tile perpendicularT2 = Main.tile[x - dirY, y - dirX];
                if (perpendicularT1.TileType == RuneBrick || perpendicularT2.TileType == RuneBrick
                    || perpendicularT1.TileType == Rune || perpendicularT2.TileType == Rune)
                {
                    break;
                }
            }
        }
        public static void FillChestWithLoot()
        {
            foreach (Chest chest in Main.chest.Where(c => c != null))
            {
                Tile t = Main.tile[chest.x, chest.y];
                if(t.TileType == ModContent.TileType<InvidiaChestTile>())
                {
                    int slot = 0;
                    int chestType = t.TileFrameX / 36;
                    int primaryItem = ModContent.ItemType<MoonShard1>();
                    if(chestType == 2)
                        primaryItem = ModContent.ItemType<MoonShard1>();
                    if (chestType == 3)
                        primaryItem = ModContent.ItemType<MoonShard2>();
                    if (chestType == 4)
                        primaryItem = ModContent.ItemType<MoonShard3>();
                    if (chestType == 5)
                        primaryItem = ModContent.ItemType<MoonShard4>();
                    if (chestType == 6)
                        primaryItem = ModContent.ItemType<MoonShard5>();
                    if (chestType == 7)
                        primaryItem = ModContent.ItemType<MoonShard6>();
                    if (chestType == 8)
                        primaryItem = ModContent.ItemType<MoonShard7>();
                    if (chestType == 9)
                        primaryItem = ModContent.ItemType<MoonShard8>();
                    chest.AddItemToChest(primaryItem, ref slot, 1);
                }
            }
        }
    }
}