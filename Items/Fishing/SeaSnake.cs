using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fishing
{
	public class SeaSnake : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Snake");
			Tooltip.SetDefault("'Not actually venomous'");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
            item.value = Item.sellPrice(0, 0, 5, 75);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(ItemID.CookedFish, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ItemID.Sashimi, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<Snakeskin>(), 4);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(ModContent.ItemType<SoulResidue>(), 1);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(ItemID.SummoningPotion, 1);
			recipe.AddRecipe();
		}
	}
}