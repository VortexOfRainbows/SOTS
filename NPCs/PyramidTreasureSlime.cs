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
            npc.value = 7000;
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
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 100.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)hitDirection, -1f, npc.alpha, new Color(186, 168, 84, 100), 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)(2 * hitDirection), -2f, npc.alpha, new Color(186, 168, 84, 100), 1f);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.player.GetModPlayer<SOTSPlayer>().PyramidBiome && spawnInfo.spawnTileType == (ushort)mod.TileType("PyramidSlabTile"))
			{
				return 0.1f;
			}
			return 0f;
		}
		public override void NPCLoot()
		{
		
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.SandBlock), Main.rand.Next(11) + 15);
		
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Snakeskin"), Main.rand.Next(2) + 3);	
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SoulResidue"), Main.rand.Next(2) + 3);	
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("JuryRiggedDrill"), Main.rand.Next(7) + 1);	
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("ExplosiveKnife"), Main.rand.Next(10) + 5);	
			
			if(Main.rand.Next(6) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("JuryRiggedDrill"), Main.rand.Next(3) + 1);	
			}
			if(Main.rand.Next(7) == 0 && Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("JuryRiggedDrill"), Main.rand.Next(2) + 1);	
			}
			
			if(Main.rand.Next(8) == 0 || (Main.expertMode && Main.rand.Next(18) == 0))
			{
				int rand = Main.rand.Next(12);
				if(rand == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Aten"), 1);
				}
				if(rand == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("EmeraldBracelet"), 1);
				}
				if(rand == 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("ImperialPike"), 1);
				}
				if(rand == 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("PharaohsCane"), 1);
				}
				if(rand == 4)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("PitatiLongbow"), 1);
				}
				if(rand == 5)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("RoyalMagnum"), 1);
				}
				if(rand == 6)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SandstoneEdge"), 1);
				}
				if(rand == 7)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SandstoneWarhammer"), 1);
				}
				if(rand == 8)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("ShiftingSands"), 1);
				}
				if(rand == 9)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("SunlightAmulet"), 1);
				}
				if(rand == 10)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.SandstorminaBottle, 1);
				}
				if(rand == 11)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.FlyingCarpet, 1);
				}
			}
		}	
	
	}
}