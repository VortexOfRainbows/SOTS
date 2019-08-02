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
    public class Dracula : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dracula");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.timeLeft = 220;
			projectile.width = 1;
			projectile.height = 1;


		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			
			int healdata = (int)(((damage - target.defense)/20) + 1);
			//if(healdata >= 1)
			//{
			//owner.statLife += healdata;
		//owner.HealEffect(healdata);
			//}
			//else
			
			owner.statLife += 1;
			owner.HealEffect(1);
			
            
		}
		public override void AI()
		{projectile.alpha = 255;
			wait += 1;
			if(wait >= 4)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, 235);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
			
			
			
			
			

}
	}
}
		
			