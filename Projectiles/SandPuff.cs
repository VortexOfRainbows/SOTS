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
			projectile.penetrate = 8;
			projectile.alpha = 35;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			projectile.alpha++;
			rotation = 6;
			rotation += 24;
			projectile.rotation += MathHelper.ToRadians(rotation);
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 40, 40, 32);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			projectile.alpha++;
			
			if(projectile.timeLeft < 61)
			{
				Vector2 circularLocation = new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f).RotatedBy(MathHelper.ToRadians(rotation));
				
				Player player  = Main.player[projectile.owner];
				projectile.velocity *= 0.86f;
				projectile.velocity += circularLocation;
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 25; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 40, 40, 32);
			Main.dust[num1].noGravity = true;
			}
		}
	}
}
		