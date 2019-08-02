using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class FrightHopper : ModItem
	{	 	int left = 0;
			int right = 0;
			int spin = 0;
			int boom = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Fall");
			Tooltip.SetDefault("Double tap slam\nGrants immunity to knockback");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 36;     
            item.height = 36;   
            item.value = 125000;
            item.rare = 7;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			  player.noKnockback = true;
			if(player.controlLeft && left <= 9 && left >= 1 && spin < 1 && boom == 0) 
			  {
				  left = 0;
				  player.velocity.X -= 8;
				  player.velocity.Y -= 15;
				  spin = 60;
			  }
			  if(left > 0)
			  {
			  left--;
			  }
			if(player.controlLeft) 
			  {
				  left = 10;
			  }
			  
			if(player.controlRight && right <= 9 && right >= 1 && spin < 1 && boom == 0) 
			  {
				  right = 0;
				  player.velocity.X += 8;
				  player.velocity.Y -= 15;
				  spin = 60;
			  }
			  if(right > 0)
			  {
			  right--;
			  }
			if(player.controlRight) 
			  {
				  right = 10;
			  }
			  if(spin > 0)
			  {
				  
				  if(spin == 1)
				  {
					//  player.rotation = 0;
					  boom = 1;
					  player.velocity.X = 0;
				  }
				  spin--;
//player.rotation++;
			  }
			  if(boom == 1)
			  {
				  player.velocity.Y = Math.Abs(player.velocity.Y);
				  if(player.velocity.Y == 0)
				  {
				  player.velocity.X = 0;
				  player.velocity.Y = 0;
					  boom = 0;
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				  }
			  }
		}
	}
}
