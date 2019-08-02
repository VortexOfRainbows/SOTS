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

namespace SOTS.Projectiles.HolyRelics
{    
    public class CompressedMoonLord : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Satan");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.height = 72;
			projectile.width = 70;
			projectile.friendly = false;
			projectile.timeLeft = 1000000;
			projectile.hostile = false;
		}
		
		public override void Kill(int timeLeft)
		{
			
				 NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y,	398);	
		}
	}
}
		