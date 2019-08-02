using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class plusBeam : ModProjectile 
    {	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Light");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(158);
            aiType =158; //18 is the demon scythe style
            projectile.width = 34;
            projectile.height = 34; 
            projectile.timeLeft = 150;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 1; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		public override void AI()
		{
			
			timer++;
			if(timer <= 60)
			{
			projectile.scale += 0.01f;
			}
			else
			{
			projectile.scale -= 0.01f;
			}


			
		}
		public override void Kill(int timeLeft)
		{
			
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y,0, 7, 682, (int)(projectile.damage), 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 7, 0, 682, (int)(projectile.damage), 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 0, -7, 682, (int)(projectile.damage), 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, -7, 0, 682, (int)(projectile.damage), 0, 0);

			
		}
		
	}
	
}