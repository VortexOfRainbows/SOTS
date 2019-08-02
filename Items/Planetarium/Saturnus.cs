using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Planetarium
{
	public class Saturnus : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Saturnus");
			Tooltip.SetDefault("A blade that scales in damage orbits you\nIncreases thrown damage by 15% and thrown crit by 10%");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 26;     
            item.height = 38;   
            item.value = 1000000;
            item.rare = 9;
			item.crit = -4;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.thrownDamage += 0.15f;
			player.thrownCrit += 10;
			Probe++;
			if(Probe == 360)
			{
				Probe = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SaturnBlade"), 1, 0, 0);
			}
		}
	}
}