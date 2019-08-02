using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class QueenHolyGuard : ModItem
	{	 	int left = 0;
			int right = 0;
			int spin = 0;
			int boom = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heaven's Guard");
			Tooltip.SetDefault("All stats up!\nMax life increased by 40\nAll damage and crit chanced increased by 7%\nThrown velocity, melee speed, and movement speed increased by 25%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 34;     
            item.height = 36;   
            item.value = 125000;
            item.rare = 7;
			item.accessory = true;
			item.expert = true;
			item.defense = 5;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			  player.statLifeMax2 += 40;
			  player.meleeDamage += 0.07f;
			  player.magicDamage += 0.07f;
			  player.rangedDamage += 0.07f;
			  player.minionDamage += 0.07f;
			  player.thrownDamage += 0.07f;
			  player.thrownVelocity += 0.25f;
			  player.meleeSpeed += 0.25f;
			  player.magicCrit += 7;
			  player.meleeCrit += 7;
			  player.thrownCrit += 7;
			  player.rangedCrit += 7;
			  player.moveSpeed += 0.25f;
			  
			  
			  
			  
			  }
		
	}
}
