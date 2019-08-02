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

namespace SOTS.Projectiles.HolyRelics
{    
    public class TearSplitGrace : ModProjectile 
    {	int bounce = 24;
		int wait = 1;         
				float oldVelocityY = 0;	
				float oldVelocityX = 0;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tear Of Grace");
			
		}
		
        public override void SetDefaults()
        {
			//projectile.CloneDefaults(98);
//aiType = 98; 
			projectile.height = 14;
			projectile.width = 14;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 40;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.magic = true;
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
			projectile.velocity.X += -oldVelocityX / 20f;
			projectile.velocity.Y += -oldVelocityY / 20f;
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 14, 14, 235);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
	}
}
		