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
			Projectile.height = 4;
			Projectile.width = 4;
			Projectile.friendly = true;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.minion = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 2;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)Projectile.position.X - Projectile.width, (int)Projectile.position.Y - Projectile.height, Projectile.width * 3, Projectile.height * 3);
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, Mod.Find<ModDust>("BigPinkDust").Type);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 0.75f;
				Main.dust[num1].velocity = circularLocation * 0.45f;

				circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i + 10));
				num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 72);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1.25f;
				Main.dust[num1].velocity = circularLocation * 0.45f;
			}
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("BigPinkDust").Type);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].scale = 0.75f;
			Main.dust[num1].velocity *= 0.7f;

			num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].scale = 1.25f;
			Main.dust[num1].velocity *= 0.7f;
		}
	}
}
		