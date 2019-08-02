using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Star
{
    public class BigTaco : ModProjectile 
    {	int Epic = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BigTaco");
			
		}
 
        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 30;
            projectile.height = 30;	       
            projectile.aiStyle = 99; // aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
            projectile.friendly = true;	
            projectile.penetrate = -1;	
			projectile.melee = true;	        
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 2000f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 1;
        }
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = (player.fishingSkill * 0.1f) + 0.01f;;
			
			Epic++;
			
			 for(int i = 0; i < 200; i++)
				{
					
				   NPC target = Main.npc[i];

					
				   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
				   float shootToY = target.position.Y + (float)target.height * 0.5f - projectile.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				   //If the distance between the projectile and the live target is active
				   if(distance < 160f && !target.friendly && target.active)
				   {
					   if(Epic >= 10) //Assuming you are already incrementing this in AI outside of for loop
					   {
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance = 3f / distance;
			   
						   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
			   
						   //Shoot projectile and set ai back to 0
					Vector2 newShootTo = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(45));
						   
						   
						   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, newShootTo.X, newShootTo.Y, ProjectileID.SporeCloud, (int)(projectile.damage * 0.6f), 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
						  
					   }
				   }
				}
				if(Epic >= 10)
				{
					Epic = 0;
				}
		}
    }
}