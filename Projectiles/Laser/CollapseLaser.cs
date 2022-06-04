using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
		public override bool ShouldUpdatePosition() 
		{
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (completedLoads > 0)
			{
				LaserDraw(spriteBatch);
			}
			return false;
		}
		public override bool CanDamage()
		{
			return false;
		}
		public override bool PreDrawExtras()
		{
			return false;
		}
		public override void SetDefaults() 
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 24;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 2;
		}
		public override bool PreAI() 
		{
			if(initialDirection.X == 0 && initialDirection.Y == 0)
			{
				initialDirection = Projectile.velocity;
				Projectile.velocity *= 0f;
				if (completedLoads == 0)
					LaserDraw(null); //this will initiate the hitboxes without requiring the projectile to be drawn. This stops some lag, and also makes hitboxes spawn regardless of lag
			}
			return true;
		}
		int counter = -2;
		public override void AI() 
		{
			Player player = Main.player[Projectile.owner];
			//Projectile.Center = npc.Center;
			Projectile.ai[0]++;
			if(completedLoads > 0)
			{
				Projectile.alpha = 0;
				if (counter >= 1)
				{
					Projectile.alpha = 230 + (counter * 1);
				}
				counter++;
			}
			if (Projectile.alpha > 255)
			{
				Projectile.active = false;
				Projectile.Kill();
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			return false; //make sure there is no hitbox on the laser before hitbox check
		}
		List<float> posListY = new List<float>();
		List<float> posListX = new List<float>();
		List<int> npcs = new List<int>();
		int currentNPC = -1;
		public int FindClosestEnemy(Vector2 pos)
		{
			Player player = Main.player[Projectile.owner];
			if(currentNPC != -1)
			{
				return currentNPC;
			}
			float minDist = 450;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			for(int i = 0; i < Main.npc.Length; i++)
			{
				NPC target = Main.npc[i];
				if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && !npcs.Contains(i) && target.CanBeChasedBy())
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
		bool currentPointRange = false;
		public bool FindClosestPoint(Vector2 pos, Vector2 pos2)
		{
			Player player = Main.player[Projectile.owner];
			if(currentPointRange)
			{
				return true;
			}
			float minDist = 450;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			
			dX = pos2.X - pos.X;
			dY = pos2.Y - pos.Y;
			distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
			if(distance < minDist)
			{
				minDist = distance;
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
			float speed = 1.25f;//this the number adjusted that adjusts turn rate, higher = less bendy
			float distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
			speed /= distance;
			Vector2 rnVelo = new Vector2(5.6f, 0).RotatedBy(radians); //this the number adjusted by the turn rate, higher = more bendy
			rnVelo += new Vector2(dX * speed, dY * speed);
			npcRad = (float)Math.Atan2(rnVelo.Y, rnVelo.X); //turn velocity into rotation, this contributes to a few things
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
				if(distance < 24 && Projectile.friendly)
				{
					if(Projectile.owner == Main.myPlayer)
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, Mod.Find<ModProjectile>("ContinuumExplosion").Type, Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, Projectile.ai[0]);
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
				if(distance < 24 && Projectile.friendly)
				{
					currentPointRange = false;
					return true;
				}
			}
			return false;
		}
		Vector2 initialDirection = new Vector2(0f, 0f);
		int toBeat = 15;
		int kToBeat = 200;
		int fToBeat = 2000;
		int completedLoads = 0;
		public void LaserDraw(SpriteBatch spriteBatch)
		{
			float newAi = Projectile.ai[0] * 2 / 13f;
			double frequency = 0.3; //set up constants for the color spectrum variables
			double center = 130;
			double width = 125;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Laser/CollapseLaserHighlight"); //load the secondary required textures
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Laser/ContinuumSphereHighlight");
			float radianDir = (float)Math.Atan2(initialDirection.Y, initialDirection.X);
			Color color = new Color(100, 100, 100); //initialize a color variable, this white color won't be used, but the variable will
			Color white = new Color(255, 255, 255); //initialize white color
			if(Projectile.alpha > 200)
			{
				white *= ((255 - Projectile.alpha) / 255f); //make the white color scale with alpha, but not as much as the other colors, this is to make sure it looks like Last Prism
			}
			
			Vector2 drawPos = Projectile.Center;
			int helixRot = (int)Projectile.ai[0];
			float unitDis = 5f; //initiate a distance constant, this determines the "speed" at which the laser moves and bends
			int k = 0;
			int i = 10;
			int j = 0;
			int forceTerminate = 0;
			while(i != -1)
			{
				forceTerminate++;
				newAi += 0.5f / 13f; //make color change on the way through the beam
				Vector2 laserVelo = new Vector2(unitDis, 0f).RotatedBy(radianDir);
				drawPos.X += laserVelo.X;
				drawPos.Y += laserVelo.Y;
				Vector2 curve = new Vector2(28f, 0).RotatedBy(MathHelper.ToRadians(helixRot * 2f));
				helixRot--;

				if (forceTerminate > fToBeat)
				{
					break;
				}
				if(j >= toBeat || k >= kToBeat)
				{
					toBeat = j;
					//kToBeat = k;
					//fToBeat = forceTerminate;
					if(toBeat == 0)
					{
						toBeat = 1;
					}
					i = -1;
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
				
				if(currentNPC == -1 && !currentPointRange)	 
				{
					k += 1;
				}
				
				if(npcs.Count >= toBeat && toBeat > -1 && FindClosestPoint(drawPos, new Vector2(posListX[j], posListY[j]))) 
				{
					bool increment = getHitbox(drawPos, false, new Vector2(posListX[j], posListY[j]));
					if(increment)
					{						
						j++;
						i = 15;
					}
				}
				else if(FindClosestEnemy(drawPos) != -1 && !(npcs.Count >= toBeat && toBeat > -1))
				{
					bool increment = getHitbox(drawPos, true, new Vector2(0,0));
					if(increment)
					{					
						j++;
						i = 15;
					}
				}
				if(spriteBatch != null) //checking if it is the second strand that starts (calculated strand)
				{
					double red = Math.Sin(frequency * (double)newAi) * width + center;
					double grn = Math.Sin(frequency * (double)newAi + 2.0) * width + center;
					double blu = Math.Sin(frequency * (double)newAi + 4.0) * width + center;
					color = new Color((int)red, (int)grn, (int)blu);
					color *= ((255 - Projectile.alpha) / 255f);
					Vector2 helixPos1 = drawPos + new Vector2(curve.X, 0).RotatedBy(radianDir + MathHelper.ToRadians(90));
					Vector2 helixPos2 = drawPos + new Vector2(curve.X, 0).RotatedBy(radianDir - MathHelper.ToRadians(90));

					if(forceTerminate % 4 == 0)
						Lighting.AddLight(drawPos, new Vector3((int)red, (int)grn, (int)blu) * (255 - Projectile.alpha) / 20000f); //adds game light at the area

					spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, helixPos1 - Main.screenPosition, null, color, radianDir, new Vector2(14, 7), 0.5f, SpriteEffects.None, 0f);
					spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, helixPos2 - Main.screenPosition, null, color, radianDir, new Vector2(14, 7), 0.5f, SpriteEffects.None, 0f);
					spriteBatch.Draw(texture, helixPos2 - Main.screenPosition, null, white, radianDir, new Vector2(14, 7), 0.5f, SpriteEffects.None, 0f);
					spriteBatch.Draw(texture, helixPos1 - Main.screenPosition, null, white, radianDir, new Vector2(14, 7), 0.5f, SpriteEffects.None, 0f);
					spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos - Main.screenPosition, null, color, radianDir, new Vector2(14, 7), 1f, SpriteEffects.None, 0f);
					spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, white, radianDir, new Vector2(14, 7), 1f, SpriteEffects.None, 0f);
				}
			}
			Vector2 laserVelo2 = new Vector2(10f, 0f).RotatedBy(radianDir); //relocate end sphere to the tip of the laser
			drawPos.X += laserVelo2.X * 2;
			drawPos.Y += laserVelo2.Y * 2;
			for(int l = 0; l < 6; l++) //because the overlapping lasers end up covering up for eachothers alpha, this will help make this look more consistent too
			{
				if(completedLoads > 0 && spriteBatch != null)
				{
					spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Mod.Find<ModProjectile>("ContinuumSphere").Type].Value, drawPos - Main.screenPosition, null, color, radianDir, new Vector2(15,15), 1f, SpriteEffects.None, 0f);
					spriteBatch.Draw(texture2, drawPos - Main.screenPosition, null, white, radianDir, new Vector2(15,15), 1f, SpriteEffects.None, 0f);
				}
			}
			completedLoads++;
			//return false;
			
			/* //This is collision checking code, for blocks
			Vector2 position = Projectile.Center + unit * Distance;	
			int i = (int)(position.X / 16);
			int j =	(int)(position.Y / 16);
			if(Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true)
			{
				Distance -= 6f;
				break;
			}
			*/
		}
	}
}