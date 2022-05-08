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
			Item.width = 44;
			Item.height = 34;
            Item.value = Item.sellPrice(0, 0, 70, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.maxStack = 99;
		}
	}
}