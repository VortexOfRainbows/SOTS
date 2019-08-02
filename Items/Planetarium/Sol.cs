using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Sol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sol");
			Tooltip.SetDefault("May the light guide you\nIncreases damage by 12%, increases max health, and boosts life regen during the day");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 26;     
            item.height = 26;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(Main.dayTime)
			{
				player.lifeRegen += 1;
				player.statLifeMax2 += player.statLifeMax2 - 80;	
				player.meleeDamage += 0.12f;
				player.magicDamage += 0.12f;
				player.thrownDamage += 0.12f;
				player.rangedDamage += 0.12f;
				player.minionDamage += 0.12f;
			}		
		}
	}
}
