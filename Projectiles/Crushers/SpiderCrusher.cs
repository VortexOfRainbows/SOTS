using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
			Projectile.width = 24;
			Projectile.height = 24;
			minExplosions = 1;
			maxExplosions = 1;
			arms = new Vector2[8];
			chargeTime = 150;
			accSpeed = 0.3f;
			armDist = 37.5f;
			finalDist = 150;
			exponentReduction = 0.7f;
			minDamage = 0.8f;
			maxDamage = 1.8f;
			trailLength = 3;
			releaseTime = 60f;
			minTimeBeforeRelease = 8;
			initialExplosiveRange = 56;
		}
        public override bool UseCustomExplosionEffect(float x, float y, float dist, float rotation, float chargePercent, int index)
        {
			for(int i = 0; i < 1 + (int)(1 * chargePercent + 0.3f); i++)
			{
				float rand = Main.rand.NextFloat(-15, 15) * chargePercent;
				float randMult = Main.rand.NextFloat(0.8f, 1.2f);
				Projectile.NewProjectileDirect(new Vector2(x, y), new Vector2(-(7 * randMult + 10 * chargePercent), 0).RotatedBy(rotation + MathHelper.ToRadians(rand)), ModContent.ProjectileType<WebBolt>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			Projectile.NewProjectileDirect(new Vector2(x, y) + new Vector2(-12, 0).RotatedBy(rotation), Projectile.velocity, ModContent.ProjectileType<Webbing>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -1);
			return true;
        }
        public override int ExplosionType()
        {
            return ModContent.ProjectileType<Webbing>();
        }
        public override Texture2D ArmTexture(int handNum, int direction)
        {
            return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/SpiderArm").Value;
        }
    }
}
			