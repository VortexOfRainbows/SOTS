using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs
{
	public class PyramidTreasureSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Pyramid Slime");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			npc.aiStyle = 1;
            npc.lifeMax = 225;  
            npc.damage = 45; 
            npc.defense = 7;  
            npc.knockBackResist = 0.4f;
            npc.width = 36;
            npc.height = 32;
            animationType = NPCID.BlueSlime;
			Main.npcFrameCount[npc.type] = 2;  
            npc.value = 10000;
            npc.npcSlots = .5f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.netAlways = false;
			npc.alpha = 90;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
		}
		public override bool PreAI()
		{
			return true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.player.GetModPlayer<SOTSPlayer>().PyramidBiome && spawnInfo.spawnTileType == (ushort)mod.TileType("PyramidSlabTile"))
			{
				return 0.4f;
			}
			return 0f;
		}
		public override void NPCLoot()
		{
		
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.SandBlock), Main.rand.Next(11) + 15);
		
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Snakeskin"), Main.rand.Next(2) + 3);	
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SoulResidue"), Main.rand.Next(2) + 3);	
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("JuryRiggedDrill"), Main.rand.Next(7) + 1);	
			
			if(Main.rand.Next(9) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("JuryRiggedDrill"), Main.rand.Next(3) + 1);	
			}
			if(Main.rand.Next(9) == 0 && Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("JuryRiggedDrill"), Main.rand.Next(2) + 1);	
			}
			
			if(Main.rand.Next(30) == 0 || (Main.expertMode && Main.rand.Next(40) == 0))
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