using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
				float Speed = 4.2f;  //jump speed
				Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
				float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
				npc.velocity.X = (float)((Math.Cos(rotation) * Speed) * -1.5);
				npc.velocity.Y = (float)((Math.Sin(rotation) * Speed) * -1) -5;
			}
			if(npc.ai[0] >= 60 && npc.velocity.X == 0) //continue air movement
			{
				float Speed = 2.6f;  //jump speed
				Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
				float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
				npc.velocity.X = (float)((Math.Cos(rotation) * Speed) * -1);
			}
			npc.rotation = npc.velocity.X * -0.045f;
			if (npc.ai[0] >= 60 && npc.velocity.Y == 0)
			{
				npc.velocity.X *= 1.05f;
				npc.ai[0] = -Main.rand.Next(31);
				Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y, mod.GetSoundSlot(SoundType.Item, "Sounds/Enemies/PotSnake"), 1.5f, -0.1f);
			}
		}
		public override void NPCLoot()
		{
			Main.PlaySound(SoundID.Shatter, (int)(npc.Center.X), (int)(npc.Center.Y), 0, 0.8f, 0.05f);
			int amount2 = 3;
			if(Main.expertMode)
			{
				amount2 += Main.rand.Next(3);
			} 
			for(int amount = amount2; amount > 0; amount--)
			{
				int npcSpawn = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Snake"));	
				Main.npc[npcSpawn].velocity.X += Main.rand.NextFloat(-2.4f,2.4f);
				Main.npc[npcSpawn].velocity.Y -= Main.rand.NextFloat(5.5f,9f);
				Main.npc[npcSpawn].netUpdate = true;
			}
		}	
	
	}
	public class Snake : ModNPC
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(randMod);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			randMod = reader.ReadSingle();
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snake");
		}
		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.GoblinPeon);
			aiType = NPCID.GoblinScout;
			npc.lifeMax = 40;  
			npc.damage = 35; 
			npc.defense = 4;  
			npc.knockBackResist = 0.5f;
			npc.width = 42;
			npc.height = 36;
			Main.npcFrameCount[npc.type] = 5;  
			npc.value = 60;
			npc.npcSlots = .2f;
			npc.boss = false;
			npc.lavaImmune = false;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.netAlways = true;
			npc.netUpdate = true;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath16;
			banner = npc.type;
			bannerItem = ItemType<SnakeBanner>();
			npc.scale = 0.9f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 10);
			Vector2 drawPos = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY + 2);
			spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, drawOrigin, npc.scale * randMod, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			texture = GetTexture("SOTS/NPCs/SnakeEye");
			spriteBatch.Draw(texture, drawPos, npc.frame, Color.White, npc.rotation, drawOrigin, npc.scale * randMod, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int i = 0; i < 30; i++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * (float)hitDirection, -2.5f, 0, new Color(), 1f);
				}
				int rand = Main.rand.Next(3);
				if (rand == 0)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SnakeGore1"), npc.scale);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SnakeGore2"), npc.scale);
				}
				else if(rand == 1)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SnakeGore1"), npc.scale);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SnakeGore3"), npc.scale);
				}
				else
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SnakeGore2"), npc.scale);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SnakeGore3"), npc.scale);
				}
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
			player.AddBuff(BuffID.Venom, 60, true);
		}  
		public override void FindFrame(int frameHeight) 
		{
			npc.frameCounter++;
			if (npc.frameCounter > 5f) 
			{
				npc.frameCounter = 0;
				npc.frame.Y += frameHeight;
				if(npc.frame.Y >= frameHeight * 5)
				{
					npc.frame.Y = 0;
				}
			}
		}
		float randMod = 1;
		bool runOnce = true;
		bool runOnce2 = true;
		public override bool PreAI()
		{
			if(runOnce2 && Main.netMode != 1)
            {
				randMod = Main.rand.NextFloat(0.85f, 1.125f);
				runOnce2 = false;
				npc.netUpdate = true;
			}
			if(runOnce && randMod != 1)
            {
				runOnce = false;
				npc.lifeMax = (int)(randMod * npc.lifeMax + 1);
				npc.life = npc.lifeMax;
				npc.scale *= 0.6f + 0.4f * randMod;
				Vector2 temp = npc.Center;
				npc.width = (int)(npc.width * npc.scale);
				npc.height = (int)(npc.height * npc.scale);
				npc.Center = temp;
			}
			npc.spriteDirection = npc.direction;
			npc.TargetClosest(true);
			return true;
		}
        public override void AI()
        {
			if (Math.Abs(npc.velocity.Y) >= 0.2f)
				npc.velocity.X *= 0.987f;
			else if (npc.velocity.Y == 0)
				npc.dontTakeDamage = false;
			float speed = 1.66f / randMod;
			npc.position.X -= npc.velocity.X;
			npc.position.X += npc.velocity.X * speed;
        }
        public override void NPCLoot()
		{
			if(Main.rand.NextBool(4))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("Snakeskin"), Main.rand.Next(2) + 1);	
		}	
	}
}