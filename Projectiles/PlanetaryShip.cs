using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles 
{    
    public class PlanetaryShip : ModProjectile 
    {	int newAlpha = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Ship");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(156);
            aiType = 156; //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.penetrate = -1; 
			projectile.ranged = true;
			projectile.width = 32;
			projectile.height = 32;
			projectile.thrown = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 240;
			
			
		}
		
		public override void AI()
		{ 
		projectile.scale = 1;
		newAlpha++;
		projectile.alpha = 0;
		projectile.alpha += newAlpha;
		int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 32, 32, 160);
			
					
		Main.dust[num1].noGravity = true;
		Main.dust[num1].velocity *= 0.1f;
					projectile.ai[1] += 1f;
			
			 for(int i = 0; i < 200; i++)
				{
				   //Enemy NPC variable being set
				   NPC target = Main.npc[i];

				   //Getting the shooting trajectory
				   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
				   float shootToY = target.position.Y + (float)target.height * 0.5f - projectile.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				   //If the distance between the projectile and the live target is active
				   if(distance < 640f && !target.friendly && target.active)
				   {
					   if(projectile.ai[1] >= 40) //Assuming you are already incrementing this in AI outside of for loop
					   {
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance = 3f / distance;
			   
						   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
			   
						   //Shoot projectile and set ai back to 0
						   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("PlanetaryFlame"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
					   }
				   }
				}
				if(projectile.ai[1] >= 40)
				{
				projectile.ai[1] = 0;	
				}
		}
		public override void Kill(int timeLeft)
		{
			
			//Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0,  696, (int)(projectile.damage * 1f), projectile.knockBack, Main.myPlayer);
			
		}
		
	}
}
		