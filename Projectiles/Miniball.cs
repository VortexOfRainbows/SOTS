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
    public class Miniball : ModProjectile 
    {	
		int bounce = 999;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miniball");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 2;
			projectile.thrown = false;
			projectile.friendly = true;
			projectile.width = 26;
			projectile.height = 26;
			projectile.timeLeft = 960;
			projectile.penetrate = 4;
			projectile.tileCollide = true;
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