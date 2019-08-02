using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles  //The directory for your .cs and .png; Example: TutorialMOD/Projectiles
{
    public class SnappedLine : ModProjectile   //make sure the sprite file is named like the class name (CustomYoyoProjectile)
    {	int time = 12;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snapped Line");
			
		}
 
        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 10;//Set the projectile hitbox width
            projectile.height = 10; //Set the projectile hitbox height            
            projectile.aiStyle = 99; // aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
            projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed. -1 = never
            projectile.melee = true; //Tells the game whether it is a melee projectile or not        
            // The following sets are only applicable to yoyo that use aiStyle 99.
            // YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player.
            // Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = time;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 1000f;
            // YoyosTopSpeed is top speed of the yoyo projectile.
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 11f;
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = time;
			time--;
            target.immune[projectile.owner] = 0;
			target.position.X = projectile.Center.X + (target.position.X - target.Center.X);
			target.position.Y = projectile.Center.Y - (target.Center.Y - target.position.Y);
        }

		

 
 
    }
}