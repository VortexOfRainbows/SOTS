using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SOTS.Items.AbandonedVillage
{
	class OldKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 32;
			Item.maxStack = 99; 
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.shopCustomPrice = Item.buyPrice(0, 50, 0, 0);
		}
	}
}