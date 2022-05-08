using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Laser
{
	public class PrismLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Photon Laser");
		}

		public override void SetDefaults() 
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.timeLeft = 85;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		bool runOnce = true;
		Color color;
		float waveLength;
		float scale = 0.5f;
		public override bool PreAI() 
		{
			if(runOnce)
			{
				int type = (int)projectile.ai[0];
				switch (type)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						waveLength = 600f;
						break;
					case 1:
						color = new Color(255, 140, 0, 0);
						waveLength = 540f;
						break;
					case 2:
						color = new Color(255, 255, 0, 0);
						waveLength = 480f;
						break;
					case 3:
						color = new Color(0, 255, 0, 0);
						waveLength = 420f;
						break;
					case 4:
						color = new Color(0, 255, 255, 0);
						waveLength = 360f;
						break;
					case 5:
						color = new Color(0, 0, 255, 0);
						waveLength = 300f;
						break;
					case 6:
						color = new Color(140, 0, 255, 0);
						waveLength = 240f;
						break;
				}
				SetPostitions();
				UpdatePositions();
				runOnce = false;
				return true;
            }
			return true;
		}
        public override void AI()
        {
			projectile.alpha += 3;
			projectile.ai[1] -= 2;
			UpdatePositions();
		}
		bool collided = false;
        public void SetPostitions()
        {
			Vector2 direction = new Vector2(14 * scale, 0).RotatedBy(projectile.velocity.ToRotation());
			int maxDist = 360;
			Vector2 currentPos = projectile.Center;
			for (int k = 0; k < maxDist; k++)
			{
				Vector2 rotationalPos = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians((projectile.ai[1] + k) + (int)projectile.ai[0] * 360f / 7f));
				posList.Add(currentPos);
				rotations.Add(projectile.velocity.ToRotation());
				int i = (int)(currentPos.X / 16);
				int j = (int)(currentPos.Y / 16);
				if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
				{
					k = maxDist;
					break;
				}
				currentPos += direction;
				if(k > waveLength / 12f)
				{
					int npc = FindClosestEnemy(currentPos);
					if (npc != -1)
					{
						NPC target = Main.npc[npc];
						if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy() && !collided)
						{
							direction = new Vector2(14 * scale, 0).RotatedBy(Redirect(direction.ToRotation(), currentPos, target.Center));
							float width = projectile.width * scale;
							float height = projectile.height * scale;
							Rectangle projHitbox = new Rectangle((int)currentPos.X - (int)width / 2, (int)currentPos.Y - (int)height / 2, (int)width, (int)height);
							if (target.Hitbox.Intersects(projHitbox))
							{
								collided = true;
							}
						}
					}
				}
			}
			posList2 = new List<Vector2>(posList);
		}
		bool runOnce2 = true;
		public void UpdatePositions()
		{
			int alpha = 0;
			Vector2 direction = new Vector2(14 * scale, 0).RotatedBy(projectile.velocity.ToRotation());
			for (int k = 0; k < posList.Count; k++)
			{
				if (k > 235)
				{
					alpha += 2;
				}
				Vector2 rotationalPos = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians((projectile.ai[1] + k) + (int)projectile.ai[0] * 360f / 7f));
				Vector2 applyRotate = new Vector2(0, rotationalPos.X).RotatedBy(projectile.velocity.ToRotation());
				posList2[k] = posList[k] + applyRotate;
				if(k > 0)
				{
					applyRotate = applyRotate + posList[k] - posList2[k - 1];
					rotations[k] = applyRotate.ToRotation();
				}
                else
                {
					rotations[k] = projectile.velocity.ToRotation();
                }
				int i = (int)(posList[k].X / 16);
				int j = (int)(posList[k].Y / 16);
				if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
				{
					Dust dust = Dust.NewDustDirect(posList2[k] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.fadeIn = 0.2f;
					dust.noGravity = true;
					dust.alpha = alpha;
					dust.color = color;
					dust.scale *= 1.5f;
					dust.velocity *= 1.5f;
					break;
				}
				if (runOnce2)
				{
					int amt = 2;
					if (SOTS.Config.lowFidelityMode)
						amt = 1;
					for (int a = 0; a < amt; a++)
					{
						Dust dust = Dust.NewDustDirect(posList2[k] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						dust.fadeIn = 0.2f;
						dust.noGravity = true;
						dust.alpha = alpha;
						dust.color = color;
						dust.scale *= 1.75f;
						dust.velocity *= 1.5f;
					}
				}
			}
			runOnce2 = false;
		}
		float redirections = 0;
		public float Redirect(float radians, Vector2 pos, Vector2 npc)
		{
			float dX = npc.X - pos.X;
			float dY = npc.Y - pos.Y;
			float npcRad = (float)Math.Atan2(dY, dX);
			//float diffRad = radians - npcRad;
			float speed = 1.25f + (redirections * 0.05f); //this the number adjusted that adjusts turn rate, higher = less bendy
			float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
			speed /= distance;
			Vector2 rnVelo = new Vector2(14f - (redirections * 0.025f), 0).RotatedBy(radians); //this the number adjusted by the turn rate, higher = more bendy
			rnVelo += new Vector2(dX * speed, dY * speed);
			npcRad = (float)Math.Atan2(rnVelo.Y, rnVelo.X); //turn velocity into rotation, this contributes to a few things
			redirections++;
			return npcRad;
		}
		int currentNPC = -1;
		public int FindClosestEnemy(Vector2 pos)
		{
			Player player = Main.player[projectile.owner];
			if (currentNPC != -1)
			{
				return currentNPC;
			}
			float minDist = waveLength;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC target = Main.npc[i];
				if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
				{
					dX = target.Center.X - pos.X;
					dY = target.Center.Y - pos.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					if (distance < minDist)
					{
						bool lineOfSight = Collision.CanHitLine(pos - new Vector2(projectile.width/2, projectile.height/2), projectile.width, projectile.height, target.position, target.width, target.height);
						if (lineOfSight)
						{
							minDist = distance;
							target2 = i;
							currentNPC = i;
						}
					}
				}
			}
			return target2;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(projectile.alpha >= 150)
            {
				return false;
            }
			float width = projectile.width * scale;
			float height = projectile.height * scale;
			for (int i = 0; i < posList2.Count; i++)
			{
				Vector2 pos = posList2[i];
				projHitbox = new Rectangle((int)pos.X - (int)width/2, (int)pos.Y - (int)height/2, (int)width, (int)height);
				if(projHitbox.Intersects(targetHitbox))
                {
					return true;
                }
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 8f, ref point);
		}
		List<Vector2> posList = new List<Vector2>();
		List<Vector2> posList2 = new List<Vector2>();
		List<float> rotations = new List<float>();
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Laser/PrismOrb").Value;
			Vector2 origin = new Vector2(texture.Width/2, texture.Height/2);
			Vector2 origin2 = new Vector2(texture2.Width / 2, texture2.Height / 2);
			Player player  = Main.player[projectile.owner];
			int alpha = 0;
			for(int i = 0; i < posList2.Count; i++)
			{
				Vector2 drawPos = posList2[i];
				if(i > 235)
                {
					alpha += 2;
                }
				spriteBatch.Draw(i == 0 ? texture2 : texture, drawPos - Main.screenPosition, null, color * ((255 - projectile.alpha) / 255f) * ((255 - alpha) / 255f), rotations[i], i == 0 ? origin2 : origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}