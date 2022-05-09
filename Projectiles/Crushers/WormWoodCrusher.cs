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
			Projectile.width = 24;
			Projectile.height = 20;
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
			return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/WormWoodArm").Value;
		}
	}
}
			