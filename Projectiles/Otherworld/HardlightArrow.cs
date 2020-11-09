using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld;
using Steamworks;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class HardlightArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Arrow");
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(1);
			projectile.aiStyle = 1;
			projectile.width = 14;
			projectile.height = 32;
			projectile.penetrate = -1;
			projectile.timeLeft = 3000;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
			triggerStop();
		}
		Vector2[] trailPos = new Vector2[12];
		public void TrailPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/HardlightArrowSmallShaft");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = new Color(150, 150, 150, 30);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (6f * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != projectile.Center)
							spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
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
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2 = mod.GetTexture("Projectiles/Otherworld/HardlightArrowShaft");
			Color color = new Color(120, 120, 120, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			if(endHow == 0)
				for (int k = 0; k < 5; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture2, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
			TrailPreDraw(spriteBatch, lightColor);
			return endHow == 0;
		}
		bool runOnce = true;
		Vector2 storeVelocity = Vector2.Zero;
		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				storeVelocity = projectile.velocity * 2f;
			}
			if (!runOnce)
			{
				projectile.velocity = storeVelocity;
				if (projectile.ai[0] % 3 == 0)
					cataloguePos();
			}
			checkPos();
			if (projectile.timeLeft < 1000 && endHow == 0)
			{
				triggerStop();
			}
			projectile.ai[0]++;
			return false;
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
			if (iterator >= trailPos.Length)
				projectile.Kill();
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 12;
			height = 12;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        int endHow = 0;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerStop();
			return false;
		}
		public void triggerStop()
		{
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric);
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity += projectile.velocity * 0.1f;
				Main.dust[dust].noGravity = true;
			}
			endHow = 1;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.velocity *= 0f;
			storeVelocity = Vector2.Zero;
		}
    }
}
		