using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.PLunar
{
	public class Vortexlet : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortexlet");
			Tooltip.SetDefault("The devouror may destroy all, but now their power is in your fingers\n35% increased range damage and crit chance");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 34;     
            item.height = 24;   
            item.value = 125000;
            item.rare = 9;

			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.rangedCrit += 35;
			player.rangedDamage += 0.35f;
			
		}
	}
}