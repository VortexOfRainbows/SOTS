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

namespace SOTS.Projectiles.Margrit
{    
    public class PlanetaryBeam : ModProjectile 
    {	int bounce = 8;
		int wait = 1;              
		float Speed = 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Homing Beam");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.width = 4;
			projectile.height = 4;
			projectile.magic = true;
			projectile.timeLeft = 90000;
			projectile.penetrate = -1;
			projectile.alpha = 255;
		}
		
		public override void AI()
		{ 
			projectile.alpha = 255;
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 4, 4, 133);
			
						
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
					float minDist = 700;
					int target2 = -1;
					float dX = 0f;
					float dY = 0f;
					float distance = 0;
					float speed = 0.8f;
					if(projectile.friendly == true && projectile.hostile == false)
					{
						for(int i = 0; i < Main.npc.Length - 1; i++)
						{
							NPC target = Main.npc[i];
							if(!target.friendly && target.dontTakeDamage == false)
							{
								dX = target.Center.X - projectile.Center.X;
								dY = target.Center.Y - projectile.Center.Y;
								distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
								if(distance < minDist)
								{
									minDist = distance;
									target2 = i;
								}
							}
						}
						
						if(target2 != -1)
						{
						NPC toHit = Main.npc[target2];
							if(toHit.active == true)
							{
								
							dX = toHit.Center.X - projectile.Center.X;
							dY = toHit.Center.Y - projectile.Center.Y;
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							speed /= distance;
						   
							projectile.velocity += new Vector2(dX * speed, dY * speed);
							projectile.tileCollide = false;
							}
						}
						
						
						
					}
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.timeLeft -= 22000;
        }
	}
}
		