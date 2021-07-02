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
			projectile.CloneDefaults(616);
            aiType = 616;
			projectile.alpha = 255;
			projectile.width = 4;
			projectile.height = 4;
			projectile.timeLeft = 3600;
			projectile.ranged = true;
		}
		public override void AI()
		{	
			projectile.alpha = 255;
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, 72);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
	}
}
		
			