using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles.Crushers
{    
    public class CardiacCollapseCrusher : CrusherProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cardiac Collapse Arm");
		}
        public override void SafeSetDefaults()
        {
			projectile.width = 20;
			projectile.height = 20;
			maxDamage = 8;
			chargeTime = 210;
			minExplosions = 1;
			maxExplosions = 3;
			explosiveRange = 60;
			releaseTime = 210;
			accSpeed = 0.4f;
			initialExplosiveRange = 48;
			exponentReduction = 0.6f;
		}
		public override int ExplosionType()
		{
			return ModContent.ProjectileType<HeartCrush>();
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/CardiacCollapseArm").Value;
		}
	}
}
		