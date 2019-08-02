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

namespace SOTS.Projectiles 
{    
    public class MourningStar : ModProjectile 
    {	public bool swap = false;
		
		int down = 0;
		int cycleRotaters = 0;
		float traveltoX = 0;
		float traveltoY = 0;
		int rotaters = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mourning Star");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 34; 
            projectile.timeLeft = 360;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = false; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void AI()
		{
			Player owner  = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)owner.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.starCen = projectile.Center;
			rotaters = 0;
			projectile.damage = 10;
			projectile.tileCollide = false;
			
			if(SOTSWorld.legendLevel == 1)
			{
				projectile.damage = 12;
			}
			if(SOTSWorld.legendLevel == 2)
			{
				projectile.damage = 15;
			}
			if(SOTSWorld.legendLevel == 3)
			{
				projectile.damage = 19;
			}
			if(SOTSWorld.legendLevel == 4)
			{
				projectile.damage = 24;
			}
			if(SOTSWorld.legendLevel == 5)
			{
				projectile.damage = 30;
			}
			if(SOTSWorld.legendLevel == 6)
			{
				projectile.damage = 32;
			}
			if(SOTSWorld.legendLevel == 7)
			{
				projectile.damage = 35;
			}
			if(SOTSWorld.legendLevel == 8)
			{
				projectile.damage = 38;
			}
			if(SOTSWorld.legendLevel == 9)
			{
				projectile.damage = 40;
			}
			if(SOTSWorld.legendLevel == 10)
			{
				projectile.damage = 42;
			}
			if(SOTSWorld.legendLevel == 11)
			{
				projectile.damage = 45;
			}
			if(SOTSWorld.legendLevel == 12)
			{
				projectile.damage = 50;
			}
			if(SOTSWorld.legendLevel == 13)
			{
				projectile.damage = 55;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				projectile.damage = 60;
			}
			if(SOTSWorld.legendLevel == 15)
			{
				projectile.damage = 63;
			}
			if(SOTSWorld.legendLevel == 16)
			{
				projectile.damage = 67;
			}
			if(SOTSWorld.legendLevel == 17)
			{
				projectile.damage = 71;
			}
			if(SOTSWorld.legendLevel == 18)
			{
				projectile.damage = 76;
			}
			if(SOTSWorld.legendLevel == 19)
			{
				projectile.damage = 80;
			}
			if(SOTSWorld.legendLevel == 20)
			{
				projectile.damage = 85;
			}
			if(SOTSWorld.legendLevel == 21)
			{
				projectile.damage = 90;
			}
			if(SOTSWorld.legendLevel == 22)
			{
				projectile.damage = 100;
			}
			if(SOTSWorld.legendLevel == 23)
			{
				projectile.damage = 112;
			}
			if(SOTSWorld.legendLevel == 24)
			{
				projectile.damage = 125;
			}
			if(SOTSWorld.legendLevel == 25)
			{
				projectile.damage = 150;
			}
			rotaters += SOTSWorld.legendLevel;
			
			
			
			if(swap == true)
			{
			projectile.tileCollide = true;
			projectile.damage = (int)(projectile.damage * 0.5);
			}
			
			
			
			
			if(rotaters >= 3 && rotaters < 6)
			{
				cycleRotaters++;
				if(cycleRotaters >= 360)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
					cycleRotaters = 0;
				}
			}
			if(rotaters >= 6 && rotaters < 9)
			{
				cycleRotaters++;
				if(cycleRotaters == 180)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters >= 360)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
					cycleRotaters = 0;
				}
				
			}
			if(rotaters >= 9 && rotaters < 12)
			{
				cycleRotaters++;
				if(cycleRotaters == 120)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 240)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters >= 360)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
					cycleRotaters = 0;
				}
				
			}
			if(rotaters >= 12 && rotaters < 15)
			{
				cycleRotaters++;
				if(cycleRotaters == 90)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 180)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 270)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters >= 360)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
					cycleRotaters = 0;
				}
				
			}
			if(rotaters >= 15 && rotaters < 18)
			{
				cycleRotaters++;
				if(cycleRotaters == 72)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 144)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 216)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 288)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters >= 360)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
					cycleRotaters = 0;
				}
			}
			if(rotaters >= 18 && rotaters < 24)
			{
				cycleRotaters++;
				if(cycleRotaters == 60)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 120)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 180)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 240)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 300)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters >= 360)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
					cycleRotaters = 0;
				}
				
			}
			if(rotaters >= 24)
			{
				cycleRotaters++;
				if(cycleRotaters == 45)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 90)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 135)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 180)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 225)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 270)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters == 315)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
				}
				if(cycleRotaters >= 360)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MourningStarRotater"), 9, 0, 0, 0f, 0f);
					cycleRotaters = 0;
				}
				
			}
			if(rotaters > 2)
			{
			modPlayer.mourningStarFire++;
			
			}
			if(modPlayer.mourningStarFire > 30)
			{
			modPlayer.mourningStarFire = 0;	
			}
			
			Vector2 vector14;
					
						if (owner.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
			
			
    //Making player variable "p" set as the projectile's owner
	projectile.rotation += 0.25f;
    //Factors for calculations
    double deg = (double) projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
    double rad = deg * (Math.PI / 180); //Convert degrees to radians
    double dist = 128; 
	
	if(owner.controlDown && down <= 9 && down >= 1) 
			  {
				  if(swap == false)
				  {
					swap = true;
				  }
				  else
				  {
					  swap = false;
				  }
				  down = 0;
              }
			  if(down > 0)
			  {
			  down--;
			  }
			if(owner.controlDown) 
			  {
				  down = 10;
			  }
			  
	if(swap == false)
	{
    traveltoX = owner.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
    traveltoY = owner.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
	}
	else
	{
	traveltoX = vector14.X;
	traveltoY = vector14.Y;
		
	}
	
    projectile.ai[1] += 0.8f;

	
	
		if(projectile.Center.X < traveltoX)
			{
				if(projectile.velocity.X < -2)
				{
				projectile.velocity.X = -2;
				}
				if(projectile.velocity.X < 15)
				{
				projectile.velocity.X += .15f;
				}
			}
			if(projectile.Center.X > traveltoX)
			{
				if(projectile.velocity.X > 2)
				{
				projectile.velocity.X = 2;
				}
				if(projectile.velocity.X > -15)
				{
				projectile.velocity.X -= .15f;
				}
			}
			if(projectile.Center.Y < traveltoY)
			{
				if(projectile.velocity.Y < -2)
				{
				projectile.velocity.Y = -2;
				}
				if(projectile.velocity.Y < 15)
				{
				projectile.velocity.Y += .15f;
				}
			}
			if(projectile.Center.Y > traveltoY)
			{
				if(projectile.velocity.Y > 2)
				{
				projectile.velocity.Y = 2;
				}
				if(projectile.velocity.Y > -15)
				{
				projectile.velocity.Y -= .15f;
				}
				
			}	
			for(int i = 0; i < 1000; i++)
				{
					Projectile reflectProjectile = Main.projectile[i];
					
						float dX = reflectProjectile.Center.X - projectile.Center.X;
						float dY = reflectProjectile.Center.Y - projectile.Center.Y;
						float distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
					if(distance < 24f && reflectProjectile.hostile && !reflectProjectile.friendly)
					{
						reflectProjectile.friendly = true;
						reflectProjectile.hostile = false;
						reflectProjectile.velocity.X *= -1;
						reflectProjectile.velocity.Y *= -1;
					}
				}
	
	
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				Main.PlaySound(SoundID.Item10, projectile.position);
				return false;
		}
		public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/MourningStarChain2");    //this where the chain of grappling hook is drawn
                                                      //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
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
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
        }
	}
	public class MourningStarRotater : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mourning Star");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22; 
            projectile.timeLeft = 361;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = false; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void AI()
		{
			Player owner  = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)owner.GetModPlayer(mod, "SOTSPlayer");	
			
			
			
			projectile.damage = 10;
		
			if(SOTSWorld.legendLevel == 1)
			{
				projectile.damage = 12;
			}
			if(SOTSWorld.legendLevel == 2)
			{
				projectile.damage = 15;
			}
			if(SOTSWorld.legendLevel == 3)
			{
				projectile.damage = 19;
			}
			if(SOTSWorld.legendLevel == 4)
			{
				projectile.damage = 24;
			}
			if(SOTSWorld.legendLevel == 5)
			{
				projectile.damage = 30;
			}
			if(SOTSWorld.legendLevel == 6)
			{
				projectile.damage = 32;
			}
			if(SOTSWorld.legendLevel == 7)
			{
				projectile.damage = 35;
			}
			if(SOTSWorld.legendLevel == 8)
			{
				projectile.damage = 38;
			}
			if(SOTSWorld.legendLevel == 9)
			{
				projectile.damage = 40;
			}
			if(SOTSWorld.legendLevel == 10)
			{
				projectile.damage = 42;
			}
			if(SOTSWorld.legendLevel == 11)
			{
				projectile.damage = 45;
			}
			if(SOTSWorld.legendLevel == 12)
			{
				projectile.damage = 50;
			}
			if(SOTSWorld.legendLevel == 13)
			{
				projectile.damage = 55;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				projectile.damage = 60;
			}
			if(SOTSWorld.legendLevel == 15)
			{
				projectile.damage = 63;
			}
			if(SOTSWorld.legendLevel == 16)
			{
				projectile.damage = 67;
			}
			if(SOTSWorld.legendLevel == 17)
			{
				projectile.damage = 71;
			}
			if(SOTSWorld.legendLevel == 18)
			{
				projectile.damage = 76;
			}
			if(SOTSWorld.legendLevel == 19)
			{
				projectile.damage = 80;
			}
			if(SOTSWorld.legendLevel == 20)
			{
				projectile.damage = 85;
			}
			if(SOTSWorld.legendLevel == 21)
			{
				projectile.damage = 90;
			}
			if(SOTSWorld.legendLevel == 22)
			{
				projectile.damage = 100;
			}
			if(SOTSWorld.legendLevel == 23)
			{
				projectile.damage = 112;
			}
			if(SOTSWorld.legendLevel == 24)
			{
				projectile.damage = 125;
			}
			if(SOTSWorld.legendLevel == 25)
			{
				projectile.damage = 150;
			}
			
			
			projectile.damage = (int)(projectile.damage * 0.25);
			
			
			
			
			for(int i = 0; i < 1000; i++)
				{
					Projectile reflectProjectile = Main.projectile[i];
					
						float dX = reflectProjectile.Center.X - projectile.Center.X;
						float dY = reflectProjectile.Center.Y - projectile.Center.Y;
						float distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
					if(distance < 24f && reflectProjectile.hostile && !reflectProjectile.friendly)
					{
						reflectProjectile.friendly = true;
						reflectProjectile.hostile = false;
						reflectProjectile.velocity.X *= -1;
						reflectProjectile.velocity.Y *= -1;
					}
				}
				
			double deg = (double) projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
			double rad = deg * (Math.PI / 180); //Convert degrees to radians
			double dist = 96; 

			projectile.position.X = modPlayer.starCen.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
			projectile.position.Y = modPlayer.starCen.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
			projectile.ai[1] += 1f;
			
			 for(int i = 0; i < 200; i++)
				{
					
				   NPC target = Main.npc[i];

					
				   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
				   float shootToY = target.position.Y + (float)target.height * 0.5f - projectile.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				   //If the distance between the projectile and the live target is active
				   if(distance < 640f && !target.friendly && target.active)
				   {
					   if(modPlayer.mourningStarFire >= 30) //Assuming you are already incrementing this in AI outside of for loop
					   {
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance = 3f / distance;
			   
						   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
			   
						   //Shoot projectile and set ai back to 0
						   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("MourningStarBeam"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
						  
					   }
				   }
				}
		}
		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
			Player owner  = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)owner.GetModPlayer(mod, "SOTSPlayer");	
			
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/MourningStarRotaterChain");    //this where the chain of grappling hook is drawn
                                                      //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
            Vector2 position = projectile.Center;
            Vector2 mountedCenter = modPlayer.starCen;
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
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
			return true;
        }
		
		
	}
}		