using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Enigma : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enigma");
			Tooltip.SetDefault("Grants sparatic power boosts\nWhenever it occurs, you will be healed completely, gain a 400% damage boost, and infinite mana for 7 seconds");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 30;     
            item.height = 30;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(Main.rand.Next(7200) == 0)
			{
					player.AddBuff(mod.BuffType("PoweredUp"), 420);
			}
					
		}
	}
}
