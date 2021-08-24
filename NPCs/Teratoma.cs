using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			npc.CloneDefaults(NPCID.GoblinPeon);
			aiType = NPCID.GoblinScout;
			npc.width = 36;
			npc.height = 46;
			npc.lifeMax = 120;
			npc.damage = 35;
			npc.value = 600;
			npc.scale = 1.0f;
			//animationType = //NPCID.GoblinPeon;
			Main.npcFrameCount[npc.type] = 7;
			npc.DeathSound = SoundID.NPCDeath1;
			//banner = npc.type;
			//bannerItem = ItemType<ArcticGoblinBanner>();
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			npc.lifeMax = 180;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
			Vector2 drawPos = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY + 2);
			if (mushForm)
			{
				int mushHeightSprite = 22;
				float regenTimerC = regenTimer - 90;
				if (regenTimerC < 0)
					regenTimerC = 0;
				float percent = regenTimerC / 50f;
				int currentHeight2 = (int)(npc.height * percent);
				if (percent > 1)
					percent = 1;
				int mushHeight = (int)(mushHeightSprite * (1f - percent));
				int currentHeight = (int)(npc.height * percent);
				if (regenTimer > 90)
                {
					for (int i = npc.height; i >= npc.height - currentHeight; i--)
					{
						int difference = i - npc.height + currentHeight2;
						if (difference > 10)
							difference = 10;
						int direction = 1;
						if (i % 4 <= 1)
							direction = -1;
						int xOffset = (10 - difference) * direction;
						Rectangle cutoutFrame = new Rectangle(0, i, npc.width, 1);
						spriteBatch.Draw(texture, drawPos + new Vector2(xOffset, i - (mushHeight - 10 * (1f - percent))), cutoutFrame, drawColor, npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				Rectangle frame = new Rectangle(0, npc.frame.Y + npc.height - mushHeightSprite, npc.width, mushHeight);
				spriteBatch.Draw(texture, drawPos + new Vector2(0, 46 - mushHeight), frame, drawColor, npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				/*if(mushHeight >= 2)
				{
					mushHeight = 2;
					frame = new Rectangle(0, npc.frame.Y + npc.height - mushHeight, npc.width, mushHeight);
					spriteBatch.Draw(texture, drawPos + new Vector2(0, 46 - mushHeight), frame, drawColor, npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}*/
			}
			else
			{
				Rectangle frame = new Rectangle(0, npc.frame.Y + 1, npc.width, npc.height - 1);
				spriteBatch.Draw(texture, drawPos, frame, drawColor, npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
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
				int num1 = Dust.NewDust(new Vector2(npc.position.X - 8, npc.position.Y + npc.height - 16), npc.width + 16, 28, mod.DustType("CurseDust"), 0, 0, 0, default, scale);
				Main.dust[num1].noGravity = true;
				float dusDisX = Main.dust[num1].position.X - npc.Center.X;
				float dusDisY = Main.dust[num1].position.Y - npc.Center.Y;
				//double dis = Math.Sqrt((double)(dusDisX * dusDisX + dusDisY * dusDisY))

				dusDisX *= 0.05f * dir;
				dusDisY *= 0.175f * dir * (0.1f + 0.9f * percent);

				Main.dust[num1].velocity.X = dusDisX;
				Main.dust[num1].velocity.Y = dusDisY;
				Main.dust[num1].alpha = npc.alpha;
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
					npc.frame.Y = 0;
					npc.velocity.Y -= 6.6f;
					mushForm = false;
                }
				npc.velocity.X *= 0.925f;
				return false;
            }
            return base.PreAI();
        }
        public override void AI()
		{
			npc.velocity.X *= 0.9875f;
			if (npc.velocity.Y == 0 && Math.Abs(npc.velocity.X) > 0.5f && !Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(npc.position + new Vector2(0, (npc.height - 2) * npc.scale) - new Vector2(5), (int)(npc.width * npc.scale), 4, mod.DustType("CurseDust3"), 0, 0, 0, default, 1.6f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
			npc.spriteDirection = npc.direction;
			npc.TargetClosest(true);
		}
		public override void FindFrame(int frameHeight)
		{
			if (mushForm)
			{
				npc.frame.Y = 6 * frameHeight;
			}
			else if (npc.velocity.Y != 0)
			{
				//jumping frame here
			}
			else
			{
				npc.frameCounter++;
				if (npc.frameCounter >= 5f)
				{
					npc.frameCounter -= 5f;
					npc.frame.Y += frameHeight;
					if (npc.frame.Y >= 6 * frameHeight)
					{
						npc.frame.Y = 0;
					}
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while (num < damage / npc.lifeMax * 40.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2.4f * hitDirection), -2f, 0, default, 1.6f);
					num++;
				}
			}
			else
			{
				if (!mushForm) // enter second phase
				{
					Main.PlaySound(SoundID.NPCDeath1, npc.Center);
					mushForm = true;
					int temp = npc.lifeMax;
					npc.life = (int)(temp * 0.5f);
					npc.netUpdate = true;
					for (int k = 0; k < 30; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2.4f * hitDirection), -2f, 0, default, 1.6f);
					}
				}
				else
				{
					for (int k = 0; k < 40; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2.4f * hitDirection), -2f, 0, default, 1.6f);
					}
				}
			}
		}
		public override void NPCLoot()
		{
			if (SOTSWorld.downedCurse && Main.rand.NextBool(3))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedMatter"), Main.rand.Next(2) + 1);
			else
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<SoulResidue>(), Main.rand.Next(2) + 1);
			if(Main.rand.NextBool(2))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<CursedTumor>(), Main.rand.Next(3) + 4);
		}
	}
}