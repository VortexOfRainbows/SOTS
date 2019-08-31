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

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseBall : ModProjectile 
    {	          
		int bounce = 999;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 18;
			projectile.width = 18;
			projectile.friendly = false;
			projectile.timeLeft = 7200;
			projectile.hostile = true;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			projectile.alpha -= 1;
			projectile.rotation += Main.rand.Next(-3,4);
			if(projectile.timeLeft <= 200)
			{
				projectile.alpha += 2;
			}
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 8, 8, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].alpha = projectile.alpha;
			
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			projectile.timeLeft -= 60;
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
			return false;
		}
	}
}
		