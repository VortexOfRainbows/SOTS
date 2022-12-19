using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class BagOfAmmoGathering : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bag of Ammunition Gathering");
			Tooltip.SetDefault("20% chance not to consume ammo\nStriking enemies has a chance to refund ammo\nLanding the killing blow on enemies may also refund ammo");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 30;     
            Item.height = 28;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.AmmoConsumptionModifier += 0.2f;
			modPlayer.AmmoRegather = true;
		}
	}
}