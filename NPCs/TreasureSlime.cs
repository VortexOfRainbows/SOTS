using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs
{
	public class TreasureSlime : ModNPC
	{	int initiateSize = 1;	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Treasure Slime");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			npc.aiStyle = 1;
            npc.lifeMax = 45;  
            npc.damage = 27; 
            npc.defense = 0;  
            npc.knockBackResist = 0.9f;
            npc.width = 32;
            npc.height = 28;
            animationType = NPCID.BlueSlime;
			Main.npcFrameCount[npc.type] = 2;  
            npc.value = 300;
            npc.npcSlots = .5f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.netAlways = false;
			npc.alpha = 90;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
		}
		public override bool PreAI()
		{
			if(initiateSize == 1)
			{
				initiateSize = -1;
				npc.scale = 1.2f;
				npc.width = (int)(npc.width * npc.scale);
				npc.height = (int)(npc.height * npc.scale);
			}
			return true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OverworldDaySlime.Chance * 0.1f;
		}
		public override void NPCLoot()
		{
		
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Leather), Main.rand.Next(4) + 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Wood), Main.rand.Next(20) + 10);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Gel), Main.rand.Next(4) + 3);
		
			//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("GelBar"), Main.rand.Next(3) + 3);	
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SlimeyFeather"), Main.rand.Next(3) + 3);	
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("GelAxe"), Main.rand.Next(7) + 1);	
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Peanut"), Main.rand.Next(12) + 1);	
			
			if(Main.rand.Next(7) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("GelAxe"), Main.rand.Next(13) + 1);	
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Peanut"), Main.rand.Next(4) + 1);	
			}
			if(Main.rand.Next(9) == 0 && Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("GelAxe"), Main.rand.Next(20) + 1);	
			}
			
			if(Main.rand.Next(220) == 0)
			{
			//	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Sling"), 1);		
			}
			if(Main.rand.Next(200) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Grenadier"), 1);		
			}
			
			if(Main.rand.Next(40) == 0 || (Main.expertMode && Main.rand.Next(40) == 0))
			{
				int rand = Main.rand.Next(4);
				if(rand == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.Aglet, 1);
				}
				if(rand == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.Radar, 1);
				}
				if(rand == 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.ClimbingClaws, 1);
				}
				if(rand == 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  3068, 1);
				}
			}
		}	
	
	}
}