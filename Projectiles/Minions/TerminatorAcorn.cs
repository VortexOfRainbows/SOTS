using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld;
using Steamworks;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{
	public class TerminatorAcorn : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terminator Acorn");
		}
		public override void SetDefaults()
		{
			projectile.arrow = false;
			projectile.width = 16;
			projectile.height = 16;
			projectile.penetrate = -1;
			projectile.timeLeft = 3000;
			projectile.ranged = false;
			projectile.friendly = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 360);
			target.immune[projectile.owner] = 0;
			triggerStop();
		}
		Vector2[] trailPos = new Vector2[7];
		public void TrailPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Minions/TerminatorAcornTrail");
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
							spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
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
			Texture2D texture = mod.GetTexture("Projectiles/Minions/TerminatorAcornTrail");
			Color color = new Color(120, 120, 120, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if(endHow == 0)
				for (int k = 0; k < 5; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
			TrailPreDraw(spriteBatch, lightColor);
			return endHow == 0;
		}
		bool runOnce = true;
		public override bool PreAI()
		{
			if(projectile.ai[0] == -1)
            {
				projectile.ai[0]--;
				for (int i = 0; i < 12; i++)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, mod.DustType("CopyDust4"));
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.2f;
					dust.velocity += projectile.velocity * 0.225f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = new Color(220, 60, 7, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.6f;
					dust.alpha = projectile.alpha;
				}
			}
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
			if (runOnce)
			{
				projectile.scale -= 0.125f * projectile.ai[0];
				for (int i = 0; i < 3; i++)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, mod.DustType("CopyDust4"));
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.2f;
					dust.velocity += projectile.velocity * 0.1f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = new Color(220, 60, 7, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.4f;
					dust.alpha = projectile.alpha;
				}
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 20, 0.8f);
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			if (!runOnce)
			{
				if (projectile.ai[1] % 2 == 0)
					cataloguePos();
			}
			checkPos();
			if (projectile.timeLeft < 1000 && endHow == 0)
			{
				triggerStop();
			}
			projectile.ai[1]++;
			return projectile.friendly;
		}
        public override void AI()
		{
			projectile.velocity.Y += 0.09f;
			base.AI();
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
			width = 8;
			height = 8;
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
			endHow = 1;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.velocity *= 0f;
			projectile.netUpdate = true;
			projectile.ai[0] = -1;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.tileCollide);
			writer.Write(projectile.friendly);
			writer.Write(endHow);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.tileCollide = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
	}
}
		