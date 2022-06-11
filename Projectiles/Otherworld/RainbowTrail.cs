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

namespace SOTS.Projectiles.Otherworld
{    
    public class RainbowTrail : ModProjectile 
    {
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rainbow Trail");
		}
        public override void SetDefaults()
        {
			Projectile.width = 54;
			Projectile.height = 54;
			Projectile.friendly = true;
			Projectile.timeLeft = 3600;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.alpha = 255;
			Projectile.ai[1] = -1;
		}
		int counter = 0;
		Color color = Color.White;
		public override bool PreAI()
		{
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			counter += 1;
			return base.PreAI();
		}
		public override void PostAI()
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.7f / 255f, (255 - Projectile.alpha) * 1f / 255f, (255 - Projectile.alpha) * 1.45f / 255f);
			checkPos();
			if (counter % 2 == 0)
				cataloguePos();
		}
		Vector2[] trailPos = new Vector2[30];
		public override bool PreDraw(ref Color lightColor)
		{
			int counter2 = counter;
			if (runOnce)
				return true;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/RainbowTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				counter2++;
				float newAi = counter2 * 4 / 13f;
				double frequency = 0.3;
				double center = 127;
				double width = 128;
				double red = Math.Sin(frequency * (double)newAi) * width + center;
				double grn = Math.Sin(frequency * (double)newAi + 2.0) * width + center;
				double blu = Math.Sin(frequency * (double)newAi + 4.0) * width + center;
				this.color = new Color((int)red, (int)grn, (int)blu);
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 0.7f;
				if (trailPos[k] == Vector2.Zero)
				{
					return true;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				Color color = this.color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.25f;
				color = color.MultiplyRGBA(new Color(140, 140, 140, 0));
				float max = betweenPositions.Length() / (4 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 7; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.4f * scale;
						float y = Main.rand.Next(-10, 11) * 0.4f * scale;
						if (j <= 1)
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
			return true;
		}
		bool runOnce = true;
		public void cataloguePos()
		{
			if (end)
			{
				Vector2 current = Projectile.Center;
				for (int i = 0; i < trailPos.Length; i++)
				{
					Vector2 previousPosition = trailPos[i];
					trailPos[i] = current;
					current = previousPosition;
				}
			}
			else
			{
				Vector2 current = Projectile.Center + new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(counter * 10));
				for (int i = 0; i < trailPos.Length; i++)
				{
					Vector2 previousPosition = trailPos[i];
					trailPos[i] = current;
					current = previousPosition;
				}
			}
		}
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			//if (iterator >= trailPos.Length)
			//	Projectile.Kill();
		}
		int endHow = 0;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 0.7f;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Vector2 currentPos = trailPos[k];
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition, currentPos, 24f * scale, ref point))
				{
					return true;
				}
				previousPosition = currentPos;
			}
			return false;
		}
		bool end = false;
		public override void AI()
		{
			if(Projectile.ai[1] != -1 && end == false)
			{
				Projectile proj = Main.projectile[(int)Projectile.ai[1]];
				if(proj.active && proj.type == ModContent.ProjectileType<Poyoyo>() && proj.owner == Projectile.owner)
				{
					Projectile.position.X = proj.Center.X - Projectile.width/2;
					Projectile.position.Y = proj.Center.Y - Projectile.height/2;
					Projectile.timeLeft = 60;
				}
				else
                {
					end = true;
				}
			}
		}
	}
}
		