using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Margrit       //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
 
{
    public class MargritSentry : ModProjectile
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Sentry");
			
		}
        public override void SetDefaults()
        {
 
            projectile.width = 38; 
            projectile.height = 38;  
            projectile.hostile = false; 
            projectile.friendly = true;   
            projectile.ignoreWater = true;    
            Main.projFrames[projectile.type] = 1;  
            projectile.timeLeft = 630; 
            projectile.penetrate = -1;
            projectile.tileCollide = true; 
            projectile.sentry = true; 
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override void AI()
        {
         
			projectile.rotation += 0.25f;
			for(int i = 0; i < 1000; i++)
					{
						Projectile hostileProjectile = Main.projectile[i];
						
						if(hostileProjectile.active && projectile.Center.X + 640 > hostileProjectile.Center.X && projectile.Center.X - 640 < hostileProjectile.Center.X && projectile.Center.Y + 640 > hostileProjectile.Center.Y && projectile.Center.Y - 640 < hostileProjectile.Center.Y && !hostileProjectile.minion && !hostileProjectile.sentry)
						{	
							
										if(projectile.Center.X + 640 > hostileProjectile.Center.X && projectile.Center.X  < hostileProjectile.Center.X - 16)
										{
											if(hostileProjectile.velocity.X > 1)
											{
											hostileProjectile.velocity.X = 1;
											}
									hostileProjectile.velocity.X -= 0.35f;
										}
										if(projectile.Center.X  - 640 < hostileProjectile.Center.X && projectile.Center.X  > hostileProjectile.Center.X + 16)
										{
											if(hostileProjectile.velocity.X < -1)
											{
											hostileProjectile.velocity.X = -1;
											}
									hostileProjectile.velocity.X += 0.35f;
										}	
										if(projectile.Center.Y + 640 > hostileProjectile.Center.Y && projectile.Center.Y < hostileProjectile.Center.Y - 16)
										{
											if(hostileProjectile.velocity.Y > 1)
											{
											hostileProjectile.velocity.Y = 1;
											}
									hostileProjectile.velocity.Y -= 0.35f;
										}
										if(projectile.Center.Y- 640 < hostileProjectile.Center.Y && projectile.Center.Y > hostileProjectile.Center.Y + 16)
										{
											if(hostileProjectile.velocity.Y < -1)
											{
											hostileProjectile.velocity.Y = -1;
											}
									hostileProjectile.velocity.Y += 0.35f;
										}
										if(hostileProjectile.type != mod.ProjectileType("MargritSentry") && Main.rand.Next(900) == 0)
										{
										hostileProjectile.penetrate = 1;
										}
						}
					}
					for(int i = 0; i < 200; i++)
					{
						NPC hostileNPC = Main.npc[i];
						
						if(hostileNPC.lifeMax <= 2500 && !hostileNPC.boss && hostileNPC.active && projectile.Center.X + 640 > hostileNPC.Center.X && projectile.Center.X - 640 < hostileNPC.Center.X && projectile.Center.Y + 640 > hostileNPC.Center.Y && projectile.Center.Y - 640 < hostileNPC.Center.Y)
						{	
							
										if(projectile.Center.X + 640 > hostileNPC.Center.X && projectile.Center.X  < hostileNPC.Center.X - 16)
										{
											if(hostileNPC.velocity.X > 1)
											{
												hostileNPC.velocity.X = 1;
											}
											hostileNPC.velocity.X -= 0.35f;
										}
										if(projectile.Center.X  - 640 < hostileNPC.Center.X && projectile.Center.X  > hostileNPC.Center.X + 16)
										{
											if(hostileNPC.velocity.X < -1)
											{
												hostileNPC.velocity.X = -1;
											}
											hostileNPC.velocity.X += 0.35f;
										}	
										if(projectile.Center.Y + 640 > hostileNPC.Center.Y && projectile.Center.Y < hostileNPC.Center.Y - 16)
										{
											if(hostileNPC.velocity.Y > 1)
											{
												hostileNPC.velocity.Y = 1;
											}
											hostileNPC.velocity.Y -= 0.35f;
										}
										if(projectile.Center.Y- 640 < hostileNPC.Center.Y && projectile.Center.Y > hostileNPC.Center.Y + 16)
										{
											if(hostileNPC.velocity.Y < -1)
											{
												hostileNPC.velocity.Y = -1;
											}
											hostileNPC.velocity.Y += 0.35f;
										}
						}
					}
        }
    }
}