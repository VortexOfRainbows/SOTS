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
    public class EtherealSpawner : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Spawner");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 28;
			projectile.width = 28;
			projectile.friendly = false;
			projectile.timeLeft = 150;
			projectile.penetrate = -1;
			projectile.hostile = true;
		}
		
		public override void Kill(int timeLeft)
		{
			NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, mod.NPCType("EtherealChaser"));	
		}
	}
}
		