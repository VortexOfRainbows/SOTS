using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;


namespace SOTS.Items.Void
{
	public class AlmondMilk : ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Almond Milk");
			Tooltip.SetDefault("Automatically consumed when void is low\nRefills 20 void");
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 38;
            item.value = Item.sellPrice(0, 0, 3, 0);
			item.rare = 2;
			item.maxStack = 66;
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
			voidPlayer.voidMeter += 20;
			}
			
		}
	}
}