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
    public class MourningStarBeam : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Beam");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(294);
            aiType = 294; //18 is the demon scythe style
			projectile.width = 22;
			projectile.height = 22;
			projectile.magic = false;
			projectile.penetrate = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 1800;

		}
		
			
}
	
}
		
			