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
			projectile.height = 70;
			projectile.width = 70;
            Main.projFrames[projectile.type] = 5;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 24;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.15f / 255f, (255 - projectile.alpha) * 0.2f / 255f, (255 - projectile.alpha) * 0.2f / 255f);
			if(runOnce)
			{
				runOnce = false;
				for (int i = 0; i < 9; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 26);
					dust.noGravity = true;
					dust.velocity *= 1.25f;
					dust.scale *= 1.75f;
					if(!Main.rand.NextBool(3))
					{
						dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 234);
						dust.noGravity = true;
						dust.velocity *= 1.25f;
						dust.scale *= 1.1f;
					}
				}
			}
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 5;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 10;
        }
	}
}