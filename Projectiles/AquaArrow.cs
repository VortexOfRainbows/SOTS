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
    public class AquaArrow : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aquatic Arrow");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; //18 is the demon scythe style
			projectile.alpha = 25;
			projectile.timeLeft = 220;
			projectile.width = 22;
			projectile.height = 40;
			projectile.aiStyle = 1;
		}
		
		public override void AI()
		{
			Dust.NewDust(new Vector2(projectile.Center.X -4, projectile.Center.Y -4), 4, 4, 15);
			wait += 1;
			if(wait == 30)
			{
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, 22, (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);

			}
			
			
			if(wait == 60){
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, 22, (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);

			}
			
			
			if(wait == 90)
			{
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, 22, (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);

			}
			
			if(wait >= 200)
			{
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 3, 22, (int)(projectile.damage * 1.2), projectile.knockBack, Main.myPlayer);

			}
}
	}
}
		
			