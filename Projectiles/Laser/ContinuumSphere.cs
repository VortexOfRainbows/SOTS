using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Projectiles.Laser
{    
    public class ContinuumSphere : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Continuum Sphere");
		}
		
        public override void SetDefaults()
        {
			Projectile.height = 30;
			Projectile.width = 30;
			Projectile.penetrate = 24;
			Projectile.friendly = false;
			Projectile.timeLeft = 6004;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
		}
		Vector2[] orbs = new Vector2[6]; //these should be the same size
		Vector2[] orbsTo = new Vector2[6];
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			float newAi = Projectile.ai[1] * 2 / 13f;
			double frequency = 0.3;
			double center = 130;
			double width = 125;
			double red = Math.Sin(frequency * (double)newAi) * width + center;
			double grn = Math.Sin(frequency * (double)newAi + 2.0) * width + center;
			double blu = Math.Sin(frequency * (double)newAi + 4.0) * width + center;
			Color color = new Color((int)red, (int)grn, (int)blu);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Laser/ContinuumSphereHighlight");
			if(orbs.Length == 1)
			{
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 255 - Projectile.alpha), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			else if(ai1 >= 9)
			{
				for(int i = 0; i < orbs.Length; i++)
				{
					Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, orbs[i] - Main.screenPosition, null, color, Projectile.rotation, drawOrigin, Projectile.scale * (0.75f + (0.25f * (ai2/85f))), SpriteEffects.None, 0f);
					Main.spriteBatch.Draw(texture, orbs[i] - Main.screenPosition, null, new Color(255, 255, 255, 255 - Projectile.alpha), Projectile.rotation, drawOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0f);
				}
			}
		}
		float expo = 1f;
		int ai1 = 0;
		float distance2 = 36;
		int distCounter = 0;
		float ai2 = 0;
		public override bool ShouldUpdatePosition() 
		{
			return false;
		}
		public override bool PreAI()
		{
			if (!Projectile.active)
				return false;

			Player player  = Main.player[Projectile.owner];
			if(Projectile.ai[1] == 0f)
			{
				Projectile.ai[0] = Main.rand.Next(1020);
			}
			Vector2 cursorArea = Main.MouseWorld;
			float shootToX = cursorArea.X - player.Center.X;
			float shootToY = cursorArea.Y - player.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
			distance = 4f / distance;
			shootToX *= distance * 5f;
			shootToY *= distance * 5f;
			expo *= 1.0075f;
			if(distance2 < 16 && orbs.Length == 6)
			{ 
				orbs = new Vector2[5];
				orbsTo = new Vector2[5];
			}
			if(distance2 < 12 && orbs.Length == 5)
			{ 
				orbs = new Vector2[4];
				orbsTo = new Vector2[4];
			}
			if(distance2 < 9 && orbs.Length == 4)
			{ 
				orbs = new Vector2[3];
				orbsTo = new Vector2[3];
			}
			if(distance2 < 6 && orbs.Length == 3)
			{ 
				orbs = new Vector2[2];
				orbsTo = new Vector2[2];
			}
			if(distance2 == 0 && orbs.Length == 2)
			{
				orbs = new Vector2[1];
				orbsTo = new Vector2[1];
				distCounter++;
			}
			for(int i = 0; i < orbs.Length; i++)
			{
				Vector2 rotate = new Vector2(distance2, 0).RotatedBy(MathHelper.ToRadians(i * (360f/orbs.Length) + (ai1 * expo)));
				orbs[i] = Projectile.Center + rotate;
				Vector2 distanceToP = new Vector2(shootToX, shootToY);
				orbsTo[i] = (distanceToP * 2f) + Projectile.Center + rotate;
			}
			Projectile.ai[1] ++;
			if(ai2 < 85)
			{
				ai2 += 1.0625f/3f;
			}
			if(distance2 > 6)
			{
				distance2 -= 0.15f;
			}
			else if(distance2 > 3)
			{
				distance2 -= 0.10f;
			}
			else if(distance2 > 0)
			{
				distance2 -= 0.05f;
			}
			else
			{
				distance2 = 0;
			}
			ai1++;

			if (Main.myPlayer == player.whoAmI)
			{
				double startingDirection = Math.Atan2((double)-shootToY, (double)-shootToX);
				startingDirection *= 180 / Math.PI;
				Projectile.ai[0] = (float)startingDirection;
				double deg = (double)Projectile.ai[0];
				double rad = deg * (Math.PI / 180);
				double dist = 32;
				Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
				Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

				Projectile.netUpdate = true;
				if (player.channel || Projectile.timeLeft > 6)
				{
					Projectile.timeLeft = 6;
					Projectile.alpha = 0;
					if (Main.myPlayer == Projectile.owner && ai1 > 1)
					{
						for (int i = 0; i < orbs.Length; i++)
						{
							float shootToX2 = orbsTo[i].X - player.Center.X;
							float shootToY2 = orbsTo[i].Y - player.Center.Y;
							float distance2 = (float)Math.Sqrt((double)(shootToX2 * shootToX2 + shootToY2 * shootToY2));
							distance2 = 3.9f / distance2;
							shootToX2 *= distance2 * 5f;
							shootToY2 *= distance2 * 5f;
							if (orbs.Length != 1)
							{
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbs[i].X + shootToX2, orbs[i].Y + shootToY2, shootToX2, shootToY2, ModContent.ProjectileType<SmallCollapseLaser>(), (int)(Projectile.damage), 1f, Projectile.owner, Projectile.ai[1], ai2); //second ai slot is scale
							}
							else
							{
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), orbs[i].X + shootToX2, orbs[i].Y + shootToY2, shootToX2, shootToY2, ModContent.ProjectileType<CollapseLaser>(), Projectile.damage * 3, 1f, Projectile.owner, Projectile.ai[1], 0f);
							}
						}
					}
				}
				if (ai1 % 20 == 0 && Main.myPlayer == player.whoAmI)
				{
					Item item = player.HeldItem;
					VoidItem vItem = item.ModItem as VoidItem;
					if (vItem != null)
						vItem.DrainMana(player);
				}
			}
			return true;
		}
	}
}