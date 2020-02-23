using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class TinyPlanetFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planet Fish");
			Tooltip.SetDefault("Caught in sky lakes");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
            item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.maxStack = 99;
			item.bait = 5;
		}
	}
}