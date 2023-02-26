using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class VialofAcid : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 2, 0);
		}
	}
	public class CorrosiveGel : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.maxStack = 999;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 5, 0);
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ModContent.ItemType<VialofAcid>(), 1).AddIngredient(ItemID.PinkGel, 2).AddTile(TileID.Solidifier).Register();
		}
	}
}