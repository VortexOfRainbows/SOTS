using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class JewelFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jeweled Cavefish");
			Tooltip.SetDefault("It's big, shiny, and probably would sell for a lot\nIt can be found swimming in pools of lava in Geode Biomes\nEasier to catch with spelunker potions");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
			item.value = 750000;
			item.rare = 6;
			item.maxStack = 99;
		}
	}
}