using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class PinkBubble : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink Bubble Stream");
		}
        public override void SetDefaults()
        {
			projectile.height = 4;
			projectile.width = 4;
			projectile.friendly = true;
			projectile.timeLeft = 120;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.alpha = 255;
			projectile.penetrate = 1;
			projectile.extraUpdates = 2;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)projectile.position.X - projectile.width, (int)projectile.position.Y - projectile.height, projectile.width * 3, projectile.height * 3);
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, mod.DustType("BigPinkDust"));
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 0.75f;
				Main.dust[num1].velocity = circularLocation * 0.45f;

				circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i + 10));
				num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 72);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1.25f;
				Main.dust[num1].velocity = circularLocation * 0.45f;
			}
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("BigPinkDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].scale = 0.75f;
			Main.dust[num1].velocity *= 0.7f;

			num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 72);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].scale = 1.25f;
			Main.dust[num1].velocity *= 0.7f;
		}
	}
}
		