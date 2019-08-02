using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class BulletSharkProj : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet Shark");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(158);
            aiType = 158; //18 is the demon scythe style
            projectile.width = 22;
            projectile.height = 38; 
            projectile.ranged = true; 
			projectile.timeLeft = 2000;
			projectile.alpha = 0;
		}
		
		public override void AI()
        {
			projectile.alpha++;
			wait++;
			if(wait >= 16)
			{
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, 0, 0, mod.ProjectileType("BulletBubble"), (int)(projectile.damage * 0.75f), 0, 0);
				wait = 0;
			}
			
}
}
}
			