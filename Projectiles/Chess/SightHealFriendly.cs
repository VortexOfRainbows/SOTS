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

namespace SOTS.Projectiles.Chess
{    
    public class SightHealFriendly : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sight Heal");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 256;
			projectile.width = 256;
			projectile.friendly = false;
			projectile.timeLeft = 150;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			
		}
		public override void AI()
		{
			
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 75);
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 75);
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 75);
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 75);
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 256, 256, 75);
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.statLife += 50;
			target.HealEffect(50);
}
	}
}
		