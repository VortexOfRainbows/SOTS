using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Pluto : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pluto");
			Tooltip.SetDefault("You cannot take less damage than 35 plus 10% of your current health");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 22;     
            item.height = 32;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
					player.AddBuff(mod.BuffType("Pluto"), 300);
			
		}
	}
}
