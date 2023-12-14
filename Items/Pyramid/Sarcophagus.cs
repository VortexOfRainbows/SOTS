using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	public class Sarcophagus : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Orange;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<SarcophagusTile>();
		}
	}
}