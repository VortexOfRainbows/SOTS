using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.PLunar
{
	public class SolarBelt : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solabelt");
			Tooltip.SetDefault("The power of the sun sits on your waist\n25% increased melee damage and crit chance");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 46;     
            item.height = 24;   
            item.value = 125000;
            item.rare = 10;

			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.meleeCrit += 25;
			player.meleeDamage += 0.25f;
			Probe++;
			if(Probe == 12)
			{
				Probe = 0;
				
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, 696, 111, 0, 0);
			}
		}
	}
}