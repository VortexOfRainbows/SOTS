using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	public class PyramidChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Pyramid Chest");
			Tooltip.SetDefault("");
			ItemID.Sets.ExtractinatorMode[item.type] = item.type;
		}

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
			item.rare = 1;
			item.consumable = true;
			item.createTile = mod.TileType("PyramidChestTile");
		}
	}
}