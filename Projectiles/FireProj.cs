using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class FireProj : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4; 
            projectile.timeLeft = 1;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = false; 
            projectile.magic = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 255;

		}
		public override void AI() //The projectile's AI/ what the projectile does
		{
			
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 4, 4, 6);
			



			
			

			
			  
		}
	}
	
}