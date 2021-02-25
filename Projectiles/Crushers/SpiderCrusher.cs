using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class SpiderCrusher : CrusherProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spider Arm");
		}
        public override void SafeSetDefaults()
        {
			projectile.width = 26;
			projectile.height = 26;
			arms = new Vector2[8];
			chargeTime = 150;
			accSpeed = 0.1f;
			armDist = 20;
			finalDist = 140;
			exponentReduction = 0.4f;
			//trailLength = 25;
		}
        public override int ExplosionType()
        {
            return ModContent.ProjectileType<SoulCrush>();
        }
        public override Texture2D ArmTexture(int handNum, int direction)
        {
            return mod.GetTexture("Projectiles/Crushers/SpiderArm");
        }
    }
}
			