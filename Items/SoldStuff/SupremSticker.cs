using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class SupremSticker : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Suprem");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
      
            item.width = 60;     
            item.height = 22;   
            item.value = 15000000;
            item.rare = 4;
			item.accessory = true;
			item.defense = 69696969;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance = -696968f;
			
            
					
		}
		
	}
}
