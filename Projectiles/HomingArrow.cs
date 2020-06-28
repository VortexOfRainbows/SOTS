using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class HomingArrow : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chlorophyte Shock");
			
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(338);
			aiType = 338;
			projectile.alpha = 255;
			projectile.penetrate = 1; 
			projectile.ranged = true;
			projectile.width = 24;
			projectile.height = 44;
		}
		public override void AI()
		{ 
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 24, 44, 107);
		}
	}
}
		