using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class CarverShield : ModProjectile 
    {	int invis = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carver Shield");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 22; 
            projectile.timeLeft = 2;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; //hostile but doesn't do damage
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.aiStyle = 1;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			for(int i = 0; i < 1000; i++)
			{
				Projectile friendlyProj = Main.projectile[i];
				
				if(friendlyProj.active && projectile.Center.X + 32 > friendlyProj.Center.X && projectile.Center.X - 32 < friendlyProj.Center.X && projectile.Center.Y + 32 > friendlyProj.Center.Y && projectile.Center.Y - 32 < friendlyProj.Center.Y && friendlyProj.friendly == true && friendlyProj.damage > 4 && !friendlyProj.sentry && !friendlyProj.minion)
				{	
					float newVelocityX = -friendlyProj.velocity.X;
					float newVelocityY = -friendlyProj.velocity.Y;
					friendlyProj.Kill();
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, newVelocityX, newVelocityY, mod.ProjectileType("CarverRevenge"), (int)(friendlyProj.damage * 0.33f), projectile.knockBack, Main.myPlayer);

				}
				
				
			}
			
			
		} 
		
		
	}
	
}