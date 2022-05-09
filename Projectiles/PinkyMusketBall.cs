using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class PinkyMusketBall : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pinky Shot");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(616);
            aiType = 616;
			Projectile.alpha = 255;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.timeLeft = 3600;
			Projectile.ranged = true;
		}
		public override void AI()
		{	
			Projectile.alpha = 255;
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, 72);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
	}
}
		
			