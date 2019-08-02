using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Chess
{
	public class FrightBlood : ModItem
	{int Probe = -1;
	int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Vial");
			Tooltip.SetDefault("Every projectiled attack has a minor chance to lifesteal\nMelee swings will always lifesteal\nMelee projectiles have no bonus chance\nE");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 36;     
            item.height = 36;   
            item.value = 125000;
            item.rare = 9;

			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
					player.AddBuff(mod.BuffType("BloodySapping"), 300);
			
		}
	}
}