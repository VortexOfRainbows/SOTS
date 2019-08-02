using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chess
{    
    public class KingCrossProj : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cross");
			
		}
		
        public override void SetDefaults()
        {
		
		
			projectile.CloneDefaults(199);
			aiType = 199;
			projectile.netImportant = true;
            projectile.width = 34;
            projectile.height = 34; 
            projectile.timeLeft = 30;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
			projectile.alpha = 0;


		}
		public override void AI() 
		{
			Player player = Main.player[projectile.owner];
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 34, 34, 57);
			
			    for(int i = 0; i < 200; i++)
				{
				
				   NPC target = Main.npc[i];

				   float shootToX = target.Center.X - projectile.Center.X;
				   float shootToY = target.Center.Y - projectile.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				   if(distance < 240f && !target.friendly && target.active)
				   {
					   if(projectile.ai[0] > 4f) 
					   {
						   distance = 3f / distance;
			   
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
			   
						   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("TurnTimeBeam"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
						  
						   projectile.ai[0] = 0f;
					   }
				   }
				}
				projectile.ai[0] += 1f;
		}
	}
}
