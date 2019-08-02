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
    public class TrinityArrow : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chlorophyte Shock");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(98);
            aiType = 98; //18 is the demon scythe style
			projectile.penetrate = 1; 
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.width = 14;
			projectile.height = 30;
		}
		
		public override void AI()
		{ 
		
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 14, 30, 72);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			

			
			
}
	}
}
		