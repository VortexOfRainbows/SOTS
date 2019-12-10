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
            npc.lifeMax = 12;
            npc.damage = 8; 
            npc.defense = 1; 
            npc.knockBackResist = 0.45f;
            npc.width = 66;
            npc.height = 39;
            animationType = NPCID.Slimer;  
			Main.npcFrameCount[npc.type] = 4;  
            npc.value = 15;
            npc.npcSlots = 0.85f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.netAlways = false;
			npc.alpha = 100;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
		}
			public override void HitEffect(int hitDirection, double damage)
			{
				if (npc.life <= 0)
				{
					for (int k = 0; k < 20; k++)
					{
						int dust = Dust.NewDust(npc.position, npc.width, npc.height, 4, 0f, 0f, 150, Color.LightBlue, 1.5f);
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].noGravity = true;
					}
					int num1 = Main.rand.Next(4);
					int num2;
					for(num2 = Main.rand.Next(4); num2 == num1; num2 = Main.rand.Next(4))
						
					if(num1 == 0 || num2 == 0)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore1"), 1f);
					if(num1 == 1 || num2 == 1)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore2"), 1f);
					if(num1 == 2 || num2 == 2)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore3"), 1f);
					if(num1 == 3 || num2 == 3)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore4"), 1f);
				}
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
			
			if(Main.rand.Next(3) == 0)
			{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SlimeyFeather"), Main.rand.Next(3) + 1);	
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Feather), Main.rand.Next(3) + 1);	
			}
		}	
	}
}