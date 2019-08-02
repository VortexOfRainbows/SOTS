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
    public class PsychoLanceProj : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Psychic Shock");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(156);
            aiType = 156; //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.penetrate = 1; 
			projectile.ranged = true;
			projectile.width = 30;
			projectile.height = 33;
			projectile.thrown = true;
			projectile.tileCollide = true;
			
			
		}
		
		public override void AI()
		{ 
		int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 30, 33, 222);
		
		int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 30, 33, 222);
		
		int num3 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 30, 33, 222);
			
			
Main.dust[num1].noGravity = true;
Main.dust[num1].velocity *= 0.1f;
			
Main.dust[num2].noGravity = true;
Main.dust[num2].velocity *= 0.1f;
Main.dust[num3].noGravity = true;
Main.dust[num3].velocity *= 0.1f;
			
}
		public override void Kill(int timeLeft)
		{
			
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0,  696, (int)(projectile.damage * 1f), projectile.knockBack, Main.myPlayer);
			
		}
		
	}
}
		