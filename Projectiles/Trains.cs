using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Trains : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thomas");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20; 
            projectile.timeLeft = 99999;
            projectile.penetrate = 100; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		
		
	}
	
}