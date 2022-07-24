using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class RubySpawnerFinder : ModProjectile
	{
		private float findLocationX
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		private float findLocationY
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Spawner Finder");
		}
        public override void SetDefaults()
        {
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
		}
		public void findTravelTo()
        {
			List<Vector2> validPos = new List<Vector2>();
			for (int i = 4; i < 20; i++)
			{
				for(int j = 0; j < 2; j++)
				{
					Vector2 circular = new Vector2(16 * i, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					circular += Projectile.Center;
					int x = (int)circular.X / 16;
					int y = (int)circular.Y / 16;
					if (SOTSWorldgenHelper.Empty(x - 1, y - 1, 3, 3))
					{
						validPos.Add(new Vector2(x * 16 + 8, y * 16 + 8));
						break;
					}
				}
			}
			for(int k = 0; k < validPos.Count; k++)
            {
				if(Main.rand.NextBool((int)Math.Pow(validPos.Count - k, 1.4)))
                {
					findLocationX = validPos[k].X;
					findLocationY = validPos[k].Y;
					break;
                }
            }
        }
		bool runOnce = true;
		Vector2[] trailPos = new Vector2[9];
		public void cataloguePos(Vector2 catalogue, Vector2[] trialArray)
		{
			Vector2 current = catalogue;
			for (int i = 0; i < trialArray.Length; i++)
			{
				Vector2 previousPosition = trialArray[i];
				trialArray[i] = current;
				current = previousPosition;
			}
		}
		public override void AI()
		{
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				if(Main.netMode != 1)
                {
					findTravelTo();
					if (findLocationX == 0 && findLocationY == 0)
					{
						findLocationY = Projectile.Center.Y - 48;
						findLocationX = Projectile.Center.X;
					}
					Projectile.netUpdate = true;
				}
            }
			else
			{
				float approaching = Projectile.timeLeft / 120f;
				Lighting.AddLight(Projectile.Center, 0.25f, 0.1f, 0.1f);
				if(Main.rand.NextBool(5))
				{
					int num2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					Dust dust = Main.dust[num2];
					dust.color = new Color(180, 140, 140, 40);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.1f;
					dust.velocity *= 0.75f;
				}
				Vector2 toLoc = Projectile.Center - new Vector2(findLocationX, findLocationY);
				if(toLoc.Length() < 4)
                {
					Projectile.velocity *= 0f;
                }
				else
				{
					toLoc = toLoc.RotatedBy(MathHelper.ToRadians((Projectile.whoAmI % 2 * 2 - 1) * 24 * approaching)) * (0.5f + 1 - approaching);
					Projectile.velocity *= approaching;
					Projectile.velocity += -toLoc * 0.04f;
				}
			}
			if (trailPos[trailPos.Length - 1] == Projectile.Center)
				Projectile.Kill();
			cataloguePos(Projectile.Center, trailPos);
		}
        public override void Kill(int timeLeft)
		{
			if(Main.netMode != 1)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<RubySpawner>(), (int)(Projectile.damage * 1f), 0, Main.myPlayer);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Vector2 circularLocation = Projectile.Center;
			if (runOnce)
				return false;
			Vector2 current = circularLocation;
			Draw(Main.spriteBatch, trailPos, current);
			return false;
		}
		public void Draw(SpriteBatch spriteBatch, Vector2[] trailArray, Vector2 current)
		{
			Texture2D texture2 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 previousPosition = current;
			Color color = new Color(100, 75, 75, 0);
			for (int k = 0; k < trailArray.Length; k++)
			{
				if (trailArray[k] == Vector2.Zero)
				{
					return;
				}
				if(trailArray[k] != Projectile.Center)
				{
					float scale = Projectile.scale * (trailArray.Length - k) / (float)trailArray.Length;
					scale *= 1f;
					Vector2 drawPos = trailArray[k] - Main.screenPosition;
					Vector2 currentPos = trailArray[k];
					Vector2 betweenPositions = previousPosition - currentPos;
					color *= 0.95f;
					float max = betweenPositions.Length() / (4 * scale);
					for (int i = 0; i < max; i++)
					{
						drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
						for (int j = 0; j < 3; j++)
						{
							float x = Main.rand.Next(-10, 11) * 0.1f * scale;
							float y = Main.rand.Next(-10, 11) * 0.1f * scale;
							if (j <= 1)
							{
								x = 0;
								y = 0;
							}
							spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin2, scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
						}
					}
					previousPosition = currentPos;
				}
			}
		}
	}
}
		