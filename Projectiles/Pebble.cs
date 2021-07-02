using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Pebble : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pebble"); //It's a fucking rock what do you want from me?
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(311);
            aiType = 311; 
			projectile.penetrate = 1; 
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 1);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 12; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 1);
				Main.dust[num1].noGravity = true;
			}
		}
	}
}
		