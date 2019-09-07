using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class Snakeskin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snakeskin");
			Tooltip.SetDefault("Slick and durable");
		}
		public override void SetDefaults()
		{

			item.width = 16;
			item.height = 22;
			item.value = Item.sellPrice(0, 0, 1, 50);
			item.rare = 4;
			item.maxStack = 999;
		}
	}
}