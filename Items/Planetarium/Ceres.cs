using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Ceres : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ceres");
			Tooltip.SetDefault("All weapons gain autoswing\nAlso boosts attack speed");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 22;     
            item.height = 30;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
					player.AddBuff(mod.BuffType("Ceres"), 300);
			
		}
	}
}
