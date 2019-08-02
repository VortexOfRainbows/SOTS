using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles 
{    
    public class PinkyMusketBall : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pinky Shot");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.width = 1;
			projectile.height = 1;
			projectile.timeLeft = 3600;
			projectile.thrown = true;
			projectile.ranged = true;

		}
		public override void AI()
		{projectile.alpha = 255;
			wait += 1;
			if(wait >= 4)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, 72);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
			
			
			
			
			

}
	}
}
		
			