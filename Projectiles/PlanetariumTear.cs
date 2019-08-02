using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class PlanetariumTear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32; 
            projectile.timeLeft = 720;
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
				if(distance < 40f + approxSize && !target.friendly && target.active && !target.boss)
				{
					target.position.X = projectile.Center.X + (target.position.X - target.Center.X);
					target.position.Y = projectile.Center.Y - (target.Center.Y - target.position.Y);
				}
			}
			
			Player player  = Main.player[projectile.owner];

			double deg = (double) projectile.ai[1]; 
			double rad = deg * (Math.PI / 180); 
			double dist = 256;
		 
			projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
			projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
		 
			projectile.ai[1] += 0.5f;
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