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
    public class PinkLightning : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink Lightning");
			
		}
		
        public override void SetDefaults()
        {
			projectile.penetrate = 1; 
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 330;
			projectile.width = 8;
			projectile.height = 8;
			projectile.alpha = 255;
			projectile.netImportant = true;
		}
		public override void AI()
		{
			projectile.alpha = 255;
			projectile.knockBack -= 0.01f;
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 134);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			int num2 = Dust.NewDust(new Vector2(projectile.Center.X + 7, projectile.Center.Y - 1), 2, 2, 255);
			Main.dust[num2].noGravity = true;
			Main.dust[num2].velocity *= 0.1f;
			
			int num3 = Dust.NewDust(new Vector2(projectile.Center.X - 7, projectile.Center.Y - 1), 2, 2, 255);
			Main.dust[num3].noGravity = true;
			Main.dust[num3].velocity *= 0.1f;
			
			int num4 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y + 7), 2, 2, 255);
			Main.dust[num4].noGravity = true;
			Main.dust[num4].velocity *= 0.1f;
			
			int num5 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 7), 2, 2, 255);
			Main.dust[num5].noGravity = true;
			Main.dust[num5].velocity *= 0.1f;
			
			if(Main.myPlayer == projectile.owner)
			{
				projectile.netUpdate = true;
				projectile.velocity.Y += (0.4f * (Main.rand.Next(-3, 4)));
				
				projectile.velocity.X += (0.4f * (Main.rand.Next(-3, 4)));
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 35; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 255);
			Main.dust[num1].noGravity = true;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			bool stopCheck = false;
			int startingChain = -1;
			for(int i = 0; i < 200; i++)
			{
				NPC targetCheck = Main.npc[i];
				if(targetCheck == target)
				{
					startingChain = i;
				}
			}
			for(int i = startingChain; i < 200; i++)
			{
				if(Main.npc[i] != target && projectile.knockBack > 0 && !stopCheck)
				{
					NPC target2 = Main.npc[i];
					
					float shootFromX = target.Center.X;
					float shootFromY = target.Center.Y;
					
					if(target2.Center.X >= target.Center.X)
					shootFromX += target.width;
						
					if(target2.Center.X <= target.Center.X)
					shootFromX -= target.width;
						
					if(target2.Center.Y >= target.Center.Y)
					shootFromY += target.height;
						
					if(target2.Center.Y <= target.Center.Y)
					shootFromY -= target.height;
						
					
					
					float shootToX = target2.position.X + (float)target2.width * 0.5f - shootFromX;
					float shootToY = target2.position.Y + (float)target2.height * 0.5f - shootFromY;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					float velocity = (float)System.Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
	
					
					if(distance < 300f && !target2.friendly && target2.active)
					{
							//Dividing the factor of 3f which is the desired velocity by distance
							distance = velocity / distance;
				
							//Multiplying the shoot trajectory with distance times a multiplier if you so choose to
							shootToX *= distance * 2;
							shootToY *= distance * 2;
				
							//Shoot projectile and set ai back to 0
							if(Main.myPlayer == projectile.owner)
							{
								Projectile.NewProjectile(shootFromX, shootFromY, shootToX * 0.3f, shootToY * 0.3f, projectile.type, projectile.damage, projectile.knockBack -= 0.35f, Main.myPlayer, 0f, 0f); //Spawning a projectile
							}
							stopCheck = true;
							break;
					}
				}
			}
			for(int i = 0; i < startingChain; i++)
			{
				if(Main.npc[i] != target && projectile.knockBack < 1 && projectile.knockBack > 0 && !stopCheck)
				{
					NPC target2 = Main.npc[i];
					
					float shootFromX = target.Center.X;
					float shootFromY = target.Center.Y;
					
					if(target2.Center.X >= target.Center.X)
					shootFromX += target.width;
						
					if(target2.Center.X <= target.Center.X)
					shootFromX -= target.width;
						
					if(target2.Center.Y >= target.Center.Y)
					shootFromY += target.height;
						
					if(target2.Center.Y <= target.Center.Y)
					shootFromY -= target.height;
							
					float shootToX = target2.position.X + (float)target2.width * 0.5f - shootFromX;
					float shootToY = target2.position.Y + (float)target2.height * 0.5f - shootFromY;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					float velocity = (float)System.Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
	
					
					if(distance < 300f && !target2.friendly && target2.active)
					{
							//Dividing the factor of 3f which is the desired velocity by distance
							distance = velocity / distance;
				
							//Multiplying the shoot trajectory with distance times a multiplier if you so choose to
							shootToX *= distance * 2;
							shootToY *= distance * 2;
				
							//Shoot projectile and set ai back to 0
							if(Main.myPlayer == projectile.owner)
							{
								Projectile.NewProjectile(shootFromX, shootFromY, shootToX * 0.3f, shootToY * 0.3f, projectile.type, projectile.damage, projectile.knockBack -= 0.35f, Main.myPlayer, 0f, 0f); //Spawning a projectile
							}
							stopCheck = true;
							break;
					}
				}
			}
		}
	}
}
		