using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Banners
{
	public class SOTSTrophies : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true; // Necessary since Style3x3Wall uses AnchorWall
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Trophy");
			AddMapEntry(new Color(120, 85, 60), name);
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			switch (frameX / 54)
			{
				case 0:
					item = ItemType<PutridPinkyTrophy>();
					break;
			}
			if (item > 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}
	public abstract class ModTrophy : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSTrophies>();
			Item.placeStyle = 0;
		}
	}
	public class PutridPinkyTrophy : ModBanner
	{
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSTrophies>();
			Item.placeStyle = 0;
		}
	}
}