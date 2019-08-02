using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamPlatform : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alpha Polymer Platform");
			Tooltip.SetDefault("Prevents the spread of Alpha Virus");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 10;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 10;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("IceCreamPlatformTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "IceCream", 1);
			recipe.AddTile(mod.TileType("IceCreamWorkbench"));
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}