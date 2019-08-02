using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Heart : ModItem
	{	 	int down = 0;
			int right = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venus");
			Tooltip.SetDefault("Killing enemies drops overheal hearts");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 22;     
            item.height = 34;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
                
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.heartActive = 1;
			  
		}
	}
}
