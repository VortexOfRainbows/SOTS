using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Star
{    
    public class GrenadierBolt : ModProjectile 
    {	int wait = 0;
		int level = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GrenadierBolt");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(24);
            aiType = 24; //18 is the demon scythe style
            projectile.width = 14;
            projectile.height = 20; 
			projectile.friendly = true;
            projectile.hostile = false; 
			projectile.timeLeft = 260;
			projectile.alpha = 0;
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
				for(int i = 0; i < 3; i++)
				{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 12, 12, 235);

			
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				}
			projectile.timeLeft -= Main.rand.Next(7);
			}	
			return true;
		}
		public override void AI()
        {
			if(projectile.knockBack <= 0.999)
			{
			projectile.friendly = false;
			projectile.timeLeft -= Main.rand.Next(7);
			}
			wait++;
			if(wait < 0)
			{
				for(int i = 0; i < 3; i++)
					{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 12, 12, 235);

				
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.1f;
					}
			}
			
		}
		public override void Kill(int timeLeft)
        {
			Player owner = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)owner.GetModPlayer(mod, "SOTSPlayer");
		
			
			
			 projectile.ai[1] = 0;
						
						 for(int i = 0; i < 200; i++)
							{
							   //Enemy NPC variable being set
							   NPC target = Main.npc[i];

							   //Getting the shooting trajectory
							   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
							   float shootToY = target.position.Y + (float)target.height * 0.5f - projectile.Center.Y;
							   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

							   //If the distance between the projectile and the live target is active
							   if(distance < 360f && !target.friendly && target.active)
							   {
								   if(projectile.ai[1] <= 3) //Assuming you are already incrementing this in AI outside of for loop
								   {
									   //Dividing the factor of 3f which is the desired velocity by distance
									   distance = 1.8f / distance;
						   
									   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
									   shootToX *= distance * 5;
									   shootToY *= distance * 5;
						   
									   //Shoot projectile and set ai back to 0
									   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("InsigniusBolt"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
									  projectile.ai[1]++;
								   }
							   }
							}
							if(projectile.knockBack > 0.99f)
							{
							if(modPlayer.megHat == true)
							{
								Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 1.15f, Main.rand.Next(-9,10) * 1.15f, mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - (float)(1f * 0.5f), Main.myPlayer, 0f, 0f); //Spawning a projectile
							}
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 1.15f, Main.rand.Next(-9,10) * 1.15f, mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 1.15f, Main.rand.Next(-9,10) * 1.15f, mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
							}
							else if(projectile.knockBack > 0f)
							{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-2,10) * 1.15f * -(Math.Abs(projectile.velocity.X)/projectile.velocity.X), Main.rand.Next(-2,10) * 1.15f * -(Math.Abs(projectile.velocity.Y)/projectile.velocity.Y), mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
			if(projectile.knockBack * Main.rand.Next(100) > 25)
			{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-2,10) * 1.15f * -(Math.Abs(projectile.velocity.X)/projectile.velocity.X), Main.rand.Next(-2,10) * 1.15f * -(Math.Abs(projectile.velocity.Y)/projectile.velocity.Y), mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
			}
			else if(projectile.knockBack * Main.rand.Next(100) > 10 && modPlayer.megHat == true)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 1.15f, Main.rand.Next(-9,10) * 1.15f, mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.21f, Main.myPlayer, 0f, 0f); //Spawning a projectile
							
			}
							}
							
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 4;     //this is the explosion radius, the highter is the value the bigger is the explosion
 
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + position.X);
                    int yPosition = (int)(y + position.Y);
 
                    if (Math.Sqrt(x * x + y * y) <= radius + 0.5)   //this make so the explosion radius is a circle
                    {
								Dust.NewDust(new Vector2(xPosition, yPosition), 1, 1, 235);
					}
                }
            }
			
        }
	}
}
			