using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	public class Sarcophagus : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Sarcophagus");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.rare = 4;
			Item.consumable = true;
			Item.createTile = mod.TileType("SarcophagusTile");
		}
	}
}