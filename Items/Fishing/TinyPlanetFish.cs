using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fishing
{
	public class TinyPlanetFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planet Fish");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
            item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
		}
	}
}