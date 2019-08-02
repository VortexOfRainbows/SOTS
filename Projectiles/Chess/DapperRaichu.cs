using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chess
{    
    public class DapperRaichu : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dapperaichu");
			
		}
		
        public override void SetDefaults()
        {
		
		
			projectile.CloneDefaults(199);
			aiType = 199;
			projectile.netImportant = true;
            projectile.width = 46;
            projectile.height = 50; 
            projectile.timeLeft = 30;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
			projectile.alpha = 0;


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
       float shootToX = target.Center.X - projectile.Center.X;
       float shootToY = target.Center.Y - projectile.Center.Y;
       float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

       //If the distance between the projectile and the live target is active
       if(distance < 360f && !target.friendly && target.active)
       {
           if(projectile.ai[0] > 4f) //Assuming you are already incrementing this in AI outside of for loop
           {
               distance = 3f / distance;
   
               shootToX *= distance * 5;
               shootToY *= distance * 5;
   
               Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("TurnTimeBeam"), 1, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
              
               projectile.ai[0] = 0f;
           }
       }
    }
    projectile.ai[0] += 1f;
	
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>(mod);
            if (player.dead)
            {
                modPlayer.DapperChu = false;
            }
            if (modPlayer.DapperChu)
            {
                projectile.timeLeft = 6;
            }
			  
		}
	}
	
}
