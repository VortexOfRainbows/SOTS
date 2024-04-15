using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class InfiniteVoid : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 42;     
            Item.height = 34;   
            Item.value = Item.sellPrice(5, 0, 0, 0);
            Item.rare = ItemRarityID.Purple;
			Item.accessory = true;
		}
        public override void UpdateInventory(Player player)
        {
			if(Item.favorited)
			{
				UpdateAccessory(player, false);
			}
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.voidMeterMax2 += 10000;
			vPlayer.flatVoidRegen += 10000;
		}
	}
}