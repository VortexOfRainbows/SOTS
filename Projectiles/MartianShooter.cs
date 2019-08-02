using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class MartianShooter : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Martian");
			
		}
		
        public override void SetDefaults()
        {
		
		

			projectile.netImportant = true;
            projectile.width = 42;
            projectile.height = 32; 
            projectile.timeLeft = 2;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
			projectile.aiStyle = 0;
			projectile.alpha = 0;


		}
		public override void AI() //The projectile's AI/ what the projectile does
		{Player player = Main.player[projectile.owner];
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 42, 32, 206);
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);

	
               Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 10, 440, projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
              
      
			  
		}
		}
	}
	

