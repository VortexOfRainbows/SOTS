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
    public class HornetGatt : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hornet Gattler");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(163);
            aiType = 163; 
			projectile.height = 22;
			projectile.width = 20;
			projectile.ranged = false;
			projectile.timeLeft = 900;
			projectile.friendly = false;
			projectile.hostile = false;
		}
		
		public override void AI()
		{ 
			wait++;
		if(projectile.velocity.X == 0 && wait >= 30)
		{
			wait = 0;
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -4, 55, 40, 0, 0);
		}
			
}
	}
}
		