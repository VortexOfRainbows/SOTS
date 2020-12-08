using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	public class PyramidWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unsafe Pyramid Wall");
			Tooltip.SetDefault("Changes the biome to pyramid when in front of\nAlso envokes the Pharaoh's Curse");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 7;
			item.useStyle = 1;
			item.rare = 5;
			item.consumable = true;
			item.createWall = mod.WallType("PyramidWallTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SandstoneSlab, 2);
			recipe.AddTile(TileID.Autohammer);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SandstoneSlab, 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PyramidSlab", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PyramidWall", 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ItemID.SandstoneSlab, 2);
			recipe.AddRecipe();
		}
	}
	public class OvergrownPyramidWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unsafe Overgrown Pyramid Wall");
			Tooltip.SetDefault("Changes the biome to pyramid when in front of\nAlso envokes the Pharaoh's Curse");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 7;
			item.useStyle = 1;
			item.rare = 5;
			item.consumable = true;
			item.createWall = mod.WallType("OvergrownPyramidWallWall");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "OvergrownPyramidBlock", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<OvergrownPyramidBlock>(), 1);
			recipe.AddRecipe();
		}
	}
}