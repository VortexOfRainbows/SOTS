using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class ChaosCircle : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chaos Ring");
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 100;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < 360; i += 6)
			{
				Color color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(i));
				Vector2 center = Projectile.Center;
				Vector2 rotation = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(i + Main.GameUpdateCount));
				rotation.X *= 1f + 0.3f * (1 - Projectile.timeLeft / 100f);
				rotation.Y *= 0.8f * (float)Math.Cos(MathHelper.ToRadians(Projectile.ai[0]));
				rotation = rotation.RotatedBy(Projectile.rotation);
				Main.spriteBatch.Draw(texture, center - Main.screenPosition + rotation, null, Projectile.GetAlpha(new Color(color.R, color.G, color.B, 0)), Projectile.rotation, drawOrigin, 0.75f, SpriteEffects.None, 0f);
			}
			return false;
		}
        public override void OnKill(int timeLeft)
        {
			for(int i = 0; i < 30; i++)
            {
				Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
				dust2.velocity += Projectile.velocity * 0.2f;
				dust2.noGravity = true;
				dust2.color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(i * 18));
				dust2.noGravity = true;
				dust2.fadeIn = 0.2f;
				dust2.scale *= 2.4f;
			}
        }
		bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
			{
				runOnce = false;
				for (int i = 0; i < 15; i++)
				{
					Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
					dust2.velocity += Projectile.velocity * 0.9f;
					dust2.noGravity = true;
					dust2.color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(i * 18));
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 2.4f;
				}
			}
			Lighting.AddLight(Projectile.Center, new Color(231, 95, 203).ToVector3());
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.alpha += 2;
			Projectile.ai[0] += Projectile.velocity.Length();
			Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.2f;
			if(Main.rand.NextBool(3))
			{
				Dust dust2 = Dust.NewDustDirect(Projectile.Center - new Vector2(8, 8), 8, 8, ModContent.DustType<CopyDust4>(), 0, 0, 100);
				dust2.velocity *= 0.2f;
				dust2.velocity -= Projectile.velocity * 0.3f;
				dust2.noGravity = true;
				dust2.color = ColorHelpers.pastelRainbow;
				dust2.noGravity = true;
				dust2.fadeIn = 0.2f;
				dust2.scale *= 1.4f;
			}
		}
	}
}