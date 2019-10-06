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

namespace SOTS.Projectiles.Celestial
{    
    public class EnergySerpent : ModProjectile 
    {	
		int worm = 0;
		int wait = 1;         
		float oldVelocityY = 0;	
		float oldVelocityX = 0;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Serpent");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 40;
			projectile.height = 52;
            Main.projFrames[projectile.type] = 3;
			projectile.friendly = false;
			projectile.timeLeft = 60;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 145;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			target.AddBuff(39, 180, false);
		}	
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 2.5f / 255f, (255 - projectile.alpha) * 1.6f / 255f, (255 - projectile.alpha) * 2.4f / 255f);
			if(projectile.frame == 0)
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
				
			if(projectile.frame == 0)
			{
				int Probe = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("EnergySerpent"), projectile.damage, 0, 0);
				Main.projectile[Probe].rotation = projectile.rotation;
				Main.projectile[Probe].timeLeft = 600;
				Main.projectile[Probe].frame = 1;
			}
			if(projectile.timeLeft <= 2 && projectile.frame != 0)
			{
				projectile.frame = 2;
			}
			if(wait == 1)
			{
				wait++;
				oldVelocityY = projectile.velocity.Y;	
				oldVelocityX = projectile.velocity.X;
			}
			worm++;
			if(worm <= 20)
			{
			projectile.velocity.X += oldVelocityY / 10f;
			projectile.velocity.Y += -oldVelocityX / 10f;
			}
			else if(worm >= 21 && worm <= 40)
			{
			projectile.velocity.X += -oldVelocityY / 10f;
			projectile.velocity.Y += oldVelocityX / 10f;
			}
			if(worm >= 40)
			{
			worm = 0;
			}
		}
	}
}
		