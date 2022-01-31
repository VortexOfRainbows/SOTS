using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class ChaosRubble : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Construct Rubble");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0;  
            npc.lifeMax = 1000;
            npc.damage = 0;
            npc.defense = 30;
            npc.knockBackResist = 0f;
            npc.width = 92;
            npc.height = 82;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.netAlways = true;
		}
        public override bool PreNPCLoot()
        {
            return false;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					for (int k = 0; k < 30; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Platinum, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
					}
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChaosConstruct/ChaosConstructGore3"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChaosConstruct/ChaosConstructGore4"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChaosConstruct/ChaosConstructGore6"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChaosConstruct/ChaosConstructGore7"), 1f);
					for (int i = 0; i < 10; i++)
						Gore.NewGore(npc.position + new Vector2(Main.rand.NextFloat(50), Main.rand.NextFloat(50)), npc.velocity, Main.rand.Next(61, 64), 1f);
				}
			}
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			npc.lifeMax = 1200;
			npc.damage = 0;
        }
        public override void AI()
        {
			npc.TargetClosest(false);
			npc.direction = 1;
			npc.spriteDirection = 1;
        }
	}
}





















