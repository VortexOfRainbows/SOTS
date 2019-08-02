using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Ouranus : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ouranus");
			Tooltip.SetDefault("Hitting enemies with melee creates a solar explosion on your cursor\nIncreases melee damage by 15% and melee crit by 10%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 34;     
            item.height = 36;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.15f;
			player.meleeCrit += 10;
			
                
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.ouranus = true;
			
		}
	}
}
