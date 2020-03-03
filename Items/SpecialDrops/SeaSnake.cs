using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
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

			item.width = 24;
			item.height = 26;
            item.value = Item.sellPrice(0, 0, 5, 75);
			item.rare = 1;
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
			recipe.SetResult(mod.ItemType("Snakeskin"), 4);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(null, "SoulResidue", 1);
			recipe.AddTile(13);
			recipe.SetResult(ItemID.SummoningPotion, 1);
			recipe.AddRecipe();
		}
	}
}