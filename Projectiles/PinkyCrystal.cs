using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles
{    
    public class PinkyCrystal : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Crystal");
			
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 2;
			projectile.height = 22;
			projectile.width = 22;
			projectile.thrown = true;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
		}
		public override void Kill(int timeLeft)
		{
			if(projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 3; i++)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-6,7), Main.rand.Next(-6,7), mod.ProjectileType("PinkyMusketBall"), (int)(projectile.damage * 1f), projectile.knockBack, Main.myPlayer);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-6,7), Main.rand.Next(-6,7), 22, (int)(projectile.damage * 1f), projectile.knockBack, Main.myPlayer);
				}
			}
		}
		
	}
}
		