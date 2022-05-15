using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class FurnTile : ModTile
    {
        protected abstract int ItemType { get; }
        protected virtual Color MapColor => new Color(191, 142, 111, 255);
        protected virtual bool Multi => true;
        public sealed override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            SetDefaults(TileObjectData.newTile);
            TileObjectData.addTile(Type);
            int item = ItemType;
            AddMapEntry(MapColor, LocalizedText.Empty, (s, i, j) => (string)Lang.GetItemName(item));
            if (!Multi)
            {
                drop = item;
            }
        }
        protected abstract void SetStaticDefaults(TileObjectData t);
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 0;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (Multi)
            {
                Item.NewItem(i * 16, j * 16, 64, 32, ItemType);
            }
        }
    }
}