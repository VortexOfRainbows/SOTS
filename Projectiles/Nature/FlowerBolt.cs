using Microsoft.Xna.Framework;
using SOTS.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{
	public class FlowerBolt : ModProjectile
	{
		public override string Texture => "SOTS/Projectiles/Nature/NatureBolt";
		public override void SetDefaults()
        {
			projectile.height = 8;
			projectile.width = 8;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.timeLeft = 720;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 105, 0.45f, -0.2f);
			DustHelper.DrawStar(projectile.Center, 231, 5f, 3f, 1.5f, 1.75f, 0.7f, 0.7f, true, 8, 0);
		}
		public override void AI()
		{
			Dust dust = Dust.NewDustPerfect(projectile.Center, 231);
			dust.velocity *= 0.05f;
			dust.noGravity = true;
			dust.scale = 1.7f;

			projectile.ai[0]++;
			float sin = (float)Math.Sin(projectile.ai[0] * MathHelper.Pi / 30f);
			Vector2 circular = new Vector2(0, sin * 8).RotatedBy(projectile.velocity.ToRotation());
			dust = Dust.NewDustPerfect(projectile.Center + circular, 231, Main.rand.NextVector2Circular(0.2f, 0.2f));
			dust.noGravity = true;
			dust.scale = 1.2f;
		}
	}
}
		