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
			this.SetResearchCost(3);
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
			Recipe.Create(ItemID.CookedFish).AddIngredient(this, 1).AddTile(TileID.CookingPots).Register();
			Recipe.Create(ItemID.Sashimi).AddIngredient(this, 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<Snakeskin>(), 4).AddIngredient(this, 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ItemID.SummoningPotion).AddIngredient(ItemID.BottledWater, 1).AddIngredient(this, 1).AddIngredient(ModContent.ItemType<SoulResidue>(), 1).AddIngredient(ItemID.Daybloom, 1).AddTile(TileID.Bottles).Register();
		}
	}
}