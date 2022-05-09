using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Projectiles.Pyramid
{
	public class RubyMonolith : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Monolith");
			Main.projFrames[Projectile.type] = 2;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 68;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
		}
		private int shader = 0;
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/RubyMonolithGlow").Value;
			Rectangle frame = new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f);
			float counter = Main.GlobalTime * 160;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 50, 0, 0);
						break;
					case 2:
						color = new Color(255, 100, 0, 0);
						break;
					case 3:
						color = new Color(255, 150, 0, 0);
						break;
					case 4:
						color = new Color(255, 200, 0, 0);
						break;
					case 5:
						color = new Color(255, 250, 0, 0);
						break;
				}
				Vector2 rotationAround = new Vector2((4 + mult) * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture4, Projectile.Center - Main.screenPosition + rotationAround, frame, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, drawColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player owner = Main.player[Projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				GameShaders.Armor.GetSecondaryShader(shader, owner).Apply(null);
			}
			return false;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			shader = player.cBody;
			if (Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 20;
			if (modPlayer.CanCurseSwap)
			{
				if (modPlayer.CurseSwap)
				{
					if(Projectile.ai[1] == 0)
						Projectile.ai[1] = 1;
					else
						Projectile.ai[1] = 0;
					if (Main.myPlayer == Projectile.owner)
						Projectile.netUpdate = true; 
				}
			}
			else
			{
				Projectile.ai[1] = 0;
			}
			if(Projectile.frame != Projectile.ai[1])
            {
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 4, 0.9f, 0.2f);
				for (int i = 0; i < 30; i++)
				{
					Vector2 circular = new Vector2(36, 0).RotatedBy(MathHelper.ToRadians(i * 12));
					circular.X *= 0.5f;
					circular = circular.RotatedBy(Projectile.rotation);
					for(int j = 0; j < 2; j++)
					{
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5) + circular, 0, 0, ModContent.DustType<CopyDust4>());
						dust.color = new Color(220, 80, 80, 40);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.alpha = 40;
						dust.velocity *= 0.1f;
						dust.velocity += circular * (0.1f + j * 0.05f);
						dust.scale = 2f - j * 0.5f;
						dust.velocity += player.velocity * 1.0f;
					}
				}
				Projectile.frame = (int)Projectile.ai[1];
			}
			if (Projectile.frame == 1)
            {
				player.AddBuff(ModContent.BuffType<RubyMonolithAttack>(), 2);
            }
			else
			{
				player.AddBuff(ModContent.BuffType<RubyMonolithDefense>(), 2);
			}
			Vector2 idlePosition = player.Center;
			idlePosition.X -= player.direction * 32f;
			Projectile.spriteDirection = player.direction;
			Projectile.ai[0]++;
			float sin = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[0] * 4)) * 4;
			idlePosition.Y += sin - 8;
			Projectile.Center = idlePosition;
			Projectile.rotation = (Projectile.oldPosition.X - Projectile.position.X) * -0.05f;
			Lighting.AddLight(Projectile.Center, 120 / 255f, 40 / 255f, 60 / 255f);
		}
	}
}