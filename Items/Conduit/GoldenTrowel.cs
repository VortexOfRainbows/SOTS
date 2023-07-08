using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Conduit
{
	public class GoldenTrowel : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 22;     
            Item.height = 38;   
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.GoldenTrowel = true;
			player.tileSpeed += 0.05f;
		}
	}
}