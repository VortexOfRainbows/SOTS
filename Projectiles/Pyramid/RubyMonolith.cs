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
			Main.projFrames[projectile.type] = 2;
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 68;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 300;
			projectile.netImportant = true;
		}
		private int shader = 0;
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D texture4 = mod.GetTexture("Projectiles/Pyramid/RubyMonolithGlow");
			Rectangle frame = new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height);
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
				Vector2 rotationAround = new Vector2((4 + mult) * projectile.scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture4, projectile.Center - Main.screenPosition + rotationAround, frame, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, frame, drawColor, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player owner = Main.player[projectile.owner];
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
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			shader = player.cHead;
			if (Main.myPlayer != projectile.owner)
				projectile.timeLeft = 20;
			if (modPlayer.CanCurseSwap)
			{
				if (modPlayer.CurseSwap)
				{
					if(projectile.ai[1] == 0)
						projectile.ai[1] = 1;
					else
						projectile.ai[1] = 0;
					if (Main.myPlayer == projectile.owner)
						projectile.netUpdate = true; 
				}
			}
			else
			{
				projectile.ai[1] = 0;
			}
			if(projectile.frame != projectile.ai[1])
            {
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 4, 0.9f, 0.2f);
				for (int i = 0; i < 30; i++)
				{
					Vector2 circular = new Vector2(36, 0).RotatedBy(MathHelper.ToRadians(i * 12));
					circular.X *= 0.5f;
					circular = circular.RotatedBy(projectile.rotation);
					for(int j = 0; j < 2; j++)
					{
						Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5) + circular, 0, 0, ModContent.DustType<CopyDust4>());
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
				projectile.frame = (int)projectile.ai[1];
			}
			if (projectile.frame == 1)
            {
				player.AddBuff(ModContent.BuffType<RubyMonolithAttack>(), 2);
            }
			else
			{
				player.AddBuff(ModContent.BuffType<RubyMonolithDefense>(), 2);
			}
			Vector2 idlePosition = player.Center;
			idlePosition.X -= player.direction * 32f;
			projectile.ai[0]++;
			float sin = (float)Math.Sin(MathHelper.ToRadians(projectile.ai[0] * 4)) * 4;
			idlePosition.Y += sin - 8;
			projectile.Center = idlePosition;
			projectile.rotation = (projectile.oldPosition.X - projectile.position.X) * -0.05f;
			Lighting.AddLight(projectile.Center, 120 / 255f, 40 / 255f, 60 / 255f);
		}
	}
}