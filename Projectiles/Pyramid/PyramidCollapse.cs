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

namespace SOTS.Projectiles.Pyramid
{    
    public class PyramidCollapse : ModProjectile 
    {	int wait = 0;       
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fallen Bricks");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 1;
			projectile.height = 16;
			projectile.width = 16;
			projectile.friendly = false;
			projectile.timeLeft = 7200;
			projectile.hostile = true;
			projectile.alpha = 0;
			projectile.tileCollide = false;
            Main.projFrames[projectile.type] = 3;
			projectile.netImportant = true;
		}
		public override void AI()
		{
			int i = (int)(projectile.Center.X / 16);
			int j =	(int)(projectile.Center.Y / 16);
			if(!Main.tile[i, j].active())
			{
				wait++;
			}
			if(wait == 2)
			{
				projectile.tileCollide = true;
			}
            projectile.frameCounter++;
            if (projectile.frameCounter == 1)
            {
                projectile.frame = (projectile.frame + Main.rand.Next(3)) % 3;
            }
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			projectile.tileCollide = false;
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X * 0.1f;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y * 0.1f;
				}
			return false;
		}
	}
}
		