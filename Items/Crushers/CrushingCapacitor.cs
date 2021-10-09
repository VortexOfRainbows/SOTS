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
			item.maxStack = 1;
            item.width = 30;     
            item.height = 26;   
            item.value = Item.buyPrice(0, 6, 0, 0);
            item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.CrushCapacitor = true;
		}
	}
}