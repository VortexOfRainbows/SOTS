using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Strike : ModItem
	{	 	int left = 0;
			int down = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Comet");
			Tooltip.SetDefault("Double tap down to teleport to your cursor\nTeleporting gives 4 seconds of immunity and has a 10 second cooldown");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 38;     
            item.height = 38;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
				Vector2 vector14;
					
						if (player.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0, mod.ProjectileType("Telecursor"), 1, 1, 0, 0.0f, 1);
            
					
			 
			
			if(player.controlDown && down <= 9 && down >= 1) 
			  {
				  if(player.FindBuffIndex(mod.BuffType("PlanetariumCooldown")) <= -1)
				  {
				player.immune = true;
				player.AddBuff(mod.BuffType("Immune"), 240);
				player.AddBuff(mod.BuffType("PlanetariumCooldown"), 600);
				  player.position.X = vector14.X - (player.width / 2);
				  player.position.Y = vector14.Y - (player.height / 2);
				  }
				  down = 0;
              }
			  if(down > 0)
			  {
			  down--;
			  }
			if(player.controlDown) 
			  {
				  down = 10;
			  }
		}
	}
}
