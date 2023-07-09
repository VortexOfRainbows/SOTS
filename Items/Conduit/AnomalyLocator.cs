using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Conduit
{
	public class AnomalyLocator : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 26;     
            Item.height = 38;   
            Item.value = Item.buyPrice(2, 0, 0, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.canBePlacedInVanityRegardlessOfConditions = true;
		}
        public override void UpdateInventory(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if(Item.favorited)
				modPlayer.AnomalyLocator = true;
		}
	}
}