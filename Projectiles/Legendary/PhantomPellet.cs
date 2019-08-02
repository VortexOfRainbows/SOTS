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
    public class PhantomPellet : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poltergeist");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 10;
			projectile.height = 10;
			projectile.alpha = 0;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 255;
			projectile.aiStyle = 0;
		}
		
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.2f / 255f, (255 - projectile.alpha) * 1.275f / 255f, (255 - projectile.alpha) * 0.675f / 255f);
			projectile.alpha++;
		}
	}
}
		
			