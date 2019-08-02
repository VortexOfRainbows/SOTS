using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class NeBolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula's Bolt");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(634);
            aiType = 634; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.magic = true;
		}
			  
			public override void AI()
        {
			projectile.type = 521; 
		}
	}
}

