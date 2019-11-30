using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;


namespace SOTS.Items.Blood
{
	public class BloodEssence: ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Essence");
			Tooltip.SetDefault("Decays while in the inventory\nAutomatically consumed to refill void when low\nRefills 2.5 void");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 4));
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 22;
			item.value = 200;
			item.rare = 10;
			item.maxStack = 999;
			item.ammo = item.type;   
			ItemID.Sets.ItemNoGravity[item.type] = true; 

		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
		
			timer += item.stack;
			if(timer >= 1200)
			{
				timer = 0;
				item.stack--;
			}
			while(voidPlayer.voidMeter < voidPlayer.voidMeterMax2 / 10)
			{
			item.stack--;
			voidPlayer.voidMeter += 2.5f;
			}
			
		}
	}
}