using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class TinyPlanet : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Planet");
			Tooltip.SetDefault("Surrounds you with orbital projectiles");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 34;     
            item.height = 34;   
            item.value = 125000;
            item.rare = 3;

			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Probe++;
			if(Probe == 45)
			{
				Probe = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), 20, 0, player.whoAmI);
			}
		}
	}
}