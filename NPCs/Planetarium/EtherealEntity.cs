using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Planetarium
{//[AutoloadBossHead]
	public class EtherealEntity : ModNPC
	{
		int readytimer = 0;
		int readyUp3 = 0;
		int readyUp2 = 0;
		int readyUp = 0;
		int dist = 200;
		float rotateAmount = 170 * 0.012f;
		int teleport = 1;
		int originY = 0;
		int originX = 0;
		int despawn = 0;
		float rotateTimer = 0;
	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Ethereal Entity");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14; 
			npc.lifeMax = 300;
            npc.damage = 37; 
            npc.defense = 17;  
            npc.knockBackResist = 0f;
            npc.width = 28;
            npc.height = 52;
            animationType = NPCID.CaveBat;
            Main.npcFrameCount[npc.type] = 5;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/StrongerMonsters");
			musicPriority = MusicPriority.BossMedium;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * 0.76f);  //boss damage increase in expermode
        }
		
		public override void AI()
		{
			if(!Main.player[npc.target].GetModPlayer<SOTSPlayer>().PlanetariumBiome)
			{
				
					npc.dontTakeDamage = true;
			}
			else
			{
				
					npc.dontTakeDamage = false;
			}
			Player target = Main.player[npc.target];
            //npc.TargetClosest(false);
			int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 28, 52, 160);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			
			for(int i = 0; i < 200; i++)
			{
				if(Main.npc[i].type == npc.type && Main.npc[i].active && Main.npc[i].active)
				{
					readyUp++;
				}
			}
			if(readyUp < 8)
			{
			readyUp = 0;
			}
			else
			{
			npc.dontTakeDamage = true;
			npc.ai[0] = 0;
			npc.ai[1] = 0;
			npc.ai[2] = 0;
			npc.damage = 0;
			float travelToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
			float travelToY = target.position.Y - (float)target.height * 2.5f  - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(travelToX * travelToX + travelToY * travelToY));

				  
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance = 0.55f / distance;
						  
						   travelToX *= distance * 5;
						   travelToY *= distance * 5;
					npc.velocity.X = travelToX;
					npc.velocity.Y = travelToY;
			readyUp2 = 0;
			readytimer++;
				for(int i = 0; i < 200; i++)
				{
					if(Main.npc[i].type == npc.type && Main.npc[i] != npc && Main.npc[i].active)
					{
						readyUp2++;
						if(npc.Center.X + readytimer * 0.1 > Main.npc[i].Center.X && npc.Center.X - readytimer  * 0.1 < Main.npc[i].Center.X && npc.Center.Y + readytimer  * 0.1 > Main.npc[i].Center.Y && npc.Center.Y - readytimer  * 0.1 < Main.npc[i].Center.Y)
						{
							Main.npc[i].active = false;
						}
					}
				}
				if(readyUp2 == 0)
				{
					npc.scale *= 1.01f;
					readyUp3++;
					if(readyUp3 > 60)
					{
					NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("EtherealEntity2"));
					npc.active = false;
					}
				}
				
			}
			
			
			
			
			
			
			 
    double deg = (double)rotateTimer; 
    double rad = deg * (Math.PI / 180);
	rotateTimer += rotateAmount;
	
    float rotateToX = target.Center.X - (int)(Math.Cos(rad) * dist) - npc.width/2;
    float rotateToY = target.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height/2;
	
			if(npc.ai[0] > 80 && npc.ai[0] < 140)
			{
				npc.position.X = rotateToX;
				npc.position.Y = rotateToY;
				Projectile.NewProjectile((npc.Center.X), npc.Center.Y, 0, 0, mod.ProjectileType("EtherealTrail"), (int)(npc.damage * 0.15f), 0, 0);

			}
			
				originY = (int)target.position.Y;
				originX = (int)target.position.X;
				
				npc.ai[0] += Main.rand.Next(5);
				if(npc.ai[0] > 280)
				{
					npc.ai[0] = 0;
					npc.position.X = originX + Main.rand.Next(-400,401);
					npc.position.Y = originY + Main.rand.Next(-500,501);
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
		   
		   
		   if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 360)
			{
			npc.active = false;
			}
			else
			{
				npc.timeLeft = 1000;
			}
		   

		}
		public override void NPCLoot()
		{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, npc.type);
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, npc.type);
		}	
	}
}