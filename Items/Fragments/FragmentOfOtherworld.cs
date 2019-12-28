using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class FragmentOfOtherworld : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Otherworld");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = 1;
			item.maxStack = 999;
		}
	}
}