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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
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
			Color color = Projectile.GetAlpha(new Color(253, 198, 234));
			color.A = 0;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			SOTS.GodrayShader.Parameters["distance"].SetValue(3);
			SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
			SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
			SOTS.GodrayShader.Parameters["rotation"].SetValue(Projectile.rotation + Projectile.whoAmI + Main.GameUpdateCount * MathHelper.PiOver2 / 180f);
			SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f);
			SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale * 0.5f, SpriteEffects.None, 0f);
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
					color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
					color.A = 0;
				}
				else
					circular *= 0f;
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, Projectile.GetAlpha(color * 0.8f), Projectile.rotation, drawOrigin, Projectile.scale * 1.0f, SpriteEffects.None, 0f);
			}
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
		float bonusRotation = 90;
        public override void AI()
		{
			float finalRotation = Projectile.ai[0];
			if(runOnce)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int amt = 4;
					for (int i = 0; i < amt; i++)
					{
						Vector2 circular = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i * 360f / amt + finalRotation));
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<DogmaLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, -0.5f);
					}
				}
				runOnce = false;
				Projectile.scale = 1;
				Projectile.alpha = 255;
			}
			if (Projectile.alpha > 0)
				Projectile.alpha -= 5;
			else
				Projectile.alpha = 0;
			if (Projectile.timeLeft > 30)
			{
				if (Projectile.timeLeft == 100)
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 15, 0.8f, 0.1f);
				Projectile.scale += 0.005f;
			}
			else
			{
				Projectile.scale -= 0.015f;
			}
			Projectile.rotation = MathHelper.ToRadians(finalRotation);
		}
        public override void Kill(int timeLeft)
        {
			for (int i = 0; i < 360; i += 24)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(10), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
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