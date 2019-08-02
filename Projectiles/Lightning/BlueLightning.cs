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

namespace SOTS.Projectiles.Lightning
{    
    public class BlueLightning : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Lightning");
			
		}
		
        public override void SetDefaults()
        {
			projectile.penetrate = 1; 
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.timeLeft = 330;
			projectile.width = 8;
			projectile.height = 8;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			projectile.alpha = 255;
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 132);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			int num2 = Dust.NewDust(new Vector2(projectile.Center.X + 7, projectile.Center.Y - 1), 2, 2, 56);
			Main.dust[num2].noGravity = true;
			Main.dust[num2].velocity *= 0.1f;
			
			int num3 = Dust.NewDust(new Vector2(projectile.Center.X - 7, projectile.Center.Y - 1), 2, 2, 56);
			Main.dust[num3].noGravity = true;
			Main.dust[num3].velocity *= 0.1f;
			
			int num4 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y + 7), 2, 2, 56);
			Main.dust[num4].noGravity = true;
			Main.dust[num4].velocity *= 0.1f;
			
			int num5 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 7), 2, 2, 56);
			Main.dust[num5].noGravity = true;
			Main.dust[num5].velocity *= 0.1f;
			
			
			projectile.velocity.Y += (0.4f * (Main.rand.Next(-3, 4)));
			
			projectile.velocity.X += (0.4f * (Main.rand.Next(-3, 4)));
			
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 35; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 56);
			Main.dust[num1].noGravity = true;
			}
		}
	}
}
		