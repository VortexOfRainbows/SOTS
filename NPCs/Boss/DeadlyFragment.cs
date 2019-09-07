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
	{	float ai1 = 0;
		int frame;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Curse Fragment");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 86;
            npc.lifeMax = 50; 
            npc.damage = 45; 
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
		public override void AI()
		{	
			Player player = Main.player[npc.target];
			ai1++;
			npc.alpha++;
			if(npc.alpha >= 255)
			{
				npc.alpha = 100;
			}
			int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y),32, 20, mod.DustType("CurseDust"));
			Main.dust[dust].alpha = npc.alpha;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity.Y = -4;
			Main.dust[dust].velocity.X = -npc.velocity.X;
			
					if(!player.GetModPlayer<SOTSPlayer>().PyramidBiome)
					{
						npc.dontTakeDamage = false;
					}
					
					
		}
		
		public override void NPCLoot()
		{
			for(int i = 0; i < 9; i ++)
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 32, 28, mod.DustType("CurseDust"));
		}	
	}
}
