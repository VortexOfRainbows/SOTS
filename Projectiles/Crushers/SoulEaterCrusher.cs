using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class SoulEaterCrusher : CrusherProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Soul Eater Crusher");
		}
		public override void SafeSetDefaults()
        {
			Projectile.width = 24;
			Projectile.height = 24;
			maxDamage = 10;
			chargeTime = 150;
			minExplosions = 2;
			maxExplosions = 2;
			explosiveRange = 60;
			releaseTime = 60;
			accSpeed = 0.35f;
			initialExplosiveRange = 48f;
			trailLength = 4;
			exponentReduction = 0.7f;
		}
        public override int ExplosionType()
        {
            return ModContent.ProjectileType<SoulCrush>();
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/SoulEaterArm").Value;
		}
	}
}
		