using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class EtherealTrail2 : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Bullet");
			
		}
		
        public override void SetDefaults()
        {
			
            projectile.penetrate = -1; 
            projectile.width = 8;
            projectile.height = 8; 
            projectile.tileCollide = false;
			projectile.timeLeft = 900;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			if(projectile.velocity.Y > 0 || projectile.velocity.X > 0 || projectile.velocity.X < 0 || projectile.velocity.Y < 0)
			{
				
			if(Main.rand.Next(2) == 0)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 235);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
			projectile.alpha++;
			projectile.timeLeft -= 5;
			}
		}
}
}
			