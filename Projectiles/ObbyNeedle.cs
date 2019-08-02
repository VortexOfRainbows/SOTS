using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 

namespace SOTS.Projectiles 
{    
    public class ObbyNeedle : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("needled lol kys");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
            projectile.width = 6;
            projectile.height = 6; 
            projectile.timeLeft = 100000;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
			projectile.alpha = 255;
            Main.projFrames[projectile.type] = 4;
		}
		
		public override void AI()
        {
			projectile.alpha = 255;
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 6, 6, 62);
			
        }
		public override void Kill(int timeLeft)
		{
			
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 6, 6, 62);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 6, 6, 62);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 6, 6, 62);
		}

	}
}
		
			