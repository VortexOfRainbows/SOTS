using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class EclipseCrusher : CrusherProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eclipse Arm");
		}
        public override void SafeSetDefaults()
        {
			projectile.width = 28;
			projectile.height = 28;
			maxDamage = 6;
			chargeTime = 180;
			explosiveRange = 70;
			releaseTime = 150;
			minExplosions = 3;
			maxExplosions = 5;
			accSpeed = 0.4f;
			initialExplosiveRange = 56;
			exponentReduction = 0.6f;
		}
		public override int ExplosionType()
		{
			return ModContent.ProjectileType<EclipseCrush>();
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return mod.GetTexture("Projectiles/Crushers/EclipseArm");
		}
	}
}
		