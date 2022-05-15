using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class Teratoma : ModNPC
	{
		public bool mushForm = false;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(mushForm);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			mushForm = reader.ReadBoolean();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Teratoma");
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !mushForm;
        }
        public override void SetDefaults()
		{
			NPC.aiStyle =3;
			NPC.width = 40;
			NPC.height = 48;
			NPC.lifeMax = 160;
			NPC.damage = 34;
			NPC.value = 600;
			NPC.scale = 1.0f;
			NPC.defense = 20;
			NPC.knockBackResist = 0.1f;
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath1;
			Main.npcFrameCount[NPC.type] = 7;
			NPC.noTileCollide = false;
			Banner = NPC.type;
			BannerItem = ItemType<TeratomaBanner>();
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			NPC.lifeMax = 240;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D texture2 = GetTexture("SOTS/NPCs/TeratomaEyes");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 14);
			Vector2 drawPos = NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY - 4);
			int height = texture.Height / 7;
			if (mushForm)
			{
				int mushHeightSprite = 27;
				float regenTimerC = regenTimer - 90;
				if (regenTimerC < 0)
					regenTimerC = 0;
				float percent = regenTimerC / 50f;
				int currentHeight2 = (int)(height * percent);
				if (percent > 1)
					percent = 1;
				int mushHeight = (int)(mushHeightSprite * (1f - percent));
				int currentHeight = (int)(height * percent);
				if (regenTimer > 90)
                {
					for (int i = height; i >= height - currentHeight; i--)
					{
						int difference = i - height + currentHeight2;
						if (difference > 10)
							difference = 10;
						int direction = 1;
						if (i % 4 <= 1)
							direction = -1;
						int xOffset = (10 - difference) * direction;
						Rectangle cutoutFrame = new Rectangle(0, i, texture.Width, 1);
						spriteBatch.Draw(texture, drawPos + new Vector2(xOffset, i - (mushHeight - 12 * (1f - percent))), cutoutFrame, drawColor, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
						spriteBatch.Draw(texture2, drawPos + new Vector2(xOffset, i - (mushHeight - 12 * (1f - percent))), cutoutFrame, Color.White, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				Rectangle frame = new Rectangle(0, NPC.frame.Y + height - mushHeightSprite, texture.Width, mushHeight);
				spriteBatch.Draw(texture, drawPos + new Vector2(0, height - mushHeight), frame, drawColor, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				spriteBatch.Draw(texture2, drawPos + new Vector2(0, height - mushHeight), frame, Color.White, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				/*if(mushHeight >= 2)
				{
					mushHeight = 2;
					frame = new Rectangle(0, NPC.frame.Y + npc.height - mushHeight, npc.width, mushHeight);
					spriteBatch.Draw(texture, drawPos + new Vector2(0, 46 - mushHeight), frame, drawColor, npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}*/
			}
			else
			{
				Rectangle frame = new Rectangle(0, NPC.frame.Y + 1, texture.Width, height - 1);
				spriteBatch.Draw(texture, drawPos, frame, drawColor, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				spriteBatch.Draw(texture2, drawPos, frame, Color.White, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			//texture = GetTexture("SOTS/NPCs/TeratomaGlow");
			//spriteBatch.Draw(texture, drawPos, frame, Color.White, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public void createDust(int dir = 1, int amt = 10)
		{
			float regenTimerC = regenTimer - 90;
			if (regenTimerC < 0)
				regenTimerC = 0;
			float percent = regenTimerC / 50f;
			float scale = 1.0f + 0.6f * percent;
			for (int i = 0; i < amt; i++)
			{
				int num1 = Dust.NewDust(new Vector2(NPC.position.X + 8, NPC.position.Y + NPC.height - 16), NPC.width - 16, 28, ModContent.DustType<CurseDust>(), 0, 0, 0, default, scale);
				Main.dust[num1].noGravity = true;
				float dusDisX = Main.dust[num1].position.X - NPC.Center.X;
				float dusDisY = Main.dust[num1].position.Y - NPC.Center.Y;
				//double dis = Math.Sqrt((double)(dusDisX * dusDisX + dusDisY * dusDisY))

				dusDisX *= 0.05f * dir;
				dusDisY *= 0.175f * dir * (0.1f + 0.9f * percent);
				Main.dust[num1].velocity.X = dusDisX;
				Main.dust[num1].velocity.Y = dusDisY;
				Main.dust[num1].alpha = NPC.alpha;
			}
		}
		float regenTimer = 0;
        public override bool PreAI()
        {
			if(mushForm)
            {
				regenTimer++;
				if(regenTimer > 90)
					createDust(-1, 1);
				if (regenTimer >= 153)
                {
					regenTimer = -30;
					NPC.frame.Y = 6 * 62;
					NPC.velocity.Y -= 7.2f;
					mushForm = false;
                }
				NPC.velocity.X *= 0.925f;
				return false;
            }
			if(NPC.velocity.Y < 0)
            {
				NPC.velocity.Y -= 0.04f;
            }
            return base.PreAI();
        }
        public override void AI()
		{
			if (NPC.velocity.Y == 0 && Math.Abs(NPC.velocity.X) > 0.5f && !Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(0, (NPC.height - 2) * NPC.scale) - new Vector2(5), (int)(NPC.width * NPC.scale), 4, Mod.Find<ModDust>("CurseDust3").Type, 0, 0, 0, default, 0.8f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
				NPC.velocity.X *= 0.97125f;
			}
			NPC.spriteDirection = NPC.direction;
			NPC.TargetClosest(true);
		}
		public override void FindFrame(int frameHeight)
		{
			if (mushForm)
			{
				NPC.frame.Y = 6 * frameHeight;
			}
			else if (NPC.velocity.Y != 0)
			{
				NPC.frame.Y = 0;
			}
			else
			{
				NPC.frameCounter++;
				if (NPC.frameCounter >= 6f)
				{
					NPC.frameCounter -= 6f;
					NPC.frame.Y += frameHeight;
					if (NPC.frame.Y >= 6 * frameHeight)
					{
						NPC.frame.Y = 1 * frameHeight;
					}
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < damage / NPC.lifeMax * 40.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2.4f * hitDirection), -2f, 0, default, 1.6f);
					num++;
				}
			}
			else
			{
				if (!mushForm) // enter second phase
				{
					SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.Center);
					mushForm = true;
					int temp = NPC.lifeMax;
					NPC.life = (int)(temp * 0.5f);
					NPC.netUpdate = true;
					for (int k = 0; k < 45; k++)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(3f * hitDirection), -2.4f, 0, default, 1.6f);
					}
				}
				else
				{
					for (int k = 0; k < 45; k++)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2.4f * hitDirection), -2.1f, 0, default, 1.6f);
					}
				}
			}
		}
		public override void NPCLoot()
		{
			if (SOTSWorld.downedCurse && Main.rand.NextBool(3))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<CursedMatter>(), Main.rand.Next(2) + 1);
			else
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<SoulResidue>(), Main.rand.Next(2) + 1);
			if(Main.rand.NextBool(2))
			{
				int type = ItemType<CursedTumor>();
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, type, Main.rand.Next(3) + 4);
			}
		}
	}
}