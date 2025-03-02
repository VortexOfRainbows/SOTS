using SOTS.Buffs;
using SOTS.Items.Invidia;
using SOTS.Items.Slime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class VeganBurger : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
			ItemID.Sets.IsFood[Type] = false;
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Apple);
			Item.width = 30;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Orange;
			Item.holdStyle = 0;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Peanut>(50).AddIngredient<InvidiaPetal>(50).AddTile(TileID.AlchemyTable).Register();
		}
	}
}