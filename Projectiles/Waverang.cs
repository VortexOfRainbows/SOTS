using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Waverang : ModProjectile 
    {	int shootTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Waverang");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 44; 
            projectile.timeLeft = 3600;
            projectile.penetrate = 7; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 3; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{
			
			

			
		}
		public override void AI()
		{
			if(Main.rand.Next(40) == 4)
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), 263, (int)(projectile.damage * 1), 2f, 0);
		
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 44, 44, 229);
		
										shootTimer++;
										float minDist = 360;
										int target2 = -1;
										float dX = 0f;
										float dY = 0f;
										float distance = 0;
										float speed = 10f;
										if(projectile.friendly == true && projectile.hostile == false)
										{
											for(int j = 0; j < Main.npc.Length - 1; j++)
											{
												NPC target = Main.npc[j];
												if(!target.friendly && target.dontTakeDamage == false)
												{
													dX = target.Center.X - projectile.Center.X;
													dY = target.Center.Y - projectile.Center.Y;
													distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
													if(distance < minDist)
													{
														minDist = distance;
														target2 = j;
													}
												}
											}
											if(shootTimer >= 30)
											{
												if(target2 != -1)
												{
												NPC toHit = Main.npc[target2];
													if(toHit.active == true)
													{
														
													dX = toHit.Center.X - projectile.Center.X;
													dY = toHit.Center.Y - projectile.Center.Y;
													distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
													speed /= distance;
												   
													Vector2 shootTo = new Vector2(dX * speed, dY * speed);
													
													Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootTo.X, shootTo.Y, mod.ProjectileType("MargritBoltFriendly"), projectile.damage, 2f, Main.myPlayer, 0f, 0f);
													shootTimer = 0;
													}
												}
											}
										}
										
										
		
		}
	}
	
}