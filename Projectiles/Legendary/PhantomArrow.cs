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

namespace SOTS.Projectiles.Legendary
{    
    public class PhantomArrow : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poltergeist");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; //18 is the demon scythe style
			projectile.width = 14;
			projectile.height = 30;
			projectile.alpha = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 255;
			projectile.aiStyle = 1;
		}
		
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 2.4f / 255f, (255 - projectile.alpha) * 2.55f / 255f, (255 - projectile.alpha) * 1.35f / 255f);
			projectile.alpha++;
		}
	}
}
		
			