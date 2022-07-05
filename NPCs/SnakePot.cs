using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.GameContent.ItemDropRules;
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
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0;
            NPC.lifeMax = 60;
            NPC.damage = 30; 
            NPC.defense = 20; 
            NPC.knockBackResist = 0.06f;
            NPC.width = 28;
            NPC.height = 42;
            NPC.value = 500;
            NPC.boss = false;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.netAlways = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = null;
			Banner = NPC.type;
			BannerItem = ItemType<SnakePotBanner>();
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if(NPC.ai[0] >= 10 && NPC.ai[0] <= 59)
				NPC.ai[0] = 0;
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				SOTSUtils.PlaySound(SoundID.Shatter, (int)NPC.Center.X, (int)NPC.Center.Y, 0.8f, 0.05f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Pots/PyramidPotGore1"), NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Pots/PyramidPotGore5"), NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Pots/PyramidPotGore12"), NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Pots/PyramidPotGore13"), NPC.scale);
			}
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.ai[0]++;
			if(NPC.ai[0] == 60) //jump timer
			{
				NPC.ai[0]++;
				float Speed = 4.2f;  //jump speed
				Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
				float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
				NPC.velocity.X = (float)((Math.Cos(rotation) * Speed) * -1.5);
				NPC.velocity.Y = (float)((Math.Sin(rotation) * Speed) * -1) -5;
			}
			if(NPC.ai[0] >= 60 && NPC.velocity.X == 0) //continue air movement
			{
				float Speed = 2.6f;  //jump speed
				Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
				float rotation = (float)Math.Atan2(vector8.Y - (player.position.Y + (player.height * 0.5f)), vector8.X - (player.position.X + (player.width * 0.5f)));
				NPC.velocity.X = (float)((Math.Cos(rotation) * Speed) * -1);
			}
			NPC.rotation = NPC.velocity.X * -0.045f;
			if (NPC.ai[0] >= 60 && NPC.velocity.Y == 0)
			{
				NPC.velocity.X *= 1.05f;
				NPC.ai[0] = -Main.rand.Next(31);
				SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Enemies/PotSnake"), (int)NPC.Center.X, (int)NPC.Center.Y, 1.5f, -0.1f);
			}
		}
        public override void OnKill()
		{
			int amount2 = 3;
			if (Main.expertMode)
			{
				amount2 += Main.rand.Next(3);
			}
			for (int amount = amount2; amount > 0; amount--)
			{
				int npcSpawn = NPC.NewNPC(NPC.GetSource_Death("SOTS:SnakePotSnakes"), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<Snake>());
				Main.npc[npcSpawn].velocity.X += Main.rand.NextFloat(-1.6f, 1.6f);
				Main.npc[npcSpawn].velocity.Y -= Main.rand.NextFloat(5.25f, 7.5f);
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
			NPC.aiStyle =3;
			NPC.lifeMax = 40;  
			NPC.damage = 35; 
			NPC.defense = 4;  
			NPC.knockBackResist = 0.5f;
			NPC.width = 32;
			NPC.height = 32;
			Main.npcFrameCount[NPC.type] = 5;  
			NPC.value = 60;
			NPC.npcSlots = .2f;
			NPC.boss = false;
			NPC.lavaImmune = false;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.netAlways = true;
			NPC.netUpdate = true;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath16;
			Banner = NPC.type;
			BannerItem = ItemType<SnakeBanner>();
			NPC.scale = 0.9f;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 10);
			Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY + 2);
			spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, drawOrigin, NPC.scale * randMod, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			texture = (Texture2D)Request<Texture2D>("SOTS/NPCs/SnakeEye");
			spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, NPC.scale * randMod, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 30; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * (float)hitDirection, -2.5f, 0, new Color(), 1f);
				}
				int rand = Main.rand.Next(3);
				if (rand == 0)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/SnakeGore1"), NPC.scale);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/SnakeGore2"), NPC.scale);
				}
				else if(rand == 1)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/SnakeGore1"), NPC.scale);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/SnakeGore3"), NPC.scale);
				}
				else
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/SnakeGore2"), NPC.scale);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/SnakeGore3"), NPC.scale);
				}
			}
			else
			{
				for (int i = 0; i < damage / (float)NPC.lifeMax * 50.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)hitDirection, -1f, 0, new Color(), 0.8f);
				}
			}
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(BuffID.Venom, 60, true);
		}  
		public override void FindFrame(int frameHeight) 
		{
			NPC.frameCounter++;
			if (NPC.frameCounter > 5f) 
			{
				NPC.frameCounter = 0;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= frameHeight * 5)
				{
					NPC.frame.Y = 0;
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
				NPC.netUpdate = true;
			}
			if(runOnce && randMod != 1)
            {
				runOnce = false;
				NPC.lifeMax = (int)(randMod * NPC.lifeMax + 0.5f);
				NPC.life = NPC.lifeMax;
				NPC.scale *= 0.55f + 0.45f * randMod;
				Vector2 temp = NPC.Center;
				NPC.width = (int)(NPC.width * NPC.scale);
				NPC.height = (int)(NPC.height * NPC.scale);
				NPC.Center = temp;
			}
			NPC.spriteDirection = NPC.direction;
			NPC.TargetClosest(true);
			return true;
		}
        public override void AI()
        {
			if (Math.Abs(NPC.velocity.Y) >= 0.2f)
            {
				if(NPC.dontTakeDamage)
					NPC.velocity.X *= 0.985f;
				else
					NPC.velocity.X *= 0.987f;
			}
			else if (NPC.velocity.Y == 0)
				NPC.dontTakeDamage = false;
			float speed = 2.3f / randMod;
			NPC.position.X -= NPC.velocity.X;
			Vector2 trueVelo = new Vector2(NPC.velocity.X * speed, NPC.velocity.Y);
			trueVelo = Collision.TileCollision(NPC.position, trueVelo, NPC.width, NPC.height, true);
			NPC.position.X += trueVelo.X;
			NPC.velocity.X = trueVelo.X / speed;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemType<Snakeskin>(), 3));
        }
	}
}