using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class CarverSingularity : ModProjectile 
    {	int invis = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carver Singularity");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
            projectile.width = 18;
            projectile.height = 18; 
            projectile.timeLeft = 480;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; //hostile but doesn't do damage
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
			projectile.alpha = 125;
		}
		public override void AI()
		{
			projectile.rotation += 0.25f;
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 18, 18, 160);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;

			for(int i = 0; i < 1000; i++)
			{
				Projectile friendlyProj = Main.projectile[i];
				
				if(friendlyProj.active && projectile.Center.X + 128 > friendlyProj.Center.X && projectile.Center.X - 128 < friendlyProj.Center.X && projectile.Center.Y + 128 > friendlyProj.Center.Y && projectile.Center.Y - 128 < friendlyProj.Center.Y && friendlyProj.type != mod.ProjectileType("CarverShield") && friendlyProj.type != mod.ProjectileType("MargritBolt") && !friendlyProj.minion && !friendlyProj.sentry)
				{	
					
								if(projectile.Center.X + 128 > friendlyProj.Center.X && projectile.Center.X  < friendlyProj.Center.X - 16)
								friendlyProj.velocity.X = -1;
							
								if(projectile.Center.X  - 128 < friendlyProj.Center.X && projectile.Center.X  > friendlyProj.Center.X + 16)
								friendlyProj.velocity.X = 1;
									
								if(projectile.Center.Y + 128 > friendlyProj.Center.Y && projectile.Center.Y < friendlyProj.Center.Y - 16)
								friendlyProj.velocity.Y = -1;
									
								if(projectile.Center.Y- 128 < friendlyProj.Center.Y && projectile.Center.Y > friendlyProj.Center.Y + 16)
								friendlyProj.velocity.Y = 1;
					
						if(projectile.timeLeft < 2)
						{
							friendlyProj.velocity.X += Main.rand.Next(-400,500) * 0.01f;
							friendlyProj.velocity.Y += Main.rand.Next(-400,500) * 0.01f;
							friendlyProj.velocity.Y *= 1.2f;
							friendlyProj.velocity.X *= 1.2f;
							friendlyProj.friendly = true;
							friendlyProj.hostile = true;
						
						}
				}
				
				
			}
			
			
		} 
		
		
	}
	
}