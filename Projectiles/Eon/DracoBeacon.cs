using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Eon
{    
    public class DracoBeacon : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draco Beacon");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(711);
            aiType =711; //18 is the demon scythe style
            projectile.width = 34;
            projectile.height = 90; 
            projectile.timeLeft = 80;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
			projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{
			
					
					for(int i = 0; i < 11; i++)
					Projectile.NewProjectile(projectile.Center.X, (projectile.Center.Y - 38), Main.rand.Next(-8, 9), Main.rand.Next(-15, -9), 711, (int)(projectile.damage * 1f), 0, 0);
					
		}
		public override void AI()
		{		
					if(Main.rand.Next(3) == 0)
				projectile.velocity.Y += Main.rand.Next(-1, 2);
			
				if(Main.rand.Next(3) == 0)
				projectile.velocity.X += Main.rand.Next(-1, 2);
		}
	}
	
}

