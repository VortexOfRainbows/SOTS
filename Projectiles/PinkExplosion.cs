using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles
{    
    public class PinkExplosion : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink Explosion");
		}
        public override void SetDefaults()
        {
			Projectile.height = 105;
			Projectile.width = 105;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 36;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
		public override bool PreAI()
		{
			return true;
		}
		public override void AI()
        {
			Projectile.rotation = Projectile.ai[0];
			Projectile.knockBack = 3.5f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
        }
	}
}
		