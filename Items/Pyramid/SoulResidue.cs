using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

			item.width = 26;
			item.height = 24;
			item.value = Item.sellPrice(0, 0, 2, 50);
			item.rare = 4;
			item.maxStack = 999;
		}
	}
}