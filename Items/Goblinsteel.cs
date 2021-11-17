using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Goblinsteel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblinsteel Bar");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.IronBar);
			item.width = 30;
			item.height = 24;
            item.value = Item.sellPrice(0, 0, 1, 0);
			item.rare = ItemRarityID.Green;
			item.maxStack = 99;
			item.placeStyle = 0;
			item.createTile = ModContent.TileType<TheBars>();
		}
	}
}