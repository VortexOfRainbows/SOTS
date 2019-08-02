using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class AngelCarp : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angel Carp");
			Tooltip.SetDefault("They say that this carp only appears for those who deserve it\nIts diet is very specialized, it won't go for most types of bait\nLegendary Fish");
		}
		public override void SetDefaults()
		{

			item.width = 36;
			item.height = 44;
			item.value = 15000000;
			item.rare = 9;
			item.maxStack = 99;
		}
	}
}