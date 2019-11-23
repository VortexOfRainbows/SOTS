using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs
{
	public class ShadowTreasureSlime : ModNPC
	{	int initiateSize = 1;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Shadow Treasure Slime");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			npc.aiStyle = 1;
            npc.lifeMax = 150;  
            npc.damage = 45; 
            npc.defense = 7;  
            npc.knockBackResist = 0.5f;
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
			npc.alpha = 80;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
		}
		public override bool PreAI()
		{
			if(initiateSize == 1)
			{
				initiateSize = -1;
				npc.scale = 1.5f;
				npc.width = (int)(npc.width * npc.scale);
				npc.height = (int)(npc.height * npc.scale);
			}
			return true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(NPC.downedBoss3)
			{
					return SpawnCondition.Underworld.Chance * 0.07f;
				
			}
			return 0f;
		}
		public override void NPCLoot()
		{
		
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Gel), Main.rand.Next(4) + 3);
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Peanut"), Main.rand.Next(12) + 1);	
			
			
			if(Main.rand.Next(9) == 0 && Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("MinersPickaxe"), Main.rand.Next(2) + 1);	
			}
			
			if(Main.rand.Next(70) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.ObsidianRose, 1);		
			}
			if(Main.rand.Next(70) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.DemonScythe, 1);		
			}
			if(Main.rand.Next(70) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.MagmaStone, 1);		
			}
			if(Main.rand.Next(140) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.GuideVoodooDoll, 1);		
			}
			if(Main.rand.Next(70) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.Cascade, 1);		
			}
			if(Main.rand.Next(2) == 0 || Main.expertMode)
			{
				int rand = Main.rand.Next(30);
				if(rand == 0){
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.SpelunkerPotion, 1);
				}
				if(rand == 1)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.FeatherfallPotion, 1);
				}
				if(rand == 2)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.ManaRegenerationPotion, 1);
				}
				if(rand == 3)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.ObsidianSkinPotion, 1);
				}
				if(rand == 4)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.MagicPowerPotion, 1);
				}
				if(rand == 5)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.InvisibilityPotion, 1);
				}
				if(rand == 6)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.HunterPotion, 1);
				}
				if(rand == 7)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.HeartreachPotion, 1);
				}
				if(rand == 8)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.GravitationPotion, 1);
				}
				if(rand == 9)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.ThornsPotion, 1); //dangersense
				}
				if(rand == 10)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.WaterWalkingPotion, 1);
				}
				if(rand == 11)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.BattlePotion, 1);
				}
				if(rand == 12)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.TeleportationPotion, 1);
				}
				if(rand == 13)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.InfernoPotion, 1);
				}
				if(rand == 14)	{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.LifeforcePotion, 1);
				}
				if(rand > 14){
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.RestorationPotion, Main.rand.Next(2) + 2);
				}
			}

			if(Main.rand.Next(20) == 0 || (Main.expertMode && Main.rand.Next(30) == 0))
			{
				int rand = Main.rand.Next(5);
				if(rand == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.DarkLance, 1);
				}
				if(rand == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.Flamelash, 1);
				}
				if(rand == 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.FlowerofFire, 1);
				}
				if(rand == 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.Sunfury, 1);
				}
				if(rand == 4)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.HellwingBow, 1);
				}
			}
		}	
	
	}
}