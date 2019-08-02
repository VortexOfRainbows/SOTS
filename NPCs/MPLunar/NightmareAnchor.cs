using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.MPLunar
{
	public class NightmareAnchor: ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Crystal Anchor");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 0;  //5 is the flying AI
            npc.lifeMax = 500;   //boss life
            npc.damage = 1;  //boss damage
            npc.defense = 0;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 87;
            npc.height = 66;
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
		npc.rotation = 0;
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 87, 66, 242);
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 87, 66, 211);
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 87, 66, 180);
			}

			
			
			
		}
		
		
	
	}
