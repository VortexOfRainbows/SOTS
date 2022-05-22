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
			Projectile.height = 8;
			Projectile.width = 8;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 720;
		}
		public override void Kill(int timeLeft)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 105, 0.45f, -0.2f);
			DustHelper.DrawStar(Projectile.Center, 231, 5f, 3f, 1.5f, 1.75f, 0.7f, 0.7f, true, 8, 0);
		}
		public override void AI()
		{
			Dust dust = Dust.NewDustPerfect(Projectile.Center, 231);
			dust.velocity *= 0.05f;
			dust.noGravity = true;
			dust.scale = 1.7f;

			Projectile.ai[0]++;
			float sin = (float)Math.Sin(Projectile.ai[0] * MathHelper.Pi / 30f);
			Vector2 circular = new Vector2(0, sin * 8).RotatedBy(Projectile.velocity.ToRotation());
			dust = Dust.NewDustPerfect(Projectile.Center + circular, 231, Main.rand.NextVector2Circular(0.2f, 0.2f));
			dust.noGravity = true;
			dust.scale = 1.2f;
		}
	}
}
		