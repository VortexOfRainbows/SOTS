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
			Projectile.aiStyle = 1;
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.friendly = false;
			Projectile.timeLeft = 7200;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.tileCollide = false;
            Main.projFrames[Projectile.type] = 3;
			Projectile.netImportant = true;
		}
		public override void AI()
		{
			int i = (int)(Projectile.Center.X / 16);
			int j =	(int)(Projectile.Center.Y / 16);
			if(!Main.tile[i, j].active())
			{
				wait++;
			}
			if(wait == 2)
			{
				Projectile.tileCollide = true;
			}
            Projectile.frameCounter++;
            if (Projectile.frameCounter == 1)
            {
                Projectile.frame = (Projectile.frame + Main.rand.Next(3)) % 3;
            }
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			Projectile.tileCollide = false;
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X * 0.1f;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y * 0.1f;
				}
			return false;
		}
	}
}
		