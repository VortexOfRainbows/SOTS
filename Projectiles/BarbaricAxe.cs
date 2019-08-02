using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles 
{    
    public class BarbaricAxe : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Barbarians Axe");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 75;
            projectile.height = 75; 
            projectile.timeLeft = 360;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.melee = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void AI()
		{
					
			Player player  = Main.player[projectile.owner];
			projectile.rotation += 0.25f;
			double deg = (double) projectile.ai[1]; 
			double rad = deg * (Math.PI / 180);
			double dist = 48;
			projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
			projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
		 
			projectile.ai[1] += 2f;
			
		}
	}
}
		
			