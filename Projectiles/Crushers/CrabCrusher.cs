using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class CrabCrusher : CrusherProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crab Claw Crusher");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			maxDamage = 8;
			chargeTime = 240;
			minExplosions = 1;
			maxExplosions = 2;
			explosiveRange = 48;
			releaseTime = 300;
			accSpeed = 0.6f;
			initialExplosiveRange = 48;
			exponentReduction = 0.45f;
			minDamage = -0.4f;
			minTimeBeforeRelease = 3;
		}
        public override int ExplosionType()
        {
            return ModContent.ProjectileType<BubbleCrush>();
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return (handNum + (direction == 1 ? 0 : 1)) % 2 == 0 ? mod.GetTexture("Projectiles/Crushers/CrabClaw1") : mod.GetTexture("Projectiles/Crushers/CrabClaw2");
		}
	}
}