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
			Item.width = 28;
			Item.height = 28;
            Item.value = Item.sellPrice(0, 0, 5, 75);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(this, 1).AddTile(TileID.CookingPots).ReplaceResult(ItemID.CookedFish);
			CreateRecipe(1).AddIngredient(this, 1).AddTile(TileID.WorkBenches).ReplaceResult(ItemID.Sashimi);
			CreateRecipe(4).AddIngredient(this, 1).AddTile(TileID.WorkBenches).ReplaceResult(ModContent.ItemType<Snakeskin>());
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(this, 1).AddIngredient(ModContent.ItemType<SoulResidue>(), 1).AddIngredient(ItemID.Daybloom, 1).AddTile(TileID.Bottles).ReplaceResult(ItemID.SummoningPotion);
		}
	}
}