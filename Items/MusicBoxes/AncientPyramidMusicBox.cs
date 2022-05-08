using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;

namespace SOTS.Items.MusicBoxes
{
	public class AncientPyramidMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Cursed Pyramid)");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = mod.TileType("AncientPyramidMusicBoxTile");
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PyramidSlab", 10);
			recipe.AddIngredient(null, "CursedHiveBlock", 10);
			recipe.AddIngredient(null, "SoulResidue", 10);
			recipe.AddIngredient(ItemID.MusicBox);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class AncientPyramidMusicBoxTile : ModTile
	{
		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Music Box");
			AddMapEntry(new Color(191, 142, 111), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType("AncientPyramidMusicBox"));
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("AncientPyramidMusicBox");
		}
	}
}