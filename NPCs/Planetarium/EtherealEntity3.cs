using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Planetarium
{//[AutoloadBossHead]
	public class EtherealEntity3 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Ethereal Entity");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 0;  //5 is the flying AI
			
				npc.lifeMax = 10000;
		
            npc.damage = 0;  //boss damage
            npc.defense = 0;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 28;
            npc.height = 28;
            animationType = NPCID.SkeletronHead;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
		//	music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/StrongerMonsters");
			bossBag = mod.ItemType("BossBagPlanetarium");
		}
		public override void AI()
		{
			npc.timeLeft = 100000;
		}
		public override void BossLoot(ref string name, ref int potionType)
		{ 
		
		SOTSWorld.downedEntity = true;
		potionType = ItemID.HealingPotion;
	
		if(Main.expertMode)
		{ 
		npc.DropBossBags();
		} 
		else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MargritCore"), 1); 
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EmptyPlanetariumOrb"), Main.rand.Next(20,40)); 
			
				}
		}
	}
}