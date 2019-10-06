using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class SnowBaller : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snow");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10; 
            projectile.timeLeft = 3;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 1; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		public override void Kill(int timeLeft)
		{	
			int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
			
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 8, 8,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -8, -8,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 8, -8,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -8, 8,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 10, 166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, -10,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 10, 0,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -10, 0,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			
			
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -4, 7, 166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -4, -7,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 7, -4,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -7, -4,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 4, 7, 166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 4, -7,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 7, 4,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -7, 4,  166, projectile.damage, projectile.knockBack, Main.myPlayer);
			  
			}
		} 
		public override void AI()
        {
            projectile.type = 166; //This is the demon scythe projectile ID
        }
	}
		
}
	
