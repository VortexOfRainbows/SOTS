using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Crushers
{    
    public class BoneCrush : ModProjectile 
    {
		bool runOnce = true;
        public override void SetDefaults()
        {
			Projectile.height = 70;
			Projectile.width = 70;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 24;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.2f / 255f, (255 - Projectile.alpha) * 0.2f / 255f);
			if(runOnce)
			{
				runOnce = false;
				for (int i = 0; i < 9; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 26);
					dust.noGravity = true;
					dust.velocity *= 1.25f;
					dust.scale *= 1.75f;
					if(!Main.rand.NextBool(3))
					{
						dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 234);
						dust.noGravity = true;
						dust.velocity *= 1.25f;
						dust.scale *= 1.1f;
					}
				}
			}
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 10;
        }
	}
}