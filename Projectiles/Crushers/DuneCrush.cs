using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class DuneCrush : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dune Crush");
		}
        public override void SetDefaults()
        {
			Projectile.height = 40;
			Projectile.width = 40;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMelee>();
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 35;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.usesIDStaticNPCImmunity = true;
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 40, 40, ModContent.DustType<ModSandDust>());
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			Projectile.alpha += 2;
			Projectile.rotation += 1.1f;
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(3.5f * Projectile.ai[1]));
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 15; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 40, 40, ModContent.DustType<ModSandDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.2f;
				Main.dust[num1].scale *= 1.2f;
			}
		}
	}
}
		