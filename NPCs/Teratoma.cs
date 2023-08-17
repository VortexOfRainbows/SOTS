using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
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
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !mushForm;
        }
        public override void SetDefaults()
		{
			NPC.aiStyle = 3;
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
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
			NPC.lifeMax = NPC.lifeMax * 3 / 4; //240
			if(Main.masterMode) //320
			{
				NPC.lifeMax = NPC.lifeMax * 8 / 9; //318
			}
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D texture2 = (Texture2D)Request<Texture2D>("SOTS/NPCs/TeratomaEyes");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 14);
			Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY - 4);
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
				Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(0, (NPC.height - 2) * NPC.scale) - new Vector2(5), (int)(NPC.width * NPC.scale), 4, ModContent.DustType<Dusts.CurseDust3>(), 0, 0, 0, default, 0.8f);
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
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				if (Main.netMode != NetmodeID.Server)
					while (num < hit.Damage / NPC.lifeMax * 40.0)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2.4f * hit.HitDirection), -2f, 0, default, 1.6f);
						num++;
					}
			}
			else
			{
				if (!mushForm) // enter second phase
				{
					Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.Center);
					mushForm = true;
					int temp = NPC.lifeMax;
					NPC.life = (int)(temp * 0.5f);
					NPC.netUpdate = true;
					if (Main.netMode != NetmodeID.Server)
						for (int k = 0; k < 45; k++)
						{
							Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(3f * hit.HitDirection), -2.4f, 0, default, 1.6f);
						}
				}
				else
				{
					if (Main.netMode != NetmodeID.Server)
						for (int k = 0; k < 45; k++)
						{
							Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2.4f * hit.HitDirection), -2.1f, 0, default, 1.6f);
						}
				}
			}
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemType<CursedTumor>(), 2, 4, 6));
        }
	}
}