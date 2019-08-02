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
    public class DNA2Leader : ModProjectile 
    {	int worm = 0;
		int wait = 1;         
				float oldVelocityY = 0;	
				float oldVelocityX = 0;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DNA");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(98);
            aiType = 98; 
			projectile.height = 10;
			projectile.width = 10;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 900;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.magic = false;
			projectile.ranged = false;
		}
		public override void AI()
		{
			if(wait == 1)
			{
				wait++;
				oldVelocityY = projectile.velocity.Y;	
				oldVelocityX = projectile.velocity.X;
			}
			worm++;
			if(worm <= 60)
			{
			projectile.velocity.X += -oldVelocityY / 30f;
			projectile.velocity.Y += oldVelocityX / 30f;
			}
			else if(worm >= 61 && worm <= 120)
			{
			projectile.velocity.X += oldVelocityY / 30f;
			projectile.velocity.Y += -oldVelocityX / 30f;
			}
			if(worm >= 120)
			{
			worm = 0;
			}
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 10, 10, 68);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("DNA2Follow"), projectile.damage, 0, 0);
              
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
	}
}
		