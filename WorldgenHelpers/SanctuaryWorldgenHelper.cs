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
        public static void GenerateSanctuary()
        {
            int ceiling = Main.UnderworldLayer;
            int underworldHeight = Main.UnderworldLayer + 65;
            int rightSideOfWorld = Main.maxTilesX * 11 / 12;
            int bottom = Main.maxTilesY - 1;
            GenerateRectangle(rightSideOfWorld - 10, ceiling, rightSideOfWorld + 10, underworldHeight - 30);
            GenerateRectangle(rightSideOfWorld - 10, underworldHeight, rightSideOfWorld + 10, bottom);
        }
        public static void GenerateRectangle(int x, int y, int endX, int endY)
        {
            ushort Evostone = (ushort)ModContent.TileType<EvostoneBrickTile>();
            for(int i = x; i <= endX; i++)
            {
                for (int j = y; j <= endY; j++)
                {
                    Tile t = Main.tile[i, j];
                    t.ClearTile();
                    t.TileType = Evostone;
                    t.HasTile = true;
                    WorldGen.TileFrame(i, j);
                }
            }
        }
    }
}