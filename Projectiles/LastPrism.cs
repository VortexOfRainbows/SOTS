using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class LastPrism : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PriPir");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20; 
            projectile.timeLeft = 20;
            projectile.penetrate = 8; 
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
			
			if(projectile.owner == Main.myPlayer)
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, 635, (int)(projectile.damage * 1.5), projectile.knockBack, Main.myPlayer);

			
		}
		
	}
	
}