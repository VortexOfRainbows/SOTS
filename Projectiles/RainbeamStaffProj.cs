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
    public class RainbeamStaffProj : ModProjectile 
    {	int bounce = 8;
		int wait = 1;              
		float Speed = 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rainbow Beam");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(294);
            aiType = 294; 
			projectile.ranged = true;
			projectile.timeLeft = 9000;
		}
		
		public override void AI()
		{ 
		projectile.alpha = 255;
			
}public override bool OnTileCollide(Vector2 oldVelocity)
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
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				Main.PlaySound(SoundID.Item10, projectile.position);
				
				
			
				return false;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 1;
        }
	}
}
		