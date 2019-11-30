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
    public class Bloodaxe : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Hammer");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(274);
            aiType = 274;
			projectile.alpha = 0;
			projectile.timeLeft = 200;
			projectile.width = 40;
			projectile.height = 40;
			projectile.penetrate = 10;
		}
		public override void AI()
		{
			if(projectile.timeLeft < 200)
			projectile.alpha++;
			if(projectile.penetrate < 4)
			{
				projectile.friendly = false;
				projectile.alpha += 3;
			}
			if(projectile.alpha > 240)
			{
				projectile.Kill();
			}
			projectile.rotation += 0.5f;
					
			float minDist = 360;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 0.5f;
			if(projectile.friendly == true && projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length - 1; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active)
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
					}
				}
			}
			
		}
	}
}
		
			