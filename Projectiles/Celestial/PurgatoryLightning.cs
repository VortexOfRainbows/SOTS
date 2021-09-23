using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{
	public class PurgatoryLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purgatory Column");
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.velocity.X);
			writer.Write(projectile.velocity.Y);
			writer.Write(projectile.scale);
			writer.Write(projectile.rotation);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.velocity.X = reader.ReadSingle();
			projectile.velocity.Y = reader.ReadSingle();
			projectile.scale = reader.ReadSingle();
			projectile.rotation = reader.ReadSingle();
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.extraUpdates = 3;
		}
		Vector2[] trailPos = new Vector2[8];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
                {
					return false;
                }
				Color color = new Color(120, 250, 130, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (texture.Height * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if(trailPos[k] != projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
		bool runOnce = true;
		public void cataloguePos()
        {
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
        }
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if(endHow == 1 && endHow != 2 && Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 1.25f;
				dust.scale *= 1.5f * (trailPos.Length - iterator) / (float)trailPos.Length;
				dust.fadeIn = 0.1f;
				dust.color = new Color(160, 255, 140, 0);
			}
			if (iterator >= trailPos.Length)
				projectile.Kill();
		}
		int endHow = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			endHow = 1;
			projectile.tileCollide = false;
			projectile.velocity *= 0f;
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < 5; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Vector2 currentPos = trailPos[k];
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition, currentPos, 14f * scale, ref point))
                {
					return true;
                }
				previousPosition = currentPos;
			}
			return false;
        }
		int counter = 0;
		int counter2 = 0;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
		public override void AI()
		{
			if (projectile.timeLeft < 220)
			{
				endHow = 2;
				projectile.tileCollide = false;
				projectile.velocity *= 0f;
			}
			if (runOnce)
			{
				originalVelo = projectile.velocity;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				originalPos = projectile.Center;
			}
			originalPos += originalVelo * 1.4f;
			checkPos();
			Player player = Main.player[projectile.owner];
			if (projectile.ai[0] > 0 && counter2 > 80 - projectile.ai[0] * 10)
			{
				for (int i = 0; i < 3; i += 2)
				{
					if (Main.myPlayer == projectile.owner)
					{
						Vector2 perturbedSpeed = new Vector2(originalVelo.X, originalVelo.Y).RotatedBy(MathHelper.ToRadians((i - 1) * 5.5f));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, projectile.type, projectile.damage, 1f, Main.myPlayer, projectile.ai[0] - 1);
					}
				}
				projectile.velocity *= 0f;
				originalVelo *= 0f;
				projectile.ai[0] = 0f;
			}
			counter++;
			counter2++;
			if (counter >= 0)
			{
				cataloguePos();
				counter = -14;
				if (projectile.velocity.Length() != 0f)
				{
					Vector2 toPos = originalPos - projectile.Center;
					projectile.velocity = new Vector2(originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(projectile.ai[1]));
					projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				}
			}
			else if (counter >= -2)
			{
				if (Main.netMode != 1 || Main.myPlayer == projectile.owner)
				{
					projectile.ai[1] = Main.rand.Next(-45, 46);
					projectile.netUpdate = true;
				}
			}
		}
	}
}