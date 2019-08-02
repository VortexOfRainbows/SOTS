using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items
{
	public class DevilTileBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Balanced Brick");
			Tooltip.SetDefault("Can only be breaked by heavy explosives");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.rare = -11;
			item.consumable = true;
			item.createTile = mod.TileType("DevilTile");
		}
	}
}