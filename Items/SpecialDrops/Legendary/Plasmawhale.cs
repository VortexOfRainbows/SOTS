using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops.Legendary
{
	public class Plasmawhale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasmawhale");
			Tooltip.SetDefault("Some travelers's tales say of how this whale doesn't exist\nThey say of how the whale is just a hallucination, formed out of pure horror\nIf only there was a way to purposefully make yourself insane\nLegendary Fish");
		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 36;
            item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = 9;
			item.maxStack = 99;
		}
	}
}