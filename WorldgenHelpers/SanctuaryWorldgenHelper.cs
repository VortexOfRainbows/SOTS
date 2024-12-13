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
using Steamworks;
using rail;
using SOTS.Items.Whips;
using SOTS.Items.ChestItems;
using SOTS.Items.Tools;
using Terraria.Graphics.Renderers;
using SOTS.Items.Potions;

namespace SOTS.WorldgenHelpers
{
    public static class SanctuaryWorldgenHelper
    {
        public static int Ceiling => Main.UnderworldLayer + 65;
        public static int Bottom => Main.maxTilesY - 1;
        public static int UnderworldHeight => Main.UnderworldLayer + 65;
        public static int SideOfWorld => Main.maxTilesX * 11 / 12;
        public static void PrepareUnderworldArea(int x, int y, int endX, int endY)
        {
            float height = endY - y;
            float length = endX - x;
            for (int i = x; i <= endX; i++)
            {
                float percentX = 1f - (endX - i) / length;
                for (int j = y; j <= endY; j++)
                {
                    float percentY = 1f - (endY - j) / height;

                }
            }
        }
        public static void GenerateSanctuary()
        {
            int size = 5;
            int spread = 55;
            for(int i = -size; i <= size; i++)
            {
                GeneratePillar(SideOfWorld + i * spread, UnderworldHeight);
            }
            GenerateRectangle(SideOfWorld - size * spread - 5, Ceiling, SideOfWorld + size * spread + 5, Ceiling + 15);
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