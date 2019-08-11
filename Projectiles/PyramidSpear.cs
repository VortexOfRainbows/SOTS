using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class PyramidSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperial Guardsman's Pike");
			
		}
		
        public override void SetDefaults()
        {
            projectile.CloneDefaults(66);
            aiType = 66;
            projectile.melee = true;
			projectile.alpha = 0;
            projectile.width = 88;
            projectile.height = 90; 
		}
	}
}