using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class SpiritBlade : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Blade");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 34;
			projectile.height = 36;
            projectile.melee = true;
			projectile.penetrate = 3;
			projectile.ranged = false;
			projectile.alpha = 60; 
			projectile.friendly = true;
			projectile.timeLeft = 12;
			projectile.tileCollide = false;
		}
		public override void AI()
        {
			projectile.alpha += 18;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(45);
			projectile.spriteDirection = 1;
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 21);
			Main.dust[num1].noGravity = true;
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 15; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 21);
			Main.dust[num1].noGravity = true;
			}
		}
	}
}