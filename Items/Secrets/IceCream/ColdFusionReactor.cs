using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class ColdFusionReactor: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Transmutation Chest");
			Tooltip.SetDefault("Allows for duplicating items using Alpha Virus\nThe item you want to duplicate goes into the last slot of the chest\nWith sufficient Alpha Virus in YOUR inventory, the item will form in the first slot of the reactor");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 10;
			item.value = 20000000;
			item.consumable = true;
			item.createTile = mod.TileType("ColdFusionReactorTile");
			item.mech = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "IceCream", 750);
			recipe.AddIngredient(null, "ReactorTank", 4);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "PlanetaryCore", 1);
			recipe.AddTile(mod.TileType("IceCreamWorkbench"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}