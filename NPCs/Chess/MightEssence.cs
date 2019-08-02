using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class MightEssence : ModNPC
	{	int growth = 0;
		int spawn = 0;
	int difficulty = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Might Essence");  //boss frame/animation
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 0;  //5 is the flying AI
            npc.lifeMax = 25;   //boss life
            npc.damage = 34;  //boss damage
            npc.defense = 0;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 16;
            npc.height = 16;
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
			growth++;
			npc.scale += 0.001f;
			
			if(growth == 600)
			{
				
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("MightBishop"));	
				 npc.life -= 800;
			}
		}
	
	}
}
