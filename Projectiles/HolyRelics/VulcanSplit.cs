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
    public class VulcanSplit : ModProjectile 
    {	int bounce = 24;
		int wait = 1;         
				float oldVelocityY = 0;	
				float oldVelocityX = 0;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulcan Split");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(98);
            aiType = 98; 
			projectile.height = 14;
			projectile.width = 14;
			projectile.friendly = false;
			projectile.timeLeft = 200;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		public override void AI()
		{
			if(wait == 1)
			{
				wait++;
				oldVelocityY = projectile.velocity.Y;	
				oldVelocityX = projectile.velocity.X;
			}
			projectile.velocity.X += -oldVelocityX / 100f;
			projectile.velocity.Y += -oldVelocityY / 100f;
			
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 14, 14, 160);
		}
		public override void Kill(int timeLeft)
		{
				
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 14);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("VulcanDetonate"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
					

		}
	}
}
		