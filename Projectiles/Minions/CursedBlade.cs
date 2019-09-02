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
namespace SOTS.Projectiles.Minions
{    
    public class CursedBlade : ModProjectile 
    {
		int initiate = -1;
		float oldPositionY;
		float oldPositionX;
		float oldPlayerPositionY;
		float oldPlayerPositionX;
		double startingRotation;
		float rotationAreaX;
		float rotationAreaY;
		float goToCursorX;
		float goToCursorY;
		float cursorDistance;
		float eternalBetweenPlayer = 196;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Blade");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 24;
			projectile.width = 24;
			projectile.minion = true;
			projectile.penetrate = -1;
			projectile.friendly = false;
            Main.projFrames[projectile.type] = 1;
			projectile.timeLeft = 10000;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		public override void AI()
		{
			if(initiate == -1)
			{
				startingRotation = Main.rand.Next(360);
				initiate = 1;
			}
			projectile.alpha = 0;
			Player player  = Main.player[projectile.owner];
			
				if(player.FindBuffIndex(mod.BuffType("CursedBlade")) > -1)
				{
					projectile.timeLeft = 6;
				}
			
				Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 2.4f / 255f, (255 - projectile.alpha) * 2.55f / 255f, (255 - projectile.alpha) * 1.35f / 255f);
			
				float betweenPlayerX = oldPositionX - oldPlayerPositionX;
				float betweenPlayerY = oldPositionY - oldPlayerPositionY;
				float betweenPlayer = (float)System.Math.Sqrt((double)(betweenPlayerX * betweenPlayerX + betweenPlayerY * betweenPlayerY));

				float betweenPlayer2 = 5.25f / betweenPlayer;
			   
				betweenPlayerX *= betweenPlayer2 * 5;
				betweenPlayerY *= betweenPlayer2 * 5;
						   
					
				double deg = (double)startingRotation; 
				double rad = deg * (Math.PI / 180);
				double dist = eternalBetweenPlayer;
				rotationAreaX = player.Center.X - (int)(Math.Cos(rad) * dist);
				rotationAreaY = player.Center.Y - (int)(Math.Sin(rad) * dist);
				
				
			
				projectile.friendly = false;
				projectile.ai[0]++;
				
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
				
				goToCursorX = cursorArea.X - projectile.Center.X;
				goToCursorY = cursorArea.Y - projectile.Center.Y;
				cursorDistance = (float)System.Math.Sqrt((double)(goToCursorX * goToCursorX + goToCursorY * goToCursorY));

				float cursorDistance2 = 2.45f / cursorDistance;
			   
				goToCursorX *= cursorDistance2 * 5;
				goToCursorY *= cursorDistance2 * 5;
						   
				double startingDirection = Math.Atan2((double)-goToCursorY, (double)-goToCursorX);
				startingDirection *= 180/Math.PI;
				
				
				
				projectile.ai[1] = (float)startingDirection;
			
				projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225);
			
					startingRotation += 1.2f;
					if(projectile.ai[0] <= 150 )
					{
						float shootToX = rotationAreaX - projectile.Center.X;
						float shootToY = rotationAreaY - projectile.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					   
						distance = .75f / distance;
				   
						shootToX *= distance * 5;
						shootToY *= distance * 5;
					   
						projectile.velocity.X = shootToX;
						projectile.velocity.Y = shootToY; 
					}
					if(projectile.ai[0] == 151 && cursorDistance > 90f)
					{
						projectile.velocity.X = goToCursorX;
						projectile.velocity.Y = goToCursorY;
						projectile.ai[0] = 150;			   
					}
					if(projectile.ai[0] >= 152 && projectile.ai[0] <= 155)
					{
						projectile.velocity.X *= 0.5f;
						projectile.velocity.Y *= 0.5f;
					}
					if(projectile.ai[0] >= 175 && projectile.ai[0] < 180)
					{
						projectile.velocity.X *= 0.94f;
						projectile.velocity.Y *= 0.94f;
						float newRotation = MathHelper.ToRadians((projectile.ai[0] - 174) * 60);
						newRotation -= MathHelper.ToRadians(50);
						projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225) + newRotation;
						if(projectile.owner == Main.myPlayer)
						{
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("CurseExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item1, (int)(projectile.Center.X), (int)(projectile.Center.Y));
						}
					}
					if(projectile.ai[0] >= 180 && projectile.ai[0] < 185)
					{
						projectile.velocity.X *= 1.3f;
						projectile.velocity.Y *= 1.3f;
						projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225 + Main.rand.Next(360));
						
						if(projectile.owner == Main.myPlayer)
						{
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("CurseExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item1, (int)(projectile.Center.X), (int)(projectile.Center.Y));
						}
					}
					if(projectile.ai[0] >= 185 && projectile.ai[0] < 190)
					{
						projectile.velocity.X *= 0.94f;
						projectile.velocity.Y *= 0.94f;
						projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225 + Main.rand.Next(360));
						
						if(projectile.owner == Main.myPlayer)
						{
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("CurseExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item1, (int)(projectile.Center.X), (int)(projectile.Center.Y));
						}
						
					}
					if(projectile.ai[0] >= 190 && projectile.ai[0] < 195)
					{
						projectile.velocity.X *= 1.3f;
						projectile.velocity.Y *= 1.3f;
						projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225 + Main.rand.Next(360));
						
						if(projectile.owner == Main.myPlayer)
						{
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("CurseExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item1, (int)(projectile.Center.X), (int)(projectile.Center.Y));
						}
					}
					if(projectile.ai[0] >= 200)
					{
						projectile.ai[0] = -30;
					}
				
			
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.immune[projectile.owner] = 15;
        }
	}
}
		