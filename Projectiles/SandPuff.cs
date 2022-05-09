using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles
{    
    public class SandPuff : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandy Puff");
		}
        public override void SetDefaults()
        {
			Projectile.height = 40;
			Projectile.width = 40;
			Projectile.magic = true;
			Projectile.friendly = true;
			Projectile.penetrate = 8;
			Projectile.alpha = 35;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			Projectile.alpha++;
			Projectile.rotation += MathHelper.ToRadians(30f);
			int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 40, 40, ModContent.DustType<ModSandDust>());
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			Projectile.alpha++;
			if(Projectile.timeLeft < 61)
			{
				Vector2 circularLocation = new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f).RotatedBy(MathHelper.ToRadians(30f));
				Projectile.velocity *= 0.86f;
				Projectile.velocity += circularLocation;
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 22; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 40, 40, ModContent.DustType<ModSandDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.2f;
				Main.dust[num1].scale *= 1.2f;
			}
		}
	}
}
		