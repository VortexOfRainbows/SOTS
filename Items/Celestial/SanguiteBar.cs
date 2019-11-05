using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class SanguiteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguite Bar");
			Tooltip.SetDefault("'It smells of sulfur and blood'");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
            item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = 8;
			item.maxStack = 99;
		}
	}
}