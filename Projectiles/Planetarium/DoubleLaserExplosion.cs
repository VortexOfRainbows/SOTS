using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Planetarium
{    
    public class DoubleLaserExplosion : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Double Laser");
		}
        public override void SetDefaults()
        {
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
        }
	}
}
		