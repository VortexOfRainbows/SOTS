using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class PikeProj : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("error");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1; 
            projectile.timeLeft = 1;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 1; //18 is the demon scythe style
			projectile.alpha = 255;
		}
		

	}
}
		
			