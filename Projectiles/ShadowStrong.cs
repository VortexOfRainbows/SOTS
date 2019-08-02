using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class ShadowStrong : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64; 
            projectile.timeLeft = 1;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 0;

		}
		public override void AI() //The projectile's AI/ what the projectile does
		{
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 64, 64, 65);
			
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, 10, 10,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X  += 32, projectile.position.Y += 32, -10, -10,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, 10, -10,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, -10, 10,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, 0, 10,  	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, 0, -10,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, 10, 0,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, -10, 0,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, -5, 10,  	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, -5, -10,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, 10, -5,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, -10, -5,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, 5, 10,  	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, 5, -10,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, 10, 5,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if((Main.rand.Next(199) == 0))
			Projectile.NewProjectile(projectile.position.X += 32, projectile.position.Y += 32, -10, 5,   	294 , (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			  
		}
	}
	
}