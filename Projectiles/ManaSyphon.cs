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
    public class ManaSyphon : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Syphon");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; //18 is the demon scythe style
			projectile.ranged = true;
			projectile.width = 18;
			projectile.height = 26;
		}
		
		public override void AI()
		{ 

			
			
}
		
		public override void OnHitNPC(NPC n, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			
			owner.statMana += 10;
			owner.HealEffect(10);
            
		}
	}
}
		