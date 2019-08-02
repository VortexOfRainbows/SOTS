using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Aura : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrarian Shadow Aura");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 256;
            projectile.height = 256; 
            projectile.timeLeft = 1;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 255;

		}
		public override void AI() //The projectile's AI/ what the projectile does
		{
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);
			
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);

			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 61);


			
			

			
			  
		}
	}
	
}