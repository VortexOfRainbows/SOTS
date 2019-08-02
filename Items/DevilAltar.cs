using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items
{
	public class DevilAltar : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Libra's Altar");
			Tooltip.SetDefault("Breakable through heavy explosives");
			ItemID.Sets.ExtractinatorMode[item.type] = item.type;
		}

		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 34;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.rare = -11;
			item.consumable = true;
			item.createTile = mod.TileType("DevilAltarTile");
		}
	}
}