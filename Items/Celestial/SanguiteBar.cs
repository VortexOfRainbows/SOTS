using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class SanguiteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguite Scales");
			Tooltip.SetDefault("'It smells of sulfur and blood'");
		}
		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 34;
            item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.Yellow;
			item.maxStack = 99;
		}
	}
}