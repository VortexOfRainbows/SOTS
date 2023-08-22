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
			// DisplayName.SetDefault("Purgatory Column");
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
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 3;
		}
		Vector2[] trailPos = new Vector2[8];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
                {
					return false;
                }
				Color color = new Color(120, 250, 130, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (texture.Height * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
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
			if(endHow == 1 && endHow != 2 && Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 1.25f;
				dust.scale *= 1.5f * (trailPos.Length - iterator) / (float)trailPos.Length;
				dust.fadeIn = 0.1f;
				dust.color = new Color(160, 255, 140, 0);
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
			for (int k = 0; k < 5; k++)
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
			if (Projectile.timeLeft < 220)
			{
				endHow = 2;
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
			}
			if (runOnce)
			{
				originalVelo = Projectile.velocity;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				originalPos = Projectile.Center;
			}
			originalPos += originalVelo * 1.4f;
			checkPos();
			Player player = Main.player[Projectile.owner];
			if (Projectile.ai[0] > 0 && counter2 > 80 - Projectile.ai[0] * 10)
			{
				for (int i = 0; i < 3; i += 2)
				{
					if (Main.myPlayer == Projectile.owner)
					{
						Vector2 perturbedSpeed = new Vector2(originalVelo.X, originalVelo.Y).RotatedBy(MathHelper.ToRadians((i - 1) * 5.5f));
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, Projectile.type, Projectile.damage, 1f, Main.myPlayer, Projectile.ai[0] - 1);
					}
				}
				Projectile.velocity *= 0f;
				originalVelo *= 0f;
				Projectile.ai[0] = 0f;
			}
			counter++;
			counter2++;
			if (counter >= 0)
			{
				cataloguePos();
				counter = -14;
				if (Projectile.velocity.Length() != 0f)
				{
					Vector2 toPos = originalPos - Projectile.Center;
					Projectile.velocity = new Vector2(originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				}
			}
			else if (counter >= -2)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient || Main.myPlayer == Projectile.owner)
				{
					Projectile.ai[1] = Main.rand.Next(-45, 46);
					Projectile.netUpdate = true;
				}
			}
		}
	}
}