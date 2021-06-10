using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Fragments
{
	public class FragmentOfPermafrost : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Permafrost");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
	}
}