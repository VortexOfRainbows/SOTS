using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Vibrant 
{    
    public class VibrantBolt : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Bolt");
		}
        public override void SetDefaults()
        {
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.width = 14;
			projectile.height = 14;
			projectile.alpha = 255;
			projectile.timeLeft = 20;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 7; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 44);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.2f;
				Main.dust[num1].alpha = 200;
			}
		}
		public override void AI()
		{
			projectile.alpha -= 30;
			if (Main.rand.NextBool(3))
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 44);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].alpha = 200;
			}

			projectile.rotation = projectile.velocity.ToRotation();
		}
	}
}
		
			