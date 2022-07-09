using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class PulverizerCrusher : CrusherProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulverizer Arm");
		}
        public override void SafeSetDefaults()
        {
			Projectile.width = 36;
			Projectile.height = 36;
			minExplosions = 1;
			maxExplosions = 1;
			arms = new Vector2[4];
			chargeTime = 180;
			accSpeed = 0.6f;
			armDist = 38f;
			finalDist = 152;
			exponentReduction = 0.6f;
			minDamage = 0.8f;
			maxDamage = 4f;
			trailLength = 5;
			releaseTime = 120f;
			minTimeBeforeRelease = 7;
			initialExplosiveRange = 56;
			minExplosionSpread = 1;
			maxExplosionSpread = 4;
			spreadDeg = 30.0f;
			armAngle = 90f;
		}
        public override bool UseCustomExplosionEffect(float x, float y, float dist, float rotation, float chargePercent, int index)
        {
			return false;
        }
        public override int ExplosionType()
        {
            return ModContent.ProjectileType<BoneCrush>();
        }
        public override Texture2D ArmTexture(int handNum, int direction)
        {
            return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/PulverizerArm" + (2 - handNum / 2)).Value;
        }
    }
}
			