using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class VoidspaceExplosion : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidspace Explosion");
		}
        public override void SetDefaults()
        {
			projectile.height = 24;
			projectile.width = 24;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 2;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 107);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.velocity += circularLocation * 0.25f;
				dust.scale *= 1.25f;
			}
			for (int i = 0; i < 360; i += 40)
			{
				Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust.velocity += circularLocation * 0.125f;
				dust.scale *= 2.5f;
				dust.fadeIn = 0.1f;
				dust.color = new Color(33, 100, 33);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
			projectile.friendly = false;
        }
	}
}
		