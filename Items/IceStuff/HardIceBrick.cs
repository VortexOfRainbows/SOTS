using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items.IceStuff
{
	public class HardIceBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardened Ice Block");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.rare = 9;
			item.consumable = true;
			item.createTile = mod.TileType("HardIceBrickTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 24);
			recipe.AddIngredient(null, "AbsoluteBar", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 8);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 4);
			recipe.AddTile(TileID.Autohammer);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}