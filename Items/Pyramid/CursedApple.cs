using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.DataStructures;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class CursedApple : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Apple");
			Tooltip.SetDefault("Summons a pet Ghost Pepper to assist after combat\nPlunder 2 Souls of Looting from every killed enemy, and store them inside your void meter\nRight click on an enemy to mark it for harvesting, consuming 10 souls\nMarked enemies drop extra loot\nEnemies can be marked multiple times\nRequires 100 souls to mark a boss\nSome rare enemies will also require more souls to mark");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 32;
			item.maxStack = 1;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 6;
			//item.consumable = true;
			//item.createTile = mod.TileType("CursedAppleTile");
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.accessory = true;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.petPepper = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.soulsOnKill += 2;
			//modPlayer.typhonRange = 120;
			if (!hideVisual)
				modPlayer.petPepper = true;
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