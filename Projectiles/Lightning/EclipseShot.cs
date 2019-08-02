using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Lightning
{    
    public class EclipseShot : ModProjectile 
    {	float distance = 30f;  
		int rotation = 10;
		int canEclipse = 0;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eclipse Shot");
			
		}
		
        public override void SetDefaults()
        {
			
			projectile.height = 26;
			projectile.width = 26;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 1200;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.alpha = 70;
		}
		public override void AI()
		{
			projectile.alpha = 70;
			Vector2 circularLocation = new Vector2(-distance, 0).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 10;
			
			Player player  = Main.player[projectile.owner];
			
				projectile.rotation = MathHelper.ToRadians(rotation);
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 180); //172 or 180
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				if(!projectile.friendly)
				{
					canEclipse++;
				}
				
				if(canEclipse >= 7)
				{
				canEclipse = 0;
				projectile.friendly = true;
				}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
			projectile.friendly = false;
        }
		public override void Kill(int timeLeft)
		{
		
		Player player = Main.player[projectile.owner];
        SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
		
		Vector2 cursorArea;
					
						if (player.gravDir == 1f)
					{
					cursorArea.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					cursorArea.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						cursorArea.X = (float)Main.mouseX + Main.screenPosition.X;
		
		
				   float shootToX = cursorArea.X - projectile.Center.X;
				   float shootToY = cursorArea.Y - projectile.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				   distance = 3.25f / distance;
	   
				   shootToX *= distance * 5;
				   shootToY *= distance * 5;
	   
				   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("PlanetaryFlame2"), (int)(projectile.damage * 0.4f), projectile.knockBack, Main.myPlayer, 0f, 0f);
				  
			  
		
		
				
		}
	}
}
		