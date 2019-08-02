using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
	public class CrypticCarver1 : ModNPC
	{	int despawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Cryptic Carver");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14;  //5 is the flying AI
            npc.lifeMax = 1200;   //boss life
            npc.damage = 26;  //boss damage
            npc.defense = 12;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 64;
            npc.height = 64;
            animationType = NPCID.SkeletronHead;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 20000;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath3;
            //music = MusicID.GoblinArmy;
            npc.netAlways = true;
			
			//bossBag = mod.ItemType("ZBossBag3");
		}
		public override void NPCLoot()
		{
			
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CrypticCarver2"));
				 NPC.NewNPC((int)npc.Center.X + 32, (int)npc.Center.Y - 32, mod.NPCType("MargritWormHead"));	
				 NPC.NewNPC((int)npc.Center.X + 32, (int)npc.Center.Y + 32, mod.NPCType("MargritWormHead"));	
				 NPC.NewNPC((int)npc.Center.X - 32, (int)npc.Center.Y + 32, mod.NPCType("MargritWormHead"));	
				 NPC.NewNPC((int)npc.Center.X - 32, (int)npc.Center.Y - 32, mod.NPCType("MargritWormHead"));
					 
		}
		
		//public override void BossLoot(ref string name, ref int potionType)
		//{ 
		//potionType = ItemID.LesserHealingPotion;
		
		
	/*
		if(Main.expertMode)
		
		{ 
		npc.DropBossBags();
		} 
		else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SolidSteam"), Main.rand.Next(3, 8)); 
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GelBar"), Main.rand.Next(5, 7)); 
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SteelBar"), Main.rand.Next(2, 4)); 

				}
		*/
	//	}
		public override void AI()
		{	
			npc.timeLeft = 600;
			if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 720)
			{
			npc.active = false;
			}
			int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 8, 8, 226);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			int num2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 56), 8, 8, 226);
			Main.dust[num2].noGravity = true;
			Main.dust[num2].velocity *= 0.1f;
			int num3 = Dust.NewDust(new Vector2(npc.position.X + 56, npc.position.Y + 56), 8, 8, 226);
			Main.dust[num3].noGravity = true;
			Main.dust[num3].velocity *= 0.1f;
			int num4 = Dust.NewDust(new Vector2(npc.position.X + 56, npc.position.Y), 8, 8, 226);
			Main.dust[num4].noGravity = true;
			Main.dust[num4].velocity *= 0.1f;
			
		}
	}
}





















