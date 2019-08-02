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

namespace SOTS.Projectiles.Chess
{    
    public class SightHoney : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sight Honey");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 16;
			projectile.width = 16;
			projectile.ranged = true;
			projectile.friendly = false;
			projectile.timeLeft = 150;
			projectile.hostile = false;
		}
		
		public override void Kill(int timeLeft)
		{
			
			int xPosition = (int)(projectile.Center.X / 16.0f);
            int yPosition = (int)(projectile.Center.Y / 16.0f);
			
						Main.tile[xPosition, yPosition].liquidType(2);
						Main.tile[xPosition, yPosition].liquid = 255;
						WorldGen.SquareTileFrame(xPosition, yPosition, false);
		}
	}
}
		