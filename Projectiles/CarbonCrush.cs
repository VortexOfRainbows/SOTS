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
    public class CarbonCrush : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carbon Crusher");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(697);
            aiType = 697; //18 is the demon scythe style
			projectile.width = 112;
			projectile.height = 108;
		
        }
	}
}
		
			