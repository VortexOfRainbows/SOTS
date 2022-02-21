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
			Tooltip.SetDefault("'This is not something you should eat'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.IronBar);
			item.width = 30;
			item.height = 24;
            item.value = Item.sellPrice(0, 0, 15, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
			item.placeStyle = 9;
			item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantOre>(), 10);
			recipe.AddIngredient(ItemID.GlowingMushroom, 2);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 1);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}