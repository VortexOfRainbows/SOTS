using SOTS.Projectiles.Planetarium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class PlasticBait : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(99);
		public override void SetDefaults()
		{
			Item.bait = 5;
			Item.width = 42;
			Item.height = 28;
			Item.maxStack = 9999;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.consumable = true;
		}
	}
}