using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;

namespace SOTS.Projectiles.Otherworld
{
	public class ThunderColumnFast : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Column");
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
			projectile.width = 14;
			projectile.height = 14;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 3600;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.extraUpdates = 4;
			projectile.scale = 0.8f;
		}
		Vector2[] trailPos = new Vector2[24];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/ThunderColumnFast");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
                {
					return false;
                }
				Color color = new Color(150, 150, 150, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (14 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 5; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
                        {
							x = 0;
							y = 0;
                        }
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
			bool flag = false;
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
			if(endHow == 1 && endHow != 2)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 242);
				Main.dust[dust].scale *= 5f * (trailPos.Length - iterator) / (float)trailPos.Length;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].noGravity = true;
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
			for (int k = 0; k < 16; k++)
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
			Player player = Main.player[(int)projectile.knockBack];
			if (projectile.Center.Y > player.Center.Y)
            {
				projectile.tileCollide = true;
            }
			if(projectile.timeLeft < 220)
			{
				endHow = 2;
				projectile.tileCollide = false;
				projectile.velocity *= 0f;
			}
			if(runOnce)
			{
				originalVelo = projectile.velocity;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				originalPos = new Vector2(projectile.ai[0], projectile.ai[1]);
				projectile.ai[0] = 0;
				projectile.ai[1] = 0;
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 92);
			}
			checkPos();
			Vector2 toPlayer = player.Center - projectile.Center;
			if(counter2 > 600)
			{
				projectile.extraUpdates = 9;
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 94);
				counter2 = -100000;
				projectile.scale *= 4.5f;
				projectile.position = originalPos - new Vector2(projectile.width / 2, projectile.height / 2);
				projectile.velocity = new Vector2(0, originalVelo.Length());
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				cataloguePos();
			}
			counter++;
			counter2++;
			if(counter >= 0)
            {
				counter = -10;
				if (counter2 < 0)
				{
					projectile.ai[1] = 0;
				}
				if (projectile.velocity.Length() != 0f)
				{
					Vector2 toPos = originalPos - projectile.Center;
					if(counter2 < 0)
                    {
						toPos = new Vector2(0, 10);
                    }
					projectile.velocity = new Vector2(originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(projectile.ai[1]));
					projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				}
				projectile.ai[1] = Main.rand.Next(-35, 36);
				cataloguePos();
            }
		}
	}
}