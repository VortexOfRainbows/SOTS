using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Star
{    
    public class ExplosionCast : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Insignius");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(158);
            aiType = 158; //18 is the demon scythe style
            projectile.width = 12;
            projectile.height = 12; 
            projectile.friendly = false; 
            projectile.hostile = false; 
			projectile.timeLeft = 480;
			projectile.alpha = 255;
		}
		
		public override void AI()
        {
			projectile.alpha = 255;
			wait++;
			if(wait >= 60)
			{
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 0, 0, mod.ProjectileType("CastCircle"), 0, 0, 0);
				wait = 0;
			}
			
}
}
}
			