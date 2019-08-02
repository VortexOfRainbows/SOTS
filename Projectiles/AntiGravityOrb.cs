using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class AntiGravityOrb : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Levitation");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(126);
            aiType = 126; //18 is the demon scythe style
            projectile.penetrate = -1; 
            projectile.width = 16;
            projectile.height = 16; 
            projectile.tileCollide = false;
			projectile.timeLeft = 1800;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			projectile.velocity.Y -= 0.04f;
		}
}
}
			