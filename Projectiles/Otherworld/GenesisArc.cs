using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class GenesisArc: ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Genesis Arc");
		}
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 120;
			Projectile.scale = 1f;
		}
		Vector2[] trailPos = new Vector2[10];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/CataclysmTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 0.9f;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Color color = new Color(255, 100, 100, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (texture.Width * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 4; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f * scale;
						float y = Main.rand.Next(-10, 11) * 0.1f * scale;
						if (j < 2)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != Projectile.Center)
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
			if (endHow == 1 && endHow != 2 && Main.rand.NextBool(3))
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 235);
				Main.dust[dust].scale *= 1f * (trailPos.Length - iterator) / (float)trailPos.Length;
				Main.dust[dust].velocity *= 1f;
				Main.dust[dust].noGravity = true;
			}
			if (iterator >= trailPos.Length)
				Projectile.Kill();
		}
		int endHow = 0;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerStop();
			return false;
		}
		int counter = 0;
		int counter2 = 0;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 rotPos = Vector2.Zero;
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
			}
			checkPos();
			Player player = Main.player[Projectile.owner];
			Vector2 toPlayer = player.Center - Projectile.Center;

			bool found = false;
			int ofTotal = 0;
			int total = 0;
			int projID = -1;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner && proj.ai[0] == Projectile.ai[0])
				{
					if (proj == Projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}

			int owner = (int)Projectile.ai[0];
			if(owner >= 0)
            {
				Projectile proj = Main.projectile[owner];
				if(proj.type == ModContent.ProjectileType<GenesisCore>() && proj.owner == Projectile.owner && proj.active && total >= 1)
				{
					Vector2 rotationDist = new Vector2(((GenesisCore)proj.ModProjectile).DistanceMult * (16 + total * 5), 0).RotatedBy(MathHelper.ToRadians(proj.ai[0] * (3 - total * 0.2f) + (ofTotal % 2) * 90));
					rotPos = proj.Center + new Vector2(rotationDist.X, 0).RotatedBy(MathHelper.ToRadians(ofTotal * (360 / total) + proj.ai[0]));
					Projectile.scale = 0.9f + proj.ai[1];
				}
				else
				{
					endHow = 2;
					Projectile.tileCollide = false;
					Projectile.velocity *= 0f;
				}
				counter++;
				counter2++;
				if (counter >= 0)
				{
					counter = -3;
					if (Projectile.velocity.Length() != 0f)
					{
						Vector2 toPos = rotPos - Projectile.Center;
						Projectile.velocity = new Vector2((proj.ai[0] * 0.002f + 0.99f) * originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(nextRot));
						Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
					}
					nextRot = Main.rand.Next(-20, 21);
					cataloguePos();
				}
			}
			else
			{
				endHow = 2;
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
			}

		}
		float nextRot = 0f;
		public void triggerStop()
		{
			endHow = 1;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.velocity *= 0f;
			Projectile.netUpdate = true;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(endHow);
			writer.Write(nextRot);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
			nextRot = reader.ReadSingle();
			base.ReceiveExtraAI(reader);
		}
	}
}