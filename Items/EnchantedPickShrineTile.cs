using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using SOTS.Items.Tools;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items
{
	public class EnchantedPickShrineTile : ModTile
    {
        public override void SetStaticDefaults()
        {
		    MineResist = 0.01f;
		    MinPick = 0;
            DustType = 32;
            Main.tileNoFail[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
		    AddMapEntry(new Color(170, 150, 85), name);
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<EnchantedPickaxe>());
        }
        public override bool CanExplode(int i, int j)
		{
			return true;
		}
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = 2;
        }
    }
}