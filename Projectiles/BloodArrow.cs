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
    public class BloodArrow : ModProjectile 
    {	int wait = 0;
		float rotate = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Arrow");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; //18 is the demon scythe style
			projectile.penetrate = 8;
			projectile.alpha = 0;
			projectile.width = 18;
			projectile.height = 38;


		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			rotate = projectile.rotation;
			projectile.tileCollide = false;
			projectile.timeLeft = 120;
			projectile.velocity.X *= 0.4f;
			projectile.velocity.Y *= 0.7f;
			return false;
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
		{
			if(!projectile.tileCollide)
			{
				projectile.velocity.X *= 0.5f;
				projectile.velocity.Y *= 0.7f;
				projectile.rotation = rotate;
			}
			if(projectile.timeLeft < 3)
			{
				for(int i = 0; i < 20; i++)
				{
					Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 18, 38, 235);
				}
			}
			projectile.alpha = 0;
			wait += 1;
			if(wait >= 4)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X - 3, projectile.Center.Y - 3), 2, 2, 235);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
		}
	}
}
		
			