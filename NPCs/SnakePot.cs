using System;
using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class SnakePot : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Snake Pot");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0;
            npc.lifeMax = 60;
            npc.damage = 30; 
            npc.defense = 20; 
            npc.knockBackResist = 0.06f;
            npc.width = 24;
            npc.height = 42;
			Main.npcFrameCount[npc.type] = 1;  
            npc.value = 500;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = false;
            npc.netAlways = true;
            npc.netUpdate = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = null;
			banner = npc.type;
			bannerItem = ItemType<SnakePotBanner>();
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.ai[0] >= 10 && npc.ai[0] <= 59)
			npc.ai[0] = 0;
		
			if (npc.life <= 0)
            {
				for(int amount = 0; amount < 3; amount++)
				Gore.NewGore(new Vector2 (npc.Center.X, npc.Center.Y), default(Vector2), Main.rand.Next(51,54), 1f);	
            }
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.ai[0]++;
				if(npc.ai[0] == 60) //jump timer
				{
						npc.ai[0]++;
				float Speed = 4f;  //jump speed
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
                npc.velocity.X = (float)((Math.Cos(rotation) * Speed) * -1.5);
				npc.velocity.Y = (float)((Math.Sin(rotation) * Speed) * -1) -5;
				
				}
				if(npc.ai[0] >= 60 && npc.velocity.X == 0) //continue air movement
				{
					float Speed = 2f;  //jump speed
					Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
					float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
					npc.velocity.X = (float)((Math.Cos(rotation) * Speed) * -1);
				}
				if(npc.ai[0] >= 60 && npc.velocity.Y == 0)
				{
					npc.ai[0] = -Main.rand.Next(31);
					Main.PlaySound(13, (int)(npc.Center.X), (int)(npc.Center.Y));
				}
		}
		public override void NPCLoot()
		{
			
            Main.PlaySound(13, (int)(npc.Center.X), (int)(npc.Center.Y));
			int amount2 = 3;
			if(Main.expertMode)
			{
				amount2 += Main.rand.Next(3);
			} 
			
			for(int amount = amount2; amount > 0; amount--)
			{
				int npcSpawn = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Snake"));	
				Main.npc[npcSpawn].velocity.X += Main.rand.Next(-5,6);
				Main.npc[npcSpawn].velocity.Y -= Main.rand.Next(3,8);
			}
		}	
	
	}
	public class Snake : ModNPC
	{	int initiateSize = 1;
		int ai1;
		int initiateSpeed = 1;
		int ai2 = 30;
		int num1;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Snake");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			npc.aiStyle = 0;
			npc.lifeMax = 40;  
			npc.damage = 35; 
			npc.defense = 4;  
			npc.knockBackResist = 0.4f;
			npc.width = 36;
			npc.height = 32;
			Main.npcFrameCount[npc.type] = 5;  
			npc.value = 60;
			npc.npcSlots = .2f;
			npc.boss = false;
			npc.lavaImmune = false;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.netAlways = true;
			npc.netUpdate = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath16;
			banner = npc.type;
			bannerItem = ItemType<SnakeBanner>();
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			ai2 = 0;
			if (npc.life <= 0)
			{
				for (int i = 0; i < 30; i++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * (float)hitDirection, -2.5f, 0, new Color(), 1f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SnakeGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SnakeGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SnakeGore3"), 1f);
			}
			else
			{
				for (int i = 0; i < damage / (float)npc.lifeMax * 50.0; i++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, (float)hitDirection, -1f, 0, new Color(), 0.8f);
				}
			}
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(BuffID.Venom, 72, true);
		}  
		public override void FindFrame(int frameHeight) 
		{
			if (ai1 > 10f) 
			{
				ai1 = 0;
				npc.frame.Y = (npc.frame.Y + frameHeight);
				if(npc.frame.Y >= 158)
				{
					npc.frame.Y = 0;
				}
			}
		}
		public override bool PreAI()
		{
			//npc.ai[2] = 1;
			Player player = Main.player[npc.target];
			ai1++;
			if(initiateSize == 1)
			{
			//	initiateSize = -1;
			//	npc.scale = (float)(Main.rand.Next(80,121) * 0.01f);
			//	npc.width = (int)(npc.width * npc.scale);
			//	npc.height = (int)(npc.height * npc.scale);
			}
			
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			if(npc.velocity.X == 0 && npc.velocity.Y == 0)
			{
				npc.aiStyle = 3;
					aiType = 73;
				initiateSpeed = -1;
			}
			else if (npc.velocity.Y == 0 && ai2 == 0)
			{
				ai2 = 1;
			}
			if(ai2 >= 1)
			{
				ai2++;
			}
				if(initiateSpeed == -1 && ai2 >= 5)
				{
					if(ai2 >= 30)
					ai2 = 30;
				
					if(player.Center.X > npc.Center.X + 12)
					{
						npc.velocity.X = 3 * npc.scale * (float)(ai2/30f);
						npc.spriteDirection = 1;
					}
					if(player.Center.X < npc.Center.X - 12)
					{
						npc.velocity.X = -3 * npc.scale * (float)(ai2/30f);
						npc.spriteDirection = -1;
					}
				}
		}
		public override void NPCLoot()
		{
			if(Main.rand.Next(4) == 0)
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Snakeskin"), Main.rand.Next(2) + 1);	
		}	
	}
}