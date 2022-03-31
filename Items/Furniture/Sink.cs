using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class Sink<TDrop> : FurnTile where TDrop : ModItem
    {
        protected override int ItemType => ModContent.ItemType<TDrop>();
        protected override void SetDefaults(TileObjectData t)
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Sinks };
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 0;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<TDrop>());
        }
    }
}