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

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseSingularity : ModProjectile 
    {	
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Singularity");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 40;
			projectile.width = 44;
            Main.projFrames[projectile.type] = 11;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.timeLeft = 960;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.ranged = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			if(projectile.owner == Main.myPlayer) //bruh, unoptimal code be like
			{
				int Proj = Projectile.NewProjectile(target.Center.X + target.width, target.Center.Y + target.height, -7, -7, 114, projectile.damage, projectile.knockBack * 0.7f, owner.whoAmI);
				Main.projectile[Proj].timeLeft = 25;
				Main.projectile[Proj].alpha = 90;
				Main.projectile[Proj].magic = false;
				Main.projectile[Proj].ranged = true;
				Main.projectile[Proj].tileCollide = false;
				
				Proj = Projectile.NewProjectile(target.Center.X + target.width, target.Center.Y, -8, 0, 114, projectile.damage, projectile.knockBack * 0.7f, owner.whoAmI);
				Main.projectile[Proj].timeLeft = 25;
				Main.projectile[Proj].alpha = 90;
				Main.projectile[Proj].magic = false;
				Main.projectile[Proj].ranged = true;
				Main.projectile[Proj].tileCollide = false;
				
				Proj = Projectile.NewProjectile(target.Center.X + target.width, target.Center.Y - target.height, -7, 7, 114, projectile.damage, projectile.knockBack * 0.7f, owner.whoAmI);
				Main.projectile[Proj].timeLeft = 25;
				Main.projectile[Proj].alpha = 90;
				Main.projectile[Proj].magic = false;
				Main.projectile[Proj].ranged = true;
				Main.projectile[Proj].tileCollide = false;
				
				Proj = Projectile.NewProjectile(target.Center.X - target.width, target.Center.Y + target.height, 7, -7, 114, projectile.damage, projectile.knockBack * 0.7f, owner.whoAmI);
				Main.projectile[Proj].timeLeft = 25;
				Main.projectile[Proj].alpha = 90;
				Main.projectile[Proj].magic = false;
				Main.projectile[Proj].ranged = true;
				Main.projectile[Proj].tileCollide = false;
				
				Proj = Projectile.NewProjectile(target.Center.X - target.width, target.Center.Y, 8, 0, 114, projectile.damage, projectile.knockBack * 0.7f, owner.whoAmI);
				Main.projectile[Proj].timeLeft = 25;
				Main.projectile[Proj].alpha = 90;
				Main.projectile[Proj].magic = false;
				Main.projectile[Proj].ranged = true;
				Main.projectile[Proj].tileCollide = false;
				
				Proj = Projectile.NewProjectile(target.Center.X - target.width, target.Center.Y - target.height, 7, 7, 114, projectile.damage, projectile.knockBack * 0.7f, owner.whoAmI);
				Main.projectile[Proj].timeLeft = 25;
				Main.projectile[Proj].alpha = 90;
				Main.projectile[Proj].magic = false;
				Main.projectile[Proj].ranged = true;
				Main.projectile[Proj].tileCollide = false;
				
				Proj = Projectile.NewProjectile(target.Center.X, target.Center.Y - target.height, 0, 8, 114, projectile.damage, projectile.knockBack * 0.7f, owner.whoAmI);
				Main.projectile[Proj].timeLeft = 25;
				Main.projectile[Proj].alpha = 90;
				Main.projectile[Proj].magic = false;
				Main.projectile[Proj].ranged = true;
				Main.projectile[Proj].tileCollide = false;
				
				Proj = Projectile.NewProjectile(target.Center.X, target.Center.Y + target.height, 0, -8, 114, projectile.damage, projectile.knockBack * 0.7f, owner.whoAmI);
				Main.projectile[Proj].timeLeft = 25;
				Main.projectile[Proj].alpha = 90;
				Main.projectile[Proj].magic = false;
				Main.projectile[Proj].ranged = true;
				Main.projectile[Proj].tileCollide = false;
				
			}
		}
		public override void AI()
        {
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 40, 44, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].alpha = projectile.alpha;
			
			if(projectile.timeLeft % 6 == 0)
			{
				projectile.alpha++;
			}
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
				projectile.friendly = true;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 11;
            }
			
					float minDist = 560;
					int target2 = -1;
					float dX = 0f;
					float dY = 0f;
					float distance = 0;
					float speed = 0.75f;
					if(projectile.friendly == true && projectile.hostile == false)
					{
						for(int i = 0; i < Main.npc.Length - 1; i++)
						{
							NPC target = Main.npc[i];
							if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5)
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
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			return false;
		}
	}
}
		