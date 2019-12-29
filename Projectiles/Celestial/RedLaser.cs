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

namespace SOTS.Projectiles.Celestial
{
	/*
	public class CeremonialKnife : ModProjectile 
    {
		double startingDirection = 0.0;
		int lockOnTarget = -1;
		bool channel = true;
		float yTargetDif;
		float xTargetDif;
		int targetID = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ceremonial Knife");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 26;
			projectile.height = 32;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.friendly = false;
            //Main.projFrames[projectile.type] = 1;
			projectile.timeLeft = 10000;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			Vector2 cursorArea;
			
			if(player.active && !player.dead)
			{
				projectile.timeLeft = 6;
			}
			float disPX = player.Center.X - projectile.Center.X;
			float disPY = player.Center.Y - projectile.Center.Y;
			float disP = (float)Math.Sqrt(disPX * disPX + disPY * disPY);
			
			float disEX;
			float disEY;
			float disE = -1;
			float minDist = 600;
			
			if(lockOnTarget == -1)
			{
				for(int j = 0; j < Main.npc.Length - 1; j++)
				{
					NPC target = Main.npc[j];
					if(!target.friendly && target.dontTakeDamage == false && target.active)
					{
						disEX = target.Center.X - projectile.Center.X;
						disEY = target.Center.Y - projectile.Center.Y;
						float disEC = (float) Math.Sqrt((double)(disEX * disEX + disEY * disEY));
						if(disEC < minDist)
						{
							minDist = disEC;
							disE = disEC;
							targetID = target.whoAmI;
						}
					}
				}
				
				if(disE < disP && disE != -1 && targetID != -1)
				{
					lockOnTarget = 1; //target npc
				}
				else
				{
					lockOnTarget = 2; //target player
				}
			}

			if (player.gravDir == 1f)
			{
				cursorArea.Y = (float)Main.mouseY + Main.screenPosition.Y;
			}
			else
			{
				cursorArea.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			cursorArea.X = (float)Main.mouseX + Main.screenPosition.X;
			if(player.channel && channel)
			{
				float goToCursorX = cursorArea.X - projectile.Center.X;
				float goToCursorY = cursorArea.Y - projectile.Center.Y;
				startingDirection = Math.Atan2((double)goToCursorY, (double)goToCursorX);
				projectile.rotation = (float)startingDirection + MathHelper.ToRadians(45f);
				startingDirection *= 180/Math.PI;
				lockOnTarget = -1;
				projectile.ai[1] = 100;
			}
			else
			{
				projectile.ai[1] = 10;
				channel = false;
			}
			Vector2 directionArea = new Vector2(2555, 0).RotatedBy(MathHelper.ToRadians((float)startingDirection));
			directionArea.X = projectile.Center.X + directionArea.X;
			directionArea.Y = projectile.Center.Y + directionArea.Y;
			
			if(lockOnTarget == 1 || lockOnTarget == 3)
			{
				NPC target = Main.npc[targetID];
				if(!target.friendly && target.dontTakeDamage == false && target.active)
				{
					if(lockOnTarget == 1)
					{
						xTargetDif = target.Center.X - projectile.Center.X;
						yTargetDif = target.Center.Y - projectile.Center.Y;
						lockOnTarget = 3;
					}
					projectile.position.X = target.Center.X - xTargetDif - projectile.width/2;
					projectile.position.Y = target.Center.Y - yTargetDif - projectile.width/2;
				}
				else
				{
					lockOnTarget = 2;
				}
			}
			else if (lockOnTarget == 2 || lockOnTarget == 4)
			{
				if(lockOnTarget == 2)
				{
					xTargetDif = player.Center.X - projectile.Center.X;
					yTargetDif = player.Center.Y - projectile.Center.Y;
					lockOnTarget = 4;
				}
				projectile.position.X = player.Center.X - xTargetDif - projectile.width/2;
				projectile.position.Y = player.Center.Y - yTargetDif - projectile.width/2;
			}
			
			
			if(player.FindBuffIndex(mod.BuffType("Catalyst")) > -1)
			{
				LaunchLaser(directionArea);
			}
			
		}
		public void LaunchLaser(Vector2 area)
		{
			Player player  = Main.player[projectile.owner];
			int Probe = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("RedLaser"), projectile.damage, 0, projectile.owner);
			Main.projectile[Probe].ai[0] = area.X;
			Main.projectile[Probe].ai[1] = area.Y;
			VoidPlayer.ModPlayer(player).voidMeter--;
			projectile.Kill();
		}
		public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/LaserTargeting");
			//Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/PinkyHook_Chain");    //this where the chain of grappling hook is drawn
													  //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
			Vector2 directionArea = new Vector2(2555, 0).RotatedBy(MathHelper.ToRadians((float)startingDirection));
			directionArea.X = projectile.Center.X + directionArea.X;
			directionArea.Y = projectile.Center.Y + directionArea.Y;
			Vector2 mountedCenter = directionArea;
			Vector2 position = projectile.Center;
			Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float num1 = (float)texture.Height;
			Vector2 vector2_4 = mountedCenter - position;
			float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
			bool flag = true;
			if (float.IsNaN(position.X) && float.IsNaN(position.Y))
				flag = false;
			if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
				flag = false;
			while (flag)
			{
				if ((double)vector2_4.Length() < (double)num1 + 1.0)
				{
					flag = false;
				}
				else
				{
					Vector2 vector2_1 = vector2_4;
					vector2_1.Normalize();
					position += vector2_1 * num1;
					vector2_4 = mountedCenter - position;
					Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
					Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
				}
			}
		}
	}
	*/
	public class RedLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Ceremonial Laser");
		}

		public override void SetDefaults() 
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.timeLeft = 60;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.magic = false; //both damage types just in case i wanna repurpose it
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override void AI() 
		{
			//projectile.Center = npc.Center;
			projectile.alpha += 20;
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] > 3f) {
				projectile.damage = 0;
			}
			if (projectile.localAI[0] > 12f) {
				projectile.Kill();
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 4;
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			Vector2 endPoint = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 unit = endPoint - projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			for (float Distance = 0; Distance <= length; Distance += 6f) 
			{
				Vector2 position = projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);
				
				if(Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
				{
					break;
				}
				if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, position, 8f, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 8f, ref point);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player  = Main.player[projectile.owner];
			Vector2 endPoint = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 unit = endPoint - projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			for (float Distance = 0; Distance <= length; Distance += 5f) {
				Distance += Main.rand.Next(4);
				Vector2 drawPos = projectile.Center + unit * Distance - Main.screenPosition;
				
				Vector2 position = projectile.Center + unit * Distance;	
				int i = (int)(position.X / 16);
				int j =	(int)(position.Y / 16);
				if(Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
				{
					Distance -= 6f;
					break;
				}
				Color alpha = new Color(200, 20, 65) * ((255 - projectile.alpha) / 255f);
				//Color alpha = ((255 - projectile.alpha) / 255f);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, alpha, Distance, new Vector2(2, 2), 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}