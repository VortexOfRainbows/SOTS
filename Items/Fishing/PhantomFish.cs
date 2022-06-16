using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fishing
{
	public class PhantomFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Fish");
			Tooltip.SetDefault("'Feeds off of energy within voidspace'");
			this.SetResearchCost(3);
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
            Item.value = Item.sellPrice(0, 0, 6, 25);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
		}
	}
}