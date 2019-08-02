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
 
namespace SOTS.Projectiles.Margrit       //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
 
{
    public class PlanetaryLatch : ModProjectile
    {	
		bool latch = false;
		float traveltoX = 0;
		float traveltoY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Latch");
			
		}
        public override void SetDefaults()
        {
 
            projectile.width = 30; 
            projectile.height = 30;  
            projectile.hostile = false; 
            projectile.friendly = true;   
            projectile.ignoreWater = true;    
            Main.projFrames[projectile.type] = 1;  
            projectile.timeLeft = 7230; 
            projectile.penetrate = -1;
            projectile.tileCollide = false; 
            projectile.sentry = true; 
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override void AI()
        {
		Player player = Main.player[projectile.owner];
					
					
			projectile.rotation += 0.25f;
			double deg = (double) projectile.ai[1]; 
			double rad = deg * (Math.PI / 180);
			double dist = 84;
			traveltoX = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
			traveltoY = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
			projectile.ai[1] += 1.25f;
					
					float minDist = 128;
					int target2 = -1;
					float dX = 0f;
					float dY = 0f;
					float distance = 0;
					float speed = (float) Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
					if(projectile.friendly == true && projectile.hostile == false && player == Main.player[projectile.owner])
					{
						for(int i = 0; i < Main.npc.Length - 1; i++)
						{
							NPC target = Main.npc[i];
							if(!target.friendly && target.dontTakeDamage == false)
							{
								dX = target.Center.X - projectile.Center.X;
								dY = target.Center.Y - projectile.Center.Y;
								distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
								if(distance < minDist)
								{
									minDist = distance;
									target2 = i;
								}
							}
						}
						
						if(target2 != -1)
						{
						NPC toHit = Main.npc[target2];
							if(toHit.active == true)
							{
								
							traveltoX = toHit.Center.X;
							traveltoY = toHit.Center.Y;
							}
						}
						
						
						
					}
						
						
					
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
			
        }
		public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Margrit/PlanetaryChain");    //this where the chain of grappling hook is drawn
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
}