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
    public class WormBullet : ModProjectile 
    {	int worm = 0;
		int wait = 1;         
				float oldVelocityY = 0;	
				float oldVelocityX = 0;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pinky Worm");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.height = 18;
			projectile.width = 18;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.magic = false;
			projectile.ranged = false;
		}
		public override void AI()
		{
			if(wait == 1)
			{
				wait++;
				oldVelocityY = projectile.velocity.Y;	
				oldVelocityX = projectile.velocity.X;
			}
			worm++;
			if(worm <= 60)
			{
			projectile.velocity.X += oldVelocityY / 30f;
			projectile.velocity.Y += -oldVelocityX / 30f;
			}
			else if(worm >= 61 && worm <= 120)
			{
			projectile.velocity.X += -oldVelocityY / 30f;
			projectile.velocity.Y += oldVelocityX / 30f;
			}
			if(worm >= 120)
			{
			worm = 0;
			}
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 18, 18, 72);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
	}
}
		