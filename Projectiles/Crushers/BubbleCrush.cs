using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Crushers
{    
    public class BubbleCrush : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Crush");
		}
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
			projectile.ownerHitCheck = true;
		}
		bool runOnce = true;
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.0f / 255f, (255 - projectile.alpha) * 1.75f / 255f, (255 - projectile.alpha) * 1.75f / 255f);
			if(runOnce)
			{
				runOnce = false;
				int ogDamage = (int)projectile.ai[0];
				for (float i = Main.rand.NextFloat(-1, 0); i < projectile.damage; i += ogDamage * 1.5f)
				{
					int proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), ProjectileID.Bubble, 0, 0, 0);
					Main.projectile[proj].friendly = false;
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
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 10;
        }
	}
}