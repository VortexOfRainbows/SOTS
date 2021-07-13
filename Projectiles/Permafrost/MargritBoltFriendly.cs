using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class MargritBoltFriendly : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryptic Bolt");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.height = 20;
			projectile.width = 16;
			projectile.ranged = true;
			projectile.timeLeft = 1800;
			projectile.friendly = true;
			projectile.hostile = false;
		}
		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 16, 20, 235);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 15; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 235);
				Main.dust[num1].noGravity = true;
			}
		}
	}
}
		