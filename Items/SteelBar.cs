using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class SteelBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steel Bar");
			Tooltip.SetDefault("What a steal");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.value = 1250;
			item.rare = 3;
			item.maxStack = 99;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SandBlock, 7);
			recipe.AddRecipeGroup("Wood", 4);
			recipe.AddRecipeGroup("IronBar", 1);
			recipe.AddIngredient(ItemID.Obsidian, 3);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}