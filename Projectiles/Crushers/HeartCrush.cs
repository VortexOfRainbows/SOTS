using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class HeartCrush : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart Crush");
		}
        public override void SetDefaults()
        {
			projectile.width = 72;
			projectile.height = 76;
            Main.projFrames[projectile.type] = 6;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 23;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.ownerHitCheck = true;
		}
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 6;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 10;
			if(target.life <= 0 && projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 4; i++)
				{
					Vector2 circularLocation = new Vector2(0, 4).RotatedBy(MathHelper.ToRadians(45 + i * 90));
					Projectile.NewProjectile(target.Center.X, target.Center.Y, circularLocation.X, circularLocation.Y, mod.ProjectileType("HealLock"), 0, 0, player.whoAmI, 3);
				}
			}
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}