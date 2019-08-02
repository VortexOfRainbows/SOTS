using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Margrit //The directory for your .cs and .png; Example: TutorialMOD/Projectiles
{
    public class MargritYoyo : ModProjectile   //make sure the sprite file is named like the class name (CustomYoyoProjectile)
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Yoyo");
			
		}
 
        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 24;//Set the projectile hitbox width
            projectile.height = 24; //Set the projectile hitbox height            
            projectile.aiStyle = 99; // aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
            projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed. -1 = never
            projectile.melee = true; //Tells the game whether it is a melee projectile or not        
            // Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 13f;
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 390f;
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 7.85f;
        }
        //Add this if you want the yoyo to make dust when used
        public override void AI()
        {
			for(int i = 0; i < 1000; i++)
			{
				Projectile hostileProjectile = Main.projectile[i];
				
				if(projectile.Center.X + 640 > hostileProjectile.Center.X && projectile.Center.X - 640 < hostileProjectile.Center.X && projectile.Center.Y + 640 > hostileProjectile.Center.Y && projectile.Center.Y - 640 < hostileProjectile.Center.Y && hostileProjectile.hostile == true)
				{	
					
								if(projectile.Center.X + 640 > hostileProjectile.Center.X && projectile.Center.X  < hostileProjectile.Center.X - 16)
								{
									if(hostileProjectile.velocity.X > 1)
									{
									hostileProjectile.velocity.X = 1;
									}
							hostileProjectile.velocity.X -= 0.15f;
								}
								if(projectile.Center.X  - 640 < hostileProjectile.Center.X && projectile.Center.X  > hostileProjectile.Center.X + 16)
								{
									if(hostileProjectile.velocity.X < -1)
									{
									hostileProjectile.velocity.X = -1;
									}
							hostileProjectile.velocity.X += 0.15f;
								}	
								if(projectile.Center.Y + 640 > hostileProjectile.Center.Y && projectile.Center.Y < hostileProjectile.Center.Y - 16)
								{
									if(hostileProjectile.velocity.Y > 1)
									{
									hostileProjectile.velocity.Y = 1;
									}
							hostileProjectile.velocity.Y -= 0.15f;
								}
								if(projectile.Center.Y- 640 < hostileProjectile.Center.Y && projectile.Center.Y > hostileProjectile.Center.Y + 16)
								{
									if(hostileProjectile.velocity.Y < -1)
									{
									hostileProjectile.velocity.Y = -1;
									}
							hostileProjectile.velocity.Y += 0.15f;
								}
								
					if(projectile.Center.X + 32 > hostileProjectile.Center.X && projectile.Center.X - 32 < hostileProjectile.Center.X && projectile.Center.Y + 32 > hostileProjectile.Center.Y && projectile.Center.Y - 32 < hostileProjectile.Center.Y && hostileProjectile.hostile == true)
					{
				hostileProjectile.friendly = true;
					}
					
				}
				
				
			}
        }

		

 
 
    }
}