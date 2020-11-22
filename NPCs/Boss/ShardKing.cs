using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
	public class ShardKing : ModNPC
	{	
		int despawn = 0;
		int transition = 0;
		int AICycle = 0;
		int AICycle2 = 0;
		float AICycle3 = 0;
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(transition);
			writer.Write(AICycle);
			writer.Write(AICycle2);
			writer.Write(AICycle3);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			transition = reader.ReadInt32();
			AICycle = reader.ReadInt32();
			AICycle2 = reader.ReadInt32();
			AICycle3 = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Icy Amalgamation");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 10;
            npc.lifeMax = 27500;
            npc.damage = 40; 
            npc.defense = 8;  
            npc.knockBackResist = 0f;
            npc.width = 120;
            npc.height = 120;
            animationType = NPCID.SkeletronHead;   
            Main.npcFrameCount[npc.type] = 1;
            npc.value = 100000;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            music = MusicID.FrostMoon;
            npc.netAlways = true;
            //npc.netUpdate = true;
            npc.buffImmune[44] = true;
			bossBag = mod.ItemType("ShardKingBossBag");
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 1.5f);  //boss damage increase in expermode
        }
		
		public override void BossLoot(ref string name, ref int potionType)
		{ 
			if(!SOTSWorld.downedAmalgamation)
			{
				Main.NewText("The key in your pocket starts to warm", 180, 250, 255);
			}
			SOTSWorld.downedAmalgamation = true;
			potionType = ItemID.GreaterHealingPotion;
		
			if(Main.expertMode)
			{ 
				npc.DropBossBags();
			} 
			else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AbsoluteBar"), Main.rand.Next(26, 35)); 
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FrostCore, Main.rand.Next(2) + 1); 
			}
		
		}
		public override void AI()
		{	
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
			bool spawnShard = false;
			if(Main.player[npc.target].dead)
			{
				despawn++;
			}
			if(despawn >= 720)
			{
				npc.active = false;
			}
			AICycle++;
			
			if(Main.expertMode && npc.life < 24000)
			{
				AICycle = 0;
				if(transition > 600)
				{
					AICycle2++;
					AICycle3 += .7f * (npc.lifeMax / (npc.life + 1250));
				}
				else
				{
					transition++;
				}
			}
			else if(npc.life < 7000)
			{
				AICycle = 0;
				if(transition > 600)
				{
					AICycle2++;
					AICycle3 += 1.2f;
				}
				else
				{
					transition++;
				}
			}
			
			if(!Main.player[npc.target].ZoneSnow)
			{
				
				npc.dontTakeDamage = true;
			}
			else
			{
				
				npc.dontTakeDamage = false;
			}
			
			if(transition > 0 && transition < 600)
			{
				npc.velocity.X *= 0.9f;
				npc.velocity.Y *= 0.9f;
				npc.dontTakeDamage = true;
				if(transition == 45 || transition == 90 || transition == 135 || transition == 180 || transition == 225 || transition == 270 || transition == 315 || transition == 360)
				{
					if(Main.netMode != 1)
					{
						int npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteSpike"));
						Main.npc[npc1].netUpdate = true;
					}
				Main.PlaySound(SoundID.Item50, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
			}
			
			
			if (AICycle == 400)
			{
				Main.PlaySound(SoundID.Item50, (int)(npc.Center.X), (int)(npc.Center.Y));
				if(Main.netMode != 1)
				{
					int npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
					Main.npc[npc1].netUpdate = true;
					
					npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
					Main.npc[npc1].netUpdate = true;
					
					npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
					Main.npc[npc1].netUpdate = true;
					
					npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
					Main.npc[npc1].netUpdate = true;
				}
				spawnShard = true;
			}
			
			if (AICycle == 800)
			{
					
				spawnShard = true;
			
			
			
			}
			if (AICycle == 850)
			{
				spawnShard = true;
			
			
			
			}
			if (AICycle == 900)
			{
					
				spawnShard = true;
			
	
			}
			
			if (AICycle == 1200)
			{
				spawnShard = true;
			}
			if (AICycle == 1600)
			{
				if(!NPC.AnyNPCs(mod.NPCType("FrostbiteProbe")))
				{
					Main.PlaySound(SoundID.Item50, (int)(npc.Center.X), (int)(npc.Center.Y));
					if(Main.netMode != 1)
					{
						int npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
						Main.npc[npc1].netUpdate = true;
						
						npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
						Main.npc[npc1].netUpdate = true;
						
						npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
						Main.npc[npc1].netUpdate = true;
						
						npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
						Main.npc[npc1].netUpdate = true;
					}
				}
				else if(!NPC.AnyNPCs(mod.NPCType("FrostHydra_Head")))
				{
					NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostHydra_Head"));
					Main.PlaySound(SoundID.Item119, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
				else
				{
					spawnShard = true;
				}
			}
			if (AICycle == 1900)
			{
					spawnShard = true;
				
			}
			if (AICycle == 1950)
			{
					spawnShard = true;
				
			}
			if (AICycle == 2000)
			{
					spawnShard = true;
				
			}
			if(AICycle >= 2400 && AICycle <= 2600)
			{
				if(!NPC.AnyNPCs(mod.NPCType("FrostbiteProbe")))
				{
					npc.rotation = MathHelper.ToRadians(Main.rand.Next(360));
				}
			}
			if(AICycle == 2400 || AICycle == 2420 || AICycle == 2440 || AICycle == 2460 || AICycle == 2480 || AICycle == 2500 || AICycle == 2520 || AICycle == 2540 || AICycle == 2560 || AICycle == 2580)
			{
				if(!NPC.AnyNPCs(mod.NPCType("FrostbiteProbe")))
				{
					spawnShard = true;
				}
				else if(!NPC.AnyNPCs(mod.NPCType("FrostHydra_Head")))
				{
					NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostHydra_Head"));
					Main.PlaySound(SoundID.Item119, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
			}
			if(AICycle >= 2600)
			{ 
				if(!NPC.AnyNPCs(mod.NPCType("FrostHydra_Head")))
				{
					NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostHydra_Head"));
					Main.PlaySound(SoundID.Item119, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
				AICycle = 0;
			}
			
			if(AICycle2 == 360)
			{
				int amount = 0;
				for(int i = 0; i < 200; i++)
				{
					NPC spike = Main.npc[i];
					if(spike.type == mod.NPCType("FrostbiteSpike") && spike.active)
					{
						amount++;
						Main.PlaySound(SoundID.Item44, (int)(spike.Center.X), (int)(spike.Center.Y));
						if(Main.netMode != 1)
						{
							Projectile.NewProjectile(spike.Center.X + Main.rand.Next(-25,26), spike.Center.Y + Main.rand.Next(-25,26), 0, 0,  mod.ProjectileType("FrostShard"), 24, 0, Main.myPlayer);
						}
					}
				}
				if(!Main.expertMode)
				{
					if(Main.netMode != 1)
					{
						int npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteSpike"));
						Main.npc[npc1].netUpdate = true;
					}
					Main.PlaySound(SoundID.Item50, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
				if(amount >= 8)
				{
					Main.PlaySound(SoundID.Item50, (int)(npc.Center.X), (int)(npc.Center.Y));
					if(Main.netMode != 1)
					{
						int npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
						Main.npc[npc1].netUpdate = true;
						
						npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
						Main.npc[npc1].netUpdate = true;
						
						npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
						Main.npc[npc1].netUpdate = true;
						
						npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteProbe"));
						Main.npc[npc1].netUpdate = true;
					}
				}
			}
			if(AICycle2 == 900)
			{
				npc.velocity.Y = 0;
				npc.velocity.X = 0;
				if(Main.netMode != 1)
				{
					int npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteLaser"));
					Main.npc[npc1].netUpdate = true;
				}
				
			}
			if(AICycle2 == 1200)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostHydra_Head"));
				Main.PlaySound(SoundID.Item119, (int)(npc.Center.X), (int)(npc.Center.Y));
				
			}
			if(AICycle2 == 1800)
			{
				npc.velocity.Y = 0;
				npc.velocity.X = 0;
				if(Main.netMode != 1)
				{
					int npc1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrostbiteLaser"));
					Main.npc[npc1].netUpdate = true;
				}
				AICycle2 = 0;
			}
			
			if(AICycle3 >= 120)
			{
				if(Main.netMode != 1)
				{
					Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-150,151), npc.Center.Y + Main.rand.Next(-150,151), 0, 0,  mod.ProjectileType("FrostShard"), 24, 0, Main.myPlayer);
				}
				Main.PlaySound(SoundID.Item44, (int)(npc.Center.X), (int)(npc.Center.Y));
				AICycle3 = 0;
			}
				
				
			if(spawnShard)
			{
				Main.PlaySound(SoundID.Item44, (int)(npc.Center.X), (int)(npc.Center.Y));
				if(Main.netMode != 1)
				{
					Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-150,151), npc.Center.Y + Main.rand.Next(-150,151), 0, 0,  mod.ProjectileType("FrostShard"), 24, 0, Main.myPlayer);
					Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-150,151), npc.Center.Y + Main.rand.Next(-150,151), 0, 0,  mod.ProjectileType("FrostShard"), 24, 0, Main.myPlayer);
				}
				if(Main.expertMode)
				{ 
					if(Main.netMode != 1)
					{
						Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-350,351), npc.Center.Y + Main.rand.Next(-350,351), 0, 0,  mod.ProjectileType("FrostShard"), 32, 0, Main.myPlayer);
						Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-350,351), npc.Center.Y + Main.rand.Next(-350,351), 0, 0,  mod.ProjectileType("FrostShard"), 32, 0, Main.myPlayer);
					}
				}
				
				if(!Main.player[npc.target].ZoneSnow)
				{
					if(Main.netMode != 1)
					{
						Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-150,151), npc.Center.Y + Main.rand.Next(-150,151), 0, 0,  mod.ProjectileType("FrostShard"), 24, 0, Main.myPlayer);
						Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-150,151), npc.Center.Y + Main.rand.Next(-150,151), 0, 0,  mod.ProjectileType("FrostShard"), 24, 0, Main.myPlayer);
						Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-150,151), npc.Center.Y + Main.rand.Next(-150,151), 0, 0,  mod.ProjectileType("FrostShard"), 24, 0, Main.myPlayer);
					}
				}
			}
		}
	}
}





















