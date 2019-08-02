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
    public class RedFire : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Redfire");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22; 
            projectile.timeLeft = 320;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = true; 
            projectile.tileCollide = false;
            projectile.ignoreWater = false; 
            projectile.magic = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void AI()
		{
		Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 22, 22, 235);
		}
		public override void Kill(int timeLeft)
        {
 
			
				 NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, mod.NPCType("DemonCell"));
        
        }
	}
}
		
			