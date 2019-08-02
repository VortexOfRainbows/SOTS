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

namespace SOTS.Projectiles.HolyRelics
{    
    public class PlanetaryCapsule : ModProjectile 
    {	int bounce = 6;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rip");
			
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 2;
			projectile.height = 38;
			projectile.width = 38;
			projectile.thrown = true;
			projectile.penetrate = 3;
			projectile.friendly = true;
			projectile.timeLeft = 45;
			projectile.tileCollide = true;
		}
		public override void AI()
		{ 
			for(int i = 0; i < 5; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 38, 38, 160);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
		}
		public override void Kill(int timeLeft)
		{
			projectile.ai[1] = 0;
							 for(int j = 0; j < 200; j++)
								{
								
								   NPC target = Main.npc[j];

								   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
								   float shootToY = target.position.Y + (float)target.height * 0.5f - projectile.Center.Y;
								   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

								   if(distance < 1000f && !target.friendly && target.active)
								   {
									   if(projectile.ai[1] < 5) 
									   {
										   distance = 1.8f / distance;
							   
										   shootToX *= distance * 5;
										   shootToY *= distance * 5;
							   
											if(projectile.owner == Main.myPlayer)
											Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("MourningStarBeam"), projectile.damage, 0, Main.myPlayer, 0f, 0f); 
										  projectile.ai[1]++;
									   }
								   }
								}
								
								for(int i = (int)(projectile.ai[1]); i < 5; i++)
								{
										if(projectile.owner == Main.myPlayer)
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (Main.rand.Next(-11,10)), (Main.rand.Next(-11,10)), mod.ProjectileType("MourningStarBeam"), projectile.damage, 0, Main.myPlayer, 0f, 0f); 
								}
		}
	}
}
		