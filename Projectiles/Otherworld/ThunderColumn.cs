using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;

namespace SOTS.Projectiles.Otherworld
{
	public class ThunderColumn : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Thunder Column");
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.velocity.X);
			writer.Write(Projectile.velocity.Y);
			writer.Write(Projectile.scale);
			writer.Write(Projectile.rotation);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.velocity.X = reader.ReadSingle();
			Projectile.velocity.Y = reader.ReadSingle();
			Projectile.scale = reader.ReadSingle();
			Projectile.rotation = reader.ReadSingle();
		}
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;
		}
		Vector2[] trailPos = new Vector2[10];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/ThunderColumn").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
                {
					return false;
                }
				Color color = new Color(130, 130, 130, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (14 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 6; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
                        {
							x = 0;
							y = 0;
                        }
						if(trailPos[k] != Projectile.Center)
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
			Vector2 current = Projectile.Center;
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
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
					flag = true;
				}
			}
			if((flag || endHow == 1) && endHow != 2)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 242);
				Main.dust[dust].scale *= 3f * (10f - iterator)/10f;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].noGravity = true;
			}
			if (iterator >= 10)
				Projectile.Kill();
		}
		int endHow = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			endHow = 1;
			Projectile.tileCollide = false;
			Projectile.velocity *= 0f;
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < 6; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Vector2 currentPos = trailPos[k];
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition, currentPos, 10f * scale, ref point))
                {
					return true;
                }
				previousPosition = currentPos;
			}
			return false;
        }
        public override void AI()
		{
			if(Projectile.timeLeft < 220)
			{
				endHow = 2;
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
			}
			if(runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			checkPos();
			Player player = Main.player[Projectile.owner];
			Projectile.ai[0]++;
			if(Projectile.ai[0] >= 0)
			{
				cataloguePos();
				Projectile.ai[0] = -20;
				if (Projectile.owner == Main.myPlayer)
				{
					if (Projectile.velocity.Length() != 0f)
					{
						Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
						Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
					}
					Projectile.ai[1] = Main.rand.Next(-40, 41);
					Projectile.netUpdate = true;
				}
            }
		}
	}
}