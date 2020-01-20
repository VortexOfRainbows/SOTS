using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Boss
{
	public class CurseFragment : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Curse Fragment");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 10;
            npc.lifeMax = 80; 
            npc.damage = 45; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 28;
            npc.height = 32;
            animationType = NPCID.DungeonSpirit;   
			Main.npcFrameCount[npc.type] = 3;   
            npc.value = 0;
            npc.npcSlots = 0.2f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = false;
		}
		public override void AI()
		{	
			Dust.NewDust(new Vector2(npc.Center.X - 8, npc.Center.Y - 8), 16, 16, mod.DustType("CurseDust"));
		
		}
		public override void NPCLoot()
		{
			for(int i = 0; i < 9; i ++)
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 28, 32, mod.DustType("CurseDust"));
		}	
	}
}
