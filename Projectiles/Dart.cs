using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Dart : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("It's nerf or nothin' motherfucker");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
			projectile.alpha = 0;
			projectile.timeLeft = 7200;
			projectile.aiStyle = 1;
			projectile.width = 8;
			projectile.height = 16;
		}
	}
}
		
			