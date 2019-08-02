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
namespace SOTS.Projectiles.Legendary
{    
    public class PhantomApparition : ModProjectile 
    {
		int lifeTime = 0;
		int staticLifeTime = 0;
		float transmogSpeed;
		float transmogLevel = 10;
		float transmogPhase = 1;
		bool grow = true;
		float oldPositionY;
		float oldPositionX;
		float oldPlayerPositionY;
		float oldPlayerPositionX;
		double startingRotation;
		float rotationAreaX;
		float rotationAreaY;
		float rotationAreaEnemyX = -1;
		float rotationAreaEnemyY = -1;
		float enemyPosX = -1;
		float enemyPosY = -1;
		float goToCursorX;
		float goToCursorY;
		float cursorDistance;
		float eternalBetweenPlayer;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Apparition");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 38;
			projectile.width = 38;
			projectile.minion = true;
			projectile.penetrate = -1;
			projectile.friendly = true;
            Main.projFrames[projectile.type] = 5;
			projectile.timeLeft = 10000;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		public override bool PreAI()
        {
			transmogSpeed = 0.3f;
			
			if(SOTSWorld.legendLevel == 1)
			transmogSpeed = 0.4f;
			
			if(SOTSWorld.legendLevel == 2)
			transmogSpeed = 0.5f;
		
			if(SOTSWorld.legendLevel == 3)
			transmogSpeed = 0.55f;
		
			if(SOTSWorld.legendLevel == 4)
			transmogSpeed = 0.6f;
		
			if(SOTSWorld.legendLevel == 5)
			transmogSpeed = 0.66f;
		
			if(SOTSWorld.legendLevel == 6)
			transmogSpeed = 0.7f;
			
			if(SOTSWorld.legendLevel == 7)
			transmogSpeed = 0.77f;
			
			if(SOTSWorld.legendLevel == 8)
			transmogSpeed = 0.8f;
		
			if(SOTSWorld.legendLevel == 9)
			transmogSpeed = 0.88f;
		
			if(SOTSWorld.legendLevel == 10)
			transmogSpeed = 0.9f;
		
			if(SOTSWorld.legendLevel == 11)
			transmogSpeed = 1f;
		
			if(SOTSWorld.legendLevel == 12)
			transmogSpeed = 1.2f;
			
			if(SOTSWorld.legendLevel == 13)
			transmogSpeed = 1.4f;
			
			if(SOTSWorld.legendLevel == 14)
			transmogSpeed = 1.6f;
		
			if(SOTSWorld.legendLevel == 15)
			transmogSpeed = 1.8f;
		
			if(SOTSWorld.legendLevel == 16)
			transmogSpeed = 2f;
		
			if(SOTSWorld.legendLevel == 17)
			transmogSpeed = 2.33f;
		
			if(SOTSWorld.legendLevel == 18)
			transmogSpeed = 2.66f;
			
			if(SOTSWorld.legendLevel == 19)
			transmogSpeed = 3f;
			
			if(SOTSWorld.legendLevel == 20)
			transmogSpeed = 3.22f;
		
			if(SOTSWorld.legendLevel == 21)
			transmogSpeed = 3.44f;
		
			if(SOTSWorld.legendLevel == 22)
			transmogSpeed = 3.66f;
		
			if(SOTSWorld.legendLevel == 23)
			transmogSpeed = 4f;
		
			if(SOTSWorld.legendLevel == 24)
			transmogSpeed = 4.5f;
		
			if(SOTSWorld.legendLevel == 25)
			transmogSpeed = 5f;
		
			if(projectile.active)
			return true;
		
			return false;
		}
		public override void AI()
		{
			
			Player player  = Main.player[projectile.owner];
			
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
				
				
			if((player.channel || projectile.knockBack <= -5) && grow)
			{
				projectile.friendly = true;
				eternalBetweenPlayer = betweenPlayer;
				if(eternalBetweenPlayer < 210)
				{
					eternalBetweenPlayer = 210;
				}
				oldPlayerPositionX = player.Center.X;
				oldPlayerPositionY = player.Center.Y;
				oldPositionX = projectile.Center.X;
				oldPositionY = projectile.Center.Y;
				
				startingRotation = Math.Atan2((double)-betweenPlayerY, (double)-betweenPlayerX);
				startingRotation *= 180/Math.PI;
				
				projectile.timeLeft = 40000;
				transmogLevel += transmogSpeed;
				
				for(int i = 0; i < 360; i += 30)
				{
					Vector2 circularLocation = new Vector2(-transmogLevel * 0.25f, 0).RotatedBy(MathHelper.ToRadians(i));
					
					int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 57);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.1f;
				}
				lifeTime = (int)(transmogLevel * 15) + 300;
				staticLifeTime = 2100;
				if(lifeTime > staticLifeTime)
				{
				lifeTime = staticLifeTime;
				}
				
				if(projectile.knockBack == -20)
				{
					lifeTime = 1000;
					transmogPhase = 1;
					eternalBetweenPlayer = Main.rand.Next(210, 401);
					projectile.knockBack = -10;
				}
				if(projectile.knockBack == -19)
				{
					lifeTime = 1000;
					transmogPhase = 2;
					projectile.frame = (projectile.frame + 1) % 5;
					eternalBetweenPlayer = Main.rand.Next(210, 401);
					projectile.knockBack = -10;
				}
				if(projectile.knockBack == -18)
				{
					lifeTime = 1000;
					transmogPhase = 3;
					projectile.frame = (projectile.frame + 2) % 5;
					eternalBetweenPlayer = Main.rand.Next(210, 401);
					projectile.knockBack = -10;
				}
				if(projectile.knockBack == -17)
				{
					lifeTime = 1000;
					transmogPhase = 4;
					projectile.frame = (projectile.frame + 3) % 5;
					eternalBetweenPlayer = Main.rand.Next(210, 401);
					projectile.knockBack = -10;
				}
				
				if(transmogLevel >= 120)
				{
					transmogLevel = 10;
					projectile.frame = (projectile.frame + 1) % 5;
					transmogPhase++;
				}
				if(transmogPhase == 5)
				{
					lifeTime = 2100;
					grow = false;
				}
				if(projectile.knockBack < 0)
				{
				projectile.knockBack++;
					if(projectile.knockBack == -5 || projectile.knockBack == -6)
					{
						projectile.knockBack = 2;
						grow = false;
					}
				}
			}
			else
			{
				projectile.friendly = false;
				grow = false;
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

				float cursorDistance2 = 1.75f / cursorDistance;
			   
				goToCursorX *= cursorDistance2 * 5;
				goToCursorY *= cursorDistance2 * 5;
						   
				double startingDirection = Math.Atan2((double)-goToCursorY, (double)-goToCursorX);
				startingDirection *= 180/Math.PI;
				
				
				
				
				
				float minDist = 400;
				int target2 = -1;
				float dX = 0f;
				float dY = 0f;
				float distanceEnemy = 0;
				Vector2 towardsEnemy = new Vector2(0, 0);
				float speed = 6.75f;
				
					for(int j = 0; j < Main.npc.Length - 1; j++)
					{
						NPC target = Main.npc[j];
						if(!target.friendly && target.dontTakeDamage == false && target.active)
							{
								dX = target.Center.X - projectile.Center.X;
								dY = target.Center.Y - projectile.Center.Y;
								distanceEnemy = (float) Math.Sqrt((double)(dX * dX + dY * dY));
								if(distanceEnemy < minDist)
									{
										minDist = distanceEnemy;
										target2 = j;
									}
								}
							}
											
					if(target2 != -1)
					{
						NPC toHit = Main.npc[target2];
						if(toHit.active == true)
						{
													
							dX = toHit.Center.X - projectile.Center.X;
							dY = toHit.position.Y - projectile.Center.Y;
							distanceEnemy = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							speed /= distanceEnemy;
											   
							towardsEnemy = new Vector2(dX * speed, dY * speed);
							projectile.tileCollide = false;
						}
					}
						   
				double turnTowardsEnemy = Math.Atan2((double)-dY, (double)-dX);
				turnTowardsEnemy *= 180/Math.PI;
				
				rotationAreaEnemyX = -1;
				rotationAreaEnemyY = -1;
				enemyPosY = -1;
				enemyPosX = -1;
				
				if(target2 != -1)
				{
				NPC toHit = Main.npc[target2];
				
					if(toHit.active)
					{
						double deg2 = (double)startingRotation; 
						double rad2 = deg2 * (Math.PI / 180);
						double dist2 = 240;
						rotationAreaEnemyX = toHit.Center.X - (int)(Math.Cos(rad2) * dist2);
						rotationAreaEnemyY = toHit.Center.Y - (int)(Math.Sin(rad2) * dist2);
						enemyPosY = toHit.Center.Y;
						enemyPosX = toHit.Center.X;
					}
				}
				
				
				
				
				if(transmogPhase == 1 || transmogPhase == 2)
				{
				projectile.ai[1] = (float)startingDirection;
			
				projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225);
				}
				
				if(transmogPhase == 3)
				{
				projectile.ai[1] = (float)turnTowardsEnemy;
							
							
				projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 230);
				}
				else if(transmogPhase == 4)
				{
					
				projectile.ai[1] = (float)turnTowardsEnemy;
							
							
				projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225);
				}
				
				
				
				
				
				
				
				if(transmogPhase == 1)
				{
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
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("PhantomExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
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
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("PhantomExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
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
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("PhantomExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
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
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("PhantomExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item1, (int)(projectile.Center.X), (int)(projectile.Center.Y));
						}
					}
					if(projectile.ai[0] >= 200)
					{
						projectile.ai[0] = -30;
					}
				}
			
				if(transmogPhase == 2)
				{
					startingRotation -= 1.8f;
					if(projectile.ai[0] <= 45 )
					{
						float shootToX = rotationAreaX - projectile.Center.X;
						float shootToY = rotationAreaY - projectile.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					   
						distance = .65f / distance;
				   
						shootToX *= distance * 5;
						shootToY *= distance * 5;
					   
						projectile.velocity.X = shootToX;
						projectile.velocity.Y = shootToY; 
					}
					if(projectile.ai[0] == 46 && cursorDistance > 180f)
					{
						projectile.velocity.X = goToCursorX;
						projectile.velocity.Y = goToCursorY;
						projectile.ai[0] = 45;			   
					}
					if(projectile.ai[0] >= 47)
					{
						projectile.velocity.X *= 0.1f;
						projectile.velocity.Y *= 0.1f;
						if(projectile.ai[0] >= 60)
						{
						projectile.ai[0] = 10;
							if(projectile.owner == Main.myPlayer)
							{
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, goToCursorX * 1.5f, goToCursorY * 1.5f, mod.ProjectileType("PhantomArrow"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item5, (int)(projectile.Center.X), (int)(projectile.Center.Y));
							
							}
						}
					}
				}
				if(transmogPhase == 3)
				{
					startingRotation += 1.2f;
					if(rotationAreaEnemyY == -1 || rotationAreaEnemyX == -1)
					{
						float shootToX = rotationAreaX - projectile.Center.X;
						float shootToY = rotationAreaY - projectile.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					   
						distance = .95f / distance;
				   
						shootToX *= distance * 5;
						shootToY *= distance * 5;
					   
						projectile.velocity.X = shootToX;
						projectile.velocity.Y = shootToY; 
						
						projectile.ai[1] = (float)startingDirection;
				
						projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225);
					}
					else
					{
						if(projectile.ai[0] >= 12)
						{
							if(projectile.owner == Main.myPlayer)
							{
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, towardsEnemy.X * 3.25f, towardsEnemy.Y * 3.25f, mod.ProjectileType("PhantomPellet"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item11, (int)(projectile.Center.X), (int)(projectile.Center.Y));
							}
							projectile.ai[0] = 0;			   
						}
						
						float shootToX = rotationAreaEnemyX - projectile.Center.X;
						float shootToY = rotationAreaEnemyY - projectile.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					   
						distance = .55f / distance;
				   
						shootToX *= distance * 5;
						shootToY *= distance * 5;
					   
						projectile.velocity.X = shootToX;
						projectile.velocity.Y = shootToY; 
					}
				}
				if(transmogPhase == 4)
				{
					startingRotation -= 0.75f;
					if(rotationAreaEnemyY == -1 || rotationAreaEnemyX == -1)
					{
						float shootToX = rotationAreaX - projectile.Center.X;
						float shootToY = rotationAreaY - projectile.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					   
						distance = 0.85f / distance;
				   
						shootToX *= distance * 5;
						shootToY *= distance * 5;
					   
						projectile.velocity.X = shootToX;
						projectile.velocity.Y = shootToY; 
						
						projectile.ai[1] = (float)startingDirection;
				
						projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225);
					}
					else
					{
						if(projectile.ai[0] >= 10)
						{
							if(projectile.owner == Main.myPlayer)
							{
							Projectile.NewProjectile(enemyPosX, enemyPosY, 0, 0, mod.ProjectileType("PhantomExplosion"), projectile.damage, -1, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item13, (int)(projectile.Center.X), (int)(projectile.Center.Y));
							}
							projectile.ai[0] = 0;			   
						}
						
						float shootToX = rotationAreaEnemyX - projectile.Center.X;
						float shootToY = rotationAreaEnemyY - projectile.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					   
						distance = .75f / distance;
				   
						shootToX *= distance * 5;
						shootToY *= distance * 5;
					   
						projectile.velocity.X = shootToX;
						projectile.velocity.Y = shootToY; 
					}
				}
				if(transmogPhase == 5)
				{
					startingRotation -= 1f;
					
						float shootToX = rotationAreaX - projectile.Center.X;
						float shootToY = rotationAreaY - projectile.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					   
						distance = .75f / distance;
				   
						shootToX *= distance * 5;
						shootToY *= distance * 5;
					   
						projectile.velocity.X = shootToX;
						projectile.velocity.Y = shootToY; 
						
					if(projectile.ai[0] >= 480)
					{
							if(projectile.owner == Main.myPlayer)
							{
								Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("PhantomApparition"), projectile.damage, -Main.rand.Next(17,21), projectile.owner, 0f, 0f);
								Main.PlaySound(SoundID.Item44, (int)(projectile.Center.X), (int)(projectile.Center.Y));
							}	
							projectile.ai[0] = 0;							
					}
				}
			}
			
			
				projectile.alpha = (int)(-255 * (float)((float)lifeTime / (float)staticLifeTime)) + 190;
				if(lifeTime < 65)
				{
				projectile.alpha = 255 - lifeTime; 	
				}
				lifeTime--;
				if(lifeTime <= 0)
				{
					projectile.Kill();
				}
				
			
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.immune[projectile.owner] = 15;
        }
	}
}
		