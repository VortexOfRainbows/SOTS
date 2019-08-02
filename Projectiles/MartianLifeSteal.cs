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
    public class MartianLifeSteal : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vampiric Laser");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.magic = true;
			projectile.width = 1;
			projectile.height = 1;
		}
		
		public override void OnHitNPC(NPC n, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			if((Main.rand.Next(7) == 0))
			{
			
			owner.statLife += 1;
			owner.HealEffect(1);
            }
		}public override void AI()
		{
		}
	}
}
		
			