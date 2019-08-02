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
			Tooltip.SetDefault("Despite being huge and inedible to humans, some fish find them quite tastey");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
			item.value = 5500;
			item.rare = 4;
			item.maxStack = 99;
			item.bait = 25;
		}
	}
}