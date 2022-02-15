using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class FriendlyPolarBullet : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polar Bullet");
		}
        public override void SetDefaults()
        {
			projectile.penetrate = 1;
			projectile.width = 12;
			projectile.height = 20;
			projectile.timeLeft = 1060;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.melee = true;
			projectile.ignoreWater = false;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		Vector2[] trailPos = new Vector2[8];
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			//target.immune[projectile.owner] = 0;
			triggerStop();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public void TrailPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Permafrost/PolarisTrail");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * 0.9f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = new Color(100, 100, 100, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (5f * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f * scale;
						float y = Main.rand.Next(-10, 11) * 0.1f * scale;
						if (j == 0)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != projectile.Center)
							spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
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
			TrailPreDraw(spriteBatch, lightColor);
			return endHow == 0;
		}
		bool runOnce = true;
		float acceleration = 0.3f;
		public override bool PreAI()
		{
			if (projectile.ai[0] == -1)
			{
				projectile.ai[0]--;
				for (int i = 0; i < 10; i++)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, mod.DustType("CopyDust4"));
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.2f;
					dust.velocity += projectile.oldVelocity * 0.5f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = new Color(200, 250, 250, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.6f;
					dust.alpha = 100;
				}
			}
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
			if (runOnce)
			{
				acceleration = 0.4f;
				projectile.position += projectile.velocity * 2;
				for (int i = 0; i < 5; i++)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, mod.DustType("CopyDust4"));
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.15f;
					dust.velocity += projectile.velocity * 0.4f;
					dust.noGravity = true;
					dust.scale *= 0.2f;
					dust.color = new Color(200, 250, 250, 100);
					dust.fadeIn = 0.1f;
					dust.scale += 1.25f;
					dust.alpha = 100;
				}
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 11, 0.8f, 0.1f);
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			if (!runOnce)
			{
				cataloguePos();
			}
			checkPos();
			if (projectile.timeLeft < 1000 && endHow == 0)
			{
				triggerStop();
			}
			if ((counter > 10 || projectile.ai[0] == -3) && endHow == 0)
			{
				Vector2 temp = projectile.velocity * acceleration;
				temp = Collision.TileCollision(projectile.Center - new Vector2(10, 10), projectile.velocity * acceleration, 20, 20, true, true);
				if(temp != projectile.velocity * acceleration)
				{
					triggerStop();
				}
			}
			projectile.position += projectile.velocity * acceleration;
			if (projectile.ai[0] != -3)
			{
				counter++;
				acceleration += 0.15f;
			}
			else
			{
				acceleration += 0.12f;
				if(projectile.timeLeft > 1000)
					projectile.timeLeft -= 2;
            }
			return false;
		}
		int counter = 0;
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
		int endHow = 0;
		public void triggerStop()
		{
			projectile.penetrate = -1;
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
		