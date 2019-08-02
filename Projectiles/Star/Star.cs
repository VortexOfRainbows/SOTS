using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Star 
{    
    public class Star : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14; 
            projectile.timeLeft = 30;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 48; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{	
			int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
			
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 10, 10,  mod.ProjectileType("Star2"), (int)(projectile.damage * 0.9), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -10, -10,  mod.ProjectileType("Star2"), (int)(projectile.damage * 0.9), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 10, -10,  mod.ProjectileType("Star2"), (int)(projectile.damage * 0.9), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -10, 10,  mod.ProjectileType("Star2"), (int)(projectile.damage * 0.9), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 10,  mod.ProjectileType("Star2"), (int)(projectile.damage * 0.9), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, -10,  mod.ProjectileType("Star2"), (int)(projectile.damage * 0.9), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 10, 0,  mod.ProjectileType("Star2"), (int)(projectile.damage * 0.9), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -10, 0,  mod.ProjectileType("Star2"), (int)(projectile.damage * 0.9), projectile.knockBack, Main.myPlayer);
			  
			  }
			
		}
		
	}
	
}