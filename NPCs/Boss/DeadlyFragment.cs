using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Boss
{
	public class DeadlyFragment : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse Fragment");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 86;
            npc.lifeMax = 35; 
            npc.damage = 35; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 32;
            npc.height = 28;
            animationType = 472;   
			Main.npcFrameCount[npc.type] = 6;   
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
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			//npc.damage = (int)(npc.damage * 1.5f); 
        }
        public override void AI()
		{	
			Player player = Main.player[npc.target];
			npc.alpha++;
			if(npc.alpha >= 255)
			{
				npc.alpha = 100;
			}
			int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 32, 28, mod.DustType("CurseDust"));
			Main.dust[dust].alpha = npc.alpha;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity.Y *= 0.6f;
			Main.dust[dust].velocity.X = -npc.velocity.X;
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
