using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Terra : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra");
			Tooltip.SetDefault("Massively increases life regen and doubles all non-summon damage when your velocity is halted\nIncreases summon damage by 20%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 42;     
            item.height = 42;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.minionDamage += 0.2f;
				
			if(player.velocity.X == 0 && player.velocity.Y == 0)
			{
				player.lifeRegen += 15;
				player.meleeDamage += 1f;
				player.magicDamage += 1f;
				player.thrownDamage += 1f;
				player.rangedDamage += 1f;
			}		
		}
	}
}
