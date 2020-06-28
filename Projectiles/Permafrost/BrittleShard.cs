using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class BrittleShard : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle Shard");	
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.penetrate = 1;
			projectile.alpha = 0;
			projectile.width = 10;
			projectile.height = 12;
			projectile.melee = true;
		}
		public override void AI()
		{
			projectile.rotation = Main.rand.Next(38);
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 12; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67);
				Main.dust[num1].noGravity = true;
			}
		}
	}
}