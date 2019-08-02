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
    public class Pebble : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pebble");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(311);
            aiType = 311; 
			projectile.penetrate = 1; 
		}
		
		public override void AI()
		{ 

			
			
}
	}
}
		