using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class ExplosiveKnife : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Knife");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(48);
            aiType = 48; 
			projectile.penetrate = 1;
		}
		public override void AI()
		{
		}
		public override void Kill(int timeLeft)
        {
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), 249, projectile.damage, 0, projectile.owner);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), 249, projectile.damage, 0, projectile.owner);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), 249, projectile.damage, 0, projectile.owner);
			}
		}
	}
}
		
			