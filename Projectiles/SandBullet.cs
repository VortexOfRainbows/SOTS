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
    public class SandBullet : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SandBullet");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14;
			projectile.alpha = 0;
			projectile.timeLeft = 220;
			projectile.width = 16;
			projectile.height = 16;


		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			if(projectile.owner == Main.myPlayer)
			{
				int Proj = Projectile.NewProjectile(target.Center.X + (projectile.velocity.X * 4), target.Center.Y + (projectile.velocity.Y * 4), projectile.velocity.X * -1f, projectile.velocity.Y * -1f, 507, projectile.damage, projectile.knockBack * 0.7f, owner.whoAmI);
				Main.projectile[Proj].timeLeft = 15;
				Main.projectile[Proj].alpha = 125;
				Main.projectile[Proj].tileCollide = false;
			}
				
    
		}
		public override void AI()
		{projectile.alpha = 0;
			wait += 1;
			if(wait >= 4)
			{
				for(int i = 0; i < 5; i++)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 16, 16, 32);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.1f;
				}
			}
			
			
			
			
			

}
	}
}
		
			