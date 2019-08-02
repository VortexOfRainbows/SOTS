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
    public class BouncingBrim : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimstone Flame");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.height = 16;
			projectile.width = 16;
			projectile.ranged = true;
			projectile.timeLeft = 360;
			projectile.friendly = false;
			projectile.hostile = true;
		}
		
		public override void AI()
		{ 
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 16, 16, 235);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}	
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			bounce--;
			if (bounce <= 0)
			{
				projectile.Kill();
			}
			else
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				Main.PlaySound(SoundID.Item10, projectile.position);
			
			return false;
		}
	}
}
		