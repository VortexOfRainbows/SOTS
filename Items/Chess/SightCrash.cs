using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class SightCrash : ModItem
	{	 	int up = 0;
			int spin = 0;
			int boom = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Fall");
			Tooltip.SetDefault("Double tap up\nGrants immunity to knockback");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 26;     
            item.height = 32;   
            item.value = 125000;
            item.rare = 7;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			  player.noKnockback = true;
			if(player.controlUp && up <= 9 && up >= 1 && spin < 1 && boom == 0) 
			  {
				  up = 0;
				  player.velocity.Y -= 45;
				  spin = 10;
			  }
			  if(up > 0)
			  {
			  up--;
			  }
			if(player.controlUp) 
			  {
				  up = 10;
			  }
			  
			
			  if(spin > 0)
			  {
				  
				  if(spin == 1)
				  {
					//  player.rotation = 0;
					  boom = 1;
					  player.velocity.Y = 1;
					  
				  }
				  spin--;
//player.rotation++;
			  }
			  if(boom == 1)
			  {
				  if(Main.rand.Next(5) == 0)
				  Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-1,2), -(Main.rand.Next(5)), 227, 55, 0, 0);
			  
				  if(Main.rand.Next(5) == 0)
				  Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-2,3), -(Main.rand.Next(5)), 228, 55, 0, 0);
				  
				  if(Main.rand.Next(5) == 0)
				  Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-2,3), -(Main.rand.Next(5)), 229, 55, 0, 0);
			  
				  player.velocity.Y = Math.Abs(player.velocity.Y);
				  if(player.velocity.Y == 0)
				  {
					  boom = 0;
				  }
			  }
		}
	}
}
