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
			Projectile.CloneDefaults(410);
			AIType = 410; 
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 255;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override void AI()
		{
			int alpha = 260 - (int)Math.Pow(Projectile.timeLeft, 1.5f);
			if (alpha < 0)
				alpha = 0;
			Projectile.alpha = alpha;
		}
	}
}
		
			