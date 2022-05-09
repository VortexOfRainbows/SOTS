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
			Projectile.height = 105;
			Projectile.width = 105;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.melee = true;
			Projectile.timeLeft = 24;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.scale = 1f;
			Projectile.ownerHitCheck = true;
		}
		bool runOnce = true;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f);
			if(runOnce)
			{
				runOnce = false;
				int ogDamage = (int)Projectile.ai[0];
				for (float i = Main.rand.NextFloat(-1, 0); i < Projectile.damage; i += ogDamage * 2f)
				{ 
					int proj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), mod.ProjectileType("PinkBullet"), 0, 0, 0);
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].timeLeft = Main.rand.Next(24, 60);
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
		