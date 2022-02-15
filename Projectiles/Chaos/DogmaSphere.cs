using log4net.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Base;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class DogmaSphere : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			projectile.width = 70;
			projectile.height = 70;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 1000;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.scale = 1f;
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = projectile.width * projectile.scale;
			float height = projectile.width * projectile.scale;
			width += 2;
			height += 2;
			hitbox = new Rectangle((int)(projectile.Center.X - width/2), (int)(projectile.Center.Y - height/2), (int)width, (int)height);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Effects/Masks/Extra_49");
			Color color = new Color(253, 198, 234);
			color.A = 0;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			for(int i  =0; i< 2; i++)
			{
				SOTS.GodrayShader.Parameters["distance"].SetValue(3);
				SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
				SOTS.GodrayShader.Parameters["noise"].SetValue(mod.GetTexture("TrailTextures/noise"));
				SOTS.GodrayShader.Parameters["rotation"].SetValue(projectile.rotation + projectile.whoAmI + Main.GameUpdateCount * MathHelper.PiOver2 / 90f * (i % 2 * 2 - 1));
				SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f);
				SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale * 1.5f, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Color color = new Color(100, 100, 100, 0);
				Vector2 circular = new Vector2(4 * projectile.scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
				if (k != 0)
				{
					color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
					color.A = 0;
				}
				else
					circular *= 0f;
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, null, color * 0.8f, 0f, drawOrigin, projectile.scale * 1.0f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		float counter = 0;
		bool runOnce = true;
		int numDarts = 4;
        public override void AI()
		{
			if(runOnce)
            {
				runOnce = false;
				projectile.scale = 0;
				projectile.alpha = 0;
			}
			float scaleMult = counter / 160f;
			if (scaleMult > 1)
				scaleMult = 1;
			projectile.scale = scaleMult * 1.2f;
			counter++;
			int target = (int)projectile.ai[0];
			if(counter >= 40)
			{
				if(counter <= 180)
				{
					if (counter % 40 == 0)
					{
						Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 0.7f, 0.4f);
						for (int k = 0; k < 360; k += 10)
						{
							Vector2 circularLocation = new Vector2(-70 * projectile.scale - 26, 0).RotatedBy(MathHelper.ToRadians(k));
							circularLocation += 0.5f * new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
							Dust dust2 = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
							dust2.velocity = -circularLocation * 0.04f;
							dust2.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(0, 6.28f), true);
							dust2.noGravity = true;
							dust2.fadeIn = 0.2f;
							dust2.scale *= 2.2f;
						}
					}
					if (counter %  40 == 20)
					{
						if(Main.netMode != NetmodeID.MultiplayerClient)
						{
							for (int i = 0; i < numDarts; i++)
							{
								Vector2 circular = new Vector2(3f, 0).RotatedBy(MathHelper.ToRadians(i * 360f / numDarts));
								Projectile.NewProjectile(projectile.Center, circular, ModContent.ProjectileType<ChaosDart>(), projectile.damage, projectile.knockBack, Main.myPlayer, target);
							}
						}
						numDarts += 2;
					}
				}
				else
				{
					if (counter % 300 == 181 && counter < 900)
					{
						Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 1f, -0.2f);
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int amt = 8;
							if (Main.expertMode)
								amt = 10;
							for (int i = 0; i < amt; i++)
							{
								Vector2 circular = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i * 360f / amt + Main.rand.NextFloat(-20, 20)));
								Projectile.NewProjectile(projectile.Center, circular, ModContent.ProjectileType<DogmaLaser>(), projectile.damage, projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(-80, 80f), Main.rand.NextFloat(-0.4f, -0.2f));
							}
							amt--;
							for (int i = 0; i < amt; i++)
							{
								Vector2 circular = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i * 360f / amt + Main.rand.NextFloat(-13, 13)));
								Projectile.NewProjectile(projectile.Center, circular, ModContent.ProjectileType<DogmaLaser>(), projectile.damage, projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(-55, 55f), Main.rand.NextFloat(-0.2f, 0.2f));
							}
							amt--;
							for (int i = 0; i < amt; i++)
							{
								Vector2 circular = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i * 360f / amt + Main.rand.NextFloat(-7, 7)));
								Projectile.NewProjectile(projectile.Center, circular, ModContent.ProjectileType<DogmaLaser>(), projectile.damage, projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(-35, 35f), Main.rand.NextFloat(0.2f, 0.5f));
							}
						}
					}
					else if(counter > 950)
                    {
						projectile.scale *= 1 - (counter - 950f) / 50f;
                    }
					//projectile.Kill();
				}
			}
			Lighting.AddLight(projectile.Center, 0.25f, 0.45f, 0.75f);
		}
        public override void Kill(int timeLeft)
        {
			for (int i = 0; i < 360; i += 6)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(10), 0).RotatedBy(MathHelper.ToRadians(i) + projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity += circularLocation * 4f;
				dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.5f;
			}
		}
    }
}