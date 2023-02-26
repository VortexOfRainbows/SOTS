using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Camera
{    
    public class DreamingFrame : ModProjectile 
    {
		public Color Green1 => new Color(86, 226, 100, 0);
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;    
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
			Projectile.friendly = true;
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			/*Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}*/
			Player player = Main.player[Projectile.owner];
			float scaleMult = 1f;
			float windUpProgress = windUp / windUpTime;
			Color color = Green1;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/CameraCenterCross");
			Texture2D textureGradient = (Texture2D)ModContent.Request<Texture2D>("SOTS/Assets/LongGradient");
			Texture2D borderTexture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/CameraBorder");
			Texture2D frameTexture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 center = Projectile.Center - Main.screenPosition;
			float squareRadius = 60;
			for (int i = 0; i < 4; i++)
			{
				float progress = windUpProgress;
				Vector2 playerToProjectile = Projectile.Center + new Vector2(0, Projectile.gfxOffY) - player.Center;
				Vector2 offset = player.Center + new Vector2(24, -5 * player.direction).RotatedBy(playerToProjectile.ToRotation());
				Vector2 framePosition = new Vector2(-squareRadius, -squareRadius);
				float borderRotation = 0;
				progress /= 0.75f;
				if (i == 1)
				{
					borderRotation = MathHelper.PiOver2;
					framePosition = Vector2.Lerp(framePosition, new Vector2(squareRadius, -squareRadius), MathHelper.Clamp(progress, 0, 1));
				}
				if (i == 2)
				{
					borderRotation = MathHelper.Pi;
					framePosition = Vector2.Lerp(framePosition, new Vector2(squareRadius, squareRadius), MathHelper.Clamp(windUpProgress / 0.95f, 0, 1));
				}
				if (i == 3)
				{
					borderRotation = -MathHelper.PiOver2;
					framePosition = Vector2.Lerp(framePosition, new Vector2(-squareRadius, squareRadius), MathHelper.Clamp(progress, 0, 1));
				}
				float borderVisibility = 1;
				Vector2 laserFramePosition = framePosition;
				if(postCounter > 0)
                {
					float theta = (postCounter % 180) / 180f;
					Vector2 nextFramePosition = laserFramePosition.RotatedBy(MathHelper.PiOver2);
					laserFramePosition = Vector2.Lerp(laserFramePosition, nextFramePosition, (1 - (float)Math.Cos(theta * Math.PI)) / 2f);
                }
				Vector2 playerToFrame = Projectile.Center + new Vector2(0, Projectile.gfxOffY) - offset + laserFramePosition;
				float lengthToFrame = playerToFrame.Length();
				for (int j = 0; j < 2; j++)
				{
					float bonusMultiplier = MathHelper.Clamp(windUpProgress, 0, 1) * (0.6f - j * 0.3f);
					Main.spriteBatch.Draw(textureGradient, offset - Main.screenPosition, null, color * bonusMultiplier, playerToFrame.ToRotation(), new Vector2(1, 1), new Vector2(1f / (textureGradient.Width - 24) * lengthToFrame, 1f), SpriteEffects.None, 0);
				}
				if (i == 1 || i == 2)
				{
					progress = progress * 0.75f - 0.75f;
					progress *= 4;
				}
				if (progress < 0)
					progress = 0;
				if (progress > 1)
					progress = 1;
				if (i == 1)
				{
					borderRotation = MathHelper.PiOver2;
					framePosition = new Vector2(squareRadius, -squareRadius);
				}
				if (i == 2)
				{
					borderRotation = MathHelper.Pi;
					framePosition = Vector2.Lerp(new Vector2(-squareRadius, squareRadius), new Vector2(squareRadius, squareRadius), progress);
				}
				Main.spriteBatch.Draw(borderTexture, center + framePosition, null, color * (0.5f + 0.75f * progress) * borderVisibility, borderRotation, new Vector2(0, 1), new Vector2(1f / borderTexture.Width * squareRadius * 2 * progress, 0.75f), SpriteEffects.None, 0);
			}
			float longerExplode = postCounter;
			if (longerExplode > 10)
				longerExplode = 10;
			float starWindUp = (windUp + longerExplode - ActivateRange + 3) / (windUpTime - ActivateRange + 13);
			if(starWindUp > 0)
			{
				scaleMult *= (1.2f - 0.2f * (float)Math.Cos(MathHelper.ToRadians(420 * starWindUp))); // 11 / 10 scale is final
				SOTSProjectile.DrawStar(Projectile.Center, color, 0.2f * starWindUp, MathHelper.PiOver4, 0f, 4, 12.8f * scaleMult, 12 * scaleMult, 1f, 540, 4.8f * scaleMult, 1);
				SOTSProjectile.DrawStar(Projectile.Center, color, 0.4f * starWindUp, 0, 0f, 4, 2.56f * scaleMult, 0, 1f, 240, 0, 1);
				for (int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(1.5f, 0).RotatedBy(MathHelper.ToRadians(45 * i));
					Main.spriteBatch.Draw(texture, center + circular, null, color * 0.6f * starWindUp * starWindUp, 0, texture.Size() / 2, scaleMult * 0.8f, SpriteEffects.None, 0);
				}
			}
			for (int i = 0; i < 4; i++)
			{
				float rotation = MathHelper.PiOver2 * i;
				Vector2 framePosition = new Vector2(-squareRadius, -squareRadius).RotatedBy(rotation);
				float remove1 = (i == 1 || i == 3) ? 0.975f : i == 2 ? 1.95f : 0; 
				float progress = windUp / windUpTime * 2 - remove1;
				if (i == 2)
					progress *= 2;
				progress = Math.Clamp(progress, 0, 1);
				Vector2 startingPosition = new Vector2(0, 0);
				float width = 0;
				float height = 0;
				if(i == 0) //start from top left, end bottom right
                {
					width = 19;
					height = 19;
					if(progress > 1f / 4f)
                    {
						width = 27;
						height = 27;
					}
					if (progress > 2f / 4f)
					{
						width = 33;
						height = 33;
					}
					if (progress > 3f / 4f)
					{
						width = 36;
						height = 36;
					}
				}
				if(i == 1) //start from bottom left, but split, end top right
				{
					if(progress > 0)
					{
						startingPosition = new Vector2(0, 33);
						width = 19;
						height = 3;
					}
					if (progress > 1f / 7f)
					{
						startingPosition = new Vector2(0, 27);
						width = 19;
						height = 9;
					}
					if (progress > 2f / 7f)
					{
						startingPosition = new Vector2(0, 19);
						width = 19;
						height = 17;
					}
					if (progress > 3f / 7f)
					{
						startingPosition = new Vector2(0, 0);
						width = 19;
						height = 36;
					}
					if (progress > 4f / 7f)
					{
						width = 27;
						height = 36;
					}
					if (progress > 5f / 7f)
					{
						width = 33;
						height = 36;
					}
					if (progress > 6f / 7f)
					{
						width = 36;
						height = 36;
					}
				}
				if (i == 3) //start from top right, but split, end bottom left
				{
					if (progress > 0)
					{
						startingPosition = new Vector2(33, 0);
						height = 19;
						width = 3;
					}
					if (progress > 1f / 7f)
					{
						startingPosition = new Vector2(27, 0);
						height = 19;
						width = 9;
					}
					if (progress > 2f / 7f)
					{
						startingPosition = new Vector2(19, 0);
						height = 19;
						width = 17;
					}
					if (progress > 3f / 7f)
					{
						startingPosition = new Vector2(0, 0);
						height = 19;
						width = 36;
					}
					if (progress > 4f / 7f)
					{
						height = 27;
						width = 36;
					}
					if (progress > 5f / 7f)
					{
						height = 33;
						width = 36;
					}
					if (progress > 6f / 7f)
					{
						height = 36;
						width = 36;
					}
				}
				if (i == 2) //finish all at once
				{
					if(progress > 0)
                    {
						width = 36;
						height = 36;
                    }						
				}
				Rectangle frameRectangle = new Rectangle((int)startingPosition.X, (int)startingPosition.Y, (int)width, (int)height);
				float scaleMult2 = 1.25f * (0.75f + 0.25f * scaleMult);
				for (int j = 0; j < 8; j++)
				{
					Vector2 circular = new Vector2(1.0f, 0).RotatedBy(MathHelper.ToRadians(45 * j));
					Main.spriteBatch.Draw(frameTexture, center + framePosition + circular - new Vector2(startingPosition.Y, startingPosition.X) * scaleMult2, frameRectangle, color * 0.6f, rotation, new Vector2(9, 9), scaleMult2, SpriteEffects.None, 0);
				}
				Main.spriteBatch.Draw(frameTexture, center + framePosition - new Vector2(startingPosition.Y, startingPosition.X) * scaleMult2, frameRectangle, color * 1.1f, rotation, new Vector2(9, 9), scaleMult2, SpriteEffects.None, 0);
			}
			return false;
		}
		private static float windUpTime = 18f;
		public static float ActivateRange = 12f;
		float postCounter = 0;
		bool runOnce = true;
		float windUp = 0f;
		public override void AI()
		{ 
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, new Vector3(86 / 255f, 226 / 255f, 100 / 255f) * windUp / windUpTime);
			Projectile.rotation += 0.2f;
			if (Projectile.alpha > 0)
				Projectile.alpha -= 6; //this should take 43 frames to fully remove
			else
				Projectile.alpha = 0;
			if (windUp < windUpTime)
				windUp += 1;
			else
				postCounter++;
			if (Main.myPlayer == Projectile.owner)
            {
				Projectile.ai[0] = Main.MouseWorld.X;
				Projectile.ai[1] = Main.MouseWorld.Y;
				Projectile.netUpdate = true;
			}
			if (player.channel || Projectile.timeLeft > 2)
			{
				if(player.itemTime < 2)
				{
					player.itemAnimation = 2;
					player.itemTime = 2;
					Projectile.timeLeft = 2;
				}
				if(Projectile.timeLeft < 2)
					Projectile.timeLeft = 2;
			}
			if(windUp > ActivateRange)
            {
				if(runOnce)
				{
					Vector2 addedVelocity = Projectile.position - Projectile.oldPosition + Projectile.velocity;
					addedVelocity *= 0.9f;
					SOTSUtils.PlaySound(SoundID.Item28, Projectile.Center, 0.7f, -0.2f, 0f);
					int total = 16;
					for(int i = 0; i < total; i++)
					{
						float scaleMult = 1f;
						Vector2 FlowerVe = new Vector2(-60f, -60f).RotatedBy(MathHelper.ToRadians(i * 360f / total));
						if (i % 4 != 0)
							scaleMult = 0.6f;
						Vector2 velo = Main.rand.NextVector2Circular(1, 1) + FlowerVe.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2f, 3f);
						SOTSProjectile.DustStar(Projectile.Center + FlowerVe, addedVelocity + velo * scaleMult, Green1 * scaleMult, 0f, 40, 0, 4, 8f, 5f, 1f, 1f * (0.2f + 0.8f * scaleMult));
					}
					for (int i = 0; i < 60; i++)
					{
						Vector2 circular = new Vector2(Main.rand.NextFloat(10f, 15f), 0).RotatedBy(MathHelper.ToRadians(i * 6f + Main.rand.NextFloat(-3f, 3f)));
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>());
						dust.color = Green1;
						dust.velocity = dust.velocity * 0.2f + circular + addedVelocity;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = dust.scale * 0.5f + 1.0f;
					}
				}
				runOnce = false;
            }
			float speedMult = windUp / windUpTime;
			if (Projectile.ai[0] > 0 && Projectile.ai[1] > 0)
            {
				Vector2 mousePos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
				Vector2 toMousePos = mousePos - Projectile.Center;
				Vector2 velo = toMousePos.SafeNormalize(Vector2.Zero) * 3.2f * speedMult;
				if(velo.Length() > toMousePos.Length())
					velo = toMousePos;
				Projectile.velocity *= 0.4f;
				Projectile.velocity += velo;
				Projectile.Center = Vector2.Lerp(Projectile.Center, mousePos, 0.016f * speedMult);
            }
			Projectile.oldPosition = Projectile.position;
		}
	}
}
		