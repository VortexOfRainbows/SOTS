using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	class PyramidKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mysterious Key");
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 30;
			Item.maxStack = 99; 
			Item.rare = ItemRarityID.Blue;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SOTS:EvilMaterial", 20);
			recipe.AddRecipeGroup("SOTS:GoldBar", 10);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}