using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;

namespace SOTS.Projectiles.Planetarium
{
	public class ThunderColumnFast : ModProjectile
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
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 3;
			Projectile.scale = 0.8f;
		}
		Vector2[] trailPos = new Vector2[22];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Planetarium/ThunderColumnFast").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
                {
					return false;
                }
				Color color = new Color(150, 150, 150, 0);
				if (SOTS.Config.lowFidelityMode)
					color = new Color(200, 200, 200, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (14 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < (SOTS.Config.lowFidelityMode ? 2 : 5); j++)
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
				}
			}
			if(endHow == 1 && endHow != 2)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 242);
				Main.dust[dust].scale *= 5f * (trailPos.Length - iterator) / (float)trailPos.Length;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].noGravity = true;
			}
			if (iterator >= trailPos.Length)
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
			for (int k = 0; k < 16; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
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
			Player player = Main.player[(int)Projectile.knockBack];
			if (Projectile.Center.Y > player.Center.Y)
            {
				Projectile.tileCollide = true;
            }
			if(Projectile.timeLeft < 220)
			{
				endHow = 2;
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
			}
			if(runOnce)
			{
				originalVelo = Projectile.velocity * 1.25f;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				originalPos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
				Projectile.ai[0] = 0;
				Projectile.ai[1] = 0;
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
			}
			checkPos();
			Vector2 toPlayer = player.Center - Projectile.Center;
			if (counter2 > 240)
				Projectile.hostile = true;
			if(counter2 > 600)
			{
				Projectile.extraUpdates = 9;
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);
				counter2 = -100000;
				Projectile.scale *= 4.5f;
				Projectile.position = originalPos - new Vector2(Projectile.width / 2, Projectile.height / 2);
				Projectile.velocity = new Vector2(0, originalVelo.Length());
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				cataloguePos();
			}
			counter++;
			counter2++;
			if(counter >= 0)
            {
				counter = -10;
				if (counter2 < 0)
				{
					Projectile.ai[1] = 0;
				}
				if (Projectile.velocity.Length() != 0f)
				{
					Vector2 toPos = originalPos - Projectile.Center;
					if(counter2 < 0)
                    {
						toPos = new Vector2(0, 10);
                    }
					Projectile.velocity = new Vector2(originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				}
				Projectile.ai[1] = Main.rand.Next(-35, 36);
				cataloguePos();
            }
		}
	}
}