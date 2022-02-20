using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GhostTown
{
	public class AncientSteelBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Steel Bar");
			Tooltip.SetDefault("'An ancient proto-steel used by various civilizations... and goblins'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.IronBar);
			item.width = 30;
			item.height = 24;
            item.value = Item.sellPrice(0, 0, 5, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.placeStyle = 11;
			item.createTile = ModContent.TileType<TheBars>();
		}
	}
}