using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class SpectreUnicorn : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrarian Patronus");
			
		}
		
        public override void SetDefaults()
        {
		
			
            projectile.CloneDefaults(ProjectileID.ZephyrFish);

            aiType = ProjectileID.ZephyrFish;
            Main.projFrames[projectile.type] = 12;
			projectile.netImportant = true;
            projectile.width = 80;
            projectile.height = 48; 
            projectile.timeLeft = 255;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
			


		}
        public override bool PreAI()
        {
			
            Player player = Main.player[projectile.owner];
			player.bunny = false; // Relic from aiType
            return true;
        }
		public override void AI() //The projectile's AI/ what the projectile does
		{Player player = Main.player[projectile.owner];
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 80, 48, 160);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 80, 48, 160);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 80, 48, 63);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 80, 48, 63);
		
			
		for(int i = 0; i < 200; i++)
		{
		   NPC target = Main.npc[i];

		   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
		   float shootToY = target.position.Y - projectile.Center.Y;
		   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

		   if(distance < 480f && !target.friendly && target.active)
		   {
			   if(projectile.ai[0] > 4f) 
			   {
			   
				   distance = 3f / distance;
	   
				   shootToX *= distance * 5;
				   shootToY *= distance * 5;
	   
				   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("PatronusShot"), 100, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
				  
				   projectile.ai[0] = 0f;
			   }
		    }
		}
    projectile.ai[0] += 1f;
			
			  
            SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>(mod);
            if (player.dead)
            {
                modPlayer.SpectreUnicorn = false;
            }
            if (modPlayer.SpectreUnicorn)
            {
                projectile.timeLeft = 2;
            }
		}
	}
	
}
