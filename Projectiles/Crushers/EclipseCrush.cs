using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class EclipseCrush : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eclipse Crush");
		}
        public override void SetDefaults()
        {
			projectile.height = 70;
			projectile.width = 70;
            Main.projFrames[projectile.type] = 6;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 23;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		bool runOnce = true;
		public override void AI()
        {
			projectile.alpha += 8;
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
			if(runOnce && projectile.owner == Main.myPlayer)
			{
				runOnce = false;
				int ogDamage = (int)projectile.ai[0];
				for (float i = 0; i < projectile.damage; i += ogDamage * 2.0f)
				{
					Vector2 direction = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f)) + projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f, 2f);
					int proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, direction.X, direction.Y, mod.ProjectileType("EclipseBubble"), (int)(projectile.damage * 0.1f), 0, projectile.owner);
					Main.projectile[proj].timeLeft = Main.rand.Next(52, 156);
					Main.projectile[proj].netUpdate = true;
				}
			}
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 6;
            }
        }
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}
		