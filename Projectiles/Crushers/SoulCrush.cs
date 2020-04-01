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
    public class SoulCrush : ModProjectile 
    {	int expand = -1;
		            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Crush");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 72;
			projectile.height = 78;
            Main.projFrames[projectile.type] = 6;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 23;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
			/*
			if(expand == -1 && projectile.owner == Main.myPlayer)
			{
				expand = 0;
				if(projectile.knockBack > 1)
				{
					for(int i = 0; i < projectile.damage; i += (int)(projectile.knockBack * 2f))
					{ 
						int proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-100, 101) * 0.025f, Main.rand.Next(-100, 101) * 0.025f, 410, 0, 0, 0);
						Main.projectile[proj].friendly = false;
						Main.projectile[proj].hostile = false;
						Main.projectile[proj].timeLeft = Main.rand.Next(24, 60);
					}
				}
			}
			*/
			projectile.knockBack = 3.5f;
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
			if(target.life <= 0 && projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 2; i++)
				{
					Vector2 circularLocation = new Vector2(0, 4).RotatedBy(MathHelper.ToRadians(i * 180));
					Projectile.NewProjectile(target.Center.X, target.Center.Y, circularLocation.X, circularLocation.Y, mod.ProjectileType("ManaLock"), 0, 0, player.whoAmI, 20);
					
					circularLocation = new Vector2(0, 4).RotatedBy(MathHelper.ToRadians(90 + (i * 180)));
					Projectile.NewProjectile(target.Center.X, target.Center.Y, circularLocation.X, circularLocation.Y, mod.ProjectileType("VoidLock"), 0, 0, player.whoAmI, 4);
				}
			}
        }
	}
}