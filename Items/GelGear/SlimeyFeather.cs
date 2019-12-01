using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class SlimeyFeather : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimey Feather");
			Tooltip.SetDefault("'I doubt this can be even a tiny bit aerodynamic'");
		}
		public override void SetDefaults()
		{

			item.width = 24;
			item.height = 24;
            item.value = Item.sellPrice(0, 0, 0, 4);
			item.rare = 1;
			item.maxStack = 999;
		}
	}
}