using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class AncientSteelBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Steel Bar");
			Tooltip.SetDefault("'An ancient proto-steel used by various civilizations... and goblins'");
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.IronBar);
			Item.width = 30;
			Item.height = 24;
            Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.placeStyle = 11;
			Item.createTile = ModContent.TileType<TheBars>();
		}
	}
}