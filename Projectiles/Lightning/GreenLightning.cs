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
    public class GreenLightning : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Lightning");
			
		}
		
        public override void SetDefaults()
        {
			projectile.penetrate = -1; 
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.magic = true;
			projectile.timeLeft = 330;
			projectile.width = 8;
			projectile.height = 8;
			projectile.alpha = 255;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.alpha = 255;
			Vector2 cursorArea = new Vector2(1400, 0).RotatedBy(Math.Atan2(projectile.ai[1] - projectile.Center.Y, projectile.ai[0] - projectile.Center.X));
			cursorArea.X += player.Center.X;
			cursorArea.Y += player.Center.Y;
			for(int i = projectile.timeLeft; i > 0; i--)
			{
				bool activeDamageBox = true;
				
				float distanceX = cursorArea.X - projectile.Center.X;
				float distanceY = cursorArea.Y - projectile.Center.Y;
				float distanceBetween = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
					
				float shootToX = distanceX;
				float shootToY = distanceY;
				float distance = distanceBetween;
				
				distance = 2f / distance;
				shootToX *= distance * 2.5f;
				shootToY *= distance * 2.5f;
				
				Vector2 circularLocation = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(32));
			
				for(int j = 10; j > 0; j--)
				{
					projectile.position.X += circularLocation.X;
					projectile.position.Y += circularLocation.Y;
					
					distanceX = cursorArea.X - projectile.Center.X;
					distanceY = cursorArea.Y - projectile.Center.Y;
					distanceBetween = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
					if(distanceBetween > 90f)
					{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 107);
				
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.1f;
					}
						for(int k = 0; k < 200; k++)
						{
							NPC npc = Main.npc[k];
							float distanceXNPC = npc.Center.X - projectile.Center.X;
							float distanceYNPC = npc.Center.Y - projectile.Center.Y;
							float distanceBetweenNPC = (float)System.Math.Sqrt((double)(distanceXNPC * distanceXNPC + distanceYNPC * distanceYNPC));
							if(distanceBetweenNPC < (float)(npc.width * .6f) + 12 && npc.active && activeDamageBox && !npc.friendly && projectile.owner == Main.myPlayer) //making sure enemy is within range
							{
								Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("GreenExplosion"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
								activeDamageBox = false;
							}
						}
				}
				distanceX = cursorArea.X - projectile.Center.X;
				distanceY = cursorArea.Y - projectile.Center.Y;
				distanceBetween = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
				if(distanceBetween < 90f)
				{
					projectile.Kill();
					break;
				}
				projectile.velocity.Y += (0.8f * (Main.rand.Next(-3, 4)));
				projectile.velocity.X += (0.8f * (Main.rand.Next(-3, 4)));
			}
		}
		public override void Kill(int timeLeft)
		{
			if(projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 25; i++)
				{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 107);
				Main.dust[num1].noGravity = true;
				}
			}
		}
	}
}
		