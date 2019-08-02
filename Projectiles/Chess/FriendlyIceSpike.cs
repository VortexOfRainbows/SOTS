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
    public class FriendlyIceSpike : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spike");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.height = 10;
			projectile.width = 20;
			projectile.timeLeft = 720;
			projectile.friendly = true;
			projectile.hostile = false;
		}
		
		public override void AI()
		{ 
		}
		
			
}
	}

		