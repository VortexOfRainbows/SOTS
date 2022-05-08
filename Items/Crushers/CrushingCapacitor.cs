using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Crushers
{
	public class CrushingCapacitor : ModItem
	{	
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Makes the fourth charge of Crushers no longer consumes void");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 30;     
            Item.height = 26;   
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.CrushCapacitor = true;
		}
	}
}