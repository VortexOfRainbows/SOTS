using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamBrickWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alpha Polymer Brick Wall");
			Tooltip.SetDefault("");
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
			item.value = 0;
			item.consumable = true;
			item.createWall = mod.WallType("IceCreamBrickWallTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "IceCreamBrick", 1);
			recipe.AddTile(mod.TileType("IceCreamWorkbench"));
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}