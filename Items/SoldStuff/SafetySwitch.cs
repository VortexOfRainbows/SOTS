using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class SafetySwitch : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Safety Switch");
			Tooltip.SetDefault("Prevents you from using void weapons if doing so would drop your void below zero");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 28;     
            item.height = 26;   
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.safetySwitch = true;
		}
	}
}