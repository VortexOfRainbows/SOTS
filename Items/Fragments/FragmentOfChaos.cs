using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class FragmentOfChaos : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Chaos");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 18;
			item.height = 34;
            item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = 1;
			item.maxStack = 999;
		}
	}
}