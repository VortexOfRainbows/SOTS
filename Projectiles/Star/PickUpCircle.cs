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

namespace SOTS.Projectiles.Star
{    
    public class PickUpCircle : ModProjectile 
    {	int wait = 0;
	float rotate = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PickUpCircle");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(274);
            aiType = 274; //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.timeLeft = 6;
			projectile.width = 60;
			projectile.height = 60;
			projectile.friendly = false;
			projectile.hostile = false;

		}
		public override void PostAI()
		{
	
				Player owner = Main.player[projectile.owner];
				if(owner.FindBuffIndex(mod.BuffType("FrozenThroughTime")) > -1)
				{
					projectile.timeLeft = 6;
				}
				projectile.position.X = owner.Center.X + (projectile.position.X - projectile.Center.X);
				projectile.position.Y = owner.Center.Y - (projectile.Center.Y - projectile.position.Y);
				for(int i = 0; i < 255; i++)
				{
				Player player2 = Main.player[i];
				
				if(player2 != owner)
				{
					if(player2.Center.X + 32 > owner.Center.X && player2.Center.X  - 32 < owner.Center.X  && player2.Center.Y + 32 > owner.Center.Y && player2.Center.Y - 32 < owner.Center.Y)
					{
						owner.position.X = player2.position.X;
						owner.position.Y = player2.position.Y - 30;
					}
				}
			}
		}			
	}
}

		
			