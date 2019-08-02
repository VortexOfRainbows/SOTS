using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class RedRibbon : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Ribbon");
			Tooltip.SetDefault("It wants to protect you\nHeals 2 health every 9 seconds");
		}
		public override void SetDefaults()
		{
      
            item.width = 28;     
            item.height = 32;   
            item.value = 250000;
            item.rare = 10;
			item.accessory = true;
			item.defense = 3;

		}

		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			timer += 1;
		
		
					if(timer == 540)
					{	
						
			player.statLife += 2;
			player.HealEffect(2);
					timer = 0;
					}
				
		}
		
	}
}
