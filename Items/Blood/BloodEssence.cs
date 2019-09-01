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
			Tooltip.SetDefault("Decays while in the inventory\nAutomatically consumed to refill void when low\nRefills 3 void");
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
			if(modPlayer.bloodDecay)
			{
			timer += (int)(item.stack * 0.01667f);
			}
			if(item.stack > 90)
			{
			timer += (int)(item.stack * 0.03333f);
			}
			if(item.stack > 300)
			{
			timer += (int)(item.stack * 0.05f);
			}
			if(item.stack > 500)
			{
			timer += (int)(item.stack * 0.1f);
			}
			if(timer >= 999)
			{
				timer = 0;
				item.stack--;
			}
			while(voidPlayer.voidMeter < 0)
			{
			item.stack--;
			voidPlayer.voidMeter += 3;
			}
			
		}
	}
}