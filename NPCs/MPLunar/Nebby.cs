using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.MPLunar
{[AutoloadBossHead]
	public class Nebby : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Nexas");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 2;  //5 is the flying AI
            npc.lifeMax = 1000000;   //boss life
            npc.damage = 100;  //boss damage
            npc.defense = 24;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 162;
            npc.height = 110;
            animationType = NPCID.DemonEye;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 4;    //boss frame/animation
            npc.value = 10000;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath61;
            npc.buffImmune[24] = true;
            music = MusicID.LunarBoss;
            npc.netAlways = true;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * .5f);  //boss damage increase in expermode
        }
		public override void BossLoot(ref string name, ref int potionType)
		{ 
	//	potionType = ItemID.SuperHealingPotion;
	
	//	if(Main.expertMode)
		
	//	{ 
	//	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("NightmareFuel"), Main.rand.Next(1, 4)); 
	//	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("NightmareManipulator"), 1); 
	//	} 
	//	else 
		//	{
			//	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("NightmareFuel"), Main.rand.Next(1, 4)); 
		Main.NewText("Its not over yet!", 255, 0, 0);
		
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Demos"));
				 
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Phobos"));
	//}
}

	public override void AI()
	{
		Main.time = 27000;
		
			if(NPC.AnyNPCs(mod.NPCType("NightmareAnchor")))
			{
				npc.defense = 9999;

			}else{npc.defense = 24;}
		Player P = Main.player[npc.target];
		npc.ai[1] = Main.rand.Next(16);
		npc.ai[0]++;
		
		if(npc.life <= 800000)
		npc.ai[2]++;
	
	if(npc.life <= 500000)
		npc.ai[3]++;
	
		
	
	
	if (npc.ai[0] >= 210 && npc.ai[0] <= 212)
			 {
				 
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("NightmareSpirit"));	
			 }
			 
			  if (npc.ai[0] >=600 && npc.ai[0] <= 700)
			{
			if(npc.ai[1] == 0)
			{
			
				  Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, 5,  468, 60, 0, Main.myPlayer); //f	
			}
			if(npc.ai[1] == 1)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, -5,  468,60, 0, Main.myPlayer); //f
			}
			if(npc.ai[1] == 2)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, -5,  468, 60, 0, Main.myPlayer); //f
			}
			if(npc.ai[1] == 3)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, 5,  468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 4)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, 0,  468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 5)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, 0, 468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 6)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -5,  468, 60, 0, Main.myPlayer);
			}
			if(npc.ai[1] == 7)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 5,  468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 8)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -2.5f, 5, 468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 9)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -2.5f, -5,  468, 60,0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 10)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, -2.5f,  468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 11)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, -2.5f,  468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 12)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2.5f, 5, 468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 13)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2.5f, -5,  468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 14)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, 2.5f,  468, 60, 0, Main.myPlayer);
			}
			
			if(npc.ai[1] == 15)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -0, 2.5f, 468, 60, 0, Main.myPlayer);
			}
			}
			
			if (npc.ai[0] == 1000)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2, 0, 465, 90, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 2, 465, 90, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -2, 0, 465, 90, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -2, 465, 90, 0, Main.myPlayer);
			}
			
			if (npc.ai[0] >= 1250 && npc.ai[0] <= 1500)
			{
			Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, 3, 448, 80, 0, Main.myPlayer);
			
			}
			
			if (npc.ai[0] == 1600)
			{
			npc.ai[0] = 0;
			}
			
			
			if(npc.ai[2] == 750)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("NightmareAnchor"));
		
		
		
			if(npc.ai[2] == 900)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -3, 348, 100, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 3, 348, 100, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -3, 0, 348, 100, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, 0, 348, 100, 0, Main.myPlayer);
               	Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -3, -3, 348, 100, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, 3, 348, 100, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -3, 3, 348, 100, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, -3, 348, 100, 0, Main.myPlayer);
				npc.ai[2] = 0;
			}
	
				
			if(npc.ai[3] == 42)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 10, 0, 462, 40, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 10, 462, 40, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -10, 0, 462, 40, 0, Main.myPlayer);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -10, 462, 40, 0, Main.myPlayer);
				
				npc.ai[3] = 2;
			}
	
	
	
	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	}
}
