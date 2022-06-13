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
			Projectile.width = 72;
			Projectile.height = 76;
            Main.projFrames[Projectile.type] = 6;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 23;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.ownerHitCheck = true;
		}
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 6;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 10;
			if(target.life <= 0 && Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 4; i++)
				{
					Vector2 circularLocation = new Vector2(0, 4).RotatedBy(MathHelper.ToRadians(45 + i * 90));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, circularLocation.X, circularLocation.Y, ModContent.ProjectileType<HealLock>(), 0, 0, player.whoAmI, 3);
				}
			}
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}