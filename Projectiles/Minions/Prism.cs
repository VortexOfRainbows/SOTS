using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Minions
{  
    public class Prism : ModProjectile
    {	int probe = -1;
		int firerate = 0;
		double dist = 128;
		int splitAmount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prism");
		}
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 26;
            projectile.height = 36;
            Main.projFrames[projectile.type] = 1;
            projectile.friendly = true;
            Main.projPet[projectile.type] = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 2f;
            projectile.penetrate = -1;
            projectile.timeLeft = 300000;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }
		public override void AI()
		{
			projectile.alpha = 75;
			{
				Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
				int shoot = mod.ProjectileType("MourningStarBeam");
				Player player = Main.player[projectile.owner];
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
				if (player.dead)
				{
					modPlayer.Prism = false;
				}
				if (modPlayer.Prism)
				{
					projectile.timeLeft = 2;
				}
				if(probe == -1)
				{
					probe = 0;
					float distanceX = player.Center.X - projectile.Center.X;
					float distanceY = player.Center.Y - projectile.Center.Y;
					float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
					dist = distance;
				}
				if(dist < 160)
				{
					dist = 160;
				}
				
				projectile.ai[1] += .5f;	
				projectile.ai[0]++;
				double deg = (double) projectile.ai[1]; 
				double rad = deg * (Math.PI / 180); 
			 
				projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
				projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
			 
				for(int i = 0; i < 200; i++)
				{
				   //Enemy NPC variable being set
				   NPC target = Main.npc[i];

				   //Getting the shooting trajectory
				   float shootToX = target.Center.X - projectile.Center.X;
				   float shootToY = target.Center.Y - projectile.Center.Y;
				   float distance2 = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				   //If the distance between the projectile and the live target is active
				   if(distance2 < 360f && !target.friendly && target.active)
				   {
					   if(projectile.ai[0] > 60f) //Assuming you are already incrementing this in AI outside of for loop
					   {
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance2 = 0.2f / distance2;
			   
						   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
						   shootToX *= distance2 * 5;
						   shootToY *= distance2 * 5;
			   
						   //Shoot projectile and set ai back to 0
						   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("MourningStarBeam"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
						  
						   projectile.ai[0] = 0f;
							
					   }
				   }
				}
			}
			
			firerate++;
				if(splitAmount >= 10)
				{
					projectile.Kill();
				}
				for(int i = 0; i < 1000; i++)
				{
					Projectile friendlyProj = Main.projectile[i];
					
					if(projectile.Center.X + 24 > friendlyProj.Center.X && projectile.Center.X - 24 < friendlyProj.Center.X && friendlyProj.sentry == false && projectile.Center.Y + 24 > friendlyProj.Center.Y && projectile.Center.Y - 24 < friendlyProj.Center.Y && friendlyProj.friendly == true && friendlyProj.owner == projectile.owner && friendlyProj.damage > 4 && friendlyProj.minion == false && friendlyProj.active && friendlyProj.type != mod.ProjectileType("MourningStarBeam"))
					{	
						if(firerate >= 12)
						{
							float VelocityX = friendlyProj.velocity.X;
							float VelocityY = friendlyProj.velocity.Y;
							friendlyProj.Kill();
							Vector2 perturbedSpeed = new Vector2(VelocityX, VelocityY).RotatedBy(MathHelper.ToRadians(5)); 
							Vector2 perturbedSpeed2 = new Vector2(VelocityX, VelocityY).RotatedBy(MathHelper.ToRadians(355)); 
							Vector2 perturbedSpeed3 = new Vector2(VelocityX, VelocityY).RotatedBy(MathHelper.ToRadians(15)); 
							Vector2 perturbedSpeed4 = new Vector2(VelocityX, VelocityY).RotatedBy(MathHelper.ToRadians(345)); 
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, friendlyProj.type, friendlyProj.damage, projectile.knockBack, Main.myPlayer);
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed2.X, perturbedSpeed2.Y, friendlyProj.type, friendlyProj.damage, projectile.knockBack, Main.myPlayer);
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed3.X, perturbedSpeed3.Y, friendlyProj.type, friendlyProj.damage, projectile.knockBack, Main.myPlayer);
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed4.X, perturbedSpeed4.Y, friendlyProj.type, friendlyProj.damage, projectile.knockBack, Main.myPlayer);
							splitAmount++;
							firerate = 0;
						}
					}
					
					
			}
			
		}
	}		
}
						
					