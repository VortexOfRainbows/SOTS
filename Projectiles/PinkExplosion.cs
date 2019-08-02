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
    public class PinkExplosion : ModProjectile 
    {	int expand = -1;
		int expertModifier = 1;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulverizer");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 105;
			projectile.width = 105;
            Main.projFrames[projectile.type] = 5;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 36;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 0;
		}
		public override void AI()
        {
			if(Main.expertMode)
			{
				expertModifier = 2;
			}
			if(expand == -1)
			{
				Main.PlaySound(SoundID.Item14, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				expand = 0;
					for(int i = 0; i < 4 + (expertModifier * 4); i++)
					{ 
						int npc = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npc].velocity.X = Main.rand.Next(-3,4) * expertModifier;
						Main.npc[npc].velocity.Y = Main.rand.Next(-3,4) * expertModifier;
					}
			}
			projectile.knockBack = 3.5f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 8)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 5;
            }
        }
	}
}
		