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

namespace SOTS.Projectiles.Crushers
{    
    public class EclipseCrush : ModProjectile 
    {	int expand = -1;
		            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eclipse Crush");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 70;
			projectile.width = 70;
            Main.projFrames[projectile.type] = 6;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 23;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		public override void AI()
        {
			projectile.alpha += 8;
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
			if(expand == -1 && projectile.owner == Main.myPlayer)
			{
				expand = 0;
				if(projectile.knockBack > 1)
				{
					for(int i = 0; i < projectile.damage; i += (int)(projectile.knockBack * 1.85f))
					{ 
						int proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-100, 101) * 0.03f, Main.rand.Next(-100, 101) * 0.03f, mod.ProjectileType("EclipseBubble"), (int)(projectile.damage * 0.08f), 0, projectile.owner);
						Main.projectile[proj].friendly = true;
						Main.projectile[proj].hostile = false;
						Main.projectile[proj].timeLeft = Main.rand.Next(52, 156);
					}
				}
			}
			projectile.knockBack = 1f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 6;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 10;
			target.AddBuff(mod.BuffType("DelayKnockback"), 48, false);
        }
	}
}
		