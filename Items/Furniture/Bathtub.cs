using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class Bathtub<TDrop> : FurnTile where TDrop : ModItem
    {
        protected override int ItemType => ModContent.ItemType<TDrop>();
        protected override void SetStaticDefaults(TileObjectData t)
        {
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            t.CopyFrom(TileObjectData.Style4x2);
            t.Width = 4;
            t.Height = 2;
            t.CoordinateHeights = new int[] { 16, 16 };
        }
    }
}