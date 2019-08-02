using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class CarryAir : ModProjectile 
    {	int alpha = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Air Cannon");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
            projectile.width = 40;
            projectile.height = 18; 
            projectile.timeLeft = 255;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
			projectile.alpha = 0;

		}
		public override void AI()
		{
			alpha++;
			projectile.alpha = alpha;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if(!target.boss)
			{
				target.immune[projectile.owner] = 1;
				if(Main.rand.Next(13) <= damage)
				{
					if(damage <= 3 && Main.rand.Next(11) < 2)
					{
					projectile.damage--;
					}
					else if(damage > 3)
					{
					projectile.damage--;
					}
				}
				if(target.width + target.height < 180)
				{
				target.position.X = projectile.Center.X + (target.position.X - target.Center.X);
				target.position.Y = projectile.Center.Y - (target.Center.Y - target.position.Y);
				}
			}
        }
	}
}