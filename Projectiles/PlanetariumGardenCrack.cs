using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class PlanetariumGardenCrack : ModProjectile 
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
				
			int newXpos2 = (int)(projectile.Center.X / 16.0f);
                    int newYpos = (int)(projectile.Center.Y / 16.0f);
 
						WorldGen.PlaceTile(newXpos2, newYpos, mod.TileType("EmptyPlanetariumBlock"));
						WorldGen.PlaceTile(newXpos2 -7, newYpos - 6, 223);
						WorldGen.PlaceTile(newXpos2 +7, newYpos - 6, 223);
						WorldGen.PlaceTile(newXpos2 -6, newYpos - 6, 223);
						WorldGen.PlaceTile(newXpos2 +6, newYpos - 6, 223);
						
						WorldGen.PlaceTile(newXpos2 -7, newYpos - 5, 223);
						WorldGen.PlaceTile(newXpos2 +7, newYpos - 5, 223);
						WorldGen.PlaceTile(newXpos2 -6, newYpos - 5, 223);
						WorldGen.PlaceTile(newXpos2 +6, newYpos - 5, 223);
						WorldGen.PlaceTile(newXpos2 -5, newYpos - 5, 223);
						WorldGen.PlaceTile(newXpos2 +5, newYpos - 5, 223);
						
						WorldGen.PlaceTile(newXpos2 -7, newYpos - 4, 223);
						WorldGen.PlaceTile(newXpos2 +7, newYpos - 4, 223);
						WorldGen.PlaceTile(newXpos2 -4, newYpos - 4, 223);
						WorldGen.PlaceTile(newXpos2 +4, newYpos - 4, 223);
						WorldGen.PlaceTile(newXpos2 -5, newYpos - 4, 223);
						WorldGen.PlaceTile(newXpos2 +5, newYpos - 4, 223);
						
						WorldGen.PlaceTile(newXpos2 -6, newYpos - 4,mod.TileType("EmptyPlanetariumBlock"));
						WorldGen.PlaceTile(newXpos2 +6, newYpos - 4,mod.TileType("EmptyPlanetariumBlock"));
						
						WorldGen.PlaceTile(newXpos2 -7, newYpos - 3, 223);
						WorldGen.PlaceTile(newXpos2 +7, newYpos - 3, 223);
						WorldGen.PlaceTile(newXpos2 -4, newYpos - 3, 223);
						WorldGen.PlaceTile(newXpos2 +4, newYpos - 3, 223);
						WorldGen.PlaceTile(newXpos2 -5, newYpos - 3, 223);
						WorldGen.PlaceTile(newXpos2 +5, newYpos - 3, 223);
						WorldGen.PlaceTile(newXpos2 -3, newYpos - 3, 223);
						WorldGen.PlaceTile(newXpos2 +3, newYpos - 3, 223);
						WorldGen.PlaceTile(newXpos2 -6, newYpos - 3, 223);
						WorldGen.PlaceTile(newXpos2 +6, newYpos - 3, 223);
						
						WorldGen.PlaceTile(newXpos2 -8, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 +8, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 -7, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 +7, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 -4, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 +4, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 -5, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 +5, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 -3, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 +3, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 -2, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 +2, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 -6, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 +6, newYpos - 2, 223);
						WorldGen.PlaceTile(newXpos2 -1, newYpos - 2, 0);
						WorldGen.PlaceTile(newXpos2 +1, newYpos - 2, 0);
						WorldGen.PlaceTile(newXpos2 , newYpos - 2, 0);
						WorldGen.PlaceTile(newXpos2 -1, newYpos - 2, 2);
						WorldGen.PlaceTile(newXpos2 +1, newYpos - 2, 2);
						WorldGen.PlaceTile(newXpos2 , newYpos - 2, 2);
						WorldGen.PlaceTile(newXpos2 , newYpos - 3, 20);
						
						
						WorldGen.PlaceTile(newXpos2 -8, newYpos - 1, 223);
						WorldGen.PlaceTile(newXpos2 +8, newYpos - 1, 223);
						
						WorldGen.PlaceTile(newXpos2 -7, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 +7, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 -4, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 +4, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 -5, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 +5, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 -3, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 +3, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 -2, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 +2, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 -6, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 +6, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 -1, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 +1, newYpos - 1, 189);
						WorldGen.PlaceTile(newXpos2 , newYpos - 1, 189);
						
						WorldGen.PlaceTile(newXpos2 -4, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 +4, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 -5, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 +5, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 -3, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 +3, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 -2, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 +2, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 -6, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 +6, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 -1, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 +1, newYpos - 0, 189);
						WorldGen.PlaceTile(newXpos2 , newYpos - 0, mod.TileType("EmptyPlanetariumBlock"));
						
						WorldGen.PlaceTile(newXpos2 -4, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 +4, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 -5, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 +5, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 -3, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 +3, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 -2, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 +2, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 -1, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 +1, newYpos + 1, 189);
						WorldGen.PlaceTile(newXpos2 , newYpos + 1, 189);
						
						WorldGen.PlaceTile(newXpos2 -3, newYpos + 2, 189);
						WorldGen.PlaceTile(newXpos2 +3, newYpos + 2, 189);
						WorldGen.PlaceTile(newXpos2 -2, newYpos + 2, 189);
						WorldGen.PlaceTile(newXpos2 +2, newYpos + 2, 189);
						WorldGen.PlaceTile(newXpos2 -1, newYpos + 2, 189);
						WorldGen.PlaceTile(newXpos2 +1, newYpos + 2, 189);
						WorldGen.PlaceTile(newXpos2 , newYpos + 2, 189);
						
}
}
}