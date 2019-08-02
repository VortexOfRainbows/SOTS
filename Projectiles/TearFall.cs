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

namespace SOTS.Projectiles
{    
    public class TearFall : ModProjectile 
    {	int bounce = 24;
		int wait = 1;         
				float oldVelocityY = 0;	
				float oldVelocityX = 0;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tear Crash");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.width = 14;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.timeLeft = 20000;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.ranged = false;
		}
		public override void AI()
		{
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 14, 14, 160);
		}
		public override void Kill(int timeLeft)
		{
				
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 14);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-8,9), Main.rand.Next(-8,9), mod.ProjectileType("TearSplit"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
					

		}
	}
}
		