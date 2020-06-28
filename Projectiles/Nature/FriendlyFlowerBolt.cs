using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{    
    public class FriendlyFlowerBolt : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower Bolt");
		}
        public override void SetDefaults()
        {
			projectile.height = 8;
			projectile.width = 8;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 0f;
			projectile.alpha = 255;
			projectile.timeLeft = 700;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i++)
			{
				int num2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 231);
				Main.dust[num2].velocity *= 1.5f;
				Main.dust[num2].noGravity = true;
				Main.dust[num2].scale = 1.5f;
			}
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 231);
			Main.dust[num1].velocity *= 0.1f;
			Main.dust[num1].noGravity = true;
			Main.dust[num1].scale = 2;

			int num2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 231);
			Main.dust[num2].velocity *= 0.3f;
			Main.dust[num2].noGravity = true;
			Main.dust[num2].scale = 1;
		}
	}
}
		