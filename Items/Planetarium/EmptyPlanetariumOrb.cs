using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;

namespace SOTS.Items.Planetarium
{
	public class EmptyPlanetariumOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Empty Planetarium Orb");
			Tooltip.SetDefault("It's planetary");
			ItemID.Sets.ExtractinatorMode[item.type] = item.type;
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
			item.rare = -1;
			item.consumable = true;
			item.createTile = mod.TileType("EmptyPlanetariumBlock");
		}
	}
}