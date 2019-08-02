using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chess
{    
    public class SnowTurtleTem : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Turtle Tem");
			
		}
		
        public override void SetDefaults()
        {
		
		
            projectile.CloneDefaults(ProjectileID.ZephyrFish);

            aiType = ProjectileID.ZephyrFish;
			projectile.netImportant = true;
            projectile.width = 56;
            projectile.height = 34; 
            projectile.timeLeft = 30;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.magic = true; 
			projectile.alpha = 0;
            Main.projFrames[projectile.type] = 9;


		}
        public override bool PreAI()
        {
			
            Player player = Main.player[projectile.owner];
			player.bunny = false; // Relic from aiType
            return true;
        }
		public override void AI() //The projectile's AI/ what the projectile does
		{Player player = Main.player[projectile.owner];
			
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
           if(projectile.ai[0] > 4f) //Assuming you are already incrementing this in AI outside of for loop
           {
               //Dividing the factor of 3f which is the desired velocity by distance
               distance = 3f / distance;
   
               //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
               shootToX *= distance * 5;
               shootToY *= distance * 5;
   
               //Shoot projectile and set ai back to 0
               Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Main.rand.Next(-900,901)/100), (float)(Main.rand.Next(-900,901)/100), mod.ProjectileType("FriendlyIceSpike"), 36, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
               Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Main.rand.Next(-900,901)/100), (float)(Main.rand.Next(-900,901)/100), mod.ProjectileType("FriendlyIceSpike"), 36, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
               Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Main.rand.Next(-900,901)/100), (float)(Main.rand.Next(-900,901)/100), mod.ProjectileType("FriendlyIceSpike"), 36, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
              
               projectile.ai[0] = 0f;
           }
       }
    }
    projectile.ai[0] += 1f;
	
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>(mod);
            if (player.dead)
            {
                modPlayer.TurtleTem = false;
            }
            if (modPlayer.TurtleTem)
            {
                projectile.timeLeft = 6;
            }
			  
			
			
			
			
		}
	}
	
}
