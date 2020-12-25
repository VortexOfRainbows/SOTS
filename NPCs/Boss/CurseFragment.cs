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
            npc.lifeMax = 60; 
            npc.damage = 35; 
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
			int num1 = Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 10.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 14; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
				}
			}
		}
	}
}
