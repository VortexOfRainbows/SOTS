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
    public class PinkExplosion : ModProjectile 
    {	int expand = -1;
		int expertModifier = 1;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink Explosion");
			
		}
		
        public override void SetDefaults()
        {
			Projectile.height = 105;
			Projectile.width = 105;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 36;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
		public override bool PreAI()
		{
			return true;
		}
		public override void AI()
        {
			Projectile.rotation = Projectile.ai[0];
			Projectile.knockBack = 3.5f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
        }
	}
}
		