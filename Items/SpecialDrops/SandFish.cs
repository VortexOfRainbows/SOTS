using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class SandFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Fish");
			Tooltip.SetDefault("An uncommon desert fish\nIts body is made out of sand and is hard to integrate into anything useful");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
			item.value = 1250;
			item.rare = 2;
			item.maxStack = 99;
		}
	}
}