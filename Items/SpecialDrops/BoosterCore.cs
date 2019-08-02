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
	public class BoosterCore : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Booster");
			Tooltip.SetDefault("Allows for short flight\nDoes not stack with rocket boots");
		}
		public override void SetDefaults()
		{
      
            item.width = 38;     
            item.height = 38;   
            item.value = 250000;
            item.rare =9;
			item.accessory = true;
			item.defense = 3;

		}

		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.rocketBoots = 3; 
			player.rocketTimeMax = 15; 
				
		}
		
	}
}
