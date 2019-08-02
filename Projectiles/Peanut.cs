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
    public class Peanut : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("A Peanut");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 2;
			projectile.thrown = false;
			projectile.friendly = true;
			projectile.width = 26;
			projectile.height = 26;
			projectile.timeLeft = 9000;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
		
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -.9f * oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -.9f * oldVelocity.Y;
				}
				projectile.timeLeft -= 2000;
			
			return false;
		}
		public override void AI()
		{ 
		projectile.velocity.X *= 0.989f;
		projectile.velocity.Y *= 0.989f;
		projectile.alpha = 0;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.timeLeft -= 15000;
        }
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 4; i++)
			{
			Main.PlaySound(0, projectile.position);
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 26, 26, 0);
			int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 26, 26, 7);
			}
			
		}
	}
}
		