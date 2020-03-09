using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	public class ZeplineLure : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zeppelin Lure");
			Tooltip.SetDefault("'A strange artifact'\nAttracts various types of exotic fish");
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