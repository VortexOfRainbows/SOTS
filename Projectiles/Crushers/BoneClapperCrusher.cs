using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class BoneClapperCrusher : CrusherProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Clapper Crusher");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			maxDamage = 4.5f;
			chargeTime = 120;
			minExplosions = 1;
			maxExplosions = 4;
			explosiveRange = 64;
			releaseTime = 60;
			accSpeed = 0.6f;
			initialExplosiveRange = 64;
			exponentReduction = 0.55f;
			minDamage = 0.2f;
			minTimeBeforeRelease = 5;
			minExplosionSpread = 1;
			maxExplosionSpread = 3;
			spreadDeg = 27.5f;
		}
		public override bool UseCustomExplosionEffect(float x, float y, float dist, float rotation, float chargePercent, int index)
		{
			/*for (int i = 0; i < 1 + (int)(1 * chargePercent + 0.3f); i++)
			{
				float rand = Main.rand.NextFloat(-15, 15) * chargePercent;
				float randMult = Main.rand.NextFloat(0.8f, 1.2f);
				Projectile.NewProjectileDirect(new Vector2(x, y), new Vector2(-(7 * randMult + 10 * chargePercent), 0).RotatedBy(rotation + MathHelper.ToRadians(rand)), ModContent.ProjectileType<WebBolt>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			Projectile.NewProjectileDirect(new Vector2(x, y) + new Vector2(-12, 0).RotatedBy(rotation), Projectile.velocity, ModContent.ProjectileType<Webbing>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -1);*/
			return false;
		}
		public override int ExplosionType()
        {
            return ModContent.ProjectileType<BoneCrush>();
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return Mod.Assets.Request<Texture2D>("Projectiles/Crushers/BoneClapperArm").Value;
		}
	}
}