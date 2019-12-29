using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles  //The directory for your .cs and .png; Example: TutorialMOD/Projectiles
{
    public class Zepline : ModProjectile   //make sure the sprite file is named like the class name (CustomYoyoProjectile)
    {	
		int reset = 0;
		int resetTops = 120;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zephyrious Zeppelin");
			
		}
 
        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 16;
            projectile.height = 16;            
            projectile.aiStyle = 99; // aiStyle 99 is used for yoyo
            projectile.friendly = true; 
            projectile.penetrate = -1; 
            projectile.melee = true; 
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
			
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 130f;
            // YoyosTopSpeed is top speed of the yoyo projectile.
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
			
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 9.5f;
        }
        public override void AI()
        {
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 9.5f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 130f;
			resetTops = 120;
			
			if(1 == 1)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 10f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 140f;
				resetTops = 115;
			}
			if(1 == 2)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 11.5f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 150f;
				resetTops = 110;
			}
			if(1 == 3)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 11.75f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 160f;
				resetTops = 110;
			}
			if(1 == 4)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 170f;
				resetTops = 105;
			}
			if(1 == 5)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13.25f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 180f;
				resetTops = 100;
			}
			if(1 == 6)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13.75f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 190f;
				resetTops = 95;
			}
			if(1 == 7)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 15f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 200f;
				resetTops = 95;
			}
			if(1 == 8)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 15.25f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 210f;
				resetTops = 90;
			}
			if(1 == 9)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 15.5f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 220f;
				resetTops = 85;
			}
			if(1 == 10)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 15.75f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 230f;
				resetTops = 80;
			}
			if(1 == 11)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 17f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 240f;
				resetTops = 75;
			}
			if(1 == 12)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 17.25f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 250f;
				resetTops = 70;
			}
			if(1 == 13)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 17.75f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 260f;
				resetTops = 70;
			}
			if(1 == 14)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 19f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 270f;
				resetTops = 65;
			}
			if(1 == 15)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 19.25f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 280f;
				resetTops = 65;
			}
			if(1 == 16)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 19.5f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 290f;
				resetTops = 60;
			}
			if(1 == 17)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 19.75f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 300f;
				resetTops = 60;
			}
			if(1 == 18)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 21f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 320f;
				resetTops = 50;
			}
			if(1 == 19)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 21.5f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 340f;
				resetTops = 45;
			}
			if(1 == 20)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 22.25f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 360f;
				resetTops = 40;
			}
			if(1 == 21)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 23f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 380f;
				resetTops = 30;
			}
			if(1 == 22)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 23.25f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 400f;
				resetTops = 25;
			}
			if(1 == 23)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 23.5f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 420f;
				resetTops = 20;
			}
			if(1 == 24)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 23.75f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 450f;
				resetTops = 16;
			}
			if(1 == 25)
			{
				ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 24f;
				ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 500f;
				resetTops = 12;
			}
			
			
			reset++;
			if(reset >= resetTops)
			{
				reset = 0;
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), mod.ProjectileType("Zephyr"), (int)(projectile.damage * 1), 0, 0);
			}
			
        }
    }
}