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

namespace SOTS.Projectiles.Chess
{    
    public class UnholyWaterBolt : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Bolt");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(27);
            aiType = 27; 
			projectile.height = 16;
			projectile.width = 16;
			projectile.ranged = true;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.timeLeft = 1000;
		}
		
	}
}
		