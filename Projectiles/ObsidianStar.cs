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
    public class ObsidianStar : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(3);
            aiType = 3;
			projectile.width = 32;
			projectile.height = 32;
			projectile.penetrate = 1;
			
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			//projectile.rotation += 1f;
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 32, 32, 6);
			
			int i = (int)(projectile.position.X / 16);
			int j =	(int)(projectile.position.Y / 16);
			if(!Main.tile[i, j].active())
			{
				wait++;
			}
			if(wait >= 5)
			{
			projectile.tileCollide = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2, 4, 85, projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1, 5, 85, projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 6, 85, projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1, 5, 85, projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2, 4, 85, projectile.damage, projectile.knockBack, Main.myPlayer);
			}
			
		}
	}
}
		
			