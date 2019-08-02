using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alpha Polymer Table");
			Tooltip.SetDefault("Fantastic interior decortation!");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 22;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 10;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("IceCreamTableTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "IceCream", 8);
			recipe.AddTile(mod.TileType("IceCreamWorkbench"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}