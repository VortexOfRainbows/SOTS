using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Star
{    
    public class Explosion : ModProjectile 
    {	int wait = 0;
		float level = 2f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Insignius");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; //18 is the demon scythe style
            projectile.width = 12;
            projectile.height = 12; 
            projectile.friendly = false; 
            projectile.hostile = false; 
			projectile.timeLeft = 480;
			projectile.alpha = 255;
			projectile.penetrate = 1;
		}
		public override bool PreAI()
        {
			if(projectile.knockBack >= 1)
			{
				return true;
			}
			else
			{
			projectile.friendly = false;
			projectile.tileCollide = false;
				for(int i = 0; i < 3; i++)
				{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 12, 12, 235);

			
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				}
			projectile.timeLeft -= Main.rand.Next(20);
			}	
			return false;
		}
		public override void AI()
        {
			projectile.alpha = 255;
			wait++;
			if(wait < 0)
			{
				projectile.friendly = true;
					for(int i = 0; i < 3; i++)
					{
						int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 12, 12, 235);

					
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 0.1f;
					}
			}
			if(wait >= 360)
			{
				projectile.velocity.X *= 400f;
				projectile.velocity.Y *= 400f;
				wait = -400;
			}
			
		}
		public override void Kill(int timeLeft)
        {
			Player owner = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)owner.GetModPlayer(mod, "SOTSPlayer");
			if(projectile.owner == Main.myPlayer)
			{
				 projectile.ai[1] = 0;
				 
							for(int repeatLevel = SOTSWorld.legendLevel; 0 < repeatLevel; repeatLevel--)
							{
								level += 0.5f;
							}
							
							 for(int i = 0; i < 200; i++)
								{
								   NPC target = Main.npc[i];

								   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
								   float shootToY = target.position.Y + (float)target.height * 0.5f - projectile.Center.Y;
								   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

								   if(distance < 360f && !target.friendly && target.active)
								   {
									   if(projectile.ai[1] <= 3) 
									   {
											distance = 1.8f / distance;
							   
											shootToX *= distance * 5;
											shootToY *= distance * 5;
								
											Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("InsigniusBolt"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
											projectile.ai[1]++;
									   }
								   }
								}
								if(projectile.knockBack > 0 && projectile.knockBack > 0.99f)
								{
									
									if(modPlayer.megHat == true)
									{
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 0.2f, Main.rand.Next(-9,10) * 0.2f, mod.ProjectileType("Explosion"), projectile.damage, projectile.knockBack - (float)(1f / level), Main.myPlayer, 0f, 0f); //Spawning a projectile
									}
									Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 0.2f, Main.rand.Next(-9,10) * 0.2f, mod.ProjectileType("Explosion"), projectile.damage, projectile.knockBack - (float)(1f / level), Main.myPlayer, 0f, 0f); //Spawning a projectile
									Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 0.2f, Main.rand.Next(-9,10) * 0.2f, mod.ProjectileType("Explosion"), projectile.damage, projectile.knockBack - (float)(1f / level), Main.myPlayer, 0f, 0f); //Spawning a projectile
								
								}
								else if(projectile.knockBack > 0f)
								{
									
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (Main.rand.Next(-2,10) * -projectile.velocity.X + Main.rand.Next(-11,12)) * 0.08f, (Main.rand.Next(-2,10) * -projectile.velocity.Y + Main.rand.Next(-11,12)) * 0.08f, mod.ProjectileType("Explosion"), projectile.damage, projectile.knockBack - (float)(1f / level), Main.myPlayer, 0f, 0f);
									
									if(projectile.knockBack * Main.rand.Next(100) > 35)
									{
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (Main.rand.Next(-2,10) * -projectile.velocity.X + Main.rand.Next(-11,12)) * 0.08f, (Main.rand.Next(-2,10) * -projectile.velocity.Y + Main.rand.Next(-11,12)) * 0.08f, mod.ProjectileType("Explosion"), projectile.damage, projectile.knockBack - (float)(1f / level), Main.myPlayer, 0f, 0f);
									}
									else if(projectile.knockBack * Main.rand.Next(100) > 55 && modPlayer.megHat == true)
									{
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (Main.rand.Next(-2,10) * -projectile.velocity.X + Main.rand.Next(-11,12)) * 0.08f, (Main.rand.Next(-2,10) * -projectile.velocity.Y + Main.rand.Next(-11,12)) * 0.08f, mod.ProjectileType("Explosion"), projectile.damage, projectile.knockBack - (float)(1f / level), Main.myPlayer, 0f, 0f);
									}
									
								}
								
				Vector2 position = projectile.Center;
				Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
				int radius = 4; 
	 
				for (int x = -radius; x <= radius; x++)
				{
					for (int y = -radius; y <= radius; y++)
					{
						int xPosition = (int)(x + position.X);
						int yPosition = (int)(y + position.Y);
	 
						if (Math.Sqrt(x * x + y * y) <= radius + 0.5)  
						{
									Dust.NewDust(new Vector2(xPosition, yPosition), 2, 2, 235);
						}
					}
				}
			}
		}
	}
}
			