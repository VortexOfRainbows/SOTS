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
	public class ChaosDiamond : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Diamond");
		}
		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 64;
			projectile.hostile = false;
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
				float radius = (18 + j * 4) * (1f + radiusMult * 2.5f);
				for (int i = 0; i < 360; i += 9)
				{
					Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i));
					Vector2 center = projectile.Center;
					Vector2 rotation = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(i + Main.GameUpdateCount));
					rotation.X *= compressions[j];
					rotation = rotation.RotatedBy(rotations[j]);
					Main.spriteBatch.Draw(texture, center - Main.screenPosition + rotation, null, projectile.GetAlpha(new Color(color.R, color.G, color.B, 0)) * (1 - radiusMult), projectile.rotation, drawOrigin, 0.75f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
        public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 1.3f, -0.2f);
			for (int i = 0; i < 30; i++)
            {
				Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
				dust2.noGravity = true;
				dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
				dust2.noGravity = true;
				dust2.fadeIn = 0.2f;
				dust2.scale *= 2.4f;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 circular = new Vector2(1, 0).RotatedBy(projectile.rotation);
				Projectile.NewProjectile(projectile.Center, circular * 2.5f, ModContent.ProjectileType<ChaosDiamondLaser>(), projectile.damage, 0, Main.myPlayer);
			}
        }
		bool runOnce = true;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
		float radiusMult = 1f;
		int counter3 = 0;
        public override void AI()
		{
			Vector2 actualVelocity = (projectile.velocity - projectile.Center).SafeNormalize(Vector2.Zero);
			if (runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 77, 1.1f, -0.3f);
				projectile.timeLeft = (int)projectile.ai[0];
				runOnce = false;
				for (int i = 0; i < 15; i++)
				{
					Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
					dust2.noGravity = true;
					dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 2.4f;
				}
				Vector2 start = projectile.velocity;
				Vector2 end = projectile.Center;
				for (int i = 0; i < 100; i++)
				{
					Vector2 spawnPosition = Vector2.Lerp(start, end, i / 100f);
					Dust dust2 = Dust.NewDustDirect(spawnPosition - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 120);
					dust2.velocity *= 0.3f;
					dust2.noGravity = true;
					dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 3.6f) + Main.GameUpdateCount * 2f);
					dust2.fadeIn = 0.2f;
					dust2.scale *= 0.5f;
					dust2.scale += 1.8f;
				}
			}
			else
				RingStuff();
			if(counter3 >= 0)
			{
				counter3++;
				float mult = counter3 / 55f;
				if (mult > 1)
					mult = 1;
				radiusMult = 1 - mult;
				if(counter3 > 60)
				{
					if(Main.netMode!= NetmodeID.MultiplayerClient)
                    {
						int numDarts = 6;
						for (int i = 0; i < numDarts; i++)
						{
							Vector2 circular = new Vector2(2.0f, 0).RotatedBy(projectile.rotation + MathHelper.ToRadians(i * 360f / numDarts));
							Projectile.NewProjectile(projectile.Center, circular, ModContent.ProjectileType<ChaosDart>(), (int)(projectile.damage * 0.5f), projectile.knockBack, Main.myPlayer, (int)projectile.ai[1], -0.8f);
						}
					}
					counter3 = -1;
                }
			}
			else
            {

            }

			Lighting.AddLight(projectile.Center, new Color(231, 95, 203).ToVector3());
			projectile.rotation = actualVelocity.ToRotation();
			if (projectile.timeLeft < 100)
				projectile.alpha += 1;
			if(Main.rand.NextBool(3))
			{
				Dust dust2 = Dust.NewDustDirect(projectile.Center - new Vector2(8, 8), 8, 8, ModContent.DustType<CopyDust4>(), 0, 0, 100);
				dust2.velocity *= 0.4f;
				dust2.noGravity = true;
				dust2.color = VoidPlayer.pastelRainbow;
				dust2.noGravity = true;
				dust2.fadeIn = 0.2f;
				dust2.scale *= 1.4f;
			}
		}
	}
}