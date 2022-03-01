using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{    
    public class PerfectStarLaser : ModProjectile 
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
			projectile.timeLeft = 1200;
			projectile.width = 24;
			projectile.height = 24;
			projectile.extraUpdates = 2;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 12;
			height = 12;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			TriggerStop();
        }
        bool runOnce = true;
		Vector2[] trailPos = new Vector2[10];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float sqrt = (float)Math.Sqrt((trailPos.Length - k) / (float)trailPos.Length);
				float scale = projectile.scale * sqrt * 1.5f;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				if (trailPos[k] != projectile.Center)
				{
					Color color = new Color(100, 100, 100, 0);
					color = projectile.GetAlpha(color);
					float max = betweenPositions.Length() / (texture.Width * 0.2f);
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
		public override bool PreAI()
		{
			int trailLength = (int)projectile.ai[0];
			if (runOnce)
			{
				for (int i = 0; i < 14; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
					Color color2 = Color.Lerp(new Color(175, 218, 118, 0), new Color(74, 186, 54, 0), Main.rand.NextFloat(1));
					dust.color = color2;
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.3f;
					dust.alpha = projectile.alpha;
					dust.velocity *= 0.7f;
					dust.velocity += projectile.velocity * Main.rand.NextFloat(0, 0.6f);
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
			if(projectile.timeLeft < trailLength || Main.rand.NextBool(8))
            {
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Color color2 = Color.Lerp(new Color(175, 218, 118, 0), new Color(74, 186, 54, 0), Main.rand.NextFloat(1));
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.4f;
				dust.alpha = projectile.alpha;
				dust.velocity *= 0.8f;
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
		