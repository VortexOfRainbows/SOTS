using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Slime
{    
    public class DumbPinkArrow : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dumb Pink Arrow");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(98);
            aiType = 98;
			projectile.penetrate = 1; 
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.width = 14;
			projectile.height = 30;
		}
		public override void AI()
		{ 
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 14, 30, 72);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
	}
}
		