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
    public class Heartdrop : ModProjectile 
    {	int wait = 0;
		int penetrateAmount = 6;
		float speedY = 0;
		float speedX = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heartdrop");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.timeLeft = 900;
			projectile.width = 4;
			projectile.height = 4;
			projectile.penetrate = 5;


		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(penetrateAmount == 6)
			{
				speedY = projectile.velocity.Y;
				speedX = projectile.velocity.X;
			}
			penetrateAmount = 5;
			if(target.life <= 0)
			{		
					Main.player[projectile.owner].statLife += 8;
					Main.player[projectile.owner].HealEffect(8);
					projectile.Kill();
			}
            
		}
		public override void AI()
		{projectile.alpha = 255;
			wait += 1;
			if(penetrateAmount != 6)
			{
				projectile.velocity.Y = speedY;
				projectile.velocity.X = speedX;
			}
			if(wait >= 16)
			{
				for(int i = 0; i < 4; i++)
				{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 4, 4, 60);

				
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				}
			}
			
			
			
			
			

}
	}
}
		
			