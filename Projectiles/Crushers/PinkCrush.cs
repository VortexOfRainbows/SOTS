using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class PinkCrush : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Collapse");
		}
        public override void SetDefaults()
        {
			projectile.height = 105;
			projectile.width = 105;
            Main.projFrames[projectile.type] = 5;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 24;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.scale = 1f;
			projectile.ownerHitCheck = true;
		}
		bool runOnce = true;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
			if(runOnce)
			{
				runOnce = false;
				int ogDamage = (int)projectile.ai[0];
				for (float i = Main.rand.NextFloat(-1, 0); i < projectile.damage; i += ogDamage * 2f)
				{ 
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), mod.ProjectileType("PinkBullet"), 0, 0, 0);
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].timeLeft = Main.rand.Next(24, 60);
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
		