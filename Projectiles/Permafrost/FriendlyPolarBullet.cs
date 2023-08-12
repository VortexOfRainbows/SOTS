using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class FriendlyPolarBullet : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Polar Bullet");
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = 1;
			Projectile.width = 12;
			Projectile.height = 20;
			Projectile.timeLeft = 1060;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
		}
		Vector2[] trailPos = new Vector2[8];
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			//target.immune[Projectile.owner] = 0;
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
        public void TrailPreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/PolarisTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			float drawAmt = 1f;
			if (SOTS.Config.lowFidelityMode)
				drawAmt = 0.5f;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * 0.9f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = new Color(100, 100, 100, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (5f * scale) * drawAmt;
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
						if (trailPos[k] != Projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
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
		public override bool PreDraw(ref Color lightColor)
		{
			TrailPreDraw(ref lightColor);
			return endHow == 0;
		}
		bool runOnce = true;
		float acceleration = 0.3f;
		public override bool PreAI()
		{
			int dustAmtMult = 3;
			if (SOTS.Config.lowFidelityMode)
				dustAmtMult = 1;
			if (Projectile.ai[0] == -1)
			{
				Projectile.ai[0]--;
				for (int i = 0; i < 3.3 * dustAmtMult; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.2f;
					dust.velocity += Projectile.oldVelocity * 0.5f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = new Color(200, 250, 250, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.6f;
					dust.alpha = 100;
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
			if (runOnce)
			{
				acceleration = 0.4f;
				Projectile.position += Projectile.velocity * 2;
				for (int i = 0; i < 1.5 * dustAmtMult; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.15f;
					dust.velocity += Projectile.velocity * 0.4f;
					dust.noGravity = true;
					dust.scale *= 0.2f;
					dust.color = new Color(200, 250, 250, 100);
					dust.fadeIn = 0.1f;
					dust.scale += 1.25f;
					dust.alpha = 100;
				}
				SOTSUtils.PlaySound(SoundID.Item11, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.8f, 0.1f);
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
			if (Projectile.timeLeft < 1000 && endHow == 0)
			{
				triggerStop();
			}
			if ((counter > 10 || Projectile.ai[0] == -3) && endHow == 0)
			{
				Vector2 temp = Projectile.velocity * acceleration;
				temp = Collision.TileCollision(Projectile.Center - new Vector2(10, 10), Projectile.velocity * acceleration, 20, 20, true, true);
				if(temp != Projectile.velocity * acceleration)
				{
					triggerStop();
				}
			}
			Projectile.position += Projectile.velocity * acceleration;
			if (Projectile.ai[0] != -3)
			{
				counter++;
				acceleration += 0.15f;
			}
			else
			{
				acceleration += 0.12f;
				if(Projectile.timeLeft > 1000)
					Projectile.timeLeft -= 2;
            }
			return false;
		}
		int counter = 0;
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
			if (iterator >= trailPos.Length)
				Projectile.Kill();
		}
		int endHow = 0;
		public void triggerStop()
		{
			Projectile.penetrate = -1;
			endHow = 1;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.velocity *= 0f;
			Projectile.netUpdate = true;
			Projectile.ai[0] = -1;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(endHow);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
	}
}
		