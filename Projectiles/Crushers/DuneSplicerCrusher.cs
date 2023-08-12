using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace SOTS.Projectiles.Crushers
{
	public class DuneSplicerCrusher : CrusherProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dune Splicer");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 30;
			maxDamage = 3.5f;
			chargeTime = 240;
			minExplosions = 2;
			maxExplosions = 6;
			explosiveRange = 48;
			releaseTime = 240;
			accSpeed = 0.5f;
			initialExplosiveRange = 64;
			exponentReduction = 0.6f;
			minDamage = 0.7f;
			finalDist = 155;
			minExplosionSpread = 1;
			maxExplosionSpread = 2;
			spreadDeg = 20;
		}
        public override void PostAI()
        {
			if (Projectile.velocity.X < 0)
				Projectile.spriteDirection = -1;
			else
				Projectile.spriteDirection = 1;
        }
        public override bool UseCustomExplosionEffect(float x, float y, float dist, float rotation, float chargePercent, int indexNumber, int armNumber)
		{
			for (int i = 1; i <= 2 + (int)(1.2f * chargePercent + 0.4f); i++)
			{
				float spread = 45 * i;
				float rand = Main.rand.NextFloat(-spread, spread);
				float randMult = Main.rand.NextFloat(0.8f, 1.2f);
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), new Vector2(x, y), new Vector2(-(4 * randMult + 3 * chargePercent), 0).RotatedBy(rotation + MathHelper.ToRadians(rand)) + Main.rand.NextVector2Circular(1.15f, 1.15f), ModContent.ProjectileType<DuneSpike>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
			}
			for (int l = 0; l < 6; l++)
			{
				Vector2 direction = new Vector2(-3 * (1 + chargePercent) - Main.rand.NextFloat(2), 0).RotatedBy(rotation);
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), new Vector2(x, y), direction + new Vector2(1.5f, 0).RotatedBy(rotation + l * MathHelper.ToRadians(60)), ModContent.ProjectileType<DuneCrush>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, Main.rand.NextFloat(-1, 1));
			}
			return true;
		}
		public override Texture2D ArmTexture(int handNum, int direction)
		{
			return Mod.Assets.Request<Texture2D>(direction * (handNum * 2 - 1) == -1 ? "Projectiles/Crushers/DuneSplicerTop" : "Projectiles/Crushers/DuneSplicerBottom").Value;
		}
	}
}
