using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class SandStrider : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Strider");
			Tooltip.SetDefault("A common bug found skitting on surfaces of sandy water");
		}
		public override void SetDefaults()
		{

			item.width = 24;
			item.height = 40;
			item.value = 1500;
			item.rare = 2;
			item.maxStack = 99;
			item.bait = 20;
		}
	}
}