using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	public class CursedPinky : ModNPC
	{	
		int timeLeft = 0;
		bool initiate = true;
		float initialVelocityX = 0;
		float initialVelocityY = 0;
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(initiate);
			writer.Write(initialVelocityX);
			writer.Write(initialVelocityY);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			initiate = reader.ReadBoolean();
			initialVelocityX = reader.ReadSingle();
			initialVelocityY = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Pink Flame");
		}
		public override void SetDefaults()
		{
            NPC.lifeMax = 1;   
            NPC.damage = 80;   
            NPC.defense = 0;     
            NPC.knockBackResist = 0f;
            NPC.width = 14;
            NPC.height = 26;
            animationType = NPCID.CaveBat;
            Main.npcFrameCount[NPC.type] = 5;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            //npc.DeathSound = SoundID.NPCHit3;
            npc.netAlways = true;
		}
		public override void AI()
		{	
			timeLeft++;
			if(timeLeft > 180)
			{
				npc.active = false;
			}
			if(Main.netMode != 1)
			{
				npc.netUpdate = true;
			}
			Player player  = Main.player[npc.target];
			if(initiate == true)
			{
				initialVelocityX = npc.velocity.X;
				initialVelocityY = npc.velocity.Y;
				initiate = false;
			}
			
				npc.rotation = 0;
				npc.velocity.X = initialVelocityX;
				npc.velocity.Y = initialVelocityY;
				
			if(Main.rand.Next(2) == 0)
			{
				int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 14, 26, 72);

				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
		}
		public override void NPCLoot()
		{
			for(int i = 0; i < 20; i++)
			{
				int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 14, 26, 72);

				Main.dust[num1].noGravity = true;	
			}
		}
	}
}





















