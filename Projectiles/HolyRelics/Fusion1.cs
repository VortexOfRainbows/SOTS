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

namespace SOTS.Projectiles.HolyRelics
{    
    public class Fusion1 : ModProjectile 
    {	int worm = 0;
		int wait = 1;       
		int boom = 0;
		int kill = 0;
		Vector2 oldVelocity;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fusion Bolt");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.height = 10;
			projectile.width = 10;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 900;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.magic = false;
			projectile.ranged = true;
		}
		public override void AI()
		{
			
			if(kill > 0)
			{
				projectile.Kill();
			}
			if(wait == 1)
			{
				wait++;
				oldVelocity = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(0));
			}
			worm++;
			if(worm <= 60)
			{
			projectile.velocity.X += oldVelocity.Y / 30f;
			projectile.velocity.Y += -oldVelocity.X / 30f;
			}
			else if(worm >= 61 && worm <= 120)
			{
			projectile.velocity.X += -oldVelocity.Y / 30f;
			projectile.velocity.Y += oldVelocity.X / 30f;
			}
			if(worm >= 120)
			{
			worm = 0;
			boom++;
			}
			
			if(boom >= 1)
			{
				for(int i = 0; i < 1000; i++)
				{
					if(Main.projectile[i].type == mod.ProjectileType("Fusion2"))
					{
						Projectile proj2 = Main.projectile[i];
						if(projectile.Center.X + 8 > proj2.Center.X && projectile.Center.X - 8 < proj2.Center.X && projectile.Center.Y + 8 > proj2.Center.Y && projectile.Center.Y - 8 < proj2.Center.Y)
						{	
										  kill = 1;
							projectile.ai[1] = 0;
							 for(int j = 0; j < 200; j++)
								{
								   //Enemy NPC variable being set
								   NPC target = Main.npc[j];

								   //Getting the shooting trajectory
								   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
								   float shootToY = target.position.Y + (float)target.height * 0.5f - projectile.Center.Y;
								   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

								   //If the distance between the projectile and the live target is active
								   if(distance < 300f && !target.friendly && target.active)
								   {
									   if(projectile.ai[1] != 1) //Assuming you are already incrementing this in AI outside of for loop
									   {
										   //Dividing the factor of 3f which is the desired velocity by distance
										   distance = 1.8f / distance;
							   
										   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
										   shootToX *= distance * 5;
										   shootToY *= distance * 5;
							   
										   //Shoot projectile and set ai back to 0
										   if(Main.myPlayer == projectile.owner)
										   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("Fusion1Bolt"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
									   
										  projectile.ai[1] = 1;
									   }
								   }
								}
							
						}
					}
				}
			}
			for(int i = 0; i < 1 + boom; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 72);
			
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
		}
	}
}
		