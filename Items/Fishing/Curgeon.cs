using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fishing
{
	public class Curgeon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curgeon");
			Tooltip.SetDefault("Has valuable eggs that can only be harvested by disection");
			this.SetResearchCost(3);
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
		}
	}
}