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
			maxExplosions = 5;
			arms = new Vector2[4];
			chargeTime = 180;
			accSpeed = 0.6f;
			armDist = 38f;
			finalDist = 152;
			exponentReduction = 0.6f;
			minDamage = 0.3f;
			maxDamage = 5f;
			trailLength = 5;
			releaseTime = 120f;
			minTimeBeforeRelease = 7;
			initialExplosiveRange = 56;
			minExplosionSpread = 1;
			maxExplosionSpread = 3;
			spreadDeg = 25.0f;
			armAngle = 90f;
			explosiveRange = 64;
		}
        public override bool UseCustomExplosionEffect(float x, float y, float dist, float rotation, float chargePercent, int explosionNumber, int armNumber)
        {
			if(armNumber == 0)
            {
				for(int i = -(explosionNumber + 1); i <= explosionNumber + 1; i++)
				{
					Vector2 outward = new Vector2(-12, 0).RotatedBy(rotation + MathHelper.ToRadians(spreadDeg * 0.25f * i));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(x, y), outward, ModContent.ProjectileType<PulverizerLaser>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack * 0.5f, Projectile.owner, 2f / (System.Math.Abs(i) + 2f));
					if(i < -1)
                    {
						i = -i - 1;
                    }
				}
				return true;
            }
            else
            {
				float randRot = Main.rand.NextFloat(90f);
				for (int i = 0; i < 3; i++)
				{
					int direction = Main.rand.Next(2) * 2 - 1;
					float rand = Main.rand.NextFloat(5 + explosionNumber * 6.5f, 45) * direction * (0.5f + chargePercent * 0.5f);
					float randMult = Main.rand.NextFloat(0.8f, 1.2f);
					randMult *= 0.5f + 0.5f * (float)System.Math.Cos(rand);
					Vector2 velo = new Vector2(-(i * 2 + explosionNumber * 3.5f + chargePercent * 1.5f) * randMult, 0).RotatedBy(rotation + MathHelper.ToRadians(rand));
					Vector2 spawnAt = new Vector2(x, y) + Main.rand.NextVector2Circular(Main.rand.NextFloat(32 + explosionNumber * 8) + i * 12 + explosionNumber * 2, 0).RotatedBy(MathHelper.ToRadians(randRot + 120 * i + Main.rand.NextFloat(-15, 15))) + velo.SafeNormalize(Vector2.Zero) * 12 * (i - 1);
					Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velo, ModContent.ProjectileType<RedNatureBolt>(), Projectile.damage, Projectile.knockBack, Projectile.owner, spawnAt.X, spawnAt.Y);
				}
			}
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
        public override Texture2D ArmGlowTexture(int handNum, int direction)
        {
			return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/PulverizerArm" + (2 - handNum / 2) + "Glow").Value;
		}
	}
}
			