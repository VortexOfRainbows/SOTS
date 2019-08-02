using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Margrit
{    
    public class MargritBoomer : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Boomer");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 34; 
            projectile.timeLeft = 7200;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 3; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void AI()
		{
			for(int i = 0; i < 1000; i++)
				{
					Projectile reflectProjectile = Main.projectile[i];
					
						float dX = reflectProjectile.Center.X - projectile.Center.X;
						float dY = reflectProjectile.Center.Y - projectile.Center.Y;
						float distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
					if(distance < 24f && reflectProjectile.hostile && !reflectProjectile.friendly)
					{
						reflectProjectile.friendly = true;
						reflectProjectile.hostile = false;
						reflectProjectile.velocity.X *= -1;
						reflectProjectile.velocity.Y *= -1;
					}
				}
		}
	}
	
}