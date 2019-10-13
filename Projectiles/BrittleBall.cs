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
    public class BrittleBall : ModProjectile 
    {	
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle Ball");	
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.penetrate = 15;
			projectile.alpha = 0;
			projectile.width = 38;
			projectile.height = 38;
			projectile.timeLeft = 960;
			projectile.melee = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			projectile.timeLeft -= 60;
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