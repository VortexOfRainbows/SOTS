using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class Peanut : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(250);
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.maxStack = 9999;      
			Item.knockBack = 1.15f;
			Item.value = Item.sellPrice(0, 0, 0, 25);
			Item.rare = ItemRarityID.Blue;
			Item.ammo = Item.type;
			Item.consumable = true;
		}
	}
}