using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			Projectile.arrow = false;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 3000;
			// Projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.friendly = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 360);
			target.immune[Projectile.owner] = 0;
			triggerStop();
		}
		Vector2[] trailPos = new Vector2[7];
		public void TrailPreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Minions/TerminatorAcornTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = new Color(150, 150, 150, 30);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
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
						if (trailPos[k] != Projectile.Center)
							spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Minions/TerminatorAcornTrail").Value;
			Color color = new Color(120, 120, 120, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if(endHow == 0)
				for (int k = 0; k < 5; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
			TrailPreDraw(spriteBatch, lightColor);
			return endHow == 0;
		}
		bool runOnce = true;
		public override bool PreAI()
		{
			if(Projectile.ai[0] == -1)
            {
				Projectile.ai[0]--;
				for (int i = 0; i < 12; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, Mod.Find<ModDust>("CopyDust4").Type);
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.2f;
					dust.velocity += Projectile.velocity * 0.225f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = new Color(220, 60, 7, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.6f;
					dust.alpha = Projectile.alpha;
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
			if (runOnce)
			{
				Projectile.scale -= 0.125f * Projectile.ai[0];
				for (int i = 0; i < 3; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, Mod.Find<ModDust>("CopyDust4").Type);
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.2f;
					dust.velocity += Projectile.velocity * 0.1f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = new Color(220, 60, 7, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.4f;
					dust.alpha = Projectile.alpha;
				}
				SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 20, 0.8f);
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			if (!runOnce)
			{
				if (Projectile.ai[1] % 2 == 0)
					cataloguePos();
			}
			checkPos();
			if (Projectile.timeLeft < 1000 && endHow == 0)
			{
				triggerStop();
			}
			Projectile.ai[1]++;
			return Projectile.friendly;
		}
        public override void AI()
		{
			Projectile.velocity.Y += 0.09f;
			base.AI();
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
			if (iterator >= trailPos.Length)
				Projectile.Kill();
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 8;
			height = 8;
            return true;
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
		