using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles 
{    
    public class ControlledChaos : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Of Destruction");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.timeLeft = 30;
		}
		public override void AI()
		{
            Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 206);
		    Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 206);
		    Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 206);
		    Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 206);
			projectile.alpha = 255;
		}
		public override void Kill(int timeLeft)
        {
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 2;     //this is the explosion radius, the highter is the value the bigger is the explosion
 
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);
 
                    if (Math.Sqrt(x * x + y * y) <= radius + 0.5)   //this make so the explosion radius is a circle
                    {
                        WorldGen.KillTile(xPosition, yPosition, false, false, false);  //this make the explosion destroy tiles  
                        Dust.NewDust(position, 22, 22, 206, 0.0f, 0.0f, 0, new Color(), 1f);  //this is the dust that will spawn after the explosion
                    }
                }
            }
        }
	}
}
		
			