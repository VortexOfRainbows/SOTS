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
    public class Rainbolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rainbolt");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(27);
            aiType = 27; 
		}
		public override void AI()
		{ 
			projectile.alpha = 255;
		//Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 59); //blue
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 16, 16, 60); //red
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 16, 16, 61); //green
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 16, 16, 62); //purple
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 16, 16, 63); //white
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 16, 16, 64); //yellow

			
			
}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 3;
        }

	}
}
		