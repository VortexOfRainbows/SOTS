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

namespace SOTS.Projectiles.Crushers
{    
    public class EclipseBubble : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eclipse Bubble");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(410);
            aiType = 410; 
			projectile.melee = true;
			projectile.penetrate = -1;
		}
		public override void AI()
		{
			projectile.alpha = 260 - projectile.timeLeft;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
			projectile.friendly = false;
		}
	}
}
		
			