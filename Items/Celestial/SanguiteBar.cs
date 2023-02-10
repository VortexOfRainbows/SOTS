using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class SanguiteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 34;
            Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.maxStack = 99;
		}
	}
}