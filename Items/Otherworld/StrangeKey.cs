using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	class StrangeKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Opens one locked Strange Chest");
		}
		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.GoldenKey);
			item.width = 18;
			item.height = 32;
			item.maxStack = 99; 
			item.rare = 2;

			//	item.useAnimation = 15;
			//	item.useTime = 10;
			//	item.useStyle = 1;
			//item.createTile = mod.TileType("LockedStrangeChest");
			//item.placeStyle = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MeteoriteKey>(), 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SkywareKey>(), 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}