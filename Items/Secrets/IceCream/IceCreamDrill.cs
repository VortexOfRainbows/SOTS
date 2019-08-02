using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamDrill: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alpha Polymer Drill");
			Tooltip.SetDefault("Drills up to 20 blocks beneath the structure\nStops when it comes in contanct with Alpha Polymer Bricks\nConnect wires to the top");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 32;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 10;
			item.value = 250000;
			item.consumable = true;
			item.createTile = mod.TileType("IceCreamDrillTile");
			item.mech = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "IceCream", 100);
			recipe.AddIngredient(null, "CrusherEmblem", 1);
			recipe.AddTile(mod.TileType("IceCreamWorkbench"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}