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

namespace SOTS.Projectiles.Chess
{    
    public class MightyBlade : ModProjectile 
    {	int bounce = 2000;
		float up = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mighty Blade");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 28;
			projectile.width = 28;
			projectile.ranged = true;
			projectile.friendly = false;
			projectile.timeLeft = 900;
			projectile.hostile = true;
			projectile.tileCollide = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			
			bounce--;
			if (bounce <= 0)
			{
				projectile.Kill();
			}
			else
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X + Main.rand.Next(-2, 3);
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y + Main.rand.Next(-2, 3); 
				}
				Main.PlaySound(SoundID.Item10, projectile.position);
			up += 0.02f;
			projectile.velocity.Y *= up;
			projectile.velocity.X *= up;
			
			return false;
		}
		public override void AI()
		{
			projectile.rotation += 1f;
		}
		
	}
}
		