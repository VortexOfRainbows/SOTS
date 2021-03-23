using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
		public override void Kill(int timeLeft)
        {
			if(projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 3; i++)
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), ProjectileID.StyngerShrapnel, projectile.damage, 0, projectile.owner);
			}
		}
	}
}
		
			