using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Chicken : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chicken");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40; 
            projectile.timeLeft = 6000;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 71; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		public override void Kill(int timeLeft)
		{
			
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, 296, (int)(projectile.damage * 1.5), projectile.knockBack, Main.myPlayer);

			
		}
		
	}
	
}