using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class ShadowflameCopy : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16; 
            projectile.timeLeft = 30;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = false; 
            projectile.magic = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void AI()
        {
			projectile.type = 659;
			projectile.alpha = 255;	
			
			projectile.velocity.Y += (0.35f * (Main.rand.Next(-3, 4)));
			
			projectile.velocity.X += (0.35f * (Main.rand.Next(-3, 4)));
			
			for(int i = 0; i < 10; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 16, 16, 27);
			
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}

        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
				target.AddBuff(153, 360, false);
			
		}	
		
		
		
		}
		
	}
	
