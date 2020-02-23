using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class PhantomFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Fish");
			Tooltip.SetDefault("'Feeds off energy within voidspace'");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 32;
            item.value = Item.sellPrice(0, 0, 6, 25);
			item.rare = 1;
			item.maxStack = 99;
		}
	}
}