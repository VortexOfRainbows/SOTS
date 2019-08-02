using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Luna : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luna");
			Tooltip.SetDefault("May the night guide you\nIncreases damage by 15%, doubles max life, and boosts life regen during the night");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 20;     
            item.height = 32;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(!Main.dayTime)
			{
				player.lifeRegen += 2;
				player.statLifeMax2 *= 2;
				player.meleeDamage += 0.15f;
				player.magicDamage += 0.15f;
				player.thrownDamage += 0.15f;
				player.rangedDamage += 0.15f;
				player.minionDamage += 0.15f;
			}		
		}
	}
}
