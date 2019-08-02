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
    public class TurnTime : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Time Turner");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(166);
            aiType = 166; //18 is the demon scythe style
			projectile.width = 22;
			projectile.height = 22;
			projectile.magic = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 9000;

		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
				target.AddBuff(mod.BuffType("FrozenThroughTime"), 280, false);
			
}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(mod.BuffType("FrozenThroughTime"), 280, false);
			}
}
			
}
	
}
		
			