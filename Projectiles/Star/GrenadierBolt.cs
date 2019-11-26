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
		
			
			if(projectile.owner == Main.myPlayer)
			{
				projectile.ai[1] = 0;
				for(int i = 0; i < 200; i++)
				{
				   NPC target = Main.npc[i];

				   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
				   float shootToY = target.position.Y + (float)target.height * 0.5f - projectile.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				   if(distance < 270f && !target.friendly && target.active)
				   {
					   if(projectile.ai[1] <= 3)
					   {
						   distance = 1.6f / distance;
				
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
				
						  Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("InsigniusBolt"), projectile.damage, 0, Main.myPlayer, 0f, 0f); 
						  projectile.ai[1]++;
					   }
				   }
				}
				if(projectile.knockBack > 0.99f)
				{
					if(modPlayer.megHat == true)
					{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 1.05f, Main.rand.Next(-9,10) * 1.05f, mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - (float)(1f * 0.5f), Main.myPlayer, 0f, 0f); //Spawning a projectile
					}
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 1.05f, Main.rand.Next(-9,10) * 1.05f, mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 1.05f, Main.rand.Next(-9,10) * 1.05f, mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
				}
				else if(projectile.knockBack > 0f)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-2,10) * 1.05f * -(Math.Abs(projectile.velocity.X)/projectile.velocity.X), Main.rand.Next(-2,10) * 1.15f * -(Math.Abs(projectile.velocity.Y)/projectile.velocity.Y), mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
					if(projectile.knockBack * Main.rand.Next(100) > 21)
					{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-2,10) * 1.05f * -(Math.Abs(projectile.velocity.X)/projectile.velocity.X), Main.rand.Next(-2,10) * 1.15f * -(Math.Abs(projectile.velocity.Y)/projectile.velocity.Y), mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.5f, Main.myPlayer, 0f, 0f); //Spawning a projectile
					}
					else if(projectile.knockBack * Main.rand.Next(100) > 12 && modPlayer.megHat == true)
					{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 1.05f, Main.rand.Next(-9,10) * 1.05f, mod.ProjectileType("GrenadierBolt"), projectile.damage, projectile.knockBack - 0.21f, Main.myPlayer, 0f, 0f); //Spawning a projectile					
					}
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
								Dust.NewDust(new Vector2(xPosition, yPosition), 1, 1, 235);
					}
                }
            }
        }
	}
}
			