using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class PlanetariumTear2 : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32; 
            projectile.timeLeft = 360;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		public override void AI()
		{
			for(int i = 0; i < 200; i++)
			{
				NPC target = Main.npc[i];
				float dX = target.Center.X - projectile.Center.X;
				float dY = target.Center.Y - projectile.Center.Y;
				float distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
				
				float approxSize = (float) Math.Sqrt((double)(target.width * target.width + target.height * target.height));
				if(distance < 75f + approxSize && !target.friendly && target.active && !target.boss)
				{
					target.position.X = projectile.Center.X + (target.position.X - target.Center.X);
					target.position.Y = projectile.Center.Y - (target.Center.Y - target.position.Y);
				}
				
				
			}
			
		Player player  = Main.player[projectile.owner];
		Vector2 vector14;
					
						if (player.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;

		double deg = (double) projectile.ai[1];
		double rad = deg * (Math.PI / 180);
		double dist = 16;
	
		projectile.position.X = vector14.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
		projectile.position.Y = vector14.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
 
		projectile.ai[1] += 1f;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			/*
            target.immune[projectile.owner] = 0;
			target.life += damage;
			target.position.X = projectile.Center.X + (target.position.X - target.Center.X);
			target.position.Y = projectile.Center.Y - (target.Center.Y - target.position.Y);
			target.rotation += 0.01f;
			*/
        }


		
		
		
	}
	
}