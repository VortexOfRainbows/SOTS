using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class WormWoodCrusher : CrusherProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Arm");
		}
        public override void SafeSetDefaults()
        {
			projectile.width = 24;
			projectile.height = 20;
			maxDamage = 5;
			chargeTime = 180;
			explosiveRange = 64;
			releaseTime = 180;
			initialExplosiveRange = 55;
			minExplosions = 3;
			maxExplosions = 4;
			accSpeed = 0.3f;
			exponentReduction = 0.45f;
		}
		public override int ExplosionType()
		{
			return ModContent.ProjectileType<PinkCrush>();
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return mod.GetTexture("Projectiles/Crushers/WormWoodArm");
		}
	}
}
			