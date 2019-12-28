using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class FragmentOfEvil : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Evil");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = 1;
			item.maxStack = 999;
		}
	}
}