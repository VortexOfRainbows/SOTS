using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles 
{    
    public class Brimstone: ModProjectile 
    {	int bounce = 1;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimstone Flame");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.height = 32;
			projectile.width = 32;
			projectile.ranged = true;
			projectile.timeLeft = 72;
			projectile.friendly = false;
			projectile.hostile = true;
		}
		
		public override void AI()
		{ 
			projectile.alpha = 255;
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 32, 32, 235);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
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
		