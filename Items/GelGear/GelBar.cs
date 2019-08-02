using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class GelBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardened Gel");
			Tooltip.SetDefault("Slippery");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.value = 125;
			item.rare = 1;
			item.maxStack = 99;
		}
	}
}