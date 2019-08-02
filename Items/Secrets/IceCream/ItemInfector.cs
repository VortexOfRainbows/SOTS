using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Secrets.IceCream
{
	public class ItemInfector : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infector Chest");
			Tooltip.SetDefault("Transforms any item put inside into a portion of its Alpha Virus value");
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
			item.value = 120000;
			item.consumable = true;
			item.createTile = mod.TileType("ItemInfectorTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "IceCream", 300);
			recipe.AddIngredient(null, "IceCreamOre", 1600);
			recipe.AddIngredient(ItemID.Glass, 20);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddTile(mod.TileType("IceCreamWorkbench"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}