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

namespace SOTS.Projectiles.Ores
{    
    public class HealProj : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heal Proj");
		}
        public override void SetDefaults()
        {
			projectile.height = 8;
			projectile.width = 8;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 720;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if(projectile.timeLeft < 720)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X , projectile.position.Y), projectile.width, projectile.height, 16);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				
				projectile.velocity = new Vector2(-14.5f, 0).RotatedBy(Math.Atan2(projectile.Center.Y - player.Center.Y, projectile.Center.X - player.Center.X));
				Vector2 toPlayer = player.Center - projectile.Center;
				float distance = toPlayer.Length();
				if(distance < 20)
				{
					player.statLife += (int)projectile.ai[0];
					player.HealEffect((int)projectile.ai[0]);
					projectile.Kill();
				}
			}
		}
	}
}
		