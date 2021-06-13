using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class ZeplineLure : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zeppelin Lure");
			Tooltip.SetDefault("'You shouldn't have this'");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 10;
			item.consumable = true;
			item.createTile = mod.TileType("ZeplineLureTile");
		}
	}
}