using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamWorkTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alpha Polymer Workbench");
			Tooltip.SetDefault("Used to craft Alpha Polymer items");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 18;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 10;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("IceCreamWorkbench");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "IceCream", 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}