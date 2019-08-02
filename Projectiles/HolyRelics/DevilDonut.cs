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

namespace SOTS.Projectiles.HolyRelics
{    
    public class DevilDonut : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Use Demon Blood");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 16;
			projectile.width = 16;
			projectile.friendly = false;
			projectile.timeLeft = 150;
			projectile.hostile = false;
		}
		
		public override void Kill(int timeLeft)
		{
			
            Player player = Main.player[Main.myPlayer];
			if(player.statLife == 1 && !NPC.AnyNPCs(mod.NPCType("Libra")))
			{
				 NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, mod.NPCType("Libra"));	
			}
		}
	}
}
		