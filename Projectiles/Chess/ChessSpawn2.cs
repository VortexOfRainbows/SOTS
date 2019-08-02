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
    public class ChessSpawn2 : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chess Spawn");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; //18 is the demon scythe style
			projectile.width = 40;
			projectile.height = 26;
			projectile.magic = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 360;
			projectile.tileCollide = false;

		}
		public override void AI()
		{
			
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 1;     //this is the explosion radius, the highter is the value the bigger is the explosion
 
                    int xPosition = (int)(position.X / 16.0f);
                    int yPosition = (int)(position.Y / 16.0f);
				WorldGen.PlaceTile(xPosition, yPosition, 223);
		}
		public override void Kill(int timeLeft)
		{
			
				 NPC.NewNPC((int)projectile.Center.X + 60, (int)projectile.Center.Y -40, mod.NPCType("ChessPortal2"));	
				 NPC.NewNPC((int)projectile.Center.X + 600, (int)projectile.Center.Y -120, mod.NPCType("Queen"));	
		}
			
}
	
}
		
			