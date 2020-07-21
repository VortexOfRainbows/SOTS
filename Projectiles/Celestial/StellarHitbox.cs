using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class StellarHitbox : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starsplosion");
			
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 120;
			projectile.width = 120;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 3;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.ranged = true;
			projectile.magic = false;
		}
		public override bool PreAI()
		{
			if(projectile.ai[1] == 1)
			{
				projectile.magic = true;
				projectile.ranged = false;
			}
			return true;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 4;
        }
	}
}
		