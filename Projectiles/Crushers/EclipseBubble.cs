using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class EclipseBubble : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eclipse Bubble");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(410);
			aiType = 410; 
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 255;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override void AI()
		{
			int alpha = 260 - (int)Math.Pow(projectile.timeLeft, 1.5f);
			if (alpha < 0)
				alpha = 0;
			projectile.alpha = alpha;
		}
	}
}
		
			