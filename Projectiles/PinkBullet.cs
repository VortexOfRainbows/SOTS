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
    public class PinkBullet : ModProjectile 
    {	int bounce = 24;
		int wait = 1;         
				float oldVelocityY = 0;	
				float oldVelocityX = 0;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Bullet");
		}
        public override void SetDefaults()
        {
			projectile.height = 20;
			projectile.width = 20;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 300;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.magic = false;
			projectile.ranged = false;
			projectile.netImportant = true;
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 360; i += 24)
			{
				Vector2 circularLocation = new Vector2(-8, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 72);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation * 0.45f;
				Main.dust[num1].scale *= 1.5f;
			}
		}
		public override void AI()
		{
			if(wait == 1)
			{
				wait++;
				oldVelocityY = projectile.velocity.Y;	
				oldVelocityX = projectile.velocity.X;
			}
			projectile.velocity.X += -oldVelocityX / 120f;
			projectile.velocity.Y += -oldVelocityY / 120f;
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 20, 20, 72);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			projectile.rotation += 0.1f;
		}
	}
}
		