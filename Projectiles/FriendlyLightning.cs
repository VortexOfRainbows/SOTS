using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class FriendlyLightning : ModProjectile 
    {	int split;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chain Lightning");
			
		}
		
        public override void SetDefaults()
        {
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
			projectile.timeLeft = 25;
			projectile.width = 6;
			projectile.height = 6;
			projectile.penetrate = 1;
			projectile.alpha = 255;	
		}
		
		public override void AI()
        {
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 6, 6, 206);
			projectile.alpha = 255;	
			
			projectile.ai[0] = (Main.rand.Next(-3, 4));
			projectile.ai[1] = (Main.rand.Next(-3, 4));
			
			projectile.velocity.Y += (0.15f * projectile.ai[1]);
			
			projectile.velocity.X += (0.15f * projectile.ai[0]);
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 6, 6, 221);
			
						
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;


        }
		public override void Kill(int timeLeft)
		{
			if(projectile.owner == Main.myPlayer)
			{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y,  mod.ProjectileType("FriendlyLightning1"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y,  mod.ProjectileType("FriendlyLightning1"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			}	
			projectile.active = false;
		}
	}
}
			