using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	class PyramidKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
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
			CreateRecipe(1).AddRecipeGroup("SOTS:EvilMaterial", 20).AddRecipeGroup("SOTS:GoldBar", 10).AddTile(TileID.Anvils).Register();
		}
	}
}