using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class WormWoodCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worm Wood Core");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 34;
			item.value = 75000;
			item.rare = 6;
			item.maxStack = 99;
		}
	}
}