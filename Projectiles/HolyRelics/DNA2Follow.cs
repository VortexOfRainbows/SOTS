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

namespace SOTS.Projectiles.HolyRelics
{    
    public class DNA2Follow : ModProjectile 
    {	int bounce = 24;
		int wait = 1;         
				float oldVelocityY = 0;	
				float oldVelocityX = 0;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DNA");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.height = 10;
			projectile.width = 10;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 90;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.ranged = false;
		}
		public override void AI()
		{
			if(Main.rand.Next(5) == 0)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 10, 10, 68);
			
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
			projectile.friendly = false;
        }
	}
}
		