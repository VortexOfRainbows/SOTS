using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 

namespace SOTS.Projectiles 
{    
    public class BloodSpike : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spilt Blood");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(24);
            aiType = 24; //18 is the demon scythe style
            projectile.width = 6;
            projectile.height = 6; 
            projectile.timeLeft = 6000;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = false; 
            projectile.melee = true; 
			projectile.alpha = 100;
            Main.projFrames[projectile.type] = 8;
		}
		
		public override void AI()
        {
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 6, 6, 235);
Main.dust[num1].noGravity = true;
Main.dust[num1].velocity *= 0.1f;
			
        }
		public override void Kill(int timeLeft)
		{
			
			
		}

	}
}
		
			