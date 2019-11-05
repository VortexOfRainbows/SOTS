using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class DimensionPortal : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dimension Portal");
			
		}
		
        public override void SetDefaults()
        {
			
			projectile.aiStyle = 0;
            projectile.timeLeft = 360;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
			projectile.alpha = 55;
			projectile.width = 40;
			projectile.height = 42;
            Main.projFrames[projectile.type] = 4;
		}
		int firerate = 0;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }
			int damage = projectile.damage;
			int projectileType = 14;
			for(int i = 54; i < 58; i++)
			{
				Item item2 = player.inventory[i];
				if(AmmoID.Bullet == item2.ammo)
				{
				int projectileAmmo = item2.shoot;
				projectileType = projectileAmmo;
				damage += item2.damage;
				break;
				}
			}
			
			firerate++;
			int reload = 12;
			for(int i = 0; i < 200; i++)
			{
				NPC target = Main.npc[i];
				if(firerate >= reload)
				{
				//Enemy NPC variable being set

				//Getting the shooting trajectory
					Vector2 playerCursor;
					
					if (player.gravDir == 1f)
					{
					playerCursor.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					playerCursor.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
					playerCursor.X = (float)Main.mouseX + Main.screenPosition.X;
				
					float shootToX = playerCursor.X - projectile.Center.X;
					float shootToY = playerCursor.Y - projectile.Center.Y;
					float distanceFromX = target.Center.X - playerCursor.X;
					float distanceFromY = target.Center.Y - playerCursor.Y;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					float distance2 = (float)System.Math.Sqrt((double)(distanceFromX * distanceFromX + distanceFromY * distanceFromY));

					if(distance2 < 160f && !target.friendly && target.active)
					{
						distance = 15.5f / distance;
					   
						shootToX *= distance * 5;
						shootToY *= distance * 5;
					   
						if(Main.myPlayer == projectile.owner)
						{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, projectileType, damage, 1f, projectile.owner, 0f, 0f); //Spawning a projectile
						}						
						firerate = 0;	
					}
				}
			}
		}	
	}
}