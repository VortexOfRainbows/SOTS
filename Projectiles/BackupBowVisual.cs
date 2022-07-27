using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items;
using SOTS.NPCs.Boss.Curse;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Projectiles
{
	public class BackupBowVisual : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Backup Bow Visual");
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
		}
		public List<CurseFoam> particleList = new List<CurseFoam>();
		public void ResetFoamLists()
		{
			List<CurseFoam> temp = new List<CurseFoam>();
			for (int i = 0; i < particleList.Count; i++)
			{
				if (particleList[i].active && particleList[i] != null)
					temp.Add(particleList[i]);
			}
			particleList = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				particleList.Add(temp[i]);
			}
		}
		public void catalogueParticles()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				CurseFoam particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					particleList.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						particleList.RemoveAt(i);
						i--;
					}
				}
			}
		}
        public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Color color = lightColor;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / frameCount / 2);
			Rectangle rectangleFrame = new Rectangle(0, texture.Height / frameCount * frame, texture.Width, texture.Height / frameCount);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			SOTS.VisionShader.Parameters["progress"].SetValue(itemAlpha);
			SOTS.VisionShader.Parameters["lightColor"].SetValue(color.ToVector4());
			SOTS.VisionShader.Parameters["colorMod"].SetValue(GetColor().ToVector4());
			SOTS.VisionShader.Parameters["uImageSize0"].SetValue(new Vector2(texture.Width, texture.Height));
			SOTS.VisionShader.Parameters["uSourceRect"].SetValue(new Vector4(0, texture.Height / frameCount * frame, texture.Width, texture.Height / frameCount));
			SOTS.VisionShader.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), rectangleFrame, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			GetColor();
		}
		public void SpawnDust(Texture2D texture, int rate, float percent, float rotation)
		{
			int width = texture.Width;
			int height = texture.Height / frameCount;
			Color[] data = new Color[texture.Width * texture.Height];
			int startAt = width * height * frame;
			texture.GetData(data);
			Vector2 position;
			int localX = 0;
			int localY = 0;
			for (int i = startAt; i < startAt + width * height; i++)
			{
				localX++;
				if (localX > width)
				{
					localX -= width;
					localY++;
				}
				float percentX = localX / (float)width;
				float percentY = localY / (float)height;
				float length = new Vector2(percentX - 0.5f, percentY - 0.5f).Length();
				if (rate <= 1)
					rate = 1;
				if (data[i].A >= 255 && Main.rand.NextBool(rate) && Math.Abs(length - percent) <= 0.03f)
				{
					Vector2 offset = -new Vector2(width / 2, height / 2) + new Vector2(localX, localY);
					offset.X *= Projectile.spriteDirection;
					position = offset.RotatedBy(rotation);
					Vector2 velocity = offset.SafeNormalize(Vector2.Zero).RotatedBy(rotation) * 0.4f;
					particleList.Add(new CurseFoam(position, velocity, 1.7f, true));
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Dusts/CopyDust4");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 6);
			Color color2 = GetColor();
			color2.A = 0;
			for (int i = 0; i < particleList.Count; i++)
			{
				Vector2 drawPos = Projectile.Center + particleList[i].position - Main.screenPosition;
				Color color = Projectile.GetAlpha(color2) * (0.35f + 0.65f * particleList[i].scale);
				Main.spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, texture.Width, texture.Height / 3), color, particleList[i].rotation, drawOrigin, particleList[i].scale * 0.9f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public Color GetColor()
		{
			return Color.Silver;
        }
		float itemAlpha = 0;
		int ticksPerFrame = 0;
		int frameCounter = 0;
		int frameCount = 1;
		int frame = 0;
		public void FindPosition()
		{
			Vector2 mousePos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			float rotation = 0f;
			if(mousePos.X > 0 && mousePos.Y > 0)
            {
				rotation = (mousePos - Projectile.Center).ToRotation();
            }
			Player player = Main.player[Projectile.owner];
			if (Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 20;
			Vector2 idlePosition = new Vector2(-24, 0).RotatedBy(rotation);
			if(player.direction < 0)
			{
				if(idlePosition.X < 8)
					idlePosition.X = 8;
			}
            else
			{
				if (idlePosition.X > -8)
					idlePosition.X = -8;
			}
			Projectile.spriteDirection = player.direction;
			//Projectile.ai[0]++;
			Projectile.rotation = idlePosition.ToRotation() - (Projectile.spriteDirection == 1 ? MathHelper.Pi : 0);
			idlePosition = player.Center + idlePosition;
			idlePosition.Y -= 2;
			Projectile.Center = idlePosition;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			FindPosition();
			catalogueParticles();
			ResetFoamLists();
			Projectile.spriteDirection *= -1;
			if (frameCount > 1)
			{
				frameCounter++;
				if (frameCounter > ticksPerFrame)
				{
					frame = (frame + 1) % frameCount;
					frameCounter -= ticksPerFrame;
				}
			}
			else
			{
				ticksPerFrame = 0;
				frame = 0;
				frameCounter = 0;
			}
			if(Main.myPlayer == Projectile.owner)
            {
				Projectile.ai[0] = Main.MouseWorld.X;
				Projectile.ai[1] = Main.MouseWorld.Y;
				if (SOTSWorld.GlobalCounter % 5 == 0)
					Projectile.netUpdate = true;
            }
			/*if (player.itemAnimation > 0)
			{
				itemAlpha = -0.48f;
			}*/
			if (itemAlpha < 2)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					SpawnDust(Terraria.GameContent.TextureAssets.Projectile[Type].Value, 7, itemAlpha, Projectile.rotation);
				}
				itemAlpha += 0.03f;
			}
			if (itemAlpha > 2)
				itemAlpha = 2;
			Lighting.AddLight(Projectile.Center, GetColor().ToVector3() * 0.3f);
		}
	}
}