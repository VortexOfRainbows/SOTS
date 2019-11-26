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

namespace SOTS.Projectiles.Celestial
{    
    public class BluePunch : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Fist");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.width = 56;
			projectile.height = 30;
			projectile.timeLeft = 96;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 20;
            Main.projFrames[projectile.type] = 5;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 3;
		}
		public override void AI()
		{ 
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1f / 255f, (255 - projectile.alpha) * 1f / 255f, (255 - projectile.alpha) * 2.4f / 255f);
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			projectile.velocity *= 0.99f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 5;
            }
			projectile.alpha += 2;	
			
			int rotation = 30 * (int)projectile.ai[1];
			
			if(projectile.timeLeft < 101 && projectile.timeLeft > 27)
			{
				Vector2 circularLocation = new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f).RotatedBy(MathHelper.ToRadians(rotation));
				
				Player player  = Main.player[projectile.owner];
				projectile.velocity *= 0.845f;
				projectile.velocity += circularLocation;
			}
		}
	}
}
		