using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Vacuum : ModItem
	{	 	int down = 0;
			int right = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Singularity");
			Tooltip.SetDefault("Singularities bend time, space, and light\nIncreases magic damage by 50% and magic crit by 25%, also decreases all other damage types by 33%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 30;     
            item.height = 30;   
            item.value = 1000000;
            item.rare = 9;
			item.defense = 10;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.magicDamage += 0.5f;
			player.magicCrit += 25;
			player.meleeDamage -= 0.33f;
			player.rangedDamage -= 0.33f;
			player.minionDamage -= 0.33f;
			player.thrownDamage -= 0.33f;
		}
	}
}
