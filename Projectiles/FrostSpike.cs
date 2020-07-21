using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles
{    
    public class FrostSpike : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("FrostSpike");
		}
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50; 
            projectile.timeLeft = 360;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
			projectile.netImportant = true;
			projectile.melee = true; 
            projectile.aiStyle = 0;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			if (projectile.timeLeft > 100)
			{
				projectile.timeLeft = 300;
			}

			Player player  = Main.player[projectile.owner];
			projectile.rotation += 0.25f;
			double deg = (double) projectile.ai[1]; 
			double rad = deg * (Math.PI / 180);
			double dist = 96;
			projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
			projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
			projectile.ai[1] += 2f;
			
		}
	}
}
		
			