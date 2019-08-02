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
    public class RedLaser2 : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Destruction");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(304);
            aiType = 304; //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.magic = true;
			projectile.timeLeft = 100000;
			projectile.height = 30;
			projectile.width = 10;
			projectile.penetrate = -1;
		}
		
		public override void AI()
		{
			projectile.alpha = 0;
			projectile.rotation = 0;
			projectile.velocity.Y = 30;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
			
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
        }
		public override void Kill(int timeLeft)
		{
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
						Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), 10, 30, 235);
		}
	}
}
		
			