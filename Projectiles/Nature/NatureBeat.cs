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

namespace SOTS.Projectiles.Nature
{    
    public class NatureBeat : ModProjectile 
    {	int expand = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Blast");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 40;
			projectile.width = 40;
            Main.projFrames[projectile.type] = 4;
			projectile.penetrate = -1;
			projectile.hostile = true;
			projectile.timeLeft = 19;
			projectile.tileCollide = false;
			projectile.alpha = 0;
		}
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 4.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f);
			projectile.knockBack = 3.5f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }
        }
	}
}
		