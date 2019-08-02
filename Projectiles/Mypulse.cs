using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 

namespace SOTS.Projectiles 
{    
    public class Mypulse : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Pulse");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(207);
            aiType = 207; 
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.magic = true; 
		}
		
		public override void AI()
        {
			projectile.alpha = 255;
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 6, 6, 220);
Main.dust[num1].noGravity = true;
Main.dust[num1].velocity *= 0.1f;
			
        }
		public override void Kill(int timeLeft)
		{
			
			
		}

	}
}
		
			