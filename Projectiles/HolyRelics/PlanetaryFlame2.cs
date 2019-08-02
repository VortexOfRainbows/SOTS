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
    public class PlanetaryFlame2 : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Flame");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(181);
            aiType = 181; //18 is the demon scythe style
			projectile.penetrate = 2;
			projectile.timeLeft = 900;
			projectile.width = 21;
			projectile.height = 39;
            Main.projFrames[projectile.type] = 4;
			projectile.tileCollide = false;


		}
		
		public override void AI()
		{
			wait += 1;
			if(wait >= 30)
			{
			projectile.frame++;
			wait = 0;
			}
			if(projectile.frame > 3)
			{
				projectile.frame = 0;
			}
			projectile.rotation = 0;
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 21, 39, 160);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			
			
			
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
			
		}
 
			
		
	}
}
		
			