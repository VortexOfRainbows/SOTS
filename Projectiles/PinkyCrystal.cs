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
    public class PinkyCrystal : ModProjectile 
    {	int bounce = 2000;
		float up = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worm Wood Crystal");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 2;
			projectile.height = 22;
			projectile.width = 22;
			projectile.thrown = true;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 4; i++)
			{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-6,7), Main.rand.Next(-6,7), mod.ProjectileType("PinkyMusketBall"), (int)(projectile.damage * 1f), projectile.knockBack, Main.myPlayer);
			
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-6,7), Main.rand.Next(-6,7), 22, (int)(projectile.damage * 1f), projectile.knockBack, Main.myPlayer);
			}
		}
		
	}
}
		