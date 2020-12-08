using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace SOTS.Items.Pyramid
{
	public class CursedApple : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Apple");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 32;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 6;
			item.consumable = true;
			item.createTile = mod.TileType("CursedAppleTile");
			item.value = Item.sellPrice(0, 10, 0, 0);
		}
	}
	public class CursedAppleTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Strange Fruit");
			AddMapEntry(new Color(185, 20, 40), name);
			TileObjectData.addTile(Type);
			soundType = SoundID.Grass;
			mineResist = 0.5f;
			dustType = DustID.Grass;

		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("CursedApple"));//this defines what to drop when this tile is destroyed
		}
	}
}