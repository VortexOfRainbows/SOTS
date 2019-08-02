using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class ChessPortal : ModNPC
	{	int growth = 0;
		int spawn = 0;
	int difficulty = 0;
	int skip = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Chess Portal");  //boss frame/animation
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 0;  //5 is the flying AI
            npc.lifeMax = 199;   //boss life
            npc.damage = 0;  //boss damage
            npc.defense = 0;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 60;
            npc.height = 126;
            animationType = NPCID.Probe;   //this boss will behavior like the DemonEye
			 Main.npcFrameCount[npc.type] = 1;  
            npc.value = 1;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
		}
		public override void AI()
		{	
		 Player player  = Main.player[npc.target];
		 
			 SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	

					npc.dontTakeDamage = true;
			 if(NPC.AnyNPCs(mod.NPCType("King")) || NPC.AnyNPCs(mod.NPCType("Queen")))
			 {
			 npc.timeLeft = 6;
			 }
			 else
			 {
			 npc.timeLeft = 1;
			npc.timeLeft--;
				
					npc.dontTakeDamage = false;
			 }
			 if(modPlayer.chessSkip == true)
			 {
					if(skip == 0)
					{
					Main.NewText("Wave 5", 255, 255, 255);
					skip++;
					}
					difficulty = 5;
					
			 }
			 else
			 {
					growth++;
			 }
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 60, 126, 65);
					
					
					if(growth == 120)
					{
					Main.NewText("Wave 1", 255, 255, 255);
					difficulty = 1;
					}
					
					if(growth == 6720)
					{
					Main.NewText("Wave 2", 255, 255, 255);
					difficulty = 2;
					}
						
					if(growth == 12320)
					{
					Main.NewText("Wave 3", 255, 255, 255);
					difficulty = 3;
					}
					
						
					if(growth == 18920)
					{
					Main.NewText("Wave 4", 255, 255, 255);
					difficulty = 4;
					}
					
					if(growth == 24920)
					{
					Main.NewText("Wave 5", 255, 255, 255);
					difficulty = 5;
					}
					
		
		
		spawn++;
		if(spawn >= 240)
		{
			int type = Main.rand.Next(3);
			spawn = 0;
			int enemyDifficulty = ((Main.rand.Next(100) * difficulty) + Main.rand.Next(100)) + 1;
			if(enemyDifficulty <= 175) // pawn
			{
				if(type == 0)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("MightPawn"));	
			 
				if(type == 1)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrightPawn"));	
			 
				if(type == 2)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SightPawn"));	
			
			}
			if(enemyDifficulty <= 255 && enemyDifficulty >= 176 ) // bishop
			{
				if(type == 0)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("MightBishop"));	
			 
				if(type == 1)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrightBishop"));	
			 
				if(type == 2)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SightBishop"));	
			
			}
			
			if(enemyDifficulty <= 370 && enemyDifficulty >= 256 ) // knight
			{
				if(type == 0)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("MightKnight"));	
			 
				if(type == 1)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrightKnight"));	
			 
				if(type == 2)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SightKnight"));	
			
			}
			
			if(enemyDifficulty >= 371) // rook
			{
				if(type == 0)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("MightRook"));	
			 
				if(type == 1)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FrightRook"));	
			 
				if(type == 2)
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SightRook"));	
			
			}
		}
	
	}
}
}