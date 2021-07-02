using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Trains : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thomas the Tank Engine");
		}
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20; 
            projectile.timeLeft = 99999;
            projectile.penetrate = 100; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 0; 
			projectile.alpha = 100;
		}
	}
}