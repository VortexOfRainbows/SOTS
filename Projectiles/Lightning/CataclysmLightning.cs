using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Lightning
{    
    public class CataclysmLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cataclysm Lightning");
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 120;
			Projectile.scale = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Projectile.alpha >= 150)
			{
				return false;
			}
			float scale = Projectile.scale;
			float width = Projectile.width * scale;
			float height = Projectile.height * scale;
			for (int i = 0; i < trailPos.Count; i++)
			{
				Vector2 pos = trailPos[i];
				projHitbox = new Rectangle((int)pos.X - (int)width / 2, (int)pos.Y - (int)height / 2, (int)width, (int)height);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			Color color = new Color(140, 170, 140, 0) * ((255 - Projectile.alpha) / 255f);
			int degradePoint = 40;
			for (int k = 0; k < trailPos.Count; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				float scale = Projectile.scale * 0.7f;
				if (k > trailPos.Count - degradePoint)
				{
					int scaleDown = k - (trailPos.Count - degradePoint);
					scale *= 1 - 0.75f * (scaleDown / (float)degradePoint);
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / (texture.Width * scale * 0.5f);
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
		List<Vector2> trailPos = new List<Vector2>();
		int dist = 140;
		bool runOnce = true;
		int currentNPC = -1;
		float speed = 5.5f;
		public void runStartCalculations()
        {
			Vector2 location = Projectile.Center;
			Vector2 originalVelo = Projectile.velocity.SafeNormalize(Vector2.Zero) * speed;
			bool collided = false;
			int defaultDeviation = 65;
			for (int i = 0; i < dist; i++)
			{
				location += originalVelo;
				trailPos.Add(location + originalVelo.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-defaultDeviation, defaultDeviation + 1))) * 1.5f);
				int u = (int)location.X / 16;
				int j = (int)location.Y / 16;
				if (!WorldGen.InWorld(u, j, 20) || Main.tile[u, j].HasTile && Main.tileSolidTop[Main.tile[u, j].TileType] == false && Main.tileSolid[Main.tile[u, j].TileType] == true)
				{
					int dust = Dust.NewDust(new Vector2(location.X - 16, location.Y - 16), 24, 24, 107);
					Main.dust[dust].scale *= 1f;
					Main.dust[dust].velocity *= 1f;
					Main.dust[dust].noGravity = true;
					trailPos.RemoveAt(i);
					break;
				}
				float scale = 1;
				int npc = FindClosestEnemy(location, i);
				if (npc != -1 && !collided && (i + Projectile.identity) % 5 == 0 && i > Projectile.identity * 7 % 33)
				{
					NPC target = Main.npc[npc];
					if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy() && !collided)
					{
						originalVelo = new Vector2(speed * scale, 0).RotatedBy(Redirect(originalVelo.ToRotation(), location, target.Center));
						float width = Projectile.width * scale;
						float height = Projectile.height * scale;
						Rectangle projHitbox = new Rectangle((int)location.X - (int)width / 2, (int)location.Y - (int)height / 2, (int)width, (int)height);
						if (target.Hitbox.Intersects(projHitbox))
						{
							collided = true;
						}
					}
				}
				if(npc == -1 || collided)
                {
					dist--;
                }
				else if (npc != -1 && !collided && dist - i < 15)
                {
					dist++;
                }
			}
		}
		int redirections = 0;
		public float Redirect(float radians, Vector2 pos, Vector2 npc)
		{
			float dX = npc.X - pos.X;
			float dY = npc.Y - pos.Y;
			float npcRad = (float)Math.Atan2(dY, dX);
			//float diffRad = radians - npcRad;
			float speed = 3f + (redirections * 0.1f); //this the number adjusted that adjusts turn rate, higher = less bendy
			float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
			speed /= distance;
			Vector2 rnVelo = new Vector2(this.speed - (redirections * 0.1f), 0).RotatedBy(radians); //this the number adjusted by the turn rate, higher = more bendy
			rnVelo += new Vector2(dX * speed, dY * speed);
			npcRad = (float)Math.Atan2(rnVelo.Y, rnVelo.X); //turn velocity into rotation, this contributes to a few things
			redirections++;
			return npcRad;
		}
		public int FindClosestEnemy(Vector2 pos, int currentIndex)
		{
			Player player = Main.player[Projectile.owner];
			if (currentNPC != -1)
			{
				return currentNPC;
			}
			float minDist = 600;
			int target2 = -1;
			if (minDist > 80)
			{
				float dX;
				float dY;
				float distance;
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
					{
						dX = target.Center.X - pos.X;
						dY = target.Center.Y - pos.Y;
						distance = (float)Math.Sqrt(dX * dX + dY * dY);
						if (distance < minDist)
						{
							bool lineOfSight = Collision.CanHitLine(pos - new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.width, Projectile.height, target.position, target.width, target.height);
							if (lineOfSight)
							{
								minDist = distance;
								target2 = i;
								currentNPC = i;
							}
						}
					}
				}
			}
			return target2;
		}
		public override void AI()
		{
			if (runOnce)
			{
				Projectile.position += Projectile.velocity.SafeNormalize(Vector2.Zero) * speed;
				runStartCalculations();
				runOnce = false;
			}
			Projectile.alpha += 4;
			if (Projectile.alpha >= 255)
				Projectile.Kill();
		}
	}
}
		