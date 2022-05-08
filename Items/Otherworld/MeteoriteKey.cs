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
			//Item.CloneDefaults(ItemID.GoldenKey);
			Item.width = 18;
			Item.height = 36;
			Item.maxStack = 99; 
			Item.rare = 2;

			//Item.useAnimation = 15;
			//Item.useTime = 10;
			//Item.useStyle = 1;
			//Item.createTile = mod.TileType("LockedMeteoriteChest");
			//Item.placeStyle = 1;
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