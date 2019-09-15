using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 

namespace SOTS.Projectiles 
{    
    public class EctoSpark : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ectoplasm Glyph");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(316);
            aiType = 316;
            projectile.width = 18;
            projectile.height = 32; 
            projectile.timeLeft = 240;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
			projectile.alpha = 100;
            Main.projFrames[projectile.type] = 8;
		}
		
		public override void AI()
        {
			
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 18, 32, 206);
			
        }
		public override void Kill(int timeLeft)
		{
			
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 18, 32, 206);
		}

	}
}
		
			