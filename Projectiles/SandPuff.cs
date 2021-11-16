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
			projectile.height = 40;
			projectile.width = 40;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.penetrate = 8;
			projectile.alpha = 35;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			projectile.alpha++;
			projectile.rotation += MathHelper.ToRadians(30f);
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 40, 40, ModContent.DustType<ModSandDust>());
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			projectile.alpha++;
			if(projectile.timeLeft < 61)
			{
				Vector2 circularLocation = new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f).RotatedBy(MathHelper.ToRadians(30f));
				projectile.velocity *= 0.86f;
				projectile.velocity += circularLocation;
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 22; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 40, 40, ModContent.DustType<ModSandDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.2f;
				Main.dust[num1].scale *= 1.2f;
			}
		}
	}
}
		