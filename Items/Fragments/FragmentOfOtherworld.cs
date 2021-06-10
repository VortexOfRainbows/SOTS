using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
			item.width = 26;
			item.height = 26;
            item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
	}
}