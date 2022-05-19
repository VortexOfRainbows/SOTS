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
			Projectile.CloneDefaults(48);
            AIType = 48; 
			Projectile.penetrate = 1;
		}
		public override void Kill(int timeLeft)
        {
			if(Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 3; i++)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), ProjectileID.StyngerShrapnel, Projectile.damage, 0, Projectile.owner);
			}
		}
	}
}
		
			