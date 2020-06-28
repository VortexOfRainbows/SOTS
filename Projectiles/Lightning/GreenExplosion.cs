using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Lightning
{    
    public class GreenExplosion : ModProjectile 
    {	int expand = -1;
		            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Shock");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 24;
			projectile.width = 24;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.alpha = 255;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 1;
        }
	}
}
		