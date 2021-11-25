using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fishing
{
	public class Curgeon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curgeon");
			Tooltip.SetDefault("Has valuable eggs that can only be harvested by disection");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
		}
	}
}