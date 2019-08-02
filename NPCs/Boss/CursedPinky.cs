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
	{	bool initiate = true;
		float initialVelocityX = 0;
		float initialVelocityY = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Pink Flame");
		}
		public override void SetDefaults()
		{
            npc.lifeMax = 1;   
            npc.damage = 80;   
            npc.defense = 0;     
            npc.knockBackResist = 0f;
            npc.width = 14;
            npc.height = 26;
            animationType = NPCID.CaveBat;
            Main.npcFrameCount[npc.type] = 5;
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





















