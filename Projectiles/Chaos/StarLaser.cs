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
			writer.Write(projectile.friendly);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.friendly = reader.ReadBoolean();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Laser");
		}
		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 1500;
			projectile.width = 24;
			projectile.height = 24;
			projectile.extraUpdates = 4;
			projectile.localNPCHitCooldown = 15;
			projectile.usesLocalNPCImmunity = true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
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
			float width = projectile.width * scale;
			float height = projectile.height * scale;
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
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 8f, ref point);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			float scale2 = 0.75f;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float sqrt = (float)Math.Sqrt((trailPos.Length - k) / (float)trailPos.Length);
				float scale = projectile.scale * (0.6f * sqrt + 0.4f) * scale2;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				if (trailPos[k] != projectile.Center)
				{
					Vector2 drawPos = trailPos[k] - Main.screenPosition;
					Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3 + k * 2), false);
					color = projectile.GetAlpha(color);
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
			Vector2 current = projectile.Center;
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
			projectile.tileCollide = false;
			projectile.velocity *= 0f;
			projectile.friendly = false;
			projectile.netUpdate = true;
		}
		public Color color
        {
			get => VoidPlayer.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3), VoidPlayer.ChaosPink);
		}
		public override bool PreAI()
		{
			projectile.ai[0]++;
			projectile.alpha = (int)(255 - projectile.ai[0] * 30);
			if (projectile.alpha < 0)
				projectile.alpha = 0;
			int trailLength = 33;
			if (runOnce)
			{
				for (int i = 0; i < 8; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
					dust.color = color;
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.4f;
					dust.alpha = 100;
					dust.velocity *= 1.2f;
					dust.velocity += projectile.velocity * Main.rand.NextFloat(0, 1f) * 1.0f;
				}
				projectile.rotation = projectile.velocity.ToRotation();
				trailPos = new Vector2[trailLength];
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			if (projectile.timeLeft > trailLength && !projectile.friendly)
            {
				projectile.timeLeft = trailLength;
            }
			if((projectile.timeLeft < trailLength && Main.rand.NextBool(4)) || Main.rand.NextBool(12))
            {
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				dust.color = color;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.4f;
				dust.alpha = projectile.alpha;
				dust.velocity *= 1.3f;
			}
			else if (projectile.timeLeft <= trailLength && projectile.friendly)
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
		