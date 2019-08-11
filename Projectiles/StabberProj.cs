using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class StabberProj : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Trident");
			
		}
		
        public override void SetDefaults()
        {
            projectile.CloneDefaults(66);
            aiType = 66;
			projectile.alpha = 0;
            projectile.width = 50;
            projectile.height = 50; 
            projectile.timeLeft = 7;
		}
		public override void OnHitNPC(NPC n, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			
			owner.statLife += 1;
			owner.HealEffect(1);
            
		}
	}
}
		
			