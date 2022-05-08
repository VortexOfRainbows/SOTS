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
			Item.CloneDefaults(ItemID.IronBar);
			Item.width = 30;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 99;
			Item.placeStyle = 7;
			Item.createTile = ModContent.TileType<TheBars>();
		}
	}
}