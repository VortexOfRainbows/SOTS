using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class AbsoluteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Absolute Alloy");
			Tooltip.SetDefault("It feels cold to touch");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.value = 7500;
			item.rare = 5;
			item.maxStack = 99;
		}
	}
}