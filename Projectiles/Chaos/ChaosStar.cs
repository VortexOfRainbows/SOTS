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
	public class ChaosStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.timeLeft = 120;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.scale = 1f;
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
			Color color = projectile.GetAlpha(new Color(253, 198, 234));
			color.A = 0;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			SOTS.GodrayShader.Parameters["distance"].SetValue(3);
			SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
			SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
			SOTS.GodrayShader.Parameters["rotation"].SetValue(projectile.rotation + projectile.whoAmI + Main.GameUpdateCount * MathHelper.PiOver2 / 180f);
			SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f);
			SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale * 0.5f, SpriteEffects.None, 0f);
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
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, null, projectile.GetAlpha(color * 0.8f), projectile.rotation, drawOrigin, projectile.scale * 1.0f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
		float bonusRotation = 90;
        public override void AI()
		{
			float finalRotation = projectile.ai[0];
			if(runOnce)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int amt = 4;
					for (int i = 0; i < amt; i++)
					{
						Vector2 circular = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i * 360f / amt + finalRotation));
						Projectile.NewProjectile(projectile.Center, circular, ModContent.ProjectileType<DogmaLaser>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0, -0.5f);
					}
				}
				runOnce = false;
				projectile.scale = 1;
				projectile.alpha = 255;
			}
			if (projectile.alpha > 0)
				projectile.alpha -= 5;
			else
				projectile.alpha = 0;
			if (projectile.timeLeft > 30)
			{
				if (projectile.timeLeft == 100)
					SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 0.8f, 0.1f);
				projectile.scale += 0.005f;
			}
			else
			{
				projectile.scale -= 0.015f;
			}
			projectile.rotation = MathHelper.ToRadians(finalRotation);
		}
        public override void Kill(int timeLeft)
        {
			for (int i = 0; i < 360; i += 24)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(10), 0).RotatedBy(MathHelper.ToRadians(i) + projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity += circularLocation;
				dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.4f;
			}
		}
    }
}