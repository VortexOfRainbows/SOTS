using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class PyramidBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Brick");
			Tooltip.SetDefault("A slab from an ancient burial site, it may be hard to break");
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("PyramidBrickTile").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SandstoneBrick, 1).AddTile(TileID.Autohammer).Register();
			CreateRecipe(1).AddIngredient(ItemID.SandstoneBrick, 1).AddTile(TileID.LunarCraftingStation).Register();
			CreateRecipe(1).AddIngredient(this, 1).AddTile(TileID.WorkBenches).ReplaceResult(ItemID.SandstoneBrick);
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
			drop = ModContent.ItemType<PyramidBrick>();
			AddMapEntry(new Color(203, 191, 112));
			MineResist = 1.0f;
			MinPick = 0;
			SoundType = SoundID.Tink;
			SoundStyle = 2;
			DustType = 32;
		}
	}
}