using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Spectre
{[AutoloadBossHead]
	public class SpectreKingSlime : ModNPC
	{	int restrictor = 0;
	int tpTimer = 0;
		int despawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Spectre Blob");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 1;  //5 is the flying AI
            npc.lifeMax = 100000;   //boss life
            npc.damage = 62;  //boss damage
            npc.defense = 0;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 180;
            npc.height = 120;
            animationType = NPCID.BlueSlime;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 4;    //boss frame/animation
            npc.value = 10000;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.buffImmune[24] = true;
            music = MusicID.PumpkinMoon;
            npc.netAlways = true;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.73f * bossLifeScale) + 1;  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 1.5f);  //boss damage increase in expermode
        }
		
		
		public override void AI()
		{	
		 Player player  = Main.player[npc.target];
			if(tpTimer >= 233)
			{	npc.rotation += 0.3f;
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 180, 120, 160);
				if(tpTimer >= 266)
				{
				npc.position.X = player.position.X;
				npc.position.Y = player.position.Y;
				tpTimer = 0;
				npc.rotation = 0f;
				}
			}
			
			
		
		   if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 720)
			{
			npc.active = false;
			}
			
		}
	
	}
}





















