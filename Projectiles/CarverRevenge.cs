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
    public class CarverRevenge : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Revenge Bolt");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.height = 26;
			projectile.width = 20;
			projectile.ranged = false;
			projectile.timeLeft = 1800;
			projectile.friendly = false;
			projectile.hostile = true;
		}
		
		public override void AI()
		{ 
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 20, 26, 235);
Main.dust[num1].noGravity = true;
Main.dust[num1].velocity *= 0.1f;
			
		}
	}
}
		