using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles
{    
    public class PinkBullet : ModProjectile 
    {	       
        public override void SetDefaults()
        {
			Projectile.height = 22;
			Projectile.width = 22;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 330;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.netImportant = true;
		}
		public override void OnKill(int timeLeft)
        {
			for(int i = 0; i < 360; i += 24)
			{
				Vector2 circularLocation = new Vector2(-8, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.Gastropod);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation * 0.45f;
			}
		}
		private Vector2 initialVelo;
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				runOnce = false;
				initialVelo = Projectile.velocity;
			}
			Projectile.velocity -= initialVelo / 120f;
			
			Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), 20, 20, DustID.Gastropod);
            dust.noGravity = true;
            dust.velocity *= 0.1f;
			
			Projectile.rotation += 0.1f;
		}
	}
}
		