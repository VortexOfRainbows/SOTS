using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class MightJump : ModItem
	{	 	int down = 0;
			int spin = 0;
			int boom = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Fall");
			Tooltip.SetDefault("Double tap down\nGrants immunity to knockback");
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
			if(player.controlDown && down <= 9 && down >= 1 && spin < 1 && boom == 0) 
			  {
				  down = 0;
				  player.velocity.Y -= 25;
				  spin = 60;
			  }
			  if(down > 0)
			  {
			  down--;
			  }
			if(player.controlDown) 
			  {
				  down = 10;
			  }
			  
			
			  if(spin > 0)
			  {
				  
				  if(spin == 1)
				  {
					//  player.rotation = 0;
					  boom = 1;
					  player.velocity.X = 0;
					  Projectile.NewProjectile(player.Center.X, player.Center.Y + 240, 0, 0, mod.ProjectileType("MightBubble"), 1, 0, 0);
				  }
				  spin--;
//player.rotation++;
			  }
			  if(boom == 1)
			  {
				  player.velocity.Y = Math.Abs(player.velocity.Y);
				  if(player.velocity.Y == 0)
				  {
					  boom = 0;
				  }
			  }
		}
	}
}
