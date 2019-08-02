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
    public class SandPuff : ModProjectile 
    {	int rotation = -1;      
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandy Puff");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 40;
			projectile.width = 40;
			projectile.thrown = false;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.penetrate = 16;
			projectile.alpha = 35;
			projectile.timeLeft = 90;
		}
		public override void AI()
		{
			if(rotation == -1)
			{
				if(Main.rand.Next(2) == 0)
				{
				rotation = 6;
				}
				else
				{
				rotation = -6;
				}
			}
			projectile.rotation += MathHelper.ToRadians(rotation);
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 40, 40, 32);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			projectile.alpha++;
			
			if(projectile.timeLeft < 61)
			{
				Vector2 circularLocation = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation));
				
				Player player  = Main.player[projectile.owner];
				projectile.velocity = circularLocation;
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 35; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 40, 40, 32);
			Main.dust[num1].noGravity = true;
			}
		}
	}
}
		