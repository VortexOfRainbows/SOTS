using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class FeatherShot : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("FeatherShot");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10; 
            projectile.timeLeft = 210;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 1; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		
		public override void AI()
        {
            projectile.type = 38; //This is the demon scythe projectile ID
        }

	}
}
		
			