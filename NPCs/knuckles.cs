using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs
{[AutoloadBossHead]
	public class knuckles : ModNPC
	{	int restrictor = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Knuckles");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14;  //5 is the flying AI
            npc.lifeMax = 1000000000;   //boss life
            npc.damage = 420;  //boss damage
            npc.defense = 420;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 156;
            npc.height = 102;
            animationType = NPCID.SkeletronHead;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 1000000000000000;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
		}
		
		
		public override void AI()
		{	
		
		npc.position.Y += Main.rand.Next(-1, 2);
		npc.position.X += Main.rand.Next(-1, 2);
		
			npc.ai[0]++;
			
			if(npc.ai[0] == 120)
					Main.NewText("WHY ARE YOU RUNNING????", 0, 255, 0);
			
			if(npc.ai[0] == 180)
					Main.NewText("DO YOU KNOW DA WAE???", 0, 255, 0);
				
			if(npc.ai[0] == 240)
					Main.NewText("YOU DO NOT KNOW DA WAE!", 0, 255, 0);
				
			if(npc.ai[0] == 320)
			{
					Main.NewText("LET US SHOW YOU DA WAE!!!!!!!!!!!!!!!!!!!!!!", 0, 255, 0);
					
		NPC.SpawnOnPlayer(0, mod.NPCType("knuckles"));
					
			}
			if(npc.ai[0] == 1000)
			{
				npc.ai[0] = 0;
				
			}
			
			
			
			
		npc.rotation += 0.3f;
		   if (Main.player[npc.target].dead)
		   {
			   npc.timeLeft = 0;
			   npc.position.Y += 100000;
		   }
		   else
			   npc.timeLeft = 10000;
		}
	
	}
}





















