using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class ThunderOrb : ModProjectile 
    {	int split;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zap");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(254);
            aiType = 254        ; //18 is the demon scythe style
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.ranged = true; 
			projectile.width = 32;
			projectile.height = 32;
			projectile.timeLeft = 50;
            Main.projFrames[projectile.type] = 5;
			projectile.alpha = 0;
		}
		public override void AI()
        {
			projectile.alpha += 5;
		}
		
		
}
}
			