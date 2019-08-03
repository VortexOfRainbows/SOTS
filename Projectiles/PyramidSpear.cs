using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class PyramidSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperial Guardsman's Pike");
			
		}
		
        public override void SetDefaults()
        {
            
            projectile.melee = true;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
			projectile.CloneDefaults(47);
            projectile.hide = true;
            aiType = 47;
			projectile.alpha = 0;
            projectile.width = 88;
            projectile.height = 90; 
            projectile.timeLeft = 70;
		}
		public override void AI()
		{
			Main.player[projectile.owner].direction = projectile.direction;
			Main.player[projectile.owner].heldProj = projectile.whoAmI;
			Main.player[projectile.owner].itemTime = Main.player[projectile.owner].itemAnimation;
			projectile.position.X = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - (float)(projectile.width / 2);
			projectile.position.Y = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - (float)(projectile.height / 2);
			projectile.position += projectile.velocity * projectile.ai[0]; if (projectile.ai[0] == 0f)
		    {
			    projectile.ai[0] = 3f;
			    projectile.netUpdate = true;
		    }
		    if (Main.player[projectile.owner].itemAnimation < Main.player[projectile.owner].itemAnimationMax / 3)
		    {
			    projectile.ai[0] -= 1.1f;
		    }
		    else
		    {
			    projectile.ai[0] += 0.80f;
		    }
		
		    if (Main.player[projectile.owner].itemAnimation == 0)
		    {
			    projectile.Kill();
		    }
		
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 2.355f;
            if (projectile.spriteDirection == -1)
            {
        	    projectile.rotation -= 1.57f;
            }
		}
	}
}