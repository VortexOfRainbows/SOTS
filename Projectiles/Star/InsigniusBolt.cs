using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Star
{    
    public class InsigniusBolt : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Insignius");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616;
			projectile.alpha = 255;
			projectile.timeLeft = 120;
			projectile.width = 4;
			projectile.height = 4;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			projectile.alpha = 255;
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, 235);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
	}
}
		
			