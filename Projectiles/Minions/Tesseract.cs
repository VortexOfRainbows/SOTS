using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Minions
{  
    public class Tesseract : ModProjectile
    {	int probe = -1;
		int firerate = 0;
		double dist = 128;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesseract");
		}
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 42;
            projectile.height = 42;
            Main.projFrames[projectile.type] = 1;
            projectile.friendly = true;
            Main.projPet[projectile.type] = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1f;
            projectile.penetrate = 1;
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
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
			int shoot = mod.ProjectileType("MourningStarBeam");
            Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if (player.dead)
            {
                modPlayer.Tesseract = false;
            }
            if (modPlayer.Tesseract)
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
					
			double deg = (double) projectile.ai[1]; 
			double rad = deg * (Math.PI / 180); 
			projectile.rotation = projectile.ai[1];
		 
			projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
			projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
		 
			projectile.ai[1] += 1f;
			
			
			for(int i = 0; i < 200; i++)
			{
			   //Enemy NPC variable being set
			   NPC target = Main.npc[i];

			   //Getting the shooting trajectory
			   float shootToX = target.Center.X - projectile.Center.X;
			   float shootToY = target.Center.Y - projectile.Center.Y;
			   float distance2 = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			   //If the distance between the projectile and the live target is active
			   if(distance2 < 120f && !target.friendly && target.active)
			   {
				   if(projectile.ai[0] > 12f) //Assuming you are already incrementing this in AI outside of for loop
				   {
					   //Dividing the factor of 3f which is the desired velocity by distance
					   distance2 = 0.2f / distance2;
		   
					   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
					   shootToX *= distance2 * 5;
					   shootToY *= distance2 * 5;
		   
					   //Shoot projectile and set ai back to 0
					   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("MourningStarBeam"), projectile.damage, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
					  
						projectile.ai[0] += 1f;
						if(projectile.ai[0] > 15f)
						{
					   projectile.ai[0] = 0f;
						}
				   }
			   }
			}
				projectile.ai[0] += 1f;
				
			if(modPlayer.Catalyst)
			{
				int minionNumber = 0;
				for(int j = 0; j < 1000; j++)
				{
					Projectile proj2 = Main.projectile[j];
					if(proj2.type == mod.ProjectileType("Tesseract"))
					{
						minionNumber++;
						if(minionNumber > 12)
						{
						minionNumber = 0;
						}
						if(proj2.position == projectile.position)
						{
							j = 1000;
						}
					}
				}
				Item item1 = player.inventory[50 - minionNumber];
				int projectileType = item1.shoot;
				int damage = (int)(0.75f * item1.damage);
				int reload = item1.useTime;
				int useAmmo = item1.useAmmo;
				float shootSpeed = 0.65f * item1.shootSpeed;
				float knockBack = item1.knockBack;
				
				if(useAmmo != 0)
				{
						projectileType = mod.ProjectileType("FireProj");
						for(int i = 0; i < 58; i++)
						{
					Item item2 = player.inventory[i];
						if(useAmmo == item2.ammo)
							{
							int projectileAmmo = item2.shoot;
							projectileType = projectileAmmo;
							damage += item2.damage;
							break;
							}
						}
						
					}
					
					if(item1.summon == false && item1.type != mod.ItemType("Obliterator") && item1.type != mod.ItemType("TrinityScatter") && item1.type != mod.ItemType("TrinityCrossbow") && item1.type != mod.ItemType("HallowedCrossbow") && item1.type != mod.ItemType("HallowedScatter") && item1.type != mod.ItemType("WormWoodScatter") && item1.type != mod.ItemType("WormWoodCrossbow") &&  item1.type != mod.ItemType("MargritBlaster") && item1.type != 71  && item1.type != 72 && item1.type != 73 && item1.type != 74)
					{	
						
					firerate++;
					for(int i = 0; i < 200; i++)
					{
							   NPC target = Main.npc[i];
								if(firerate >= reload && item1.channel == false && item1.damage > 0)
								{
							   //Enemy NPC variable being set

							   //Getting the shooting trajectory
					Vector2 playerCursor;
					
					if (player.gravDir == 1f)
					{
					playerCursor.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					playerCursor.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						playerCursor.X = (float)Main.mouseX + Main.screenPosition.X;
						
							   float distanceFromX = target.Center.X - playerCursor.X;
							   float distanceFromY = target.Center.Y - playerCursor.Y;
							   float shootToX = target.Center.X - projectile.Center.X;
							   float shootToY = target.Center.Y - projectile.Center.Y;
							   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
							   float distance2 = (float)System.Math.Sqrt((double)(distanceFromX * distanceFromX + distanceFromY * distanceFromY));

							   //If the distance between the projectile and the live target is active
								   if(distance2 < 240f && !target.friendly && target.active)
								   {
										  
											   //Dividing the factor of 3f which is the desired velocity by distance
											   distance = shootSpeed / distance;
								   
											   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
											   shootToX *= distance * 5;
											   shootToY *= distance * 5;
								   
											   //Shoot projectile and set ai back to 0
											   Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, projectileType, damage, knockBack, Main.myPlayer, 0f, 0f); //Spawning a projectile
									firerate = 0;
											  
												
										   
									}
								}
						
					
						}
					}
			}
		}
    }
}