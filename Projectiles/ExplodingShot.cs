using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class ExplodingShot : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("FireWork");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2; 
            projectile.timeLeft = 100;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 1; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{	
		
					Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0,  694, (int)(projectile.damage * 3), projectile.knockBack, Main.myPlayer);
					
				int rnd = Main.rand.Next(4);
				
					if(rnd == 0)
					Projectile.NewProjectile(projectile.position.X, projectile.position.Y -= 10, 0, -16,  167, (int)(projectile.damage * 3), projectile.knockBack, Main.myPlayer);
					
					if(rnd == 1)
					Projectile.NewProjectile(projectile.position.X, projectile.position.Y -= 10, 0, -16,  168, (int)(projectile.damage * 3), projectile.knockBack, Main.myPlayer);
					
					if(rnd == 2)
					Projectile.NewProjectile(projectile.position.X, projectile.position.Y -= 10, 0, -16,  169, (int)(projectile.damage * 3), projectile.knockBack, Main.myPlayer);
					
					if(rnd == 3)
					Projectile.NewProjectile(projectile.position.X, projectile.position.Y -= 10, 0, -16,  170, (int)(projectile.damage * 3), projectile.knockBack, Main.myPlayer);
			  
			  }
	}
}
		
			