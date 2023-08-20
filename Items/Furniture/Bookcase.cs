﻿using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class Bookcase<TDrop> : FurnTile where TDrop : ModItem
    {
        protected override int ItemType => ModContent.ItemType<TDrop>();
        protected override void SetStaticDefaults(TileObjectData t)
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileTable[Type] = true;
            t.CopyFrom(TileObjectData.Style3x3);
            t.Width = 3;
            t.Height = 4;
            t.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            t.Origin = new Point16(1, 3);
            AdjTiles = new int[] { TileID.Bookcases };
        }
    }
}