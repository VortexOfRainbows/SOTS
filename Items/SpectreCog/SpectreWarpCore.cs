using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpectreCog
{
	public class SpectreWarpCore : ModItem
	{ int saveTimer = 0;
	int cooldown = 0;
		
				float savePosX = 0;
				float savePosY = 0;
				int saveLife = 0;
	  
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Warp Core");
			Tooltip.SetDefault("When your health goes below 200 you're teleported to your position a moment ago and given your health you had that moment\nHas a 30 second cooldown, while on cooldown, you release ghosts to assist you");
		}
		public override void SetDefaults()
		{
      
            item.width = 48;     
            item.height = 32;   
            item.rare = 9;
			item.value = 1000000;
			item.accessory = true;
			item.expert = true;

		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{ saveTimer += 1;
			
			
			
			if(saveTimer == 1800)
			{
				savePosX = player.position.X;
				savePosY = player.position.Y;
				saveLife = player.statLife;
				cooldown = 1;
				saveTimer = 0;
			}
			
			
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			
			if(player.statLife <= 200 && !modPlayer.SpectreCool && cooldown == 1)
			{
				

				player.position.X = savePosX;
				player.position.Y = savePosY;
				player.statLife = saveLife;
				player.AddBuff(mod.BuffType("Relocated"), 1800);
				
			}
		}
	}
}
