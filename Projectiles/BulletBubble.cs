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
    public class BulletBubble : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Bubble");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(405);
            aiType = 405; //18 is the demon scythe style
			projectile.timeLeft = 600;
		}
		
		public override void AI()
		{
			
			projectile.alpha = 120;
			
			
}
	}
}
		
			