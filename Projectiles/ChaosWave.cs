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
    public class ChaosWave : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Of Destruction");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.alpha = 255;
			projectile.timeLeft = 1000000;
		}
		public override void AI()
		{
			for(int i = 0; i < 4; i++)
			{
				Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 206);
			}
			projectile.alpha = 255;
		}
		public override void Kill(int timeLeft)
        {
 
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 20;   
 
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);
 
                    if (Math.Sqrt(x * x + y * y) <= radius + 0.5)
                    {
                        WorldGen.KillTile(xPosition, yPosition, false, false, false); 
                        Dust.NewDust(position, 22, 22, DustID.Smoke, 0.0f, 0.0f, 206, new Color(), 1f); 
                    }
                }
            }
			
        }
	}
}
		
			