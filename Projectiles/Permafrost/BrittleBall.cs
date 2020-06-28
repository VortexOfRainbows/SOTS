using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class BrittleBall : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle Ball");	
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.penetrate = 15;
			projectile.alpha = 0;
			projectile.width = 38;
			projectile.height = 38;
			projectile.timeLeft = 960;
			projectile.melee = true;
		}
		public void makeDust(int num)
		{
			for (int i = 0; i < num; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67);
				Main.dust[num1].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			makeDust(30);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			makeDust(15);
			projectile.timeLeft -= 60;
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}
}