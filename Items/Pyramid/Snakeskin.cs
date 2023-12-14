using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class Snakeskin : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 1, 50);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
		}
	}
}