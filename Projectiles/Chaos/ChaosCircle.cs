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
			DisplayName.SetDefault("Chaos Ring");
		}
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 100;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < 360; i += 6)
			{
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i));
				Vector2 center = projectile.Center;
				Vector2 rotation = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(i + Main.GameUpdateCount));
				rotation.X *= 1f + 0.3f * (1 - projectile.timeLeft / 100f);
				rotation.Y *= 0.8f * (float)Math.Cos(MathHelper.ToRadians(projectile.ai[0]));
				rotation = rotation.RotatedBy(projectile.rotation);
				Main.spriteBatch.Draw(texture, center - Main.screenPosition + rotation, null, projectile.GetAlpha(new Color(color.R, color.G, color.B, 0)), projectile.rotation, drawOrigin, 0.75f, SpriteEffects.None, 0f);
			}
			return false;
		}
        public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 30; i++)
            {
				Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
				dust2.velocity += projectile.velocity * 0.2f;
				dust2.noGravity = true;
				dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
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
					Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
					dust2.velocity += projectile.velocity * 0.9f;
					dust2.noGravity = true;
					dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 2.4f;
				}
			}
			Lighting.AddLight(projectile.Center, new Color(231, 95, 203).ToVector3());
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.alpha += 2;
			projectile.ai[0] += projectile.velocity.Length();
			projectile.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 0.2f;
			if(Main.rand.NextBool(3))
			{
				Dust dust2 = Dust.NewDustDirect(projectile.Center - new Vector2(8, 8), 8, 8, ModContent.DustType<CopyDust4>(), 0, 0, 100);
				dust2.velocity *= 0.2f;
				dust2.velocity -= projectile.velocity * 0.3f;
				dust2.noGravity = true;
				dust2.color = VoidPlayer.pastelRainbow;
				dust2.noGravity = true;
				dust2.fadeIn = 0.2f;
				dust2.scale *= 1.4f;
			}
		}
	}
}