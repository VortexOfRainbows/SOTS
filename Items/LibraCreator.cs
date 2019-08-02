using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class LibraCreator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Libra Crearor");
			Tooltip.SetDefault("Notices that creator is mispelled\nDoesn't care");
		}
		public override void SetDefaults()
		{
            item.magic = true;   //its a gun so set this to true
            item.width = 14;     //gun image width
            item.height = 26;   //gun image  height
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
 
 
 
 
						WorldGen.PlaceTile(xPosition, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition, yPosition +1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition, yPosition +2, mod.TileType("DevilTile"));
						
						
						WorldGen.PlaceTile(xPosition + 1, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 1, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition + 2, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 2, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition + 3, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 3, yPosition, mod.TileType("DevilTile"));
						
						WorldGen.PlaceTile(xPosition + 4, yPosition, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition - 4, yPosition, TileID.ObsidianBrick);
						
						WorldGen.PlaceTile(xPosition + 1, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 1, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition + 2, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 2, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition + 3, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 3, yPosition + 1, mod.TileType("DevilTile"));
						
						WorldGen.PlaceTile(xPosition + 4, yPosition + 1, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition - 4, yPosition + 1, TileID.ObsidianBrick);
						
						WorldGen.PlaceTile(xPosition + 1, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 1, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition + 2, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 2, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition + 3, yPosition + 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 3, yPosition + 2, mod.TileType("DevilTile"));
						
						WorldGen.PlaceTile(xPosition + 5, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 5, yPosition + 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition + 5, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 5, yPosition, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition + 5, yPosition - 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 5, yPosition - 1, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition + 5, yPosition - 2, mod.TileType("DevilTile"));
						WorldGen.PlaceTile(xPosition - 5, yPosition - 2, mod.TileType("DevilTile"));	
							
						
						WorldGen.PlaceTile(xPosition + 6, yPosition, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition - 6, yPosition, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition + 6, yPosition - 1, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition - 6, yPosition - 1, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition + 6, yPosition - 2, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition - 6, yPosition - 2, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition + 6, yPosition - 3, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition - 6, yPosition - 3, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition + 6, yPosition - 4, TileID.ObsidianBrick);
						WorldGen.PlaceTile(xPosition - 6, yPosition - 4, TileID.ObsidianBrick);
						
						WorldGen.PlaceTile(xPosition + 5, yPosition - 4, 341);
						WorldGen.PlaceTile(xPosition - 5, yPosition - 4, 341);
						WorldGen.PlaceTile(xPosition, yPosition - 1, mod.TileType("DevilAltarTile"));
					
					return false;
					
          
	}
	
}
}
