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

namespace SOTS.Projectiles.Lightning
{    
    public class GreenLightning2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Green Lightning");
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 120;
			Projectile.scale = 1f;
		}
		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			Color color = new Color(140, 200, 140, 0) * ((255 - Projectile.alpha) / 255f);
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				float scale = Projectile.scale;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / (texture.Width * scale * 0.5f);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != Projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		bool runOnce = true;
		Vector2[] trailPos = new Vector2[90];
		Vector2 addPos = Vector2.Zero;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
		int[] randStorage = new int[90];
		int dist = 90;
		float extraLength = 0;
		float lengthMultiplier = 0.4f;
		bool runOnce2 = true;
		public override void AI()
		{
			if (runOnce)
			{
				Projectile.position += Projectile.velocity.SafeNormalize(Vector2.Zero) * 24;
				SOTSUtils.PlaySound(SoundID.Item94, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, 0.2f);
				for (int i = 0; i < randStorage.Length; i++)
				{
					randStorage[i] = Main.rand.Next(-75, 76);
				}
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				originalVelo = Projectile.velocity.SafeNormalize(Vector2.Zero) * 8f;
				originalPos = Projectile.Center;
				runOnce = false;
				if (Projectile.ai[0] == -1) 
					lengthMultiplier = 0.25f;
			}

			Vector2 temp = originalPos;
			addPos = Projectile.Center;
			for (int i = 0; i < dist; i++)
			{
				originalPos += originalVelo * 0.125f * (8f + lengthMultiplier * extraLength);
				for (int reps = 0; reps < 18; reps++)
				{
					Vector2 attemptToPosition = (originalPos + originalVelo * (8f + lengthMultiplier * extraLength)) - addPos;
					addPos += new Vector2(originalVelo.Length(), 0).RotatedBy(attemptToPosition.ToRotation() + MathHelper.ToRadians(randStorage[i]));
					trailPos[i] = addPos;
				}
				if(runOnce2)
				{
					extraLength++;
				}
			}
			runOnce2 = false;
			extraLength += 3.5f;
			originalPos = temp;
			Projectile.alpha += 5;
			if (Projectile.alpha >= 255)
				Projectile.Kill();

			Projectile.scale *= 0.98f;
			Projectile.friendly = false;
		}
	}
}
		