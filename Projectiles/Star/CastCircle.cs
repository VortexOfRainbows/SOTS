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

namespace SOTS.Projectiles.Star
{    
    public class CastCircle : ModProjectile 
    {	int wait = 0;
	float rotate = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CastCircle");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(274);
            aiType = 274; //18 is the demon scythe style
			projectile.alpha = 135;
			projectile.timeLeft = 480;
			projectile.width = 60;
			projectile.height = 60;
			projectile.friendly = false;
			projectile.hostile = false;

		}
		public override void PostAI()
		{
			if(wait == 0)
			{
				projectile.alpha = 135;
				wait++;
				projectile.scale += (float)(Main.rand.Next(-10,11)/100f);
			}
			projectile.rotation += 0.25f;
			//int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 60, 60, 235);

			
			//Main.dust[num1].noGravity = true;
			//Main.dust[num1].velocity *= 0.1f;
		}			
			
			
			
			

}
	}

		
			