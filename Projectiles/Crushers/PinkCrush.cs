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
    public class PinkCrush : ModProjectile 
    {	int expand = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Devourer");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 70;
			projectile.width = 70;
            Main.projFrames[projectile.type] = 5;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 24;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
			if(expand == -1 && projectile.owner == Main.myPlayer)
			{
				expand = 0;
				if(projectile.knockBack > 1)
				{
					for(int i = 0; i < projectile.damage; i += (int)(projectile.knockBack * 3f))
					{ 
						int proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-100, 101) * 0.02f, Main.rand.Next(-100, 101) * 0.02f, mod.ProjectileType("PinkBullet"), 0, 0, 0);
						Main.projectile[proj].hostile = false;
						Main.projectile[proj].timeLeft = Main.rand.Next(24, 60);
					}
				}
			}
			projectile.knockBack = 3.5f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 5;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 10;
        }
	}
}
		