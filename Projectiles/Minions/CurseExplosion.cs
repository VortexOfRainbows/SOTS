using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class CurseExplosion : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Curse");
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(263);
			AIType = 263;
			Projectile.height = 48;
			Projectile.width = 48;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
		}
		public override void AI()
		{
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CurseDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].scale = 1f;
				Main.dust[num1].alpha = 100;
			}
		}
	}
}
		