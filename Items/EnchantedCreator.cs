using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class EnchantedCreator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Crearor");
			Tooltip.SetDefault("Enchanted Pickaxes are cool");
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
				
                    int xPosition = (int)(vector14.X / 16.0f);
                    int yPosition = (int)(vector14.Y / 16.0f);
 
 
 
 
						WorldGen.PlaceTile(xPosition, yPosition, 1);
						WorldGen.PlaceTile(xPosition, yPosition + 1, 1);
						
						WorldGen.PlaceTile(xPosition +1, yPosition, 1);
						WorldGen.PlaceTile(xPosition -1, yPosition, 1);
						WorldGen.PlaceTile(xPosition +1, yPosition + 1, 1);
						WorldGen.PlaceTile(xPosition -1, yPosition + 1, 1);
						
						WorldGen.PlaceTile(xPosition +2, yPosition, 1);
						WorldGen.PlaceTile(xPosition -2, yPosition, 1);
						WorldGen.PlaceTile(xPosition +3, yPosition, 1);
						WorldGen.PlaceTile(xPosition -3, yPosition, 1);
						
						WorldGen.PlaceTile(xPosition +2, yPosition, 1);
						WorldGen.PlaceTile(xPosition -2, yPosition, 1);
						WorldGen.PlaceTile(xPosition +3, yPosition, 1);
						WorldGen.PlaceTile(xPosition -3, yPosition, 1);
						
						WorldGen.PlaceTile(xPosition, yPosition + 2, 30);
						WorldGen.PlaceTile(xPosition + 1, yPosition + 2, 30);
						WorldGen.PlaceTile(xPosition - 1, yPosition + 2, 30);
						WorldGen.PlaceTile(xPosition + 2, yPosition + 2, 30);
						WorldGen.PlaceTile(xPosition - 2, yPosition + 2, 30);
						WorldGen.PlaceTile(xPosition + 2, yPosition + 1, 30);
						WorldGen.PlaceTile(xPosition - 2, yPosition + 1, 30);
						WorldGen.PlaceTile(xPosition + 3, yPosition + 2, 30);
						WorldGen.PlaceTile(xPosition - 3, yPosition + 2, 30);
						WorldGen.PlaceTile(xPosition + 3, yPosition + 1, 30);
						WorldGen.PlaceTile(xPosition - 3, yPosition + 1, 30);
						WorldGen.PlaceTile(xPosition + 4, yPosition + 2, 30);
						WorldGen.PlaceTile(xPosition - 4, yPosition + 2, 30);
						WorldGen.PlaceTile(xPosition + 4, yPosition + 1, 30);
						WorldGen.PlaceTile(xPosition - 4, yPosition + 1, 30);
						WorldGen.PlaceTile(xPosition + 4, yPosition, 30);
						WorldGen.PlaceTile(xPosition - 4, yPosition, 30);
						WorldGen.PlaceTile(xPosition + 4, yPosition - 1, 30);
						WorldGen.PlaceTile(xPosition - 4, yPosition - 1, 30);
						WorldGen.PlaceTile(xPosition + 4, yPosition - 2, 30);
						WorldGen.PlaceTile(xPosition - 4, yPosition - 2, 30);
						WorldGen.PlaceTile(xPosition + 4, yPosition - 3, 30);
						WorldGen.PlaceTile(xPosition - 4, yPosition - 3, 30);
						WorldGen.PlaceTile(xPosition + 4, yPosition - 4, 30);
						WorldGen.PlaceTile(xPosition - 4, yPosition - 4, 30);
						WorldGen.PlaceTile(xPosition + 4, yPosition - 5, 30);
						WorldGen.PlaceTile(xPosition - 4, yPosition - 5, 30);
						WorldGen.PlaceTile(xPosition + 4, yPosition - 6, 30);
						WorldGen.PlaceTile(xPosition - 4, yPosition - 6, 30);
						WorldGen.PlaceTile(xPosition + 3, yPosition - 6, 30);
						WorldGen.PlaceTile(xPosition - 3, yPosition - 6, 30);
						WorldGen.PlaceTile(xPosition + 2, yPosition - 6, 30);
						WorldGen.PlaceTile(xPosition - 2, yPosition - 6, 30);
						WorldGen.PlaceTile(xPosition + 2, yPosition - 5, 124);
						WorldGen.PlaceTile(xPosition - 2, yPosition - 5, 124);
						WorldGen.PlaceTile(xPosition + 2, yPosition - 4, 124);
						WorldGen.PlaceTile(xPosition - 2, yPosition - 4, 124);
						WorldGen.PlaceTile(xPosition + 2, yPosition - 3, 124);
						WorldGen.PlaceTile(xPosition - 2, yPosition - 3, 124);
						WorldGen.PlaceTile(xPosition + 2, yPosition - 2, 124);
						WorldGen.PlaceTile(xPosition - 2, yPosition - 2, 124);
						WorldGen.PlaceTile(xPosition + 2, yPosition - 1, 124);
						WorldGen.PlaceTile(xPosition - 2, yPosition - 1, 124);
						
						
						WorldGen.PlaceTile(xPosition, yPosition - 1, mod.TileType("EnchantedPickShrineTile"));
					
					return false;
					
          
	}
	
}
}
