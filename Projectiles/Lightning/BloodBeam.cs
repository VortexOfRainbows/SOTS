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
    public class BloodBeam : ModProjectile 
    {	int scale = 1;
		float chargeCurrent;
		float initialDamage;
		float increaseDamage;
		bool initiate = true;
		bool initiateWhenFull;
		float reticleX = -1;
		float reticleY = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Rail");
			
		}
		
        public override void SetDefaults()
        {
			projectile.penetrate = -1; 
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.timeLeft = 3000;
			projectile.width = 16;
			projectile.height = 16;
			projectile.alpha = 255;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			
			Player player = Main.player[projectile.owner];
			
						Vector2 cursorArea;
						
						if (player.gravDir == 1f)
						{
						cursorArea.Y = (float)Main.mouseY + Main.screenPosition.Y;
						}
						else
						{
						cursorArea.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
						}
							cursorArea.X = (float)Main.mouseX + Main.screenPosition.X;
							
			if(initiate)
			initialDamage = projectile.damage;
		
			initiate = false;
			if(player.FindBuffIndex(mod.BuffType("PulverizerCharge")) > -1 && !initiateWhenFull)
			{
				projectile.position.X = player.Center.X - projectile.width/2;
				projectile.position.Y = player.Center.Y - projectile.height/2;
				if(chargeCurrent <= 240)
				{
					increaseDamage = (float)((5f/240f * chargeCurrent) + .5f);
					scale = 100 + (int)(1400f/240f * chargeCurrent);
				}
				else
				{
					chargeCurrent = 240;
					increaseDamage = (float)((5f/240f * chargeCurrent) + .5f);
					scale = 100 + (int)(1400f/240f * chargeCurrent);
					initiateWhenFull = true;
				}
				chargeCurrent += (float)(((SOTSWorld.legendLevel * 1.75f) + 2f) * 0.6f);
				projectile.damage = (int)(initialDamage * increaseDamage);
				
				
				for(int i = 0; i < 360; i += 30)
				{
				Vector2 circularLocation = new Vector2(-scale * 0.05f, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(player.Center.X + circularLocation.X - 4, player.Center.Y + circularLocation.Y - 4), 4, 4, 235);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				}
				
							
							float shootToX = cursorArea.X - player.Center.X;
							float shootToY = cursorArea.Y - player.Center.Y;
							Vector2 location2 = new Vector2(-scale, 0).RotatedBy(MathHelper.ToRadians((float)Math.Atan2(-(double)shootToY, -(double)shootToX)) * 180/Math.PI);
							reticleX = player.Center.X + location2.X;
							reticleY = player.Center.Y + location2.Y;
					for(int i = 0; i < 360; i += 60)
						{
							Vector2 circularLocation3 = new Vector2(-8, 0).RotatedBy(MathHelper.ToRadians(i));
							int num1 = Dust.NewDust(new Vector2(reticleX + circularLocation3.X - 4, reticleY + circularLocation3.Y - 4), 4, 4, 235);
							Main.dust[num1].noGravity = true;
							Main.dust[num1].velocity *= 0.1f;
						}
							
			}
			else
            {
				Main.PlaySound(SoundID.Item125, (int)(player.Center.X), (int)(player.Center.Y));
				if(projectile.owner == Main.myPlayer)
				{
				projectile.alpha = 255;	
					bool activeDamageBox = true;
					for(int i = projectile.timeLeft; i > 0; i--)
					{
						if(i % 30 == 0)
						{
							activeDamageBox = true;
						}
						float shootToX = cursorArea.X - player.Center.X;
						float shootToY = cursorArea.Y - player.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
						
						
						float distancePlayerX = player.Center.X - projectile.Center.X;
						float distancePlayerY = player.Center.Y - projectile.Center.Y;
						float distancePlayer = (float)System.Math.Sqrt((double)(distancePlayerX * distancePlayerX + distancePlayerY * distancePlayerY));
						
						
						for(int j = 0; j < 3; j++)
						{
							int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 16, 16, 235);
						
							Main.dust[num1].noGravity = true;
							Main.dust[num1].velocity *= 0.1f;
						}
						if(distancePlayer >= 100 + (int)(1400f/240f * chargeCurrent))
						{
							break;
						}
							
						
						distance = 2f / distance;
						shootToX *= distance * 2.5f;
						shootToY *= distance * 2.5f;
						
						projectile.position.X += shootToX;
						projectile.position.Y += shootToY;
					
						for(int k = 0; k < 200; k++)
						{
							NPC npc = Main.npc[k];
							float distanceXNPC = npc.Center.X - projectile.Center.X;
							float distanceYNPC = npc.Center.Y - projectile.Center.Y;
							float distanceBetweenNPC = (float)System.Math.Sqrt((double)(distanceXNPC * distanceXNPC + distanceYNPC * distanceYNPC));
							if(distanceBetweenNPC < (float)(npc.width * .6f) + 48 && npc.active && activeDamageBox && !npc.friendly) //making sure enemy is within range
							{
								Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("RedExplosion"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
								activeDamageBox = false;
							}
						}
					}
				}
				projectile.Kill();
			}
		}
		public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
			if(reticleX != -1 && reticleY != -1)
			{
				Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/LaserTargeting");
				//Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/PinkyHook_Chain");    //this where the chain of grappling hook is drawn
														  //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
				Vector2 position = new Vector2(reticleX, reticleY);
				Vector2 mountedCenter = Main.player[projectile.owner].Center;
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
	}
}
		