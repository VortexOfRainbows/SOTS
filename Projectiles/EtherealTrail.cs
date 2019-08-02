using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class EtherealTrail : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Bullet");
			
		}
		
        public override void SetDefaults()
        {
			
            projectile.penetrate = -1; 
            projectile.width = 8;
            projectile.height = 8; 
            projectile.tileCollide = false;
			projectile.timeLeft = 255;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			projectile.alpha++;
		}
}
}
			