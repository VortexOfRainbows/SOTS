using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class PyramidBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<PyramidBrickTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SandstoneBrick, 1).AddTile(TileID.Autohammer).Register();
			CreateRecipe(1).AddIngredient(ItemID.SandstoneBrick, 1).AddTile(TileID.LunarCraftingStation).Register();
			Recipe.Create(ItemID.SandstoneBrick).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class PyramidBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<PyramidBrick>();
			AddMapEntry(new Color(203, 191, 112));
			MineResist = 1.0f;
			MinPick = 0;
			HitSound = SoundID.Tink;
			DustType = 32;
		}
	}
}