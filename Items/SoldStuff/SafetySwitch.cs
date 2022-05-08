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
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 26;   
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.safetySwitch = true;
			vPlayer.safetySwitchVisual = !hideVisual;
		}
	}
}