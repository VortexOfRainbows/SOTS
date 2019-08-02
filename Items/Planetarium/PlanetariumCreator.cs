using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class PlanetariumCreator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetarium Creator");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.magic = true;   //its a gun so set this to true
            item.width = 14;     //gun image width
            item.height = 34;   //gun image  height
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
			for(int i = 0; i < 1; i++) //Grass islands
			{ 
				
				float scaleDistance = 1 + i * 0.25f;
					int newXpos = xPosition;
					int newYpos = yPosition;
					
						if(i % 2 == 0)
						{
						scaleDistance = 1 + i * 0.25f;
						newXpos = xPosition;
						}
						else
						{
						scaleDistance = 1 + (i - 1) * 0.25f;
						newXpos = xPosition;
						}
						
						newYpos = yPosition + Main.rand.Next((int)(-10 * i), (int)(10 * i + 1));
					
					if(newXpos - 20 < 0)
					{
						newXpos = xPosition + 30 + Main.rand.Next((int)(12 * i + 5));
					}
					if(newXpos + 20 > Main.maxTilesX)
					{
						newXpos = xPosition - 30 - Main.rand.Next((int)(12 * i + 5));
					}
						int radius2 = 10;     //this is the explosion radius, the highter is the value the bigger is the explosion
 
				for (int x = -radius2; x <= radius2; x++)
				{
					for (int y = -radius2; y <= radius2; y++)
					{
						int xPosition3 = (int)(x + newXpos);
						int yPosition3 = (int)(y + newYpos);
	 
						if (Math.Sqrt(x * x + y * y) <= radius2 + 0.5)   //this make so the explosion radius is a circle
						{
							WorldGen.KillTile(xPosition3 , yPosition3 , false, false, false);  //this make the explosion destroy tiles  
						}
					}
				}
				
				int[,] _grassIsland = {
				{0,0,0,0,0,0,2,2,0},
				{0,0,0,0,0,2,2,2,0},
				{0,0,0,0,2,2,5,2,0}, 
				{0,0,2,2,2,2,2,2,0}, 
				{3,3,2,2,2,2,2,2,2}, 
				{1,1,1,1,1,1,1,1,2}, 
				{5,1,1,1,1,1,1,1,0}, 
				{1,1,1,1,1,1,1,0,0}, 
				{1,1,1,1,1,1,0,0,0}, 
				{1,1,1,1,0,0,0,0,0}, 
			};
					int Xs = newXpos;
					int Ys = newYpos;
					Ys -= (int)(.5f * _grassIsland.GetLength(0));
			
				for (int y = 0; y < _grassIsland.GetLength(0); y++) {
					for (int x = 0; x < _grassIsland.GetLength(1); x++) {
						int k = Xs + x;
						int l = Ys + y;
						int k2 = Xs - x;
						int l2 = Ys - y;
						if (WorldGen.InWorld(k, l, 30)) {
							Tile tile = Framing.GetTileSafely(k, l);
							Tile tile2 = Framing.GetTileSafely(k2, l);
							switch (_grassIsland[y, x]) {
								case 0:
									//tile.active(false);
									//tile2.active(false);
									break;
								case 1:
									tile.type = 189; //cloud
									tile.active(true);
									tile2.type = 189;
									tile2.active(true);
									break;
								case 2:
									tile.type = 223; //titanium
									tile.active(true);
									tile2.type = 223;
									tile2.active(true);
									break;
								case 3:
									tile.type = 2; //grass
									tile.active(true);
									tile2.type = 2;
									tile2.active(true);
									break;
								case 4:
									tile.type = 20; //acorn
									tile.active(true);
									tile2.type = 20;
									tile2.active(true);
									break;
								case 5:
									tile.type = (ushort)mod.TileType("EmptyPlanetariumBlock");
									tile.active(true);
									tile2.type = (ushort)mod.TileType("EmptyPlanetariumBlock");
									tile2.active(true);
									break;
							}
						}
					}
				}
			}
 
			
					return false;
					
          }  
	}
}
