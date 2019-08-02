using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Mars : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mars");
			Tooltip.SetDefault("War\nIncreases melee damage by 50% and melee crit by 25%, also decreases all other damage types by 33%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 32;     
            item.height = 32;   
            item.value = 1000000;
            item.rare = 9;
			item.defense = 10;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.5f;
			player.meleeCrit += 25;
			player.rangedDamage -= 0.33f;
			player.magicDamage -= 0.33f;
			player.minionDamage -= 0.33f;
			player.thrownDamage -= 0.33f;
		}
	}
}
