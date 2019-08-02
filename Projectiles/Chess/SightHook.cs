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

namespace SOTS.Projectiles.Chess
{    
    public class SightHook : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SightHook");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(310);
            aiType = 310; 
			projectile.height = 16;
			projectile.width = 16;
			projectile.ranged = true;
			projectile.timeLeft = 360;
			projectile.friendly = false;
			projectile.hostile = true;
		}
		
		public override void AI()
		{ 
			wait++;
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 16, 16, 75);
Main.dust[num1].noGravity = true;
Main.dust[num1].velocity *= 0.1f;
		if(projectile.velocity.X == 0 && wait >= 15)
		{
			wait = 0;
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -4, -4, 96, 21, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -4, 4, 96, 21, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 4, -4, 96, 21, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 4, 4, 96, 21, 0, 0);
		}
			
}
	}
}
		