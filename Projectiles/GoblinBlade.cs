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
    public class GoblinBlade : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GoblinBlade");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1;
			projectile.aiStyle = 3;
			projectile.alpha = 0;
			projectile.timeLeft = 220;
			projectile.width = 22;
			projectile.height = 16;
			projectile.penetrate = 7;
		}
	}
}

		
			