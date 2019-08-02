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
    public class PutridPinkyEye : ModProjectile 
    {	int expand = 1;
		            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Eye");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 8;
			projectile.height = 16;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 6;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 0;
		}
		public override void AI()
        {
			if(NPC.AnyNPCs(mod.NPCType("PutridPinkyPhase2")))
			{
				projectile.timeLeft = 6;
			}
				
			if(expand == -1)
			{
				expand = 0;
					for(int i = 0; i < 8; i++)
					{ 
						int npc = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npc].velocity.X = Main.rand.Next(-3,4);
						Main.npc[npc].velocity.Y = Main.rand.Next(-3,4);
					}
				
			}
        }
	}
}
		