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
    public class GoldThunder : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; //18 is the demon scythe style
			projectile.alpha = 25;
			projectile.width = 44;
			projectile.height = 22;
			projectile.timeLeft = 1800;
		}
		
		public override void AI()
		{
			if(wait < 45)
			{
			projectile.scale += 0.02f;
			projectile.alpha += 2;
			}
			if(wait >= 45)
			{
			projectile.scale -= 0.02f;
			projectile.alpha -= 2;
			}
			wait += 1;
			if(wait >= 90)
			{
				wait = 0;
				projectile.velocity.X *= (0.03f * Main.rand.Next(-101,100));
				projectile.velocity.Y *= (0.03f * Main.rand.Next(-101,100));
			}
}
	}
}
		
			