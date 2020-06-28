using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Pyramid
{
    public class AtenStar : ModProjectile
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("AtenProj");
			
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.FallingStar);
            aiType = ProjectileID.FallingStar;
            projectile.friendly = true;
            projectile.magic = false;
            projectile.ranged = false;
            projectile.melee = true;
            projectile.alpha = 0;
        }
        public override void AI()
        {
            projectile.alpha = 0;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = true;
            return true;
        }
    }
}