using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class ChloroBomb : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chlorophyte Bomb");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20; 
            projectile.timeLeft = 100000;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 1; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 10; i++)
			{
				if(projectile.owner == Main.myPlayer)
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), 207, (int)(projectile.damage * 0.25) + 1, 0, 0);
			}
		}
		
	}
	
}