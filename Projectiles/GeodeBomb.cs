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
    public class GeodeBomb : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Bomb");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; //18 is the demon scythe style
			projectile.timeLeft = 18000;
			projectile.hostile = true;
			projectile.width = 20;
			projectile.height = 20;
			projectile.penetrate = -1;
		}
		public override void AI()
		{
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
		projectile.velocity.X = 0;
		projectile.position.X = (int)projectile.position.X;
		projectile.position.Y = (int)projectile.position.Y;
		
				projectile.Kill();
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 1;     //this is the explosion radius, the highter is the value the bigger is the explosion
					int type = 37;
                    int xPosition = (int)(position.X / 16.0f);
                    int yPosition = (int)(position.Y / 16.0f);
					if(Main.rand.Next(3) == 0)
					{
						type = mod.TileType("GeodeBlock");
					}
				WorldGen.PlaceTile(xPosition, yPosition, type);
				WorldGen.PlaceTile(xPosition +1, yPosition, type);
				WorldGen.PlaceTile(xPosition -1, yPosition, type);
				WorldGen.PlaceTile(xPosition, yPosition +1, type);
				WorldGen.PlaceTile(xPosition, yPosition -1, type);
				 
			
			return false;
			
		}
	}
}
		
			