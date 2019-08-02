using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Planetarium
{
	public class Juno : ModItem
	{int Probe = -1;
	int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Juno");
			Tooltip.SetDefault("4 Asteroids rotate around you cursor, throwing back projectiles");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 26;     
            item.height = 34;   
            item.value = 1000000;
            item.rare = 9;
			item.crit = -4;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			Probe2++;
			
			if(Probe2 == 90)
			{
				Probe2 = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Asteroid"), 1, 0, 0);
			}
		}
	}
}