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
    public class PinkBullet : ModProjectile 
    {	int bounce = 24;
		int wait = 1;         
				float oldVelocityY = 0;	
				float oldVelocityX = 0;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Pinky");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(98);
            aiType = 98; 
			projectile.height = 20;
			projectile.width = 20;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 360;
			projectile.tileCollide = false;
			projectile.hostile = true;
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
			projectile.velocity.X += -oldVelocityX / 90f;
			projectile.velocity.Y += -oldVelocityY / 90f;
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 20, 20, 72);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
	}
}
		