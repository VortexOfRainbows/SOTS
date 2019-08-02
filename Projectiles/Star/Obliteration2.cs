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
    public class Obliteration2 : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obliteration");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.timeLeft = 240;
			projectile.width = 1;
			projectile.height = 1;
			projectile.tileCollide = false;

		}
		public override void AI()
		{
			projectile.alpha = 255;
			
				Dust dust;
						// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
						Vector2 position = projectile.Center;
						dust = Main.dust[Terraria.Dust.NewDust(position, 1, 1, 181, 0f, 0f, 0, new Color(255,255,255), 2.039474f)];
						dust.noGravity = true;
						dust.velocity *= 0.1f;
			


			
			

		}
	}
}
		
			