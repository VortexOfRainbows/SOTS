using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseBall : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Curse");
		}
        public override void SetDefaults()
        {
			Projectile.height = 18;
			Projectile.width = 18;
			Projectile.friendly = false;
			Projectile.timeLeft = 7200;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 5;
			Projectile.netImportant = true;
		}
		public override void AI()
		{
			Projectile.alpha -= Projectile.alpha > 0 ? 1 : 0;
			Projectile.rotation += Main.rand.Next(-3,4);
			Projectile.alpha = Projectile.timeLeft <= 255 ? 200 - Projectile.timeLeft : Projectile.alpha;
			int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 18, 18, ModContent.DustType<CurseDust>());
			Main.dust[num1].noGravity = true;
			Main.dust[num1].alpha = Projectile.alpha;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			Projectile.timeLeft -= 60;
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}
}
		