using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class CurseExplosion : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse");
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(263);
			aiType = 263;
			projectile.height = 48;
			projectile.width = 48;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
		}
		public override void AI()
		{
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, mod.DustType("CurseDust"));
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].scale = 1f;
				Main.dust[num1].alpha = 100;
			}
		}
	}
}
		