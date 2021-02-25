using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Crushers
{    
    public class HellCrush : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hell Crush");
		}
        public override void SetDefaults()
        {
			projectile.height = 70;
			projectile.width = 70;
            Main.projFrames[projectile.type] = 5;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.timeLeft = 24;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
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
			if(runOnce && projectile.owner == Main.myPlayer)
			{
				runOnce = false;
				int ogDamage = (int)projectile.ai[0];
				for (float i = 0; i < projectile.damage; i += ogDamage * 2.5f)
				{ 
					int proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-100, 101) * 0.015f, Main.rand.Next(-100, 101) * 0.015f, 85, (int)(projectile.damage * 0.1f), 0, projectile.owner);
					Main.projectile[proj].melee = true;
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
			if(Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.OnFire, 360, false);
        }
	}
}