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
    public class CaveIn : ModProjectile 
    {	int bounce = 2000;
		float up = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cave In");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 2;
			projectile.height = 18;
			projectile.width = 18;
			projectile.penetrate = 1;
			projectile.friendly = false;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
		}
		public override void Kill(int timeLeft)
		{
			 Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 3;     //this is the explosion radius, the highter is the value the bigger is the explosion
 
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);
 
                    if (Math.Sqrt(x * x + y * y) <= radius + 0.5)   //this make so the explosion radius is a circle
                    {
                        if(Main.rand.Next(5) != 4 && Main.tile[xPosition, yPosition].type == 0)
						Main.tile[xPosition, yPosition].type = 123;
							
                        if(Main.rand.Next(4) != 3 && Main.tile[xPosition, yPosition].type == 1)
						Main.tile[xPosition, yPosition].type = 123;
					
                        if(Main.rand.Next(4) != 3 && Main.tile[xPosition, yPosition].type == 2)
						Main.tile[xPosition, yPosition].type = 123;
					
						if(Main.rand.Next(4) == 0 && ( Main.tile[xPosition, yPosition].type == 2 || Main.tile[xPosition, yPosition].type == 1 || Main.tile[xPosition, yPosition].type == 0 || Main.tile[xPosition, yPosition].type == 123))
						{
									WorldGen.KillTile(xPosition, yPosition, false, false, false);
						}
					}
				}
			}
		
	}
}
}
	