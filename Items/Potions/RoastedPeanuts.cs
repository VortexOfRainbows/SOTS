using SOTS.Buffs;
using SOTS.Items.Slime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class RoastedPeanuts : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
			ItemID.Sets.IsFood[Type] = false;
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.GrilledSquirrel);
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.holdStyle = 0;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Peanut>(10).AddTile(TileID.CookingPots).Register();
		}
	}
}