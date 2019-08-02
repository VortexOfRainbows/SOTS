using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Planetarium
{[AutoloadBossHead]
	public class EtherealEntity2 : ModNPC
	{
		int fireTimer = 0;
		int readyUp2 = 0;
		int readyUp = 0;
    double dist = 50;
	float rotateAmount = 3f;
	int teleport = 1;
	int originY = 0;
	int originX = 0;
	int despawn = 0;
	float rotateTimer = 0;
	int CornerX = 0;
	int CornerY = 0;
	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Ethereal Entity");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14;  //5 is the flying AI
			
				npc.lifeMax = 10000;
		
            npc.damage = 50;  //boss damage
            npc.defense = 10;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 74;
            npc.height = 116;
            animationType = NPCID.CaveBat;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 5;    //boss frame/animation
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/StrongerMonsters");
			musicPriority = MusicPriority.BossMedium;
		//	bossBag = mod.ItemType("BossBagPlanetarium");
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)((npc.lifeMax * bossLifeScale * 0.75f));  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 0.9f);  //boss damage increase in expermode
        }
		
		public override void AI()
		{
            npc.TargetClosest(false);
			int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 74, 116, 160);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
					  Player target = Main.player[npc.target];
		 if(!Main.player[npc.target].GetModPlayer<SOTSPlayer>().PlanetariumBiome)
			{
				
					npc.dontTakeDamage = true;
			}
			else
			{
				
					npc.dontTakeDamage = false;
			}
				 
    double deg = (double)rotateTimer; 
    double rad = deg * (Math.PI / 180);
	rotateTimer += rotateAmount;
				npc.ai[0]++;
	if(npc.ai[0] == 120) // calibrate spin
	{
			if(Main.rand.Next(2) == 0)
			{
				CornerX = 500;
			}
			else
			{
				CornerX = -500;
			}
			if(Main.rand.Next(2) == 0)
			{
				CornerY = 300;
			}
			else
			{
				CornerY = -300;
			}
			
	}
    float rotateToX = target.Center.X + CornerX - (int)(Math.Cos(rad) * dist) - npc.width/2;
    float rotateToY = target.Center.Y + CornerY - (int)(Math.Sin(rad) * dist) - npc.height/2;
				originY = (int)target.position.Y;
				originX = (int)target.position.X;
				
				
			if(npc.ai[0] >= 120 && npc.ai[0] <= 240) //spin
			{
				npc.position.X = rotateToX;
				npc.position.Y = rotateToY;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
				if(npc.ai[0] == 240)
				{
							
				float centerOfCircleX = target.Center.X + CornerX;
				float centerOfCircleY = target.Center.Y + CornerY;
				Projectile.NewProjectile(centerOfCircleX, centerOfCircleY, 0, 0, mod.ProjectileType("EtherealBomb"), (int)(npc.damage * 0.25f), 0, 0);
				
				}
			}
			if(npc.ai[0] == 280 || npc.ai[0] == 360) //dash
				{
					npc.position.X = originX + Main.rand.Next(-500,501);
					npc.position.Y = originY + Main.rand.Next(-500,501);

				   float travelToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
				   float travelToY = target.position.Y + (float)target.height * 0.5f  - npc.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(travelToX * travelToX + travelToY * travelToY));

				  
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance = 1.25f / distance;
						  
						   travelToX *= distance * 5;
						   travelToY *= distance * 5;
					npc.velocity.X = travelToX;
					npc.velocity.Y = travelToY;
						
				}
			if(npc.ai[0] == 310 || npc.ai[0] == 390) //summon minions
				{
				   float fireToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
				   float fireToY = target.position.Y + (float)target.height * 0.5f  - npc.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(fireToX * fireToX + fireToY * fireToY));

				  
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance = 1.75f / distance;
						  
						   fireToX *= distance * 5;
						   fireToY *= distance * 5;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, fireToX, fireToY, mod.ProjectileType("EtherealSpawner"), (int)(npc.damage * 0.35f), 0, 0);
						
				}
			if(npc.ai[0] == 600) //obstruct
				{
					
					target.AddBuff(BuffID.Obstructed, 300);
				}
			if(npc.ai[0] == 605 || npc.ai[0] == 615 || npc.ai[0] == 625 || npc.ai[0] == 635 || npc.ai[0] == 645 || npc.ai[0] == 655 || npc.ai[0] == 665 || npc.ai[0] == 675) //cross
				{
					
					if(Main.rand.Next(2) == 0)
					{
				Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("XBeam"), (int)(npc.damage * 0.35f), 0, 0);
					}
					else
					{
				Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("plusBeam"), (int)(npc.damage * 0.35f), 0, 0);
					}
				}
			if(npc.ai[0] >= 800 && npc.ai[0] <= 1040) //teleport and fire
				{
					fireTimer++;
					if(fireTimer >= 15)
					{
						if(Main.rand.Next(2) == 0)
						{
							CornerX = 500;
						}
						else
						{
							CornerX = -500;
						}
						if(Main.rand.Next(2) == 0)
						{
							CornerY = 300;
						}
						else
						{
							CornerY = -300;
						}
						
						npc.position.X = target.Center.X + CornerX;
						npc.position.Y = target.Center.Y + CornerY;
						fireTimer = 0;
						float fireToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
						float fireToY = target.position.Y + (float)target.height * 0.5f  - npc.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(fireToX * fireToX + fireToY * fireToY));

					  
							   //Dividing the factor of 3f which is the desired velocity by distance
						distance = 1.75f / distance;
							  
						fireToX *= distance * 5;
						fireToY *= distance * 5;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, fireToX, fireToY, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.35f), 0, 0);
						
					}
				}
				
				if(npc.ai[0] == 1200) //calibrate spin
				{
					if(Main.rand.Next(2) == 0)
					{
						CornerX = 500;
					}
					else
					{
						CornerX = -500;
					}
					if(Main.rand.Next(2) == 0)
					{
						CornerY = 300;
					}
					else
					{
						CornerY = -300;
					}
				rotateToX = target.Center.X + CornerX - (int)(Math.Cos(rad) * dist) - npc.width/2;
				rotateToY = target.Center.Y + CornerY - (int)(Math.Sin(rad) * dist) - npc.height/2;
				}	
				if(npc.ai[0] >= 1200 && npc.ai[0] <= 1320) //spin
				{
					npc.position.X = rotateToX;
					npc.position.Y = rotateToY;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
					if(npc.ai[0] == 1320)
					{
								
					float centerOfCircleX = target.Center.X + CornerX;
					float centerOfCircleY = target.Center.Y + CornerY;
					Projectile.NewProjectile(centerOfCircleX, centerOfCircleY, 0, 0, mod.ProjectileType("EtherealBomb"), (int)(npc.damage * 0.25f), 0, 0);
					
					}
				}
				if(npc.ai[0] >= 1500 && npc.ai[0] <= 1700) //heal
				{
					npc.velocity.X *= 0.9f;
					npc.velocity.Y *= 0.9f;
				}
				if(npc.ai[0] >= 1700 && npc.ai[0] <= 2000) //teleport, fire, and cross
				{
					fireTimer++;
					if(fireTimer >= 15)
					{
						if(Main.rand.Next(2) == 0)
						{
							CornerX = 500;
						}
						else
						{
							CornerX = -500;
						}
						if(Main.rand.Next(2) == 0)
						{
							CornerY = 300;
						}
						else
						{
							CornerY = -300;
						}
						
						npc.position.X = target.Center.X + CornerX;
						npc.position.Y = target.Center.Y + CornerY;
						fireTimer = 0;
						float fireToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
						float fireToY = target.position.Y + (float)target.height * 0.5f  - npc.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(fireToX * fireToX + fireToY * fireToY));

					  
							   //Dividing the factor of 3f which is the desired velocity by distance
						distance = 1.75f / distance;
							  
						fireToX *= distance * 5;
						fireToY *= distance * 5;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, fireToX, fireToY, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.35f), 0, 0);
						if(Main.rand.Next(2) == 0)
						{
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("XBeam"), (int)(npc.damage * 0.35f), 0, 0);
						}
						else
						{
							Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("plusBeam"), (int)(npc.damage * 0.35f), 0, 0);
						}
					}
				}
				if(npc.ai[0] == 2100) //calibrate spin
				{
					if(Main.rand.Next(2) == 0)
					{
						CornerX = 500;
					}
					else
					{
						CornerX = -500;
					}
					if(Main.rand.Next(2) == 0)
					{
						CornerY = 300;
					}
					else
					{
						CornerY = -300;
					}
				
				rotateToX = target.Center.X + CornerX - (int)(Math.Cos(rad) * dist) - npc.width/2;
				rotateToY = target.Center.Y + CornerY - (int)(Math.Sin(rad) * dist) - npc.height/2;
				}	
				if(npc.ai[0] >= 2100 && npc.ai[0] <= 2220) //spin
				{
					npc.position.X = rotateToX;
					npc.position.Y = rotateToY;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
					if(npc.ai[0] == 2220)
					{
								
					float centerOfCircleX = target.Center.X + CornerX;
					float centerOfCircleY = target.Center.Y + CornerY;
					Projectile.NewProjectile(centerOfCircleX, centerOfCircleY, 0, 0, mod.ProjectileType("EtherealBomb"), (int)(npc.damage * 0.25f), 0, 0);
					
					}
				}
				if(npc.ai[0] == 2300)
				{
					if(Main.rand.Next(2) == 0)
					{
						CornerX = 500;
					}
					else
					{
						CornerX = -500;
					}
					if(Main.rand.Next(2) == 0)
					{
						CornerY = 300;
					}
					else
					{
						CornerY = -300;
					}
				
				}	
				if(npc.ai[0] >= 2300 && npc.ai[0] <= 2420) //spin
				{
					npc.position.X = rotateToX;
					npc.position.Y = rotateToY;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
					if(npc.ai[0] == 2420)
					{
								
					float centerOfCircleX = target.Center.X + CornerX;
					float centerOfCircleY = target.Center.Y + CornerY;
					Projectile.NewProjectile(centerOfCircleX, centerOfCircleY, 0, 0, mod.ProjectileType("EtherealBomb"), (int)(npc.damage * 0.25f), 0, 0);
					
					}
				}
				if(npc.ai[0] == 2480 || npc.ai[0] == 2560) //dash
				{
								npc.position.X = originX + Main.rand.Next(-501,500);
								npc.position.Y = originY + Main.rand.Next(-501,500);
								if(originX > 0)
								{
									originX += 70;
								}
								if(originX < 0)
								{
									originX -= 70;
								}
							   float travelToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
							   float travelToY = target.position.Y + (float)target.height * 0.5f  - npc.Center.Y;
							   float distance = (float)System.Math.Sqrt((double)(travelToX * travelToX + travelToY * travelToY));

							  
									   //Dividing the factor of 3f which is the desired velocity by distance
									   distance = 1.25f / distance;
									  
									   travelToX *= distance * 5;
									   travelToY *= distance * 5;
								npc.velocity.X = travelToX;
								npc.velocity.Y = travelToY;
									
							}
				if(npc.ai[0] == 2510 || npc.ai[0] == 2590) //summon minions
				{
							   float fireToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
							   float fireToY = target.position.Y + (float)target.height * 0.5f  - npc.Center.Y;
							   float distance = (float)System.Math.Sqrt((double)(fireToX * fireToX + fireToY * fireToY));

							  
									   //Dividing the factor of 3f which is the desired velocity by distance
									   distance = 1.75f / distance;
									  
									   fireToX *= distance * 5;
									   fireToY *= distance * 5;
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y, fireToX, fireToY, mod.ProjectileType("EtherealSpawner"), (int)(npc.damage * 0.35f), 0, 0);
									
							}
				if(npc.ai[0] >= 2600 && npc.ai[0] <= 2800) //heal
				{
					npc.velocity.X *= 0.9f;
					npc.velocity.Y *= 0.9f;
				}
				if(npc.ai[0] >= 2800) //repeat pattern
				{
					npc.ai[0] = 0;
				}
				
			
				
		   
		   
		   if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 360)
			{
			npc.active = false;
			}
		   

		}
		public override void NPCLoot()
		{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("EtherealEntity3"));
		}	
	}
}