using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles        //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
 
{
    public class PinkyTurret : ModProjectile
    {	float shootX = 5;
		float shootY = 0;
		float shootX2 = 0;
		float shootY2 = -5;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PinkyTurret");
			
		}
        public override void SetDefaults()
        {
 
            projectile.width = 30; //Set the hitbox width
            projectile.height = 30;   //Set the hitbox heinght
            projectile.hostile = false;    //tells the game if is hostile or not.
            projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            Main.projFrames[projectile.type] = 1;  //this is where you add how many frames u'r projectile has to make the animation
            projectile.timeLeft = 7200;  // this is the projectile life time( 60 = 1 second so 900 is 15 seconds )    
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain
            projectile.sentry = true; //tells the game that this is a sentry
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override void AI()
        {
         
 
            //---------------------------------------------------This make this projectile1 shot another projectile2 to a target if is in between the distance and this projectile1 ------------------------------------------------------------------------

            projectile.rotation += 11.11111111f;   
 
            //Getting the npc to fire at
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
 
                //Getting the shooting trajectory
              float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
              float shootToY = target.position.Y + (float)target.height * 0.5f - projectile.Center.Y;
             float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
 
                //If the distance between the projectile and the live target is active
                if (distance < 1020f && !target.friendly && target.active)  //distance < 520 this is the projectile1 distance from the target if the tarhet is in that range the this projectile1 will shot the projectile2
                {
                    if (projectile.ai[0] > 20f)//this make so the projectile1 shoot a projectile every 2 seconds(60 = 1 second so 120 = 2 seconds)
                    {
                        //Dividing the factor of 2f which is the desired velocity by distance
                        distance = 1.6f / distance;
 
                        //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                        //shootToX *= distance * 3;
                      //  shootToY *= distance * 3;                
                                          //Shoot projectile and set ai back to 0
						if(shootX > 0.1f)	
						{							
						shootX -= 0.1f;
						}
						else 
						{
						shootX = 5;
						}
						if(shootY < 4.9f)	
						{							
						shootY += 0.1f;
						}
						else
						{
						shootY = 0;
						}
						if(shootX2 < 4.9f)	
						{							
						shootX2 += 0.1f;
						}
						else
						{
						shootX2 = 0;
						}
						if(shootY2 < -0.1f)	
						{							
						shootY2 += 0.1f;
						}
						else
						{
						shootY2 = -5;
						}
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootX, shootY, 22, projectile.damage, 0, Main.myPlayer, 0f, 0f);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -shootX, -shootY, 22, projectile.damage, 0, Main.myPlayer, 0f, 0f);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootX2, shootY2, 22, projectile.damage, 0, Main.myPlayer, 0f, 0f);
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -shootX2, -shootY2, 22, projectile.damage, 0, Main.myPlayer, 0f, 0f);
                        Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 24); //24 is the sound, so when this projectile is shot will make that sound
                        projectile.ai[0] = 0f;
                    }
                }
            }
            projectile.ai[0] += 1f;
 
        }
    }
}