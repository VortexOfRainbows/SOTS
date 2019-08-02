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
    public class SandyWater : ModProjectile 
    {	int bounce = 2000;
		float up = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandy Water");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 2;
			projectile.height = 24;
			projectile.width = 24;
			projectile.thrown = true;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(13, (int)(projectile.Center.X), (int)(projectile.Center.Y), 13);
			for(int i = 0; i < 25; i++)
			{
				
			if(projectile.owner == Main.myPlayer)
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, Main.rand.Next(-6,7), Main.rand.Next(-6,7), mod.ProjectileType("SandyCloud"), (int)(projectile.damage * 1f), projectile.knockBack, Main.myPlayer);
		
			}
		}
		
	}
}
		