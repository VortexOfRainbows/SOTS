using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Nova : ModItem
	{	 	int down = 0;
			int right = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova");
			Tooltip.SetDefault("Tap down and up at the same time to radiate cosmic energy at the cost of your health\nThe energy is pure stardust and will form into random blocks\nInflicts cosmic radiation, destroying all defense off the enemy and removing immunity frames\nDefense eradication may persist after death");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 30;     
            item.height = 22;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			if(player.controlDown && player.controlUp) 
			  {
				  if(player.statLife > 1)
				  {
				  player.statLife--;
				  }
                  Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5), mod.ProjectileType("Nova"),1, 0, 0);
                  Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5), mod.ProjectileType("Nova"),1, 0, 0);
                  Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5), mod.ProjectileType("Nova"),1, 0, 0);
			  }
			  
		}
	}
}
