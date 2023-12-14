using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fishing
{
	public class TinyPlanetFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(3);
		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
            Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 9999;
		}
	}
}