using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Eon
{    
    public class Latios : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Latios");
			
		}
		
        public override void SetDefaults()
        {
		
		

projectile.netImportant = true;
			projectile.CloneDefaults(317);
            aiType = 317; //18 is the demon scythe style
			projectile.minion = true;
			projectile.minionSlots = 0;			
            projectile.width = 56;
            projectile.height = 44; 
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = false; 
			projectile.alpha = 0;


		}
		public override void AI() //The projectile's AI/ what the projectile does
		{
    //Making player variable "p" set as the projectile's owner
    Player player  = Main.player[projectile.owner];

    //Factors for calculations
    double deg = (double) projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
    double rad = deg * (Math.PI / 180); //Convert degrees to radians
    double dist = 128; //Distance away from the player
 
    /*Position the player based on where the player is, the Sin/Cos of the angle times the /
    /distance for the desired distance away from the player minus the projectile's width   /
    /and height divided by two so the center of the projectile is at the right place.     */
    projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
    projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
 
    //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
    projectile.ai[1] += 1f;
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
	for(int i = 0; i < 200; i++)
    {
       //Enemy NPC variable being set
       NPC target = Main.npc[i];

       //Getting the shooting trajectory
       float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
       float shootToY = target.position.Y - projectile.Center.Y;
       float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

       //If the distance between the projectile and the live target is active
       if(distance < 720f && !target.friendly && target.active)
       {
           if(projectile.ai[0] > 180f) //Assuming you are already incrementing this in AI outside of for loop
           {
               //Dividing the factor of 3f which is the desired velocity by distance
               distance = 3f / distance;
   
               //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
               shootToX *= distance * 5;
               shootToY *= distance * 5;
   
               //Shoot projectile and set ai back to 0
               Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("DracoOrb"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
              
               projectile.ai[0] = 0f;
           }
       }
    }
    projectile.ai[0] += 1f;
              

           }
	
			
      

			
			  
		}
}

	
	

