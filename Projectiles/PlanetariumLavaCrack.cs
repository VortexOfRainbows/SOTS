using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class PlanetariumLavaCrack : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetarium Crack");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(158);
            aiType = 158; //18 is the demon scythe style
            projectile.penetrate = -1; 
            projectile.width = 16;
            projectile.height = 16; 
            projectile.tileCollide = false;
            projectile.thrown = true; 
			projectile.timeLeft = 8;
			projectile.alpha = 0;
		}
		
		public override void AI()
        {
				
			int xPosition = (int)(projectile.Center.X / 16.0f);
                    int yPosition = (int)(projectile.Center.Y / 16.0f);
 
						WorldGen.PlaceTile(xPosition, yPosition, mod.TileType("EmptyPlanetariumBlock"));
						
						int type = 189;
						if(Main.hardMode)
						{
							type = 223;
						}
						WorldGen.PlaceTile(xPosition - 1, yPosition, type);
						WorldGen.PlaceTile(xPosition + 1, yPosition, type);
						WorldGen.PlaceTile(xPosition, yPosition - 1, type);
						WorldGen.PlaceTile(xPosition, yPosition + 1, type);
						WorldGen.PlaceTile(xPosition - 1, yPosition - 1, type);
						WorldGen.PlaceTile(xPosition + 1, yPosition - 1, type);
						WorldGen.PlaceTile(xPosition + 1, yPosition + 1, type);
						WorldGen.PlaceTile(xPosition -1, yPosition + 1, type);
						
						WorldGen.PlaceTile(xPosition +5, yPosition -2, 189);
						WorldGen.PlaceTile(xPosition -5, yPosition -2, 189);
						
						WorldGen.PlaceTile(xPosition +5, yPosition -1, 189);
						WorldGen.PlaceTile(xPosition -5, yPosition -1, 189);
						WorldGen.PlaceTile(xPosition +4, yPosition -1, 189);
						WorldGen.PlaceTile(xPosition -4, yPosition -1, 189);
						WorldGen.PlaceTile(xPosition +3, yPosition -1, 189);
						WorldGen.PlaceTile(xPosition -3, yPosition -1, 189);
						WorldGen.PlaceTile(xPosition +2, yPosition -1, 189);
						WorldGen.PlaceTile(xPosition -2, yPosition -1, 189);
			
						WorldGen.PlaceTile(xPosition +4, yPosition , 189);
						WorldGen.PlaceTile(xPosition -4, yPosition , 189);
						WorldGen.PlaceTile(xPosition +3, yPosition , 189);
						WorldGen.PlaceTile(xPosition -3, yPosition , 189);
						WorldGen.PlaceTile(xPosition +2, yPosition , 189);
						WorldGen.PlaceTile(xPosition -2, yPosition , 189);

						WorldGen.PlaceTile(xPosition +3, yPosition , 189);
						WorldGen.PlaceTile(xPosition -3, yPosition , 189);
						WorldGen.PlaceTile(xPosition +2, yPosition , 189);
						WorldGen.PlaceTile(xPosition -2, yPosition , 189);
						
						WorldGen.PlaceTile(xPosition +3, yPosition +1, 189);
						WorldGen.PlaceTile(xPosition -3, yPosition +1, 189);
						WorldGen.PlaceTile(xPosition +2, yPosition +1, 189);
						WorldGen.PlaceTile(xPosition -2, yPosition +1, 189);
						WorldGen.PlaceTile(xPosition +1, yPosition +1, 189);
						WorldGen.PlaceTile(xPosition -1, yPosition +1, 189);
						WorldGen.PlaceTile(xPosition , yPosition +1, 189);
						
						WorldGen.PlaceTile(xPosition +2, yPosition +2, 189);
						WorldGen.PlaceTile(xPosition -2, yPosition +2, 189);
						WorldGen.PlaceTile(xPosition +1, yPosition +2, 189);
						WorldGen.PlaceTile(xPosition -1, yPosition +2, 189);
						WorldGen.PlaceTile(xPosition , yPosition +2, 189);
						
						WorldGen.PlaceTile(xPosition +1, yPosition +3, 189);
						WorldGen.PlaceTile(xPosition -1, yPosition +3, 189);
						WorldGen.PlaceTile(xPosition , yPosition +3, 189);
						
						WorldGen.PlaceTile(xPosition , yPosition +4, 189);
						WorldGen.PlaceTile(xPosition , yPosition +5, 189);
						
						
						WorldGen.PlaceTile(xPosition, yPosition - 2, 58);
						WorldGen.PlaceTile(xPosition -1, yPosition - 2, 58);
						WorldGen.PlaceTile(xPosition +1, yPosition - 2, 58);
						WorldGen.PlaceTile(xPosition -2, yPosition - 2, 58);
						WorldGen.PlaceTile(xPosition +2, yPosition - 2, 58);
						WorldGen.PlaceTile(xPosition -3, yPosition - 2, 58);
						WorldGen.PlaceTile(xPosition +3, yPosition - 2, 58);
						WorldGen.PlaceTile(xPosition -4, yPosition - 2, 58);
						WorldGen.PlaceTile(xPosition +4, yPosition - 2, 58);
						WorldGen.PlaceTile(xPosition, yPosition - 3, 58);
						WorldGen.PlaceTile(xPosition -1, yPosition - 3, 58);
						WorldGen.PlaceTile(xPosition +1, yPosition - 3, 58);
						WorldGen.PlaceTile(xPosition -2, yPosition - 3, 58);
						WorldGen.PlaceTile(xPosition +2, yPosition - 3, 58);
						WorldGen.PlaceTile(xPosition -3, yPosition - 3, 58);
						WorldGen.PlaceTile(xPosition +3, yPosition - 3, 58);
						
}
}
}