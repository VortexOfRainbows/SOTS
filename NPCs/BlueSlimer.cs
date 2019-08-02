using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs
{
	public class BlueSlimer : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Blue Slimer");
		}
		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.Slimer);
            npc.aiStyle = 44;
            npc.lifeMax = 15;
            npc.damage = 9; 
            npc.defense = 0; 
            npc.knockBackResist = 0.45f;
            npc.width = 66;
            npc.height = 39;
            animationType = NPCID.Slimer;  
			Main.npcFrameCount[npc.type] = 4;  
            npc.value *= .25f;
            npc.npcSlots = 0.85f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.netAlways = false;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OverworldDaySlime.Chance * 0.2f;
		}
		public override void NPCLoot()
		{
			if(Main.expertMode)
			{
			 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.BlueSlime);	
			} 
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Gel), Main.rand.Next(3) + 1);
		
			//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("GelBar"), Main.rand.Next(3) + 1);	
				
			if(Main.rand.Next(4) == 0)
			{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SlimeyFeather"), Main.rand.Next(3) + 1);	
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Feather), Main.rand.Next(3) + 1);	
			}
		}	
	
	}
}