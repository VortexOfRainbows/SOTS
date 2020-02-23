using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
	public class PutridPinky1 : ModNPC
	{	int despawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Putrid Pinky");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14; 
            npc.lifeMax = 400;   
            npc.damage = 40; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 38;
            npc.height = 38;
            animationType = NPCID.SkeletronHead;  
            Main.npcFrameCount[npc.type] = 1; 
            npc.value = 20000;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.netAlways = true;
		}
		public override void NPCLoot()
		{
			for(int i = 0; i < 2; i++)
			{
				NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-40,41), (int)npc.Center.Y + Main.rand.Next(-40,41), NPCID.Pinky);
			}				
			NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("PutridPinkyPhase2"));	
		}
		public override void AI()
		{	
			if(npc.life < 250)
			{
				for(int i = 0; i < 2; i++)
				{
					NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-40,41), (int)npc.Center.Y + Main.rand.Next(-40,41), NPCID.Pinky);
				}				
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("PutridPinkyPhase2"));	
				npc.active = false;
			}
			npc.timeLeft = 600;
			if(Main.player[npc.target].dead)
			{
				despawn++;
			}
			if(despawn >= 720)
			{
				npc.active = false;
			}
		}
	}
}





















