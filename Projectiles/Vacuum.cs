using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Vacuum : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 2048;
            projectile.height = 2048; 
            projectile.timeLeft = 2;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 255;
		}
		public override void AI()
{
			projectile.alpha = 255;
    //Making player variable "p" set as the projectile's owner
    Player player  = Main.player[projectile.owner];
	Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 2048, 2048, 65);
	Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 2048, 2048, 65);
	Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 2048, 2048, 65);
	Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 2048, 2048, 65);
	Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 2048, 2048, 65);
	Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 2048, 2048, 65);
}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 3;
			target.life += damage;
			target.velocity.X *= .1f;
			target.velocity.Y *= .1f;
        }

		
		
		
	}
	
}