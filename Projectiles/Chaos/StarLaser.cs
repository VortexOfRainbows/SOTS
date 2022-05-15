using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class StarLaser : ModProjectile
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.friendly);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.friendly = reader.ReadBoolean();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Laser");
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1500;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.extraUpdates = 4;
			Projectile.localNPCHitCooldown = 15;
			Projectile.usesLocalNPCImmunity = true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return true;
		}
		bool runOnce = true;
		Vector2[] trailPos = new Vector2[10];
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (runOnce)
			{
				return false;
			}
			float scale = 0.75f;
			float width = Projectile.width * scale;
			float height = Projectile.height * scale;
			for (int i = 0; i < trailPos.Length * 0.5f; i += 2)
			{
				Vector2 pos = trailPos[i];
				projHitbox = new Rectangle((int)pos.X - (int)width / 2, (int)pos.Y - (int)height / 2, (int)width, (int)height);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			float scale2 = 0.75f;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float sqrt = (float)Math.Sqrt((trailPos.Length - k) / (float)trailPos.Length);
				float scale = Projectile.scale * (0.6f * sqrt + 0.4f) * scale2;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				if (trailPos[k] != Projectile.Center)
				{
					Vector2 drawPos = trailPos[k] - Main.screenPosition;
					Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3 + k * 2), false);
					color = Projectile.GetAlpha(color);
					color.A = 0; 
					float max = betweenPositions.Length() / (texture.Width * 0.5f);
					for (int i = 0; i < max; i++)
					{
						drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
						Main.spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, new Vector2(1, scale), SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
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
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			TriggerStop();
			return false;
		}
		public void TriggerStop()
		{
			Projectile.tileCollide = false;
			Projectile.velocity *= 0f;
			Projectile.friendly = false;
			Projectile.netUpdate = true;
		}
		public Color color
        {
			get => VoidPlayer.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3), VoidPlayer.ChaosPink);
		}
		public override bool PreAI()
		{
			Projectile.ai[0]++;
			Projectile.alpha = (int)(255 - Projectile.ai[0] * 30);
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;
			int trailLength = 33;
			if (runOnce)
			{
				for (int i = 0; i < 8; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
					dust.color = color;
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.4f;
					dust.alpha = 100;
					dust.velocity *= 1.2f;
					dust.velocity += Projectile.velocity * Main.rand.NextFloat(0, 1f) * 1.0f;
				}
				Projectile.rotation = Projectile.velocity.ToRotation();
				trailPos = new Vector2[trailLength];
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			if (Projectile.timeLeft > trailLength && !Projectile.friendly)
            {
				Projectile.timeLeft = trailLength;
            }
			if((Projectile.timeLeft < trailLength && Main.rand.NextBool(4)) || Main.rand.NextBool(12))
            {
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				dust.color = color;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.4f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 1.3f;
			}
			else if (Projectile.timeLeft <= trailLength && Projectile.friendly)
            {
				TriggerStop();
            }		
			return true;
		}
		public override void AI()
		{
			cataloguePos();
		}
	}
}
		