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
    public class HomingArrow : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chlorophyte Shock");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(338);
            aiType = 338; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.penetrate = 1; 
			projectile.ranged = true;
			projectile.width = 24;
			projectile.height = 44;
		}
		
		public override void AI()
		{ 
			
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 24, 44, 107);

			
			
}
	}
}
		