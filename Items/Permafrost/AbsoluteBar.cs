using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class AbsoluteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Absolute Alloy");
			Tooltip.SetDefault("'It burns to touch'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.IronBar);
			item.width = 30;
			item.height = 24;
			item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.Pink;
			item.maxStack = 99;
			item.placeStyle = 7;
			item.createTile = ModContent.TileType<TheBars>();
		}
	}
}