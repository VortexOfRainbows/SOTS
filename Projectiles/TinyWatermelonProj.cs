using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class TinyWatermelonProj : ModProjectile 
    {	int timeTaken = 0;
		int stopTaken = 0;
		int original = 0;
		int originStart = 0;
		int posStart = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Melons");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8; 
            projectile.timeLeft = 100000;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 255;
		}
		public override void AI()
{
    //Making player variable "p" set as the projectile's owner
	
    Player player  = Main.player[projectile.owner];
	
	if(originStart == 0)
	{
	original = Main.rand.Next(110, 150);
	originStart = 1;
	posStart = Main.rand.Next(361);
	}
	if(stopTaken == 0)
	timeTaken++;

	if(timeTaken == posStart)
	{
			stopTaken = 1;
			timeTaken = -2;
			projectile.timeLeft = 720;
			projectile.alpha = 0;
            projectile.friendly = true; 
	}
    //Factors for calculations
    double deg = (double) projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
    double rad = deg * (Math.PI / 180); //Convert degrees to radians
    double dist = (float)(original + Main.rand.Next(-1,2)); //Distance away from the player
 
    /*Position the player based on where the player is, the Sin/Cos of the angle times the /
    /distance for the desired distance away from the player minus the projectile's width   /
    /and height divided by two so the center of the projectile is at the right place.     */
    projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
    projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
 
    //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
    projectile.ai[1] += 1;
}
		
		
		
	}
	
}