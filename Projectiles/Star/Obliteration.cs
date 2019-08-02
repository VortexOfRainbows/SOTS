using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Star
{    
    public class Obliteration : ModProjectile 
    {	int wait = 0;
		int level = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obliteration");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; //18 is the demon scythe style
            projectile.width = 4;
            projectile.height = 4; 
			projectile.friendly = true;
            projectile.hostile = false; 
			projectile.timeLeft = 640;
			projectile.alpha = 255;
			projectile.penetrate = 1;
		}
		public override bool PreAI()
        {
			if(projectile.knockBack >= 1)
			{
				projectile.penetrate = -1;
				return true;
			}
			else
			{
				
			projectile.timeLeft -= Main.rand.Next(15);
			}	
			return true;
		}
		public override void AI()
        {
			projectile.alpha = 255;
			if(projectile.knockBack <= 0.999 && projectile.knockBack != 0)
			{
			projectile.friendly = false;
			projectile.timeLeft -= Main.rand.Next(15);
			}
			Dust dust;
						// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
						Vector2 position = projectile.Center;
						dust = Main.dust[Terraria.Dust.NewDust(position, 4, 4, 181, 0f, 0f, 0, new Color(255,255,255), 2.039474f)];
						dust.noGravity = true;
						dust.velocity *= 0.1f;
			
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

							   if(distance < 360f && !target.friendly && target.active)
							   {
								   if(projectile.ai[1] <= 3)
								   {
									   distance = 1.8f / distance;
						   
									   shootToX *= distance * 5;
									   shootToY *= distance * 5;
						   
									   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("Obliteration2"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
									  projectile.ai[1]++;
								   }
							   }
							}
							if(projectile.knockBack < 0.99f)
							{
							if(projectile.knockBack > 0f)
								{
									if(modPlayer.megHat == true)
										{
											Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 0.30f, Main.rand.Next(-9,10) * 0.30f, mod.ProjectileType("Obliteration"), projectile.damage, projectile.knockBack - 0.07f, Main.myPlayer, 0f, 0f); //Spawning a projectile
										}
									Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-2,10) * 0.30f * -(Math.Abs(projectile.velocity.X)/projectile.velocity.X), Main.rand.Next(-2,10) * 0.30f * -(Math.Abs(projectile.velocity.Y)/projectile.velocity.Y), mod.ProjectileType("Obliteration"), projectile.damage, projectile.knockBack - 0.08f, Main.myPlayer, 0f, 0f); //Spawning a projectile
							
									if(projectile.knockBack * Main.rand.Next(100) > 35)
									{
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-2,10) * 0.30f * -(Math.Abs(projectile.velocity.X)/projectile.velocity.X), Main.rand.Next(-2,10) * 0.30f * -(Math.Abs(projectile.velocity.Y)/projectile.velocity.Y), mod.ProjectileType("Obliteration"), projectile.damage, projectile.knockBack - 0.07f, Main.myPlayer, 0f, 0f); //Spawning a projectile
									}
									
									if(projectile.knockBack * Main.rand.Next(100) > 25)
									{
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-2,10) * 0.30f * -(Math.Abs(projectile.velocity.X)/projectile.velocity.X), Main.rand.Next(-2,10) * 0.30f * -(Math.Abs(projectile.velocity.Y)/projectile.velocity.Y), mod.ProjectileType("Obliteration"), projectile.damage, projectile.knockBack - 0.06f, Main.myPlayer, 0f, 0f); //Spawning a projectile
									}
									else if(projectile.knockBack * Main.rand.Next(100) > 40 && modPlayer.megHat == true)
									{
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 0.30f, Main.rand.Next(-9,10) * 0.30f, mod.ProjectileType("Obliteration"), projectile.damage, projectile.knockBack - 0.07f, Main.myPlayer, 0f, 0f); //Spawning a projectile				
									}
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
							Dust dust;
							dust = Main.dust[Terraria.Dust.NewDust(new Vector2(xPosition, yPosition), 4, 4, 181, 0f, 0f, 0, new Color(255,255,255), 2.039474f)];
							dust.noGravity = true;
						}
					}
				}
			}
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)owner.GetModPlayer(mod, "SOTSPlayer");
			for(int i = 0; i < 2; i++)
			{
				
				if(projectile.knockBack > 0.99f)
				{
					if(projectile.owner == Main.myPlayer)
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-9,10) * 0.30f, Main.rand.Next(-9,10) * 0.30f, mod.ProjectileType("Obliteration"), projectile.damage, projectile.knockBack - 0.15f, Main.myPlayer, 0f, 0f); //Spawning a projectile
				}
			
			}
			
		}
	}
}
			