using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class CrackedBeam : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crack The Sky");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(158);
            aiType = 158; //18 is the demon scythe style
            projectile.width = 14;
            projectile.height = 30; 
            projectile.thrown = true; 
			projectile.timeLeft = 80;
            projectile.penetrate = -1; 
			projectile.alpha = 0;
            projectile.tileCollide = false;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = Main.rand.Next(2);
			
        }
		public override void AI()
		{
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 14, 30, 160);
			
Main.dust[num1].noGravity = true;
Main.dust[num1].velocity *= 0.1f;
		}
		
}
}
			