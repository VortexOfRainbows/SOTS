using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using System;

namespace SOTS.Projectiles.Otherworld
{
	public class PhaseColumn : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Thunder");
		}
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.penetrate = -1;
			projectile.extraUpdates = 3;
			projectile.scale = 0.75f;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.velocity.X);
			writer.Write(projectile.velocity.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.velocity.X = reader.ReadSingle();
			projectile.velocity.Y = reader.ReadSingle();
		}
		Vector2[] trailPos = new Vector2[5];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/PhaseColumn");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
                {
					return false;
                }
				Color color = new Color(120, 130, 160, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (14 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 4; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
                        {
							x = 0;
							y = 0;
                        }
						if(trailPos[k] != projectile.Center)
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
			if(endHow == 1 && endHow != 2 && Main.rand.NextBool(18))
			{
				int dust1 = Dust.NewDust(projectile.position - new Vector2(5), projectile.width, projectile.height, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[dust1];
				dust.scale *= 1f * (10f - iterator)/10f;
				dust.velocity += projectile.velocity * 0.3f;
				dust.noGravity = true;
				dust.color = new Color(100, 80, 200);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = projectile.alpha;
			}
			if (iterator >= trailPos.Length)
				projectile.Kill();
		}
		int endHow = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			endHow = 1;
			projectile.tileCollide = false;
			projectile.velocity *= 0f;
            return false;
        }
		int counter = 0;
		int counter2 = 0;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
        public override void AI()
		{
			if(projectile.timeLeft < 220)
			{
				endHow = 2;
				projectile.tileCollide = false;
				projectile.velocity *= 0f;
			}
			if(runOnce)
			{
				originalVelo = projectile.velocity;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				originalPos = projectile.Center;
			}
			originalPos += originalVelo * 1.4f;
			checkPos();
			Player player = Main.player[projectile.owner];
			Vector2 toPlayer = player.Center - projectile.Center;
			if(counter2 > 30 - projectile.ai[0] * 1)
			{
				if (projectile.ai[0] > 0 && endHow == 0)
				{
					for (int i = 0; i < 3; i += 2)
					{
						if (projectile.owner == Main.myPlayer)
						{
							Vector2 perturbedSpeed = new Vector2(originalVelo.X, originalVelo.Y).RotatedBy(MathHelper.ToRadians((i - 1) * 45f));
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("PhaseColumn"), projectile.damage, 1f, Main.myPlayer, projectile.ai[0] - 1);
						}
					}
				}
				projectile.velocity *= 0f;
				originalVelo *= 0f;
				projectile.ai[0] = 0f;
				projectile.friendly = false;
			}
			counter++;
			counter2++;
			if(counter >= 0)
			{
				cataloguePos();
				counter = -10;
				if (Main.myPlayer == projectile.owner)
				{
					if (projectile.velocity.Length() != 0f)
					{
						Vector2 toPos = originalPos - projectile.Center;
						projectile.velocity = new Vector2(originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(projectile.ai[1]));
						projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
					}
					projectile.ai[1] = Main.rand.Next(-45, 46);
					if (projectile.owner == Main.myPlayer)
						projectile.netUpdate = true;
				}
			}
		}
	}
}