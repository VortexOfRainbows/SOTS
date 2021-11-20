using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class SoulResidue : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Residue");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 2, 50);
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 999;
		}
	}
}