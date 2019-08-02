using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Planetarium
{
	public class Gravity : ModItem
	{int Probe = -1;
	int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbit");
			Tooltip.SetDefault("Surrounds you with 4 rings and your cursor with 1\nThe rings will pick up enemies");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 26;     
            item.height = 42;   
            item.value = 1000000;
            item.rare = 9;
			item.crit = -4;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Probe++;
			Probe2++;
			if(Probe == 180)
			{
				Probe = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("PlanetariumTear"), 0, 0, 0);
			}
			if(Probe2 == 360)
			{
				Probe2 = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("PlanetariumTear2"), 0, 0, 0);
			}
		}
	}
}