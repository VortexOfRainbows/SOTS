using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace SOTS.Items.Planetarium
{
	public class Patience : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Patience");
			Tooltip.SetDefault("Slowly boosts ranged damage while standing still\nSlowly lowers the boost while moving\nThe boost maxes out at 1000% damage");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 28;     
            item.height = 28;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(boost < 0)
			{
			boost = 0;
			}
			if(boost > 10)
			{
			boost = 10;
			}
			if(player.velocity.X == 0 && player.velocity.Y == 0)
			{
			boost += 0.001f;
			player.rangedDamage += boost;
			}
			else
			{
			boost -= 0.0025f;
			player.rangedDamage += boost;
			}
			
		}
}
}