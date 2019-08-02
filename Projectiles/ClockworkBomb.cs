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
    public class ClockworkBomb : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(274);
            aiType = 274; //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.timeLeft = 120;
			projectile.width = 42;
			projectile.height = 42;
			

		}
		public override void AI()
		{
			Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 42, 42, 206);
			Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 42, 42, 206);
			Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 42, 42, 206);
			Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 42, 42, 206);
			Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 42, 42, 206);
			Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 42, 42, 206);
			
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("ChaosWave"), 33, 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("ChaosWave"), 33, 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("ChaosWave"), 33, 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("ChaosWave"), 33, 0, 0);
				
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("ChaosWave"), 33, 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("ChaosWave"), 33, 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("ChaosWave"), 33, 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("ChaosWave"), 33, 0, 0);
			
}
public override void Kill(int timeLeft)
        {
 
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 1000;     //this is the explosion radius, the highter is the value the bigger is the explosion
 
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);
 
                    if (Math.Sqrt(x * x + y * y) <= radius + 0.5)   //this make so the explosion radius is a circle
                    {
                        WorldGen.KillTile(xPosition, yPosition, false, false, false);  //this make the explosion destroy tiles  
                        Dust.NewDust(position, 22, 22, DustID.Smoke, 0.0f, 0.0f, 206, new Color(), 1f);  //this is the dust that will spawn after the explosion
                    }
                }
            }
        }
	
		
	}
}
		
			