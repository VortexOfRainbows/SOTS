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
    public class Dart : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Get Nerfed");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
             //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.timeLeft = 100000;
			projectile.aiStyle = 1;
		}
		
	}
}
		
			