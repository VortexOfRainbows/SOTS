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
            npc.lifeMax = 20;
            npc.damage = 8; 
            npc.defense = 1; 
            npc.knockBackResist = 0.45f;
            npc.width = 66;
            npc.height = 39;
            animationType = NPCID.Slimer;  
			Main.npcFrameCount[npc.type] = 4;  
            npc.value = 15;
            npc.npcSlots = 0.85f;
            npc.noGravity = true;
			npc.alpha = 100;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 100.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)hitDirection, -1f, npc.alpha, new Color(0, 80, 255, 100), 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)(2 * hitDirection), -2f, npc.alpha, new Color(0, 80, 255, 100), 1f);
				}
				int num1 = Main.rand.Next(4);
					
				if(num1 != 0)
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore1"), 1f);
				if(num1 != 1)
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore2"), 1f);
				if(num1 != 2)
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore3"), 1f);
				if(num1 != 3)
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore4"), 1f);

				if (Main.expertMode)
				{
					NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.BlueSlime);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			bool ZoneForest = !player.GetModPlayer<SOTSPlayer>().PyramidBiome && !player.ZoneDesert && !player.ZoneCorrupt && !player.ZoneDungeon && !player.ZoneDungeon && !player.ZoneHoly && !player.ZoneMeteor && !player.ZoneJungle && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneGlowshroom && !player.ZoneUndergroundDesert && (player.ZoneDirtLayerHeight || player.ZoneOverworldHeight) && !player.ZoneBeach;
			if (ZoneForest)
			{
				return SpawnCondition.OverworldDaySlime.Chance * 0.16f;
			}
			return 0;
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Gel), Main.rand.Next(3) + 1);
			if(Main.rand.Next(5) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Feather), Main.rand.Next(2) + 1);	
			}
		}	
	}
}