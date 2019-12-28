using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class FragmentOfNature : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Nature");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 28;
            item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = 1;
			item.maxStack = 999;
		}
	}
}