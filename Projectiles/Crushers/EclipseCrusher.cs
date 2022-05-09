using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
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
			Projectile.width = 28;
			Projectile.height = 28;
			maxDamage = 7;
			chargeTime = 180;
			explosiveRange = 70;
			releaseTime = 150;
			minExplosions = 3;
			maxExplosions = 5;
			accSpeed = 0.4f;
			initialExplosiveRange = 56;
			exponentReduction = 0.6f;
			minDamage = 0.3f;
			finalDist = 150;
		}
		public override int ExplosionType()
		{
			return ModContent.ProjectileType<EclipseCrush>();
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/EclipseArm").Value;
		}
        public override void ExplosionSound()
		{
			SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.Center.X, (int)Projectile.Center.Y, 13, 1.1f, -0.25f);
			base.ExplosionSound();
		}
    }
}
		