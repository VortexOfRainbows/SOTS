using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles 
{    
    public class Heartdrop : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heartdrop");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(ProjectileID.VortexBeaterRocket);
            aiType = ProjectileID.VortexBeaterRocket; 
			projectile.alpha = 255;
			projectile.timeLeft = 900;
			projectile.width = 4;
			projectile.height = 4;
			projectile.penetrate = 1;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(target.life <= 0)
			{		
				Main.player[projectile.owner].statLife += 8;
				Main.player[projectile.owner].HealEffect(8);
				projectile.Kill();
			}
		}
		int counter = 0;
		public override void AI()
		{
			projectile.alpha = 255;
			counter++;
			if(counter >= 10)
			{
				for(int i = 0; i < 4; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), 4, 4, 60);
					dust.noGravity = true;
					dust.velocity *= 0.1f;
				}
			}
		}
	}
}
		
			