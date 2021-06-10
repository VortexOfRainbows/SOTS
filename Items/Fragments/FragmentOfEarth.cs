using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Fragments
{
	public class FragmentOfEarth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Earth");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
            item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
	}
}