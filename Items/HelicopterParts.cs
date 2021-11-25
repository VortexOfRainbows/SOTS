using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class HelicopterParts : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helicopter Parts");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 34;
            item.value = Item.sellPrice(0, 0, 70, 0);
			item.rare = ItemRarityID.LightPurple;
			item.maxStack = 99;
		}
	}
}