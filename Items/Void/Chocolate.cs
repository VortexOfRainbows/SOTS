using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;


namespace SOTS.Items.Void
{
	public class Chocolate : ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chocolate");
			Tooltip.SetDefault("Automatically consumed when void is low\nRefills 15 void\n'The number one thing to bring on pirating adventures'");
		}
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 32;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = 3;
			item.maxStack = 99;
			item.ammo = item.type;   
			ItemID.Sets.ItemNoGravity[item.type] = false; 

		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
			while(voidPlayer.voidMeter < voidPlayer.voidMeterMax2 / 10)
			{
				item.stack--;
				voidPlayer.voidMeter += 15;
				VoidPlayer.VoidEffect(player, 15);
			}
			
		}
	}
}