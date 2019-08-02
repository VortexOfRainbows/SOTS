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
            
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
			projectile.CloneDefaults(47);
            aiType = 47; //18 is the demon scythe style
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
		
			