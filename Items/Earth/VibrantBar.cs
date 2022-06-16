using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Fragments;

namespace SOTS.Items.Earth
{
	public class VibrantBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Alloy");
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.IronBar);
			Item.width = 30;
			Item.height = 24;
            Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.placeStyle = 9;
			Item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ModContent.ItemType<VibrantOre>(), 10).AddIngredient(ItemID.GlowingMushroom, 2).AddRecipeGroup(RecipeGroupID.IronBar, 1).AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 1).AddTile(TileID.Furnaces).Register();
		}
	}
}