using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Minions
 
{
    public class PinkyTurret : ModProjectile
    {	float shootX = 5;
		float shootY = 0;
		float shootX2 = 0;
		float shootY2 = -5;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Turret");
			
		}
        public override void SetDefaults()
        {
 
            projectile.width = 30; 
            projectile.height = 30; 
            projectile.hostile = false; 
            projectile.friendly = false;
            projectile.ignoreWater = true;  
            Main.projFrames[projectile.type] = 1; 
            projectile.timeLeft = 7200;    
            projectile.penetrate = -1;
            projectile.tileCollide = true; 
            projectile.sentry = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override void AI()
        {
            projectile.rotation += 0.11f;   
 
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
 
				float shootToX = target.Center.X - projectile.Center.X;
				float shootToY = target.Center.Y - projectile.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
 
                if (distance < 1020f && !target.friendly && target.active)
                {
                    if (projectile.ai[0] > 24f)
                    {
                        distance = 1.6f / distance;
 
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
						
						if(projectile.owner == Main.myPlayer)
						{
								Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootX, shootY, 22, projectile.damage, 0, Main.myPlayer, 0f, 0f);
								Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -shootX, -shootY, 22, projectile.damage, 0, Main.myPlayer, 0f, 0f);
								Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootX2, shootY2, 22, projectile.damage, 0, Main.myPlayer, 0f, 0f);
								Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -shootX2, -shootY2, 22, projectile.damage, 0, Main.myPlayer, 0f, 0f);
						}
						Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 21);  //21 is water cast sound
                        projectile.ai[0] = 0f;
                    }
                }
            }
            projectile.ai[0] += 1f;
        }
    }
}