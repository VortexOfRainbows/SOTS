using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles.Crushers
{
	public class IrradiatedCrusher : CrusherProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Irradiated Arm");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			maxDamage = 5;
			chargeTime = 100;
			minExplosions = 1;
			maxExplosions = 9;
			explosiveRange = 54;
			releaseTime = 20;
			accSpeed = 0.5f;
			initialExplosiveRange = 48;
			exponentReduction = 0.55f;
			minDamage = 0.25f;
			finalDist = 150;
		}
		public override int ExplosionType()
		{
			return ModContent.ProjectileType<IrradiatedCrush>();
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return mod.GetTexture("Projectiles/Crushers/IrradiatedArm");
		}
	}
}
