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

namespace SOTS.Projectiles.Laser
{    
    public class ContinuumExplosion : ModProjectile 
    {	int expand = -1;
		            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Collapse Laser");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 24;
			projectile.width = 24;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 1;
        }
	}
}
		