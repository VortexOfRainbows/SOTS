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
using SOTS.Void;

namespace SOTS.Projectiles.Laser
{
	public class CollapseLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Continuum Collapse");
		}

		public override void SetDefaults() 
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.timeLeft = 60;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override void AI() 
		{
			//projectile.Center = npc.Center;
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] == 2f) {
				projectile.damage = 0;
				projectile.alpha += 25;
			}
			if (projectile.localAI[0] > 35f) {
				projectile.Kill();
			}
			projectile.alpha += 7;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
			projectile.damage--;
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 8f, ref point);
		}
		List<float> posListY = new List<float>();
		List<float> posListX = new List<float>();
		List<int> npcs = new List<int>();
		/*
			first we need to find nearby enemies, the closest one will be targeted first
			next we should choose to either move up or down in the enemy index chain, this will be based on whichever enemy is closest, this should be easy, 
			considering we have ricochet code for items implemented already, (arrow son of arrow)
			
			we will save a list as the projectile travels through enemys, once each list hits a capacity of 10 (10 enemies accounted for), 
			we can save the values and continue drawing the laser so it lasts longer then a frame, this will also be useful doing hitboxes
			
			the homing effect for enemies could be a connection of straight lines, but i think a laser turning would look cooler, so instead, we will write a method to find the radian difference between the direction the projectile is travelling
			and the radians of the difference between the enemy pos and the laser pos, if the difference between these two is less then 180 degrees counterclockwise, turning left would be prefered, otherwise, we will turn right.
			the turn will stop once we get with in a -1 to 1 range of the laser rotation and enemy rotation, because we cannot add perfectly to make it strike through, so it will have to snap into place instead.
			the lasers motion won't be halted while turning, so continue moving the laser while we change the angular vector (or we can use a different vector if i can't find how to use .normalize())
			
			finally, we can finish the rest of the visual effects, like a helix around the primary laser
			
			REASON FOR FAILURE
			_______________________
			The reason why the laser is curving the wrong direction of the second go is because it changes targets midway throught the original burst
			
			Problems to fix
			_______________________
			The first laser will seek for a target until death, the second will stop upon hitting all its targets, this makes one longer than the other. The main problem at hand, however, is that the laser isn't hitting gaurenteed, which it always should
		*/
		int currentNPC = -1;
		public int FindClosestEnemy(Vector2 pos)
		{
			Player player = Main.player[projectile.owner];
			if(currentNPC != -1)
			{
				return currentNPC;
			}
			float minDist = 1200;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			for(int i = 0; i < Main.npc.Length - 1; i++)
			{
				NPC target = Main.npc[i];
				if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && !npcs.Contains(i))
				{
					dX = target.Center.X - pos.X;
					dY = target.Center.Y - pos.Y;
					distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
					if(distance < minDist)
					{
						minDist = distance;
						target2 = i;
						currentNPC = i;
					}
				}
			}
			return target2;
		}
		public bool FindClosestPoint(Vector2 pos, Vector2 pos2)
		{
			return true;
			float minDist = 1200;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			
			dX = pos2.X - pos.X;
			dY = pos2.Y - pos.Y;
			distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
			if(distance < minDist)
			{
				minDist = distance;
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
			float speed = 1f;
			float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
			speed /= distance;
			Vector2 rnVelo = new Vector2(3f, 0).RotatedBy(radians);
			rnVelo += new Vector2(dX * speed, dY * speed);
			npcRad = (float)Math.Atan2(rnVelo.Y, rnVelo.X);
			return npcRad;
		}
		public bool getHitbox(Vector2 drawpos, bool hitbox, Vector2 destination)
		{
			if(hitbox)
			{
				NPC npc = Main.npc[FindClosestEnemy(drawpos)];
				float dX = npc.Center.X - drawpos.X;
				float dY = npc.Center.Y - drawpos.Y;
				float distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
				if(distance < 24 && projectile.friendly)
				{
					if(projectile.owner == Main.myPlayer)
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("GreenExplosion"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
					currentNPC = -1;
					posListX.Add(npc.Center.X);
					posListY.Add(npc.Center.Y);
					npcs.Add(npc.whoAmI);
					return true;
				}
			}
			else
			{
				float dX = destination.X - drawpos.X;
				float dY = destination.Y - drawpos.Y;
				float distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
				if(distance < 24 && projectile.friendly)
				{
					return true;
				}
			}
			return false;
		}
		Vector2 initialDirection = new Vector2(0f, 0f);
		int toBeat = 10;
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player  = Main.player[projectile.owner];
			
			Color alpha = new Color(255, 0, 0) * ((255 - projectile.alpha) / 255f);
			if(initialDirection.X == 0 && initialDirection.Y == 0)
			{
				initialDirection = projectile.velocity;
				projectile.velocity *= 0f;
			}
			float radianDir = (float)Math.Atan2(initialDirection.Y, initialDirection.X);
			
			Vector2 drawPos = projectile.Center;
			float unitDis = 12f;
			int k = 0;
			int i = 10;
			int j = 0;
			while(i != -1)
			{
				k++;
				
				if(j >= toBeat || k >= 1000)
				{
					if(j < toBeat && j != 0)
					{
						NPC npc = Main.npc[FindClosestEnemy(drawPos)];
						posListX.Add(npc.Center.X);
						posListY.Add(npc.Center.Y);
						npcs.Add(npc.whoAmI);
					}
					
					toBeat = j;
					if(toBeat == 0)
					{
						toBeat = 1;
					}
					break;
				}
				i++;
				if(i >= 30)
				{
					if(npcs.Count >= toBeat && toBeat > -1 && FindClosestPoint(drawPos, new Vector2(posListX[j], posListY[j])))
					{
						radianDir = Redirect(radianDir, drawPos, new Vector2(posListX[j], posListY[j]));
					}
					else if(FindClosestEnemy(drawPos) != -1 && !(npcs.Count >= toBeat && toBeat > -1))
					{
						radianDir = Redirect(radianDir, drawPos, Main.npc[FindClosestEnemy(drawPos)].Center);
					}
				}
				
				if(npcs.Count >= toBeat && toBeat > -1 && FindClosestPoint(drawPos, new Vector2(posListX[j], posListY[j])))
				{
					bool increment = getHitbox(drawPos, false, new Vector2(posListX[j], posListY[j]));
					if(increment)
					{						
						j++;
						i = 20;
					}
				}
				else if(FindClosestEnemy(drawPos) != -1 && !(npcs.Count >= toBeat && toBeat > -1))
				{
					bool increment = getHitbox(drawPos, true, new Vector2(0,0));
					if(increment)
					{					
						j++;
						i = 20;
					}
				}
				Vector2 laserVelo = new Vector2(unitDis, 0f).RotatedBy(radianDir);
				drawPos.X += laserVelo.X;
				drawPos.Y += laserVelo.Y;
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos - Main.screenPosition, null, alpha, 0f, new Vector2(2, 2), 1f, SpriteEffects.None, 0f);
				
			}
			return false;
			
			/* //This is collision checking code, for blocks
			Vector2 position = projectile.Center + unit * Distance;	
			int i = (int)(position.X / 16);
			int j =	(int)(position.Y / 16);
			if(Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
			{
				Distance -= 6f;
				break;
			}
			*/
		}
	}
}