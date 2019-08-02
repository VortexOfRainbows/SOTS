using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Boss
{	[AutoloadBossHead]
	public class PinkyFlyer : ModNPC
	{	
		int OriginNum = 0;
		int Num = 0;
		int Num2 = 0;
		int AICounter;
		int despawn;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Pink Slimer");
		}
		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.Slimer);
            npc.aiStyle = 44;
            npc.lifeMax = 100;
            npc.damage = 9; 
            npc.defense = 0; 
            npc.knockBackResist = 0.45f;
            npc.width = 66;
            npc.height = 39;
            animationType = NPCID.Slimer;  
			Main.npcFrameCount[npc.type] = 4;  
            npc.value *= 1.25f;
            npc.npcSlots = 4f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.netAlways = false;
		}
		public override void AI()
		{	
			if(Main.player[npc.target].dead)
			{
				despawn++;
			}
			if(despawn >= 720)
			{
				npc.active = false;
			}
			
			int expertModifier = 1;
			if(Main.expertMode)
			{
				expertModifier = 2;
			}
			Player player  = Main.player[npc.target];
			
					float shootToX = player.Center.X - npc.Center.X;
					float shootToY = player.Center.Y - npc.Center.Y;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

					distance = 1.1f / distance;
						  
					shootToX *= distance * 5;
					shootToY *= distance * 5;
					
			OriginNum = 0;
			Num = 0;
			Num2++;
			for(int i = 0; i < 200; i++)
			{
				NPC count = Main.npc[i];
				if(count.type == npc.type && count.active)
				{
					OriginNum++;
				}
				if(count.type == npc.type)
				{
					Num++;
				}
				if(count == npc)
				{
					if(Num == 1)
						npc.aiStyle = 44;
					
					if(Num == 2)
						npc.aiStyle = 10;
						
					if(Num == 3)
						npc.aiStyle = 22;
						
					if(Num == 4)
					{
						if(Num2 >= 240)
						{
							if(npc.aiStyle == 10)
							{
								npc.aiStyle = 22;
							}
							else if(npc.aiStyle == 22)
							{
								npc.aiStyle = 44;
							}
							else if(npc.aiStyle == 44)
							{
								npc.aiStyle = 10;
							}
							Num2 = 0;
						}
					}
				}
			}
			
			if(OriginNum == 4)
			{
				AICounter++;
				if(AICounter == 120)
				{
					int npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
					Main.npc[npcProj].velocity.X = shootToX;
					Main.npc[npcProj].velocity.Y = shootToY;
			
					Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
				if(AICounter >= 150)
				{
					AICounter = 0;
				}
			}
			if(OriginNum == 3)
			{
				if(npc.lifeMax != 250 * expertModifier)
				{
					npc.lifeMax = 250 * expertModifier;
					npc.life += 150 * expertModifier;
				}
				AICounter++;
				if(AICounter == 120)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 140)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(3 * shootToY), (int)npc.Center.Y - (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(3 * shootToY), (int)npc.Center.Y + (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 160)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(6 * shootToY), (int)npc.Center.Y - (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(6 * shootToY), (int)npc.Center.Y + (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter >= 180)
					{
						AICounter = 0;
					}
			}
			if(OriginNum == 2)
			{
				if(npc.lifeMax != 500 * expertModifier)
				{
					npc.lifeMax = 500 * expertModifier;
					npc.life += 250 * expertModifier;
				}
				AICounter += 2;
				if(AICounter == 120)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 140)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(3 * shootToY), (int)npc.Center.Y - (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(3 * shootToY), (int)npc.Center.Y + (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 160)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(6 * shootToY), (int)npc.Center.Y - (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(6 * shootToY), (int)npc.Center.Y + (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 160)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(9 * shootToY), (int)npc.Center.Y - (int)(9 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(9 * shootToY), (int)npc.Center.Y + (int)(9 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 180)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(12 * shootToY), (int)npc.Center.Y - (int)(12 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(12 * shootToY), (int)npc.Center.Y + (int)(12 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter >= 200)
					{
						AICounter = 0;
					}
			}
			if(OriginNum == 1)
			{
				if(npc.lifeMax != 750 * expertModifier)
				{
					npc.lifeMax = 750 * expertModifier;
					npc.life += 250 * expertModifier;
				}
				AICounter += 2;
				if(AICounter == 120)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 144)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(3 * shootToY), (int)npc.Center.Y - (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(3 * shootToY), (int)npc.Center.Y + (int)(3 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 168)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(6 * shootToY), (int)npc.Center.Y - (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(6 * shootToY), (int)npc.Center.Y + (int)(6 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 192)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(9 * shootToY), (int)npc.Center.Y - (int)(9 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(9 * shootToY), (int)npc.Center.Y + (int)(9 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 216)
					{
						int npcProj = NPC.NewNPC((int)npc.Center.X + (int)(12 * shootToY), (int)npc.Center.Y - (int)(12 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
			
						npcProj = NPC.NewNPC((int)npc.Center.X - (int)(12 * shootToY), (int)npc.Center.Y + (int)(12 * shootToX), mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CursedPinky"));	
						Main.npc[npcProj].velocity.X = shootToX;
						Main.npc[npcProj].velocity.Y = shootToY;
						
						Main.PlaySound(SoundID.Item21, (int)(npc.Center.X), (int)(npc.Center.Y));
					}
					if(AICounter == 240)
					{
						for(int i = 30; i < 330; i += 20/expertModifier)
						{
							Vector2 circularVelocity = new Vector2(-shootToX, -shootToY).RotatedBy(MathHelper.ToRadians(i));
							Projectile.NewProjectile((int)npc.Center.X, (int)npc.Center.Y, circularVelocity.X, circularVelocity.Y, mod.ProjectileType("PinkBullet"), 24, 1, 0);
						}
					}
					if(AICounter >= 264)
					{
						AICounter = 0;
					}
			}
		}
		public override void NPCLoot()
		{
			if(Main.expertMode)
			{
			 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.Pinky);	
			} 
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.PinkGel), Main.rand.Next(19) + 1);
		
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("GelBar"), Main.rand.Next(9) + 1);	
				
			if(Main.rand.Next(4) == 0)
			{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SlimeyFeather"), Main.rand.Next(9) + 1);	
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Feather), Main.rand.Next(9) + 1);	
			}
		}	
	
	}
}