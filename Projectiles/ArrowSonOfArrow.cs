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
    public class ArrowSonOfArrow : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arrow Son Of Arrow");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1;
			projectile.penetrate = 1; 
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.width = 18;
			projectile.height = 38;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 35; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, 1);
			
			
			Main.dust[num1].noGravity = true;
			//Main.dust[num1].velocity *= 0.1f;
			
			}
			
			if(projectile.knockBack > 1)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, 612, projectile.damage, 1, Main.myPlayer, 0f, 0f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			bool stopCheck = false;
			int startingChain = target.whoAmI;
			for(int i = startingChain; i < 200; i++)
			{
				if(Main.npc[i] != target && projectile.knockBack < 1 && projectile.knockBack > 0 && !stopCheck && projectile.owner == Main.myPlayer)
				{
					NPC target2 = Main.npc[i];
					
					float shootFromX = target.Center.X;
					float shootFromY = target.Center.Y;
				
					if(target2.Center.X >= target.Center.X)
					shootFromX += target.width;
						
					if(target2.Center.X <= target.Center.X)
					shootFromX -= target.width;
						
					if(target2.Center.Y >= target.Center.Y)
					shootFromY += target.height;
						
					if(target2.Center.Y <= target.Center.Y)
					shootFromY -= target.height;
				
					float shootToX = target2.position.X + (float)target2.width * 0.5f - shootFromX;
					float shootToY = target2.position.Y + (float)target2.height * 0.5f - shootFromY;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					float velocity = (float)System.Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
				
					
					if(distance < 800f && !target2.friendly && target2.active)
					{
							//Dividing the factor of 3f which is the desired velocity by distance
							distance = velocity / distance;
				
							//Multiplying the shoot trajectory with distance times a multiplier if you so choose to
							shootToX *= distance * 2;
							shootToY *= distance * 2;
				
							//Shoot projectile and set ai back to 0
							Projectile.NewProjectile(shootFromX, shootFromY, shootToX, shootToY, mod.ProjectileType("ArrowSonOfArrow"), projectile.damage, projectile.knockBack -= 0.1f, Main.myPlayer, 0f, 0f); 
							stopCheck = true;
							break;
					}
				}
			}
			for(int i = 0; i < startingChain; i++)
			{
				if(Main.npc[i] != target && projectile.knockBack < 1 && projectile.knockBack > 0 && !stopCheck && projectile.owner == Main.myPlayer)
				{
					NPC target2 = Main.npc[i];
					
					float shootFromX = target.Center.X;
					float shootFromY = target.Center.Y;
				
					if(target2.Center.X >= target.Center.X)
					shootFromX += target.width;
						
					if(target2.Center.X <= target.Center.X)
					shootFromX -= target.width;
						
					if(target2.Center.Y >= target.Center.Y)
					shootFromY += target.height;
						
					if(target2.Center.Y <= target.Center.Y)
					shootFromY -= target.height;
						
					float shootToX = target2.position.X + (float)target2.width * 0.5f - shootFromX;
					float shootToY = target2.position.Y + (float)target2.height * 0.5f - shootFromY;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					float velocity = (float)System.Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
					if(distance < 800f && !target2.friendly && target2.active)
					{
							//Dividing the factor of 3f which is the desired velocity by distance
							distance = velocity / distance;
				
							//Multiplying the shoot trajectory with distance times a multiplier if you so choose to
							shootToX *= distance * 2;
							shootToY *= distance * 2;
				
							//Shoot projectile and set ai back to 0
							Projectile.NewProjectile(shootFromX, shootFromY, shootToX, shootToY, mod.ProjectileType("ArrowSonOfArrow"), projectile.damage, projectile.knockBack -= 0.1f, Main.myPlayer, 0f, 0f); 
							stopCheck = true;
							break;
					}
				}
			}
		}
	}
}
		