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
    {	       
		float oldVelocityY = 0;	
		float oldVelocityX = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Bullet");
		}
        public override void SetDefaults()
        {
			Projectile.height = 22;
			Projectile.width = 22;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 330;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.magic = false;
			Projectile.ranged = false;
			Projectile.netImportant = true;
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 360; i += 24)
			{
				Vector2 circularLocation = new Vector2(-8, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 72);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation * 0.45f;
			}
		}
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				runOnce = false;
				oldVelocityY = Projectile.velocity.Y;	
				oldVelocityX = Projectile.velocity.X;
			}
			Projectile.velocity.X += -oldVelocityX / 120f;
			Projectile.velocity.Y += -oldVelocityY / 120f;
			
			int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 20, 20, 72);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			Projectile.rotation += 0.1f;
		}
	}
}
		