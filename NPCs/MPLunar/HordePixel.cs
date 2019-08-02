using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.MPLunar
{
	public class HordePixel : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Horde Pixel");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 0;  //5 is the flying AI
            npc.lifeMax = 100;   //boss life
            npc.damage = 0;  //boss damage
            npc.defense = 100000;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 2;
            npc.height = 1;
            animationType = NPCID.SkeletronHead;   //this boss will behavior like the DemonEye
			 Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 0;
            npc.npcSlots = 0f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.buffImmune[24] = true;
            npc.netAlways = false;
		}
		public override void AI()
		{	
			}

			
			
			
		}
		
		
	
	}
