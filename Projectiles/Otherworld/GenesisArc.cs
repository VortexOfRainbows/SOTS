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
			projectile.width = 14;
			projectile.height = 14;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 120;
			projectile.scale = 1f;
		}
		Vector2[] trailPos = new Vector2[10];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/CataclysmTrail");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 0.9f;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Color color = new Color(255, 100, 100, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
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
						if (trailPos[k] != projectile.Center)
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
			Vector2 current = projectile.Center;
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
			Vector2 current = projectile.Center;
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
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 235);
				Main.dust[dust].scale *= 1f * (trailPos.Length - iterator) / (float)trailPos.Length;
				Main.dust[dust].velocity *= 1f;
				Main.dust[dust].noGravity = true;
			}
			if (iterator >= trailPos.Length)
				projectile.Kill();
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
			if (projectile.timeLeft < 220)
			{
				endHow = 2;
				projectile.tileCollide = false;
				projectile.velocity *= 0f;
			}
			if (runOnce)
			{
				originalVelo = projectile.velocity;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			checkPos();
			Player player = Main.player[projectile.owner];
			Vector2 toPlayer = player.Center - projectile.Center;

			bool found = false;
			int ofTotal = 0;
			int total = 0;
			int projID = -1;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner && proj.ai[0] == projectile.ai[0])
				{
					if (proj == projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}

			int owner = (int)projectile.ai[0];
			if(owner >= 0)
            {
				Projectile proj = Main.projectile[owner];
				if(proj.type == ModContent.ProjectileType<GenesisCore>() && proj.owner == projectile.owner && proj.active && total >= 1)
				{
					Vector2 rotationDist = new Vector2(((GenesisCore)proj.modProjectile).DistanceMult * (16 + total * 5), 0).RotatedBy(MathHelper.ToRadians(proj.ai[0] * (3 - total * 0.2f) + (ofTotal % 2) * 90));
					rotPos = proj.Center + new Vector2(rotationDist.X, 0).RotatedBy(MathHelper.ToRadians(ofTotal * (360 / total) + proj.ai[0]));
					projectile.scale = 0.9f + proj.ai[1];
				}
				else
				{
					endHow = 2;
					projectile.tileCollide = false;
					projectile.velocity *= 0f;
				}
				counter++;
				counter2++;
				if (counter >= 0)
				{
					counter = -3;
					if (projectile.velocity.Length() != 0f)
					{
						Vector2 toPos = rotPos - projectile.Center;
						projectile.velocity = new Vector2((proj.ai[0] * 0.002f + 0.99f) * originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(nextRot));
						projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
					}
					nextRot = Main.rand.Next(-20, 21);
					cataloguePos();
				}
			}
			else
			{
				endHow = 2;
				projectile.tileCollide = false;
				projectile.velocity *= 0f;
			}

		}
		float nextRot = 0f;
		public void triggerStop()
		{
			endHow = 1;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.velocity *= 0f;
			projectile.netUpdate = true;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.tileCollide);
			writer.Write(projectile.friendly);
			writer.Write(endHow);
			writer.Write(nextRot);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.tileCollide = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
			nextRot = reader.ReadSingle();
			base.ReceiveExtraAI(reader);
		}
	}
}