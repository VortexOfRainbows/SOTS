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
    public class TurnTimeBeam : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hook Beam");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(260);
            aiType = 260; //18 is the demon scythe style
			projectile.width = 4;
			projectile.height = 4;
			projectile.magic = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 180;

		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
				target.AddBuff(mod.BuffType("FrozenThroughTime"), 360, false);
			
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(mod.BuffType("FrozenThroughTime"), 360, false);
			}
}
			
}
	
}
		
			