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
    public class RimBeam : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Rainbow");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.penetrate = -1; 
			projectile.ranged = true;
			projectile.width = 20;
			projectile.height = 56;
			
			
            projectile.tileCollide = false;
		}
		
		public override void AI()
		{ 
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 59); //blue
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 60); //red
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 61); //green
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 62); //purple
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 63); //white
		
		Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 64); //yellow
		
			
			
}
		public override void Kill(int timeLeft)
		{
			
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 3, 0,  644, (int)(projectile.damage * 2.5f), projectile.knockBack, Main.myPlayer);
			
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 4;
        }
		
	}
}
		