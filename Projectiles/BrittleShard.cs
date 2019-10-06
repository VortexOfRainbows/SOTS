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
    public class BrittleShard : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BrittleShard");	
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; //18 is the demon scythe style
			projectile.penetrate = 2;
			projectile.alpha = 0;
			projectile.width = 10;
			projectile.height = 12;


		}
	}
}