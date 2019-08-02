using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.MPLunar
{
	public class NightmareSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Alive Nightmare");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 56;  //5 is the flying AI
            npc.lifeMax = 1000;   //boss life
            npc.damage = 159;  //boss damage
            npc.defense = 40;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 24;
            npc.height = 31;
            animationType = NPCID.DungeonSpirit;   //this boss will behavior like the DemonEye
			 Main.npcFrameCount[npc.type] = 3;    //boss frame/animation
            npc.value = 20000;
            npc.npcSlots = 0f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit8;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.buffImmune[24] = true;
            npc.netAlways = false;
		}
		public override void AI()
		{	
			 if(!NPC.downedMoonlord)
			 {
				  npc.lifeMax = 2;
				  npc.life = 2;
				  npc.value = 0;
				  npc.damage = 4;
				  npc.defense = 32;
			 }
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 24, 31, 198);
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 24, 31, 54);
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 24, 31, 191);
			}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(NPC.AnyNPCs(mod.NPCType("DarkLord")))
			{
				
			return NPC.downedMoonlord ? 200f : 0f;
			}else
			{
				
			if(NPC.downedMoonlord == true)
			{
			return NPC.downedMoonlord ? 0.01f : 0.01f;
			
		}
		else
		{ 
	return NPC.downedMoonlord ? 0 : 0;
	};
				}
	
		}
		public override void NPCLoot()
		{	
			if(NPC.downedMoonlord)
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NightmareFuel"), 1);
		}

			
			
			
		}
		
		
	
	}
