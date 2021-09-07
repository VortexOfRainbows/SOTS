using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class PyramidChest : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.rare = ItemRarityID.Blue;
			item.consumable = true;
			item.createTile = ModContent.TileType<PyramidChestTile>();
		}
	}
}