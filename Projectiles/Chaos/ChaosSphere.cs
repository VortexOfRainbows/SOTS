using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class ChaosSphere : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Ring");
		}
		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 64;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 100;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
		}
		float[] nextRotations = new float[3];
		float[] nextCompressions = new float[3];
		float[] prevRotations = new float[3];
		float[] prevCompressions = new float[3];
		int counter = -1;
		float[] rotations = new float[3] { 1.0f, 0, 2.0f };
		float[] compressions = new float[3] { 0.5f, 0.5f, 0.5f };
		public void RingStuff()
		{
			if (counter <= 0)
			{
				counter = 0;
				for (int i = 0; i < 3; i++)
				{
					nextRotations[i] = Main.rand.NextFloat(-1 * (float)Math.PI, (float)Math.PI);
					nextCompressions[i] = Main.rand.NextFloat(0, 1);
					prevRotations[i] = rotations[i];
					prevCompressions[i] = compressions[i];
				}
			}
			if (counter < 180)
				counter += 9;
			float scale = 0.5f - 0.5f * (float)Math.Cos(MathHelper.ToRadians(counter));
			if (counter >= 180)
			{
				counter = 0;
			}
			for (int i = 0; i < 3; i++)
			{
				rotations[i] = MathHelper.Lerp(prevRotations[i], nextRotations[i], scale);
				compressions[i] = MathHelper.Lerp(prevCompressions[i], nextCompressions[i], scale);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for(int j = 0; j < 3; j++)
			{
				int radius = 20 + j * 8;
				for (int i = 0; i < 360; i += 6)
				{
					Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i));
					Vector2 center = projectile.Center;
					Vector2 rotation = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(i + Main.GameUpdateCount));
					rotation.X *= compressions[j];
					rotation = rotation.RotatedBy(rotations[j]);
					Main.spriteBatch.Draw(texture, center - Main.screenPosition + rotation, null, projectile.GetAlpha(new Color(color.R, color.G, color.B, 0)), projectile.rotation, drawOrigin, 0.75f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
        public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 1.3f, -0.2f);
			for (int i = 0; i < 30; i++)
            {
				Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
				dust2.velocity += projectile.velocity * 0.2f;
				dust2.noGravity = true;
				dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
				dust2.noGravity = true;
				dust2.fadeIn = 0.2f;
				dust2.scale *= 2.4f;
			}
			if(Main.netMode != NetmodeID.MultiplayerClient)
			{
				for(int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(1, 0).RotatedBy(projectile.rotation + MathHelper.ToRadians(45 * i));
					Projectile.NewProjectile(projectile.Center, circular * 2.5f, ModContent.ProjectileType<ChaosCircle>(), (int)(projectile.damage * 0.9f), 0, Main.myPlayer);
				}
			}
        }
		bool runOnce = true;
        public override void AI()
		{
			if (runOnce)
			{
				if(projectile.ai[0] > projectile.timeLeft)
					projectile.timeLeft = (int)projectile.ai[0];
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
			else
				RingStuff();
			Lighting.AddLight(projectile.Center, new Color(231, 95, 203).ToVector3());
			if(projectile.velocity.Length() > 2f)
			projectile.rotation = projectile.velocity.ToRotation();
			if (projectile.timeLeft < 100)
				projectile.alpha += 1;
			projectile.velocity *= 0.96f;
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