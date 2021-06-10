using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
			item.width = 22;
			item.height = 36;
            item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
	}
}