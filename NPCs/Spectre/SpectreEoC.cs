using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Spectre
{[AutoloadBossHead]
	public class SpectreEoC: ModNPC
	{	int restrictor = 0;
	int runTimer = 0;
		int despawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Reanimated Horror");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14;  //5 is the flying AI
            npc.lifeMax = 233200;   //boss life
            npc.damage = 124;  //boss damage
            npc.defense = 999;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 160;
            npc.height = 120;
            animationType = NPCID.FlyingFish;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 4;    //boss frame/animation
            npc.value = 10000;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.buffImmune[24] = true;
            music = MusicID.PumpkinMoon;
            npc.netAlways = true;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.73f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 1.5f);  //boss damage increase in expermode
        }
		
		public override void BossLoot(ref string name, ref int potionType)
		{ 
		potionType = ItemID.SuperHealingPotion;
	
		if(Main.expertMode)
		
		{ 
		Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SpectreWarpCore")); 
		Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SpectreManipulator"), 1);
		} 
		else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SpectreManipulator"), 1); 


				}
		
		}
		public override void AI()
		{	
		 Player player  = Main.player[npc.target];
			if(!NPC.AnyNPCs(mod.NPCType("TechnoWormHead")))
			{
				npc.defense = 0;
				player.AddBuff(BuffID.Obstructed, 300);
				runTimer++;
					npc.dontTakeDamage = false;
			}
			else
			{
				
					npc.dontTakeDamage = true;
				
			}
			
			
			
			if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 720)
			{
			npc.active = false;
			}
		}
	
	}
}

















