using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class SpiderSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spider Bite");
			
		}
		
        public override void SetDefaults()
        {
            projectile.CloneDefaults(66);
            aiType = 66;
            projectile.melee = true;
			projectile.alpha = 0; 
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
				target.AddBuff(BuffID.Venom, 180, false);
			
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(BuffID.Venom, 180, false);
			}
}
	}
}
