using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Otherworld
{    
    public class HardlightArrowDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Arrow");
		}
        public override void SetDefaults()
		{
			projectile.CloneDefaults(1);
			projectile.aiStyle = 1;
			projectile.height = 24;
			projectile.width = 24;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.timeLeft = 6;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void Kill(int timeLeft)
		{
			for (int h = 0; h < 20; h++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 12, projectile.Center.Y - 12), 16, 16, DustID.Electric);
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity += projectile.velocity * 0.1f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
		