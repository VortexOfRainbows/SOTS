using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using System;

namespace SOTS.Projectiles.Otherworld
{
	public class CataclysmBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysm Bullet");
		}
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.extraUpdates = 3;
			projectile.scale = 0.75f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			triggerStop();
			Player player = Main.player[projectile.owner];
			target.immune[projectile.owner] = 0;
			if (projectile.owner == Main.myPlayer)
			{
				int npcIndex = -1;
				double distanceTB = 216;
				for (int i = 0; i < 200; i++) //find first enemy
				{
					NPC npc = Main.npc[i];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						if (npcIndex != i && target.whoAmI != i)
						{
							float disX = projectile.Center.X - npc.Center.X;
							float disY = projectile.Center.Y - npc.Center.Y;
							double dis = Math.Sqrt(disX * disX + disY * disY);
							if (dis < distanceTB)
							{
								distanceTB = dis;
								npcIndex = i;
							}
						}
					}
				}
				if (npcIndex != -1)
				{
					NPC npc = Main.npc[npcIndex];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("CataclysmLightningZap"), (int)(projectile.damage * 0.2f), 0, projectile.owner, npc.whoAmI);
					}
				}
			}
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
				Color color = new Color(220, 100, 100, 0);
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
			texture = mod.GetTexture("Projectiles/Otherworld/CataclysmBullet");
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int j = 0; j < 4; j++)
			{
				Color color = new Color(255, 100, 100, 0);
				Vector2 drawPos = projectile.Center - Main.screenPosition;
				float x = Main.rand.Next(-10, 11) * 0.2f;
				float y = Main.rand.Next(-10, 11) * 0.2f;
				if (j < 2)
				{
					x = 0;
					y = 0;
				}
				if (projectile.velocity != projectile.velocity * 0f)
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.velocity.ToRotation() + MathHelper.ToRadians(90), drawOrigin, 1f, SpriteEffects.None, 0f);
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
			if(endHow == 1 && endHow != 2 && Main.rand.NextBool(3))
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

			}
			counter++;
			counter2++;
			if(counter >= 0)
			{
				counter = -3;
				if (projectile.velocity.Length() != 0f)
				{
					Vector2 toPos = originalPos - projectile.Center;
					projectile.velocity = new Vector2(originalVelo.Length(), 0).RotatedBy(toPos.ToRotation() + MathHelper.ToRadians(projectile.ai[1]));
					projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
				}
				projectile.ai[1] = Main.rand.Next(-20, 21);
				cataloguePos();
			}
		}
		public void triggerStop()
		{
			endHow = 1;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.velocity *= 0f;
		}
	}
}