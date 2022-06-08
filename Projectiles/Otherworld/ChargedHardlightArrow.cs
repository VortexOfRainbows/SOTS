using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Otherworld
{
	public class ChargedHardlightArrow : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Charged Hardlight Arrow");
		}
		public override void SetDefaults() 
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 30;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.alpha = 0;
		}
        public override bool? CanDamage()/* Suggestion: Return null instead of false */
        {
			return false;
        }
        public override bool PreAI() 
		{
			if(initialDirection.X == 0 && initialDirection.Y == 0)
			{
				Projectile.ai[0] = Main.rand.Next(360);
				Projectile.position += Projectile.velocity.SafeNormalize(Vector2.Zero) * 18;
				initialDirection = Projectile.velocity;
				if(completedLoads == 0)
					LaserDraw(null);
				Terraria.Audio.SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 94, 0.6f);
			}
			return true;
		}
		public override void AI() 
		{
			Player player = Main.player[Projectile.owner];
			//Projectile.Center = npc.Center;
			Projectile.ai[0]++;
			if(completedLoads > 0)
			{
				Projectile.alpha += 10;
			}
			if(Projectile.alpha > 255)
			{
				Projectile.active = false;
				Projectile.Kill();
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			return false; //make sure there is no hitbox on the laser before hitbox check
		}
		bool direct = false;
		float posY = -1f;
		float posX = -1f;
		Rectangle posRect;
		public int FindClosestEnemy(Vector2 pos, float dist)
		{
			Player player = Main.player[Projectile.owner];
			float minDist = dist;
			int target2 = -1;
			float dX;
			float dY;
			float distance;
			for(int i = 0; i < Main.npc.Length; i++)
			{
				NPC target = Main.npc[i];
				if(target.active && target.CanBeChasedBy())
				{
					dX = target.Center.X - pos.X;
					dY = target.Center.Y - pos.Y;
					distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
					if(distance < minDist)
					{
						minDist = distance;
						target2 = i;
					}
				}
			}
			if(target2 == -1)
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if (target.active && !target.friendly && !target.dontTakeDamage)
					{
						if (target.Hitbox.Intersects(new Rectangle((int)pos.X - 8, (int)pos.Y -8, Projectile.width, Projectile.height)))
						{
							direct = true;
							target2 = i;
							break;
						}
					}
				}
			return target2;
		}
		bool currentPointRange = false;
		public bool FindClosestPoint(Vector2 pos, Vector2 pos2, float dist)
		{
			Player player = Main.player[Projectile.owner];
			if(currentPointRange)
			{
				return true;
			}
			float minDist = dist;
			float dX;
			float dY;
			float distance;
			
			dX = pos2.X - pos.X;
			dY = pos2.Y - pos.Y;
			distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
			if(distance < minDist || posRect.Intersects(new Rectangle((int)pos.X - 8, (int)pos.Y - 8, Projectile.width, Projectile.height)))
			{
				currentPointRange = true;
				return true;
			}
			return false;
		}
		public float Redirect(float radians, Vector2 pos, Vector2 npc)
		{
			float dX = npc.X - pos.X;
			float dY = npc.Y - pos.Y;
			float npcRad = (float)Math.Atan2(dY, dX);
			//float diffRad = radians - npcRad;
			float speed = 1.25f; //this the number adjusted that adjusts turn rate, higher = less bendy
			float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
			speed /= distance;
			Vector2 rnVelo = new Vector2(14f, 0).RotatedBy(radians); //this the number adjusted by the turn rate, higher = more bendy
			rnVelo += new Vector2(dX * speed, dY * speed);
			npcRad = (float)Math.Atan2(rnVelo.Y, rnVelo.X); //turn velocity into rotation, this contributes to a few things
			return npcRad;
		}
		public bool getHitbox(Vector2 drawpos, bool hitbox, Rectangle npcHitbox, float dist)
		{
			if(hitbox)
			{
				NPC target = Main.npc[FindClosestEnemy(drawpos, dist)];
				if(target.Hitbox.Intersects(new Rectangle((int)drawpos.X - 8, (int)drawpos.Y - 8, Projectile.width, Projectile.height)) && Projectile.friendly && !target.dontTakeDamage)
				{
					if(Projectile.owner == Main.myPlayer)
					{
						SOTSProjectile instance = Projectile.GetGlobalProjectile<SOTSProjectile>();
						Projectile.NewProjectile(drawpos, Projectile.velocity, ModContent.ProjectileType<HardlightArrowDamage>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, instance.frostFlake, 0f);
					}
					posX = target.Center.X;
					posY = target.Center.Y;
					posRect = target.Hitbox;
					return true;
				}
			}
			else
			{
				if(npcHitbox.Intersects(new Rectangle((int)drawpos.X - 8, (int)drawpos.Y - 8, Projectile.width, Projectile.height)) && Projectile.friendly)
				{
					currentPointRange = false;
					return true;
				}
			}
			return false;
		}
		Vector2 initialDirection = new Vector2(0f, 0f);
		int distance = 550;
		int completedLoads = 0;
		public override bool ShouldUpdatePosition() 
		{
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public void LaserDraw(SpriteBatch spriteBatch)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			float radianDir = (float)Math.Atan2(initialDirection.Y, initialDirection.X);
			Vector2 drawPos = Projectile.Center;
			int helixRot = (int)Projectile.ai[0];
			float unitDis = 3f; //initiate a distance constant, this determines the "speed" at which the laser moves and bends
			bool stop = false;
			int counter = 0; 
			while(counter < distance)
			{
				Vector2 curve = new Vector2(10f, 0).RotatedBy(MathHelper.ToRadians(helixRot * 2f));
				helixRot--;
				Vector2 laserVelo = new Vector2(unitDis, 0f).RotatedBy(radianDir);
				drawPos.X += laserVelo.X;
				drawPos.Y += laserVelo.Y;
				counter++;
				float enemyMinDist = counter * 0.175f + 20 + SOTSPlayer.ModPlayer(player).typhonRange * 1.2f;
				if(stop)
				{
					break;
				}

				if(completedLoads != 0 && FindClosestPoint(drawPos, new Vector2(posX, posY), enemyMinDist)) 
				{
					if(!direct)
						radianDir = Redirect(radianDir, drawPos, new Vector2(posX, posY));
				}
				else if(FindClosestEnemy(drawPos, enemyMinDist) != -1 && completedLoads == 0)
				{
					if (!direct)
						radianDir = Redirect(radianDir, drawPos, Main.npc[FindClosestEnemy(drawPos, enemyMinDist)].Center);
				}
				
				if(completedLoads != 0 && FindClosestPoint(drawPos, new Vector2(posX, posY), enemyMinDist)) 
				{
					bool increment = getHitbox(drawPos, false, posRect, enemyMinDist);
					if(increment)
					{
						stop = true;
					}
				}
				else if(FindClosestEnemy(drawPos, enemyMinDist) != -1 && completedLoads == 0)
				{
					bool increment = getHitbox(drawPos, true, posRect, enemyMinDist);
					if(increment)
					{
						stop = true;
					}
				}
				if(completedLoads > 0 && spriteBatch != null) //checking if it is the second strand that starts (calculated strand)
				{
					Color color = new Color(255, 255, 255, 0) * ((255 - Projectile.alpha) / 255f);
					Vector2 helixPos1 = drawPos + new Vector2(curve.X, 0).RotatedBy(radianDir + MathHelper.ToRadians(90));
					//if (forceTerminate % 4 == 0)
						//Lighting.AddLight(drawPos, color.ToVector3() * (255 - Projectile.alpha) / 20000f); //adds game light at the area
					spriteBatch.Draw(texture, helixPos1 - Main.screenPosition, null, color, radianDir + MathHelper.ToRadians(90), new Vector2(texture.Width/2, texture.Height/2), Projectile.scale, SpriteEffects.None, 0f);
					spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color, radianDir + MathHelper.ToRadians(90), new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
				}
				int i = (int)drawPos.X / 16;
				int j = (int)drawPos.Y / 16;
				if (!WorldGen.InWorld(i, j, 20) || Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true)
				{
					if (Projectile.owner == Main.myPlayer && completedLoads == 0)
					{
						SOTSProjectile instance = Projectile.GetGlobalProjectile<SOTSProjectile>();
						Projectile.NewProjectile(drawPos, Projectile.velocity, ModContent.ProjectileType<HardlightArrowDamage>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, instance.frostFlake, 0f);
					}
					distance = counter;
					break;
                }
			}
			if (completedLoads == 0)
			{
				for (int h = 0; h < 20; h++)
				{
					int dust = Dust.NewDust(new Vector2(drawPos.X - 12, drawPos.Y - 12), 16, 16, DustID.Electric);
					Main.dust[dust].scale *= 1f;
					Main.dust[dust].velocity += Projectile.velocity * 0.1f;
					Main.dust[dust].noGravity = true;
				}
			}
			completedLoads++;
		}
		public override void PostDraw(Color lightColor)
		{
			if(completedLoads > 0)
			{
				LaserDraw(spriteBatch);
			}
		}
	}
}