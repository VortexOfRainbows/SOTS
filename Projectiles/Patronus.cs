using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Patronus : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrarian Patronus");
			
		}
		
        public override void SetDefaults()
        {
		
		

			projectile.netImportant = true;
            projectile.width = 84;
            projectile.height = 98; 
            projectile.timeLeft = 255;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
			projectile.aiStyle = 0;


		}
		public override void AI() //The projectile's AI/ what the projectile does
		{Player player = Main.player[projectile.owner];
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 84, 98, 63);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 84, 98, 63);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 84, 98, 63);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 84, 98, 63);
			projectile.alpha += 1;
		
			
			    for(int i = 0; i < 200; i++)
				{
				   //Enemy NPC variable being set
				   NPC target = Main.npc[i];

				   //Getting the shooting trajectory
				   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
				   float shootToY = target.position.Y - projectile.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				   //If the distance between the projectile and the live target is active
				   if(distance < 480f && !target.friendly && target.active)
				   {
					   if(projectile.ai[0] > 4f) //Assuming you are already incrementing this in AI outside of for loop
					   {
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance = 3f / distance;
			   
						   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
			   
						   //Shoot projectile and set ai back to 0
						   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("PatronusShot"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
						  
						   projectile.ai[0] = 0f;
					   }
					}
				}
    projectile.ai[0] += 1f;
			
			  
		}
	}
	
}
