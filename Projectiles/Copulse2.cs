using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 

namespace SOTS.Projectiles 
{    
    public class Copulse2 : ModProjectile 
    {	int bounce = 2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Pulse");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(357);
            aiType = 357; 
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.magic = true; 
			projectile.width = 6;
			projectile.height = 6;
			projectile.timeLeft = 6000;
		}
		
		public override void AI()
        {
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 6, 6, 180);
Main.dust[num1].noGravity = true;
Main.dust[num1].velocity *= 0.1f;
			projectile.alpha = 255;
			
        }
		public override void Kill(int timeLeft)
		{
			
			
		}public override bool OnTileCollide(Vector2 oldVelocity)
		{	
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

}
}
		
			