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
			// DisplayName.SetDefault("Chaos Ball");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 70;
			Projectile.height = 70;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 930;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = Projectile.width * Projectile.scale;
			float height = Projectile.width * Projectile.scale;
			width += 2;
			height += 2;
			hitbox = new Rectangle((int)(Projectile.Center.X - width/2), (int)(Projectile.Center.Y - height/2), (int)width, (int)height);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
			Color color = new Color(253, 198, 234);
			color.A = 0;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			for(int i  =0; i< 2; i++)
			{
				SOTS.GodrayShader.Parameters["distance"].SetValue(3);
				SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
				SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
				SOTS.GodrayShader.Parameters["rotation"].SetValue(Projectile.rotation + Projectile.whoAmI + Main.GameUpdateCount * MathHelper.PiOver2 / 90f * (i % 2 * 2 - 1));
				SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f);
				SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale * 1.5f, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
        public override void PostDraw(Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Color color = new Color(100, 100, 100, 0);
				Vector2 circular = new Vector2(4 * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
				if (k != 0)
				{
					color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(k * 60));
					color.A = 0;
				}
				else
					circular *= 0f;
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, color * 0.8f, 0f, drawOrigin, Projectile.scale * 1.0f, SpriteEffects.None, 0f);
			}
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
				Projectile.scale = 0;
				Projectile.alpha = 0;
			}
			float scaleMult = counter / 160f;
			if (scaleMult > 1)
				scaleMult = 1;
			Projectile.scale = scaleMult * 1.2f;
			counter++;
			int target = (int)Projectile.ai[0];
			if(counter >= 40)
			{
				if(counter <= 180)
				{
					if (counter % 40 == 0)
					{
						SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f, 0.4f);
						for (int k = 0; k < 360; k += 10)
						{
							Vector2 circularLocation = new Vector2(-70 * Projectile.scale - 26, 0).RotatedBy(MathHelper.ToRadians(k));
							circularLocation += 0.5f * new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
							Dust dust2 = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
							dust2.velocity = -circularLocation * 0.04f;
							dust2.color = ColorHelpers.pastelAttempt(Main.rand.NextFloat(0, 6.28f), true);
							dust2.noGravity = true;
							dust2.fadeIn = 0.2f;
							dust2.scale *= 2.2f;
						}
					}
					if (Projectile.ai[1] != -1)
					{
						if (counter % 40 == 20)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								for (int i = 0; i < numDarts; i++)
								{
									Vector2 circular = new Vector2(3f, 0).RotatedBy(MathHelper.ToRadians(i * 360f / numDarts));
									Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<ChaosDart>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, target, Projectile.ai[1] == -1 ? -1 : 0);
								}
							}
							numDarts += 2;
						}
					}
				}
				else
				{
					if (counter % 240 == 181 && counter < 750)
					{
						SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f, -0.2f);
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int amt = 8;
							if (Main.expertMode)
								amt = 10;
							if (Projectile.ai[1] < 0)
								amt -= 2;
							if (Projectile.ai[1] == -1)
								amt -= 1;
							for (int i = 0; i < amt; i++)
							{
								Vector2 circular = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i * 360f / amt + Main.rand.NextFloat(-20, 20)));
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<DogmaLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(-80, 80f), Main.rand.NextFloat(-0.4f, -0.2f));
							}
							amt--;
							for (int i = 0; i < amt; i++)
							{
								Vector2 circular = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i * 360f / amt + Main.rand.NextFloat(-13, 13)));
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<DogmaLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(-60, 60f), Main.rand.NextFloat(-0.2f, 0.2f));
							}
							amt--;
							for (int i = 0; i < amt; i++)
							{
								Vector2 circular = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i * 360f / amt + Main.rand.NextFloat(-7, 7)));
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<DogmaLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(-40, 40f), Main.rand.NextFloat(0.2f, 0.5f));
							}
						}
					}
					else if(counter > 900)
                    {
						Projectile.scale *= 1 - (counter - 900f) / 30f;
                    }
					//Projectile.Kill();
				}
			}
		}
        public override void Kill(int timeLeft)
        {
			for (int i = 0; i < 360; i += 5)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(10), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity += circularLocation * 3f;
				dust.color = ColorHelpers.pastelAttempt(Main.rand.NextFloat(6.28f), true);
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.4f;
			}
		}
    }
}