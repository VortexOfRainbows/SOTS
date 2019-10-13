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

namespace SOTS.Projectiles.Celestial
{    
    public class BlueArrow : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Arrow");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14;
			projectile.penetrate = 1; 
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.width = 14;
			projectile.height = 28;
			projectile.alpha = 25;
		}
		public override void AI()
		{ 
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.6f / 555f, (255 - projectile.alpha) * 0.4f / 555f, (255 - projectile.alpha) * 2.5f / 555f);
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + (MathHelper.ToRadians(90f));
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 360; i += 24)
			{
				Vector2 circularLocation = new Vector2(-15, 0).RotatedBy(MathHelper.ToRadians(i));
						
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 56);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				
			}
		}
	}
}
		