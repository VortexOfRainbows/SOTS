using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class DrillWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drill Wall");
			Tooltip.SetDefault("Destroys tiles infront of it\nOnly works if visible");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 14;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 10;
			item.value = 10000;
			item.consumable = true;
			item.createWall = mod.WallType("DrillWallTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "IceCreamBrickWall", 20);
			recipe.AddIngredient(null, "IceCream", 80);
			recipe.AddIngredient(null, "IceCreamTorch", 4);
			recipe.AddIngredient(null, "IceCreamDrill", 1);
			recipe.AddTile(mod.TileType("IceCreamWorkbench"));
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}
	}
}