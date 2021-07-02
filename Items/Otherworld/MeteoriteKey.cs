using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	class MeteoriteKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Opens one locked Meteorite Chest");
		}
		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.GoldenKey);
			item.width = 18;
			item.height = 36;
			item.maxStack = 99; 
			item.rare = 2;

			//item.useAnimation = 15;
			//item.useTime = 10;
			//item.useStyle = 1;
			//item.createTile = mod.TileType("LockedMeteoriteChest");
			//item.placeStyle = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SkywareKey>(), 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StrangeKey>(), 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}