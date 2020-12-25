using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class BulletBubble : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Bubble");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(405);
            aiType = 405; //18 is the demon scythe style
			projectile.timeLeft = 600;
		}
		public override void AI()
		{
			projectile.alpha = 120;
		}
	}
}
		
			