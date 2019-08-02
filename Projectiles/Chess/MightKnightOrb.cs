using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chess
{    
    public class MightKnightOrb : ModProjectile 
    {	int fire = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Might Knight Orb");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36; 
            projectile.timeLeft = 360;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.melee = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		public override void AI()
{
    //Making player variable "p" set as the projectile's owner
    Player player  = Main.player[projectile.owner];

    //Factors for calculations
    double deg = (double) projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
    double rad = deg * (Math.PI / 180); //Convert degrees to radians
    double dist = 384; //Distance away from the player
 
    /*Position the player based on where the player is, the Sin/Cos of the angle times the /
    /distance for the desired distance away from the player minus the projectile's width   /
    /and height divided by two so the center of the projectile is at the right place.     */
    projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
    projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
 
    //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
    projectile.ai[1] += 1f;
	
	fire++;
	if(fire >= 60)
	{
Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 27, 60, 1, 0);
		fire = 0;
	}
	
	
	
}
		
		
		
	}
	
}