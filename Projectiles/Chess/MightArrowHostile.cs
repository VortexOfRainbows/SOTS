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
    public class MightArrowHostile : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mighty Arrow");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(310);
            aiType = 310; 
			projectile.height = 18;
			projectile.width = 18;
			projectile.ranged = true;
			projectile.timeLeft = 360;
			projectile.friendly = false;
			projectile.hostile = true;
		}
		
		public override void AI()
		{ 
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 18, 36, 15);
Main.dust[num1].noGravity = true;
Main.dust[num1].velocity *= 0.1f;
		if(projectile.velocity.X == 0 && Main.rand.Next(100) == 0)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), mod.ProjectileType("UnholyWaterBolt"), 22, 0, 0);
		}
			
}
	}
}
		