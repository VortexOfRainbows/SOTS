using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using System;

namespace SOTS.Projectiles.Otherworld
{
	public class ChargedCataclysmBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charged Cataclysm Bullet");
		}
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 120;
			projectile.scale = 1f;
		}
        public override bool? CanHitNPC(NPC target)
        {
			return false;
        }
        Vector2[] trailPos = new Vector2[200];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/CataclysmTrail");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			Color color = new Color(255, 100, 100, 0) * ((255 - projectile.alpha) / 255f);
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				float scale = projectile.scale;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / (texture.Width * scale * 0.5f);
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
						if (trailPos[k] != projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        bool runOnce = true;
		Vector2 addPos = Vector2.Zero;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
		Vector2 nextPos = Vector2.Zero;
		int[] randStorage = new int[200];
		int dist = 200;
        public override void AI()
		{
			if (runOnce)
			{
				projectile.position += projectile.velocity.SafeNormalize(Vector2.Zero) * 24;
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 0.6f);
				for (int i = 0; i < randStorage.Length; i++)
				{
					randStorage[i] = Main.rand.Next(-55, 56);
				}
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				originalVelo = projectile.velocity.SafeNormalize(Vector2.Zero) * 8f;
				originalPos = projectile.Center;
				runOnce = false;
			}

			Vector2 temp = originalPos;
			addPos = projectile.Center;
			for (int i = 0; i < dist; i ++)
			{
				bool collided = false;
				originalPos += originalVelo;

				for(int reps = 0; reps < 3; reps++)
				{
					Vector2 attemptToPosition = (originalPos + originalVelo * 3f) - addPos;
					addPos += new Vector2(originalVelo.Length(), 0).RotatedBy(attemptToPosition.ToRotation() + MathHelper.ToRadians(randStorage[i]));
					trailPos[i] = addPos;
					int u = (int)addPos.X / 16;
					int j = (int)addPos.Y / 16;
					if (!WorldGen.InWorld(u, j, 20) || Main.tile[u, j].active() && Main.tileSolidTop[Main.tile[u, j].type] == false && Main.tileSolid[Main.tile[u, j].type] == true)
					{
						int dust = Dust.NewDust(new Vector2(addPos.X - 16, addPos.Y - 16), 24, 24, 235);
						Main.dust[dust].scale *= 1f;
						Main.dust[dust].velocity *= 1f;
						Main.dust[dust].noGravity = true;
						for (int k = i + 1; k < trailPos.Length; k++)
						{
							trailPos[k] = Vector2.Zero;
						}
						collided = true;
						break;
					}
				}
				for(int n = 0; n < Main.npc.Length; n++)
                {
					NPC npc = Main.npc[n];
					if (npc.active && npc.Hitbox.Intersects(new Rectangle((int)addPos.X - 12, (int)addPos.Y - 12, 24, 24)) && !npc.friendly && !npc.dontTakeDamage)
					{
						if (projectile.owner == Main.myPlayer && projectile.friendly)
							Projectile.NewProjectile(addPos.X, addPos.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("CataclysmBulletDamage"), projectile.damage, projectile.knockBack, Main.myPlayer, -1f, 5f);
						if (projectile.friendly)
							collided = true;
						projectile.friendly = false;
						for (int k = i + 1; k < trailPos.Length; k++)
						{
							trailPos[k] = Vector2.Zero;
						}
						break;
					}
				}
				if (collided)
				{
					dist = i + 1;
					break;
				}
			}
			originalPos = temp;
			projectile.alpha += 4;
			if (projectile.alpha >= 255)
				projectile.Kill();

			projectile.scale *= 0.98f;
			projectile.friendly = false;
		}
	}
}