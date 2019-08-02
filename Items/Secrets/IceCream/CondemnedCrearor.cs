using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets.IceCream
{
	public class CondemnedCrearor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Condemned Crearor");
			Tooltip.SetDefault("this is a meme");
		}
		public override void SetDefaults()
		{
            item.magic = true;   //its a gun so set this to true
            item.width = 22;     //gun image width
            item.height = 24;   //gun image  height
            item.useTime = 15;  //how fast 
            item.useAnimation = 15;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 30000;
            item.rare = 6;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
            item.shoot =  mod.ProjectileType("PikeProj"); 
            item.shootSpeed = 24;

		}
          public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
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
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0, type, damage, 1, Main.myPlayer, 0.0f, 1);
				
                    int radius = 6;     //this is the explosion radius, the highter is the value the bigger is the explosion
 
						
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + vector14.X / 16.0f);
                    int yPosition = (int)(y + vector14.Y / 16.0f);
 
                    if (Math.Sqrt(x * x + y * y) <= radius + 0.5)   //this make so the explosion radius is a circle
                    {
                        
                        WorldGen.KillTile(xPosition, yPosition, false, false, false);  //this make the explosion destroy tiles  
						WorldGen.PlaceTile(xPosition, yPosition, mod.TileType("IceCreamBrickTile"));
					}
				}
			}
 
                    int xPosition2 = (int)(vector14.X / 16.0f);
                    int yPosition2 = (int)(vector14.Y / 16.0f);
                        WorldGen.KillTile(xPosition2, yPosition2, false, false, false);  //this make the explosion destroy tiles  
						WorldGen.PlaceTile(xPosition2, yPosition2, mod.TileType("IceCreamBottleTile"));
 
						
					
					return false;
					
          
	}
	
}
}
