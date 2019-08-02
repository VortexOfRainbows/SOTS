using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Planetarium
{
	public class CloudPlatform : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloud Platform");
			Tooltip.SetDefault("Fluffy yet stable! The perfect platform!");
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
			item.rare = 0;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("CloudPlatformTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Cloud, 1);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}