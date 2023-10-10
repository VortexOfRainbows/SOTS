using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Items.Banners;
using SOTS.Items.Planetarium;
using SOTS.Items.Planetarium.FromChests;
using SOTS.NPCs.Constructs;
using SOTS.Projectiles.Planetarium;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.NPCs.Boss.Advisor
{	[AutoloadBossHead]
	public class TheAdvisorHead : ModNPC
	{
		int despawn = 0;
		private float attackPhase1 {
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}

		private float attackPhase2
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}

		private float attackTimer1 {
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}

		private float attackTimer2 {
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		bool runOnce = true;
		float fireToX = 0;
		float fireToY = 0;
		float eyeReset = 2.5f;
		bool glow = false;
		int lastAttackPhase1 = -1;
		int lastAttackPhase2 = -1;
		Vector2[] hookPos = {new Vector2(-1, -1) , new Vector2(-1, -1) , new Vector2(-1, -1) , new Vector2(-1, -1) };
		Vector2[] hookPosTrue = { new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1) };
		public override void SetStaticDefaults()
        {
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 2;
			NPCID.Sets.ShouldBeCountedAsBoss[NPC.type] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				CustomTexturePath = "SOTS/BossCL/AdvisorPortrait",
				PortraitScale = 0.5f, // Portrait refers to the full picture when clicking on the icon in the bestiary
				Scale = 0.5f,
				PortraitPositionYOverride = 10f,
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.ShadowFlame] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(ConstructIds[0]);
			writer.Write(ConstructIds[1]);
			writer.Write(ConstructIds[2]);
			writer.Write(ConstructIds[3]);

			writer.Write(dormant);
			writer.Write(moveLegsDynamic);
			writer.Write(moveLegsReturn);
			writer.Write(watchPlayer);
			writer.Write(NPC.dontTakeDamage);
			writer.Write(NPC.dontCountMe);
			writer.Write(NPC.boss);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			ConstructIds[0] = reader.ReadInt32();
			ConstructIds[1] = reader.ReadInt32();
			ConstructIds[2] = reader.ReadInt32();
			ConstructIds[3] = reader.ReadInt32();

			dormant = reader.ReadBoolean();
			moveLegsDynamic = reader.ReadBoolean();
			moveLegsReturn = reader.ReadBoolean();
			watchPlayer = reader.ReadBoolean();
			NPC.dontTakeDamage = reader.ReadBoolean();
			NPC.dontCountMe = reader.ReadBoolean();
			NPC.boss = reader.ReadBoolean();
		}
		public override bool CheckActive()
		{
			return !dormant;
		}
		public const int NormalModeHP = 12500;
		public const int NormalModeDamage = 54;
		public const float ExpertLifeScale = 0.64002f;
		public override void SetDefaults()
        {
            NPC.aiStyle =0;
            NPC.lifeMax = NormalModeHP;
            NPC.damage = 54;
            NPC.defense = 24;
            NPC.knockBackResist = 0f;
            NPC.width = 78;
            NPC.height = 98;
            NPC.value = 150000;
            NPC.npcSlots = 15f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
			Music = -1;
			SceneEffectPriority = (SceneEffectPriority)(-1);
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment * ExpertLifeScale); //16000
			NPC.damage = (int)(NPC.damage * 0.8f); //86
		}
		public static int[] ConstructIds = { -1, -1, -1, -1 };
		bool dormant = true;
		int dormantCounter = 0;
		int ai1 = 0;
		float ai3 = 0;
		float hookDistortion = 1f;
		float hookDistortionShake = 0f;
		float laserDirection = 0f;
		float nextLaserDirection = 0f;
		int highlightFrame = 0;
        public override void FindFrame(int frameHeight)
        {
			NPC.frameCounter++;
			if(NPC.frameCounter >= 4)
            {
				NPC.frameCounter = 0;
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (frameHeight * 2);
				highlightFrame = (highlightFrame + 1) % 10;
            }
            base.FindFrame(frameHeight);
        }
        public void DrawGlow(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/TheAdvisorHead_Spirit");
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.Center - screenPos;
			Color color = new Color(100, 100, 100, 0);
			if (attackPhase2 == 0)
			{
				Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/AdvisorMissileAttachment");
				Texture2D texture3 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/AdvisorMissileAttachment_Highlight");
				Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
				if (attackTimer2 > 0)
				{
					float speed = 90f;
					float deg = 90f * attackTimer2 / speed;
					float amt = 48f * attackTimer2 / speed;
					if(deg > 90)
                    {
						deg = 90f;
					}
					if(amt > 48)
                    {
						amt = 48f;
					}
					float att = attackTimer2 - (900f - speed);
					if(att > 0)
					{
						att = speed - att;
						deg = 90f * att / speed;
						amt = 48f * att / speed;
					}
					for (int direction = -1; direction <= 1; direction += 2)
					{
						Vector2 circularRotation = new Vector2(0, -amt).RotatedBy(MathHelper.ToRadians(deg * direction));
						spriteBatch.Draw(texture2, new Vector2((float)(NPC.Center.X - (int)screenPos.X), (float)(NPC.Center.Y - (int)screenPos.Y) + 20) + circularRotation, null, drawColor, 0f, drawOrigin2, 1f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
						if (glow)
							for (int k = 0; k < 7; k++)
							{
								float x = Main.rand.Next(-10, 11) * 0.1f;
								float y = Main.rand.Next(-10, 11) * 0.1f;
								spriteBatch.Draw(texture3, new Vector2((float)(NPC.Center.X - (int)screenPos.X), (float)(NPC.Center.Y - (int)screenPos.Y) + 20) + circularRotation + new Vector2(x, y), null, color, 0f, drawOrigin2, 1f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
							}
					}
				}
			}
			if (attackPhase2 == 1)
			{
				Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/AdvisorLaserAttachment");
				Texture2D texture3 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/AdvisorLaserAttachment_Highlight");
				Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
				if (attackTimer2 > 0)
				{
					float speed = 90f;
					float deg = laserDirection;
					float amt = 48f * attackTimer2 / speed;
					if (amt > 48)
					{
						amt = 48f;
					}
					float att = attackTimer2 - (750f - speed);
					if (att > 0)
					{
						att = speed - att;
						amt = 48f * att / speed;
					}
					Vector2 shift = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(deg));
					for (int direction = -1; direction <= 1; direction += 2)
					{
						Vector2 circularRotation = new Vector2(0, -amt).RotatedBy(MathHelper.ToRadians(deg + 90 * direction));
						spriteBatch.Draw(texture2, new Vector2((float)(NPC.Center.X - (int)screenPos.X), (float)(NPC.Center.Y - (int)screenPos.Y) + Math.Abs(shift.X) + 4) + circularRotation, null, drawColor, MathHelper.ToRadians(laserDirection), drawOrigin2, 1f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
						if (glow)
							for (int k = 0; k < 7; k++)
							{
								float x = Main.rand.Next(-10, 11) * 0.1f;
								float y = Main.rand.Next(-10, 11) * 0.1f;
								spriteBatch.Draw(texture3, new Vector2((float)(NPC.Center.X - (int)screenPos.X), (float)(NPC.Center.Y - (int)screenPos.Y) + Math.Abs(shift.X) + 4) + circularRotation + new Vector2(x, y), null, color, MathHelper.ToRadians(laserDirection), drawOrigin2, 1f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
							}
					}
				}
			}
			if (attackPhase2 == 2)
			{
				Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/AdvisorTazerAttachment");
				Texture2D texture3 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/AdvisorTazerAttachment_Highlight");
				Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
				if (attackTimer2 > 0)
				{
					float speed = 90f;
					float deg = new Vector2(fireToX - NPC.Center.X, fireToY - NPC.Center.Y).ToRotation();
					float amt = 56f * attackTimer2 / speed;
					if (amt > 56f)
					{
						amt = 56f;
					}
					float att = attackTimer2 - (810f - speed);
					if (att > 0)
					{
						att = speed - att;
						amt = 56f * att / speed;
					}
					if(attackTimer2 > 180f)
                    {
						amt *= 0.6f + (0.4f * (attackTimer2 % 90) / 90f);
                    }
					Vector2 shift = new Vector2(16, 0).RotatedBy(deg);
					Vector2 circularRotation = new Vector2(amt, 0).RotatedBy(deg);
                    spriteBatch.Draw(texture2, new Vector2((float)(NPC.Center.X - (int)screenPos.X), (float)(NPC.Center.Y - (int)screenPos.Y) + Math.Abs(shift.X) + 4) + circularRotation, null, drawColor, deg, drawOrigin2, 1f, SpriteEffects.FlipHorizontally, 0f);
					if (glow)
						for (int k = 0; k < 7; k++)
						{
							float x = Main.rand.Next(-10, 11) * 0.1f;
							float y = Main.rand.Next(-10, 11) * 0.1f;
							spriteBatch.Draw(texture3, new Vector2((float)(NPC.Center.X - (int)screenPos.X), (float)(NPC.Center.Y - (int)screenPos.Y) + Math.Abs(shift.X) + 4) + circularRotation + new Vector2(x, y), null, color, deg, drawOrigin2, 1f, SpriteEffects.FlipHorizontally, 0f);
						}
				}
			}
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				y += 7;
				spriteBatch.Draw(texture, new Vector2((float)(NPC.Center.X - (int)screenPos.X) + x, (float)(NPC.Center.Y - (int)screenPos.Y) + y), null, color, 0f, drawOrigin, 1.125f, SpriteEffects.None, 0f);
			}
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			for(int j = 0; j < 4;)
			{
				float scale = NPC.scale;
				int ai2 = 0;
				if (j > 1)
					ai2 += 180;
				float npcRadians = NPC.rotation;
				Vector2 toPos = NPC.Center + hookPos[j].RotatedBy(npcRadians);
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/OtherworldVine");
				Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/OtherworldVine_Highlight");
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Vector2 npcPos = new Vector2(NPC.Center.X + (-96 + 64 * j) * 1f * 0.25f, NPC.position.Y + NPC.height).RotatedBy(npcRadians);
				Vector2 distanceToOwner = npcPos - toPos;
				Vector2 centerOfCircle = toPos + distanceToOwner / 2;
				float startingRadians = distanceToOwner.ToRotation();
				float radius = distanceToOwner.Length() / 3;
				float distance = distanceToOwner.Length();
				float minDist = 200f;
				float midPointDist = minDist - distance;
				if (midPointDist < 0) midPointDist = 0;
				midPointDist /= 5f;
				Vector2 point1 = centerOfCircle + new Vector2(0, midPointDist).RotatedBy(startingRadians + MathHelper.ToRadians(ai2));
				Vector2 point2 = centerOfCircle + new Vector2(0, midPointDist).RotatedBy(startingRadians + MathHelper.ToRadians(180 + ai2));

				Vector2 pointNpcTo1 = npcPos - point1;
				pointNpcTo1 = pointNpcTo1.SafeNormalize(new Vector2(1, 0));
				pointNpcTo1 *= radius;
				point1 = npcPos -pointNpcTo1;
				Vector2 pointEndTo2 = point2 - toPos;
				pointEndTo2 = pointEndTo2.SafeNormalize(new Vector2(1, 0));
				pointEndTo2 *= radius;
				point2 = toPos + pointEndTo2;
				Vector2 point1To2 = point1 - point2;
				float dynamLength = 0.2f * hookDistortion;
				Color color = new Color(100, 100, 100, 0);
				int totalSeg = 12;
				int currentSeg = 0;
				int currentSegMult = 0;
				int max = 8;
				for(int i = 0; i < max; i++)
                {
					Vector2 dist = -pointNpcTo1/(float)(max);
					Vector2 pos = npcPos + dist * i;
					Vector2 dynamicAddition = new Vector2(dynamLength * ((float)currentSegMult / totalSeg), 0).RotatedBy(MathHelper.ToRadians(currentSeg * 180f / totalSeg + ai1));
					Vector2 drawPos = pos - screenPos;
					dynamicAddition += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)) * hookDistortionShake;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, NPC.GetAlpha(drawColor), pointNpcTo1.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					if (glow)
						for (int k = 0; k < 7; k++)
						{
							float x = Main.rand.Next(-10, 11) * 0.1f;
							float y = Main.rand.Next(-10, 11) * 0.1f;
							spriteBatch.Draw(texture2, drawPos + dynamicAddition + new Vector2(x, y), null, color, pointNpcTo1.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
						}
					currentSeg++;
					currentSegMult++;
				}
				max = 4;
				for (int i = 0; i < max; i++)
				{
					Vector2 bobbing = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(180f / max * i)); //creates length of circle
					bobbing.Y *= (midPointDist/40f);
					Vector2 circularLocation = new Vector2(0, bobbing.Y).RotatedBy(point1To2.ToRotation() + MathHelper.ToRadians(ai2)); //applies rotation
					Vector2 dist = -point1To2 / (float)(max*2);
					Vector2 pos = point1 + dist * i;
					Vector2 dynamicAddition = new Vector2(dynamLength * ((float)currentSegMult / totalSeg), 0).RotatedBy(MathHelper.ToRadians(currentSeg * 180f / totalSeg + ai1));
					dynamicAddition += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)) * hookDistortionShake;
					Vector2 drawPos = pos + circularLocation - screenPos;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, NPC.GetAlpha(drawColor), point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					if (glow)
						for (int k = 0; k < 7; k++)
						{
							float x = Main.rand.Next(-10, 11) * 0.1f;
							float y = Main.rand.Next(-10, 11) * 0.1f;
							spriteBatch.Draw(texture2, drawPos + dynamicAddition + new Vector2(x, y), null, color, point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
						}
					currentSeg++;
					currentSegMult++;
				}
				for (int i = 0; i < max; i++)
				{
					Vector2 bobbing = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(180f / max * i));
					bobbing.Y *= (midPointDist / 40f);
					Vector2 circularLocation = new Vector2(0, bobbing.Y).RotatedBy(point1To2.ToRotation() + MathHelper.ToRadians(ai2)); //applies rotation
					Vector2 dist = -point1To2 / (float)(max * 2);
					Vector2 pos = centerOfCircle + dist * i;
					Vector2 dynamicAddition = new Vector2(dynamLength * ((float)currentSegMult / totalSeg), 0).RotatedBy(MathHelper.ToRadians(currentSeg * 180f / totalSeg + ai1));
					dynamicAddition += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)) * hookDistortionShake;
					Vector2 drawPos = pos + circularLocation - screenPos;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, NPC.GetAlpha(drawColor), point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					if (glow)
						for (int k = 0; k < 7; k++)
						{
							float x = Main.rand.Next(-10, 11) * 0.1f;
							float y = Main.rand.Next(-10, 11) * 0.1f;
							spriteBatch.Draw(texture2, drawPos + dynamicAddition + new Vector2(x, y), null, color, point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
						}
					currentSeg++;
					currentSegMult--;
				}
				max = 8;
				for (int i = 0; i < max; i++)
				{
					Vector2 dist = -pointEndTo2 / (float)(max);
					Vector2 pos = point2 + dist * i;
					Vector2 dynamicAddition = new Vector2(dynamLength * ((float)currentSegMult / totalSeg), 0).RotatedBy(MathHelper.ToRadians(currentSeg * 180f / totalSeg + ai1));
					dynamicAddition += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)) * hookDistortionShake;
					Vector2 drawPos = pos - screenPos;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, NPC.GetAlpha(drawColor), pointEndTo2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					if (glow)
						for (int k = 0; k < 7; k++)
						{
							float x = Main.rand.Next(-10, 11) * 0.1f;
							float y = Main.rand.Next(-10, 11) * 0.1f;
							spriteBatch.Draw(texture2, drawPos + dynamicAddition + new Vector2(x, y), null, color, pointEndTo2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
						}
					currentSeg++;
					currentSegMult--;
				}
				if (j == 0)
					j = 3;
				else if (j == 3)
					j = 1;
				else if (j == 1)
					j = 2;
				else if (j == 2)
					j = 4;
			}
			DrawGlow(spriteBatch, screenPos, drawColor);
			return true;
        }
		bool moveLegsDynamic = true;
		bool moveLegsReturn = true;
		bool watchPlayer = true;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !dormant;
        }
        public override void BossHeadSlot(ref int index)
        {
			index = dormant ? -1 : index;
        }
        public override bool PreAI()
		{
			if(NPC.CountNPCS(ModContent.NPCType<TheAdvisorHead>()) > 1)
            {
				NPC.active = false;
            }
			NPC.boss = !dormant;
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if(watchPlayer)
			{
				fireToX = player.Center.X;
				fireToY = player.Center.Y;
			}
			ai1++;
			if(hookDistortion >= 1f)
            {
				hookDistortion -= 0.05f;
				hookDistortion *= 0.96f;
			}
			if (hookDistortionShake >= 0f)
			{
				hookDistortionShake -= 0.01f;
				hookDistortionShake *= 0.98f;
			}
			else
            {
				hookDistortionShake = 0;
            }
			if (runOnce)
			{
				runOnce = false;
				for (int i = 0; i < 4; i++)
				{
					float extraX = 0;
					if (i == 0)
						extraX = -44;
					if (i == 1)
						extraX = -26;
					if (i == 2)
						extraX = 26;
					if (i == 3)
						extraX = 44;
					float extraY = 0;
					if (i == 0 || i == 3)
						extraY = 48;
					hookPos[i] = new Vector2(extraX * 2f, 120 - extraY);
					hookPosTrue[i] = new Vector2(extraX * 2f, 120 - extraY);
				}
			}
			bool flag = true;
			if (flag)
			{
				if (NPC.velocity.X == 0f)
					ai3++;
				ai3 += NPC.velocity.X * 1.2f;
				for (int i = 0; i < 4; i++)
				{
					Vector2 toPos = hookPos[i] - hookPosTrue[i];
					Vector2 rotatePos = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(ai3 + 90 * i));
					if(dormant && ConstructIds[i] != -1)
                    {
						NPC construct = Main.npc[ConstructIds[i]];
						if(construct.active && construct.type == ModContent.NPCType<OtherworldlyConstructHead2>())
						{
							Vector2 direction = hookPos[i] + NPC.Center - new Vector2(construct.ai[2], construct.ai[3]);
							direction = direction.SafeNormalize(Vector2.Zero);
							Vector2 worldPos = hookPos[i] + NPC.Center;
							int i2 = (int)(worldPos.X / 16);
							int j2 = (int)(worldPos.Y / 16);

							if (Main.tile[i2, j2].HasTile && !Main.tile[i2, j2].IsActuated && Main.tileSolid[Main.tile[i2, j2].TileType] || Main.tileSolidTop[Main.tile[i2, j2].TileType])
							{

							}
							else
								hookPos[i] += direction * -6;

						}
						else
                        {
							ConstructIds[i] = -1;
                        }
                    }
					else if (toPos.Length() <= 24)
					{
						if (moveLegsDynamic)
						{
							hookPos[i].Y = hookPosTrue[i].Y + rotatePos.Y;
							hookPos[i].X = hookPosTrue[i].X + rotatePos.X;
							hookPos[i] -= NPC.velocity * 0.01f;
						}
					}
					else if (moveLegsReturn && (!moveLegsDynamic || toPos.Length() > 24))
					{
						float speed = 1.75f + 0.07f * toPos.Length();
						if (speed > toPos.Length())
							speed = toPos.Length();
						toPos = toPos.SafeNormalize(Vector2.Zero);
						hookPos[i] -= toPos * speed;
					}
				}
			}
			bool lineOfSight = Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
			if (dormant)
			{
				ScaleExpertStats();
				attackPhase1 = -1;
				attackPhase2 = -1;
				NPC.velocity.Y = new Vector2(0.08f, 0).RotatedBy(MathHelper.ToRadians(ai1)).Y;
				NPC.dontTakeDamage = true;
				NPC.dontCountMe = true;
				bool constructsActive = false;
				for (int i = 0; i < 4; i++)
				{
					if (ConstructIds[i] != -1)
					{
						NPC construct = Main.npc[ConstructIds[i]];
						if (construct.active && construct.type == Mod.Find<ModNPC>("OtherworldlyConstructHead2").Type)
						{
							constructsActive = true;
						}
					}
				}
				if (!constructsActive)
				{
					if ((NPC.Center - player.Center).Length() < 400f && lineOfSight)
					{
						dormantCounter++;
					}
				}
				if(dormantCounter > 90)
				{
					SOTSUtils.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 1.25f);
					if (Main.netMode != NetmodeID.Server)
						Main.NewText(Language.GetTextValue("Mods.SOTS.BossAwoken.Advisor"), 175, 75, byte.MaxValue);
					dormant = false;
					NPC.dontTakeDamage = false;
					NPC.dontCountMe = false;
					NPC.boss = true;
				}
				if(Main.netMode == NetmodeID.Server)
					NPC.netUpdate = true;
				return false;
			}
			return true;
		}
		public void ScaleExpertStats()
		{
			NPC.life = NormalModeHP;
			NPC.lifeMax = NormalModeHP;
			NPC.damage = NormalModeDamage;
			NPC.ScaleStats(null, Main.GameModeInfo, null);
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/TheAdvisorHead_Eye");
			Texture2D texture3 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/TheAdvisorHead_EyeClosed");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Advisor/TheAdvisorHead_Highlight");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = NPC.Center - screenPos;

			float shootToX = fireToX - NPC.Center.X;
			float shootToY = fireToY - NPC.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			float reset = eyeReset;
			if (dormant)
            {
				reset = 0f;
				texture = texture3;
			}
			distance = reset * 1f * NPC.scale / distance;

			shootToX *= distance;
			shootToY *= distance;

			drawPos.X += shootToX;
			drawPos.Y += shootToY + 4 + (dormant ? 2 : 0);
			Color color = new Color(100, 100, 100, 0);
			spriteBatch.Draw(texture, drawPos, null, NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			if (glow)
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
					y += 4;
					Rectangle frame = new Rectangle(0, texture2.Height / 10 * highlightFrame, texture2.Width, texture2.Height / 10);
					spriteBatch.Draw(texture2, new Vector2((float)(NPC.Center.X - (int)screenPos.X) + x, (float)(NPC.Center.Y - (int)screenPos.Y) + y), frame, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
		}
		public override void AI()
		{
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Advisor");
			SceneEffectPriority = SceneEffectPriority.BossMedium;
			NPC.TargetClosest(false);
			NPC.spriteDirection = 1;
			bool phase2 = NPC.life < NPC.lifeMax * (0.45f + (Main.expertMode ? 0.2f : 0));
			if(attackPhase1 == -1 && (attackPhase2 == -1 || phase2))
            {	
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					while (attackPhase1 == lastAttackPhase1 || attackPhase1 < 0)
					{
						attackPhase1 = Main.rand.Next(3);
					}
					lastAttackPhase1 = (int)attackPhase1;
					attackTimer1 = 0;
					NPC.netUpdate = true;
				}
			}
			if(attackPhase2 == -1 && (attackPhase1 == -2 || phase2))
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					while(attackPhase2 == lastAttackPhase2 || attackPhase2 < 0)
					{
						attackPhase2 = Main.rand.Next(3);
					}
					lastAttackPhase2 = (int)attackPhase2;
					attackTimer2 = 0;
					NPC.netUpdate = true;
				}
			}
			if (attackPhase1 == -2)
			{
				attackPhase1 = -1;
			}
			if (attackTimer1 >= 0 && attackTimer2 >= 0)
			{
				DoAttack1();
			}
			else if(attackTimer1 < 0)
            {
				NPC.velocity *= 0.98f;
				attackTimer1++;
			}
			if (attackTimer2 >= 0 && attackTimer1 >= 0)
			{
				DoAttack2();
			}
			else if (attackTimer2 < 0)
			{
				attackTimer2++;
			}

		}
		float steepTurn = 20f;
		public override void PostAI()
        {
			Player player = Main.player[NPC.target];
			if (player.dead && !dormant)
				despawn++;
			else
				despawn = 0;
			if(despawn >= 480)
            {
				NPC.active = false;
            }

			base.PostAI();
        }
		int tracerCounter = 0;
        public void DoAttack1() //body and PHASE attacks
		{
			Player player = Main.player[NPC.target];
			attackTimer1++;
			if(attackPhase1 == 0) //hovering attack
			{
				moveLegsDynamic = true;
				Vector2 circularAddition = new Vector2(7, 0).RotatedBy(MathHelper.ToRadians(attackTimer1 * 8));
				float speedMod = 1f;
				if (attackTimer1 >= 360)
				{
					moveLegsReturn = false;
					moveLegsDynamic = false;
					speedMod = (120f - (attackTimer1 - 360)) / 120f;
					if (speedMod < 0)
						speedMod = 0;
					circularAddition.Y -= (attackTimer1 - 360) * 1f;
					glow = true;
					if(attackTimer1 == 390)
						SOTSUtils.PlaySound(SoundID.Item15, (int)NPC.Center.X, (int)NPC.Center.Y, 0.7f);
					if (attackTimer1 == 420)
						SOTSUtils.PlaySound(SoundID.Item15, (int)NPC.Center.X, (int)NPC.Center.Y, 1f);
					if(attackTimer1 == 450)
						SOTSUtils.PlaySound(SoundID.Item15, (int)NPC.Center.X, (int)NPC.Center.Y, 1.3f);
					if (attackTimer1 >= 480)
					{
						if (attackTimer1 == 480)
							SOTSUtils.PlaySound(SoundID.Item96, (int)NPC.Center.X, (int)NPC.Center.Y, 1.4f);
						if (attackTimer1 < 510)
						{
							hookDistortion += 8.5f;
						}
						ai1 += 16;
						for (int i = 0; i < 4; i++)
						{
							float extraDeg = 0;
							if (i == 0)
								extraDeg = 21;
							if (i == 1)
								extraDeg = 7;
							if (i == 2)
								extraDeg = -7;
							if (i == 3)
								extraDeg = -21;
							for(int j = 0; j < 3; j++)
							{
								Vector2 worldPos = hookPos[i] + NPC.Center;
								int i2 = (int)(worldPos.X / 16);
								int j2 = (int)(worldPos.Y / 16);
								if (!(Main.tile[i2, j2].HasTile && !Main.tile[i2, j2].IsActuated && Main.tileSolid[Main.tile[i2, j2].TileType] || Main.tileSolidTop[Main.tile[i2, j2].TileType]))
								{
									Vector2 downStrike = new Vector2(0, 8).RotatedBy(MathHelper.ToRadians(extraDeg));
									if (attackTimer1 == 480 && j == 0)
										if (Main.netMode != NetmodeID.MultiplayerClient)
										{
											int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
											Projectile.NewProjectile(NPC.GetSource_FromAI(), hookPos[i].X + NPC.Center.X - downStrike.X, hookPos[i].Y + NPC.Center.Y - downStrike.Y, downStrike.X * 1f, downStrike.Y * 1f, ModContent.ProjectileType<PhaseSpear>(), damage2, 0, Main.myPlayer);
										}
									hookPos[i] += downStrike;

								}
							}
						}
					}
					else
					{
						if(hookDistortionShake < 0.3f)
							hookDistortionShake += 0.0125f;
						for (int i = 0; i < 4; i++)
						{
							if (attackTimer1 == 360)
								hookPos[i] = hookPosTrue[i];
							hookPos[i] *= 0.995f;
						}
					}
					if(attackTimer1 >= 600)
					{
						if (!NPC.AnyNPCs(ModContent.NPCType<PhaseEye>()))
							for (int i = 0; i < 4; i++)
							{
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									int npc1 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(hookPos[i].X + NPC.Center.X), (int)(hookPos[i].Y + NPC.Center.Y - 20), Mod.Find<ModNPC>("PhaseEye").Type);
									Main.npc[npc1].netUpdate = true;
								}
								for (int j = 0; j < 20; j++)
								{
									int dust = Dust.NewDust(new Vector2((hookPos[i].X + NPC.Center.X) - 8, (int)(hookPos[i].Y + NPC.Center.Y) - 8), 4, 4, DustID.PinkTorch);
									Main.dust[dust].velocity *= 2f;
									Main.dust[dust].scale *= 4f;
									Main.dust[dust].velocity += new Vector2(0, -5);
									Main.dust[dust].noGravity = true;
								}
							}
						moveLegsReturn = true;
						moveLegsDynamic = true;
						glow = false;
						attackPhase1 = -2;
						attackTimer1 = -60;
                    }
				}
				Vector2 toLocation = player.Center + new Vector2(0, -200 + circularAddition.Y);
				MoveTo(toLocation, 7f * speedMod * speedMod, 0.007f * speedMod * speedMod, 20f);
            }
			if(attackPhase1 == 1)
			{
				Vector2 circularAddition = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(attackTimer1 * 7));
				Vector2 toLocation = player.Center + new Vector2(0, -200 + circularAddition.Y);
				MoveTo(toLocation, 1.4f, 0.0002f, 10f);
				if (attackTimer1 >= 120)
				{
					glow = true;
					NPC.velocity *= 0.9f;
					if (attackTimer1 % 45 == 0)
					{
						int damage = SOTSNPCs.GetBaseDamage(NPC) / 2;
						tracerCounter++;
						if (tracerCounter < 9)
						{
							float locX = player.Center.X + Main.rand.Next(-200, 201);
							float locY = player.Center.Y + Main.rand.Next(-200, 201);
							bool inBlock = true;
							while (inBlock)
							{
								int i = (int)locX / 16;
								int j = (int)locY / 16;
								if (Main.tileSolid[Main.tile[i, j ].TileType] && Main.tile[i, j].HasTile && !Main.tileSolidTop[Main.tile[i, j ].TileType])
								{
									locX = player.Center.X + Main.rand.Next(-200, 201);
									locY = player.Center.Y + Main.rand.Next(-200, 201);
									inBlock = true;
								}
								else
								{
									inBlock = false;
									break;
								}
							}
							SOTSUtils.PlaySound(SoundID.Item30, (int)locX, (int)locY, 0.2f);
							if (Main.netMode != NetmodeID.MultiplayerClient)
								Projectile.NewProjectile(NPC.GetSource_FromAI(), locX, locY, 0, 0, ModContent.ProjectileType<OtherworldlyTracer>(), damage, 0f, Main.myPlayer, (1071) - (attackTimer1 * 2), NPC.whoAmI);
						}
					}
					if (tracerCounter >= 9)
					{
						tracerCounter = 0;
						attackTimer1 = -150;
						attackPhase1 = -2;
						glow = false;
						SOTSUtils.PlaySound(SoundID.Item92, NPC.Center);
						for (int i = 0; i < Main.projectile.Length; i++)
						{
							Projectile proj = Main.projectile[i];
							if (proj.active && proj.type == ModContent.ProjectileType<OtherworldlyTracer>() && proj.ai[1] == NPC.whoAmI)
							{
								int damage = SOTSNPCs.GetBaseDamage(NPC) / 2;
								Vector2 toProj = proj.Center - NPC.Center;
								toProj /= 30f;
								if (Main.netMode != NetmodeID.MultiplayerClient)
									Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, toProj.X, toProj.Y, ModContent.ProjectileType<OtherworldlyBall>(), damage, 0, Main.myPlayer);
							}
						}
						MoveTo(toLocation, -12f, 0, 1);
						ai3 += NPC.velocity.X * 1.2f;
					}
				}
				else
                {
					tracerCounter = 0;
				}
			}
			if(attackPhase1 == 2)
            {
				watchPlayer = false;
				if(attackTimer1 > 150)
				{
					glow = true; 
					if (attackTimer1 % 240 <= 60)
					{
						ai3 += 2;
						NPC.velocity *= 0f;
						Vector2 skyPos = new Vector2(player.Center.X + (attackTimer1 % 240 - 30) * 30 * ((NPC.whoAmI % 2) * 2 - 1) * (attackTimer1 % 480 >= 240 ? 1 : -1), NPC.Center.Y - 200f);
						fireToX = skyPos.X;
						fireToY = skyPos.Y;
						if (attackTimer1 % 10 == 0)
						{
							eyeReset = -3f;
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
								damage2 = (int)(damage2 * 1.75f);
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, -10f, ModContent.ProjectileType<ThunderColumnFast>(), damage2, NPC.target, Main.myPlayer, fireToX, fireToY);
							}
						}
					}
					else 
					{
						watchPlayer = true;
						if (attackTimer1 % 240 >= 150)
						{
							Vector2 circularAddition = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(attackTimer1 * 7));
							Vector2 toLocation = player.Center + new Vector2(0, -200 + circularAddition.Y);
							MoveTo(toLocation, 15f, 0.004f, 1f);
						}
					}
				}
				else
				{
					Vector2 circularAddition = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(attackTimer1 * 7));
					Vector2 toLocation = player.Center + new Vector2(0, -200 + circularAddition.Y);
					MoveTo(toLocation, 3f, 0.002f, 1f);
				}
				if (eyeReset < 2.5f)
				{
					eyeReset += 1f;
					if (eyeReset > 2.5f)
						eyeReset = 2.5f;
				}
				if(attackTimer1 > 900)
                {
					NPC.velocity *= 0f;
					glow = false;
					attackPhase1 = -2;
					attackTimer1 = -90;
                }
            }
		}
		public void MoveTo(Vector2 toLocation, float speed, float speedMultDist, float steepCoef, bool noSteep = false)
		{
			if(steepCoef < 2f)
            {
				noSteep = true;
			}
			float distance = (NPC.Center - toLocation).Length();
			int i = (int)(NPC.Center.X / 16);
			int j = (int)(NPC.Center.Y / 16);
			if (Main.tile[i, j].HasTile && Main.tileSolidTop[Main.tile[i, j ].TileType] == false && Main.tileSolid[Main.tile[i, j ].TileType] == true)
            {
				if (speed < 4f)
					speed = 4f;
				speed *= 2.4f;
				speedMultDist *= 2f;
			}
			else if (distance > 2400f && speedMultDist > 0)
			{
				speed *= 3f;
				speedMultDist *= 3f;
			}
			speed = speed + distance * speedMultDist;
			if (speed > distance)
			{
				speed = distance;
			}
			Vector2 velo = (NPC.Center - toLocation).SafeNormalize(new Vector2(1, 0));
			NPC.velocity.Y *= 0f;
			NPC.velocity.X *= 0f;
			if (velo.X > 0.2f)
			{
				steepTurn++;
				if (steepTurn > steepCoef)
					steepTurn = steepCoef;
			}
			else if (velo.X < -0.2f)
			{
				steepTurn--;
				if (steepTurn < -steepCoef)
					steepTurn = -steepCoef;
			}
			else
			{
				steepTurn = 0;
			}
			if(noSteep)
			{
				NPC.velocity += -new Vector2(velo.X, velo.Y) * speed;
			}
			else
				NPC.velocity += -new Vector2(Math.Abs(velo.X) * steepTurn / steepCoef, velo.Y) * speed;
		}
		public void DoAttack2() //weapon and holo attacks
		{
			Player player = Main.player[NPC.target];
			attackTimer2++;
			if(attackPhase2 == 0)
			{
				Vector2 circularAddition = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(attackTimer2 * 7));
				Vector2 toLocation = player.Center + new Vector2(0, -200 + circularAddition.Y);
				if(attackPhase1 == -1 || attackPhase1 == -2)
				{
					MoveTo(toLocation, 0.8f, 0.0001f, 10f);
					ai3 += NPC.velocity.X * 1.2f;
				}
				if (attackTimer2 == 30)
					SOTSUtils.PlaySound(SoundID.Mech, (int)NPC.Center.X, (int)NPC.Center.Y, 1f);
				if (attackTimer2 == 60)													    
					SOTSUtils.PlaySound(SoundID.Mech, (int)NPC.Center.X, (int)NPC.Center.Y, 1.15f);
				if (attackTimer2 == 90)													    
					SOTSUtils.PlaySound(SoundID.Mech, (int)NPC.Center.X, (int)NPC.Center.Y, 1.3f);
				if (attackTimer2 == 810)												    
					SOTSUtils.PlaySound(SoundID.Mech, (int)NPC.Center.X, (int)NPC.Center.Y, 1.3f);
				if (attackTimer2 == 840)												    
					SOTSUtils.PlaySound(SoundID.Mech, (int)NPC.Center.X, (int)NPC.Center.Y, 1.15f);
				if (attackTimer2 == 870)												    
					SOTSUtils.PlaySound(SoundID.Mech, (int)NPC.Center.X, (int)NPC.Center.Y, 1f);
				if(attackTimer2 > 90 && attackTimer2 < 810)
				{
					float FasterRate = 1f;
					if (Main.expertMode == true)
						FasterRate = 1.25f;
					if(attackTimer2 % (int)(60 / FasterRate) == 0)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
							damage2 = (int)(damage2 * 0.8f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X - 54, NPC.Center.Y + 20, Main.rand.Next(-10, 11) * 0.5f, Main.rand.Next(-10, 11) * 0.5f, ModContent.ProjectileType<HoloMissile>(), damage2, 0, Main.myPlayer, 0, NPC.target);
						}
						SOTSUtils.PlaySound(SoundID.Item61, (int)NPC.Center.X - 54, (int)NPC.Center.Y + 20, 1f);
						for (int i = 0; i < 15; i++)
						{
							int dust = Dust.NewDust(NPC.Center + new Vector2(-54 - 8, 20 - 8), 8, 8, DustID.Electric, 0, 0, 0, default, 1.25f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2f;
						}
					}
					if (attackTimer2 % (int)(60 / FasterRate) == (int)(30 / FasterRate))
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 54, NPC.Center.Y + 20, Main.rand.Next(-10, 11) * 0.5f, Main.rand.Next(-10, 11) * 0.5f, ModContent.ProjectileType<HoloMissile>(), damage2, 0, Main.myPlayer, 0, NPC.target);
						}
						SOTSUtils.PlaySound(SoundID.Item61, (int)NPC.Center.X + 54, (int)NPC.Center.Y + 20, 1f);
						for (int i = 0; i < 15; i++)
						{
							int dust = Dust.NewDust(NPC.Center + new Vector2(54 - 8, 20 - 8), 8, 8, DustID.Electric, 0, 0, 0, default, 1.25f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2.5f;
						}
					}
				}
				if (attackTimer2 > 900)
				{
					attackPhase2 = -1;
					attackTimer2 = -150;
				}
			}

			if(attackPhase2 == 1)
			{
				Vector2 circularAddition = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(attackTimer2 * 7));
				Vector2 toLocation = player.Center + new Vector2(0, -200 + circularAddition.Y);
				if (attackTimer2 % 160 < 80)
				{
					if (attackPhase1 == -1 || attackPhase1 == -2)
					{
						MoveTo(toLocation, 1f, 0.0001f, 10f);
						ai3 += NPC.velocity.X * 1.2f;
					}
				}
				else
                {
					NPC.velocity *= 0f;
                }
				if (laserDirection != nextLaserDirection)
                {
					if(laserDirection < nextLaserDirection)
                    {
						laserDirection += 1f;
                    }
					else
                    {
						laserDirection -= 1f;
                    }
                }
				if(attackTimer2 % 16 == 0 && attackTimer2 < 640)
                {
					if(attackTimer2 % 160 == 80)
					{
						for (int direction = -1; direction <= 1; direction += 2)
						{
							Vector2 shift = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(laserDirection));
							Vector2 velo = new Vector2(80 * direction, 0).RotatedBy(MathHelper.ToRadians(laserDirection));
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + velo.X, NPC.Center.Y + Math.Abs(shift.X) + 4 + velo.Y, velo.SafeNormalize(Vector2.Zero).X * 4f, velo.SafeNormalize(Vector2.Zero).Y * 4f, Mod.Find<ModProjectile>("ChargeBeam").Type, damage2, 0, Main.myPlayer);
							}
						}
					}
					else if(attackTimer2 % 160 < 32)
					{
						SOTSUtils.PlaySound(SoundID.Mech, (int)NPC.Center.X, (int)NPC.Center.Y, 1f);
					}
					if (attackTimer2 % 160 == 0)
					{
						nextLaserDirection += 30 * ((NPC.whoAmI % 2) * 2 - 1);
					}
				}
				if(attackTimer2 > 750)
				{
					attackPhase2 = -1;
					attackTimer2 = -120;
				}
			}
		
			if (attackPhase2 == 2)
			{
				watchPlayer = true; //found out that this actually worked better?
				Vector2 circularAddition = new Vector2(48, 0).RotatedBy(MathHelper.ToRadians(attackTimer2 * 7));
				Vector2 toLocation = player.Center + new Vector2(0, -40 + circularAddition.Y);
				if (attackTimer2 > 180 && attackTimer2 % 90 <= 45 && attackTimer2 < 720)
				{
					float speedMod = attackTimer2 % 90;
					speedMod /= 90f;
					if (attackPhase1 == -1 || attackPhase1 == -2)
					{
						MoveTo(toLocation, 17.5f * speedMod, 0.012f * speedMod, 10f);
						ai3 += NPC.velocity.X * 1.2f;
					}
				}
				else
				{
					NPC.velocity *= 0f;
				}
				fireToX = player.Center.X;
				fireToY = player.Center.Y;
				eyeReset = 2.5f;
				if (attackTimer2 >= 180 && attackTimer2 % 90 == 0 && attackTimer2 < 810)
				{
					SOTSUtils.PlaySound(SoundID.Item93, (int)NPC.Center.X, (int)NPC.Center.Y, 1.3f);
					Vector2 shift = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(new Vector2(fireToX, fireToY).ToRotation()));
					Vector2 playerCenter = new Vector2(fireToX, fireToY);
					Vector2 fromCenter = playerCenter - NPC.Center;
					fromCenter = fromCenter.SafeNormalize(Vector2.Zero);
					fromCenter *= 72f;
					float velocityA = 3.55f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
						damage2 = (int)(damage2 * 0.8f);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + fromCenter.X, NPC.Center.Y + Math.Abs(shift.X) + 4 + fromCenter.Y, fromCenter.SafeNormalize(Vector2.Zero).X * velocityA, fromCenter.SafeNormalize(Vector2.Zero).Y * velocityA, Mod.Find<ModProjectile>("ThunderColumnBlue").Type, damage2, 0, Main.myPlayer, 3f);
					}
				}
				if (attackTimer2 > 810)
				{
					watchPlayer = true;
					attackPhase2 = -1;
					attackTimer2 = -90;
				}
			}
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Lead, 2.5f * (float)hit.HitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Advisor/TheAdvisorGore1"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Advisor/TheAdvisorGore2"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Advisor/TheAdvisorGore3"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Advisor/TheAdvisorGore4"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Advisor/TheAdvisorGore5"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Advisor/TheAdvisorGore6"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Advisor/TheAdvisorGore7"), 1f);
				for (int i = 0; i < 24; i++)
                {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2((float)(NPC.width * Main.rand.Next(100)) / 100f, (float)(NPC.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, NPC.velocity, Main.rand.Next(61, 64), 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2((NPC.width * Main.rand.Next(100)) / 100f, NPC.height) - Vector2.One * 10f, NPC.velocity, ModGores.GoreType("Gores/OtherworldVineGore"), 1f);
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TheAdvisorBossBag>()));
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<OtherworldlyAlloy>(), 3, 10, 16))
				.OnFailedRoll(ItemDropRule.Common(ModContent.ItemType<MeteoriteKey>(), 1, 1, 1));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<StarlightAlloy>(), 3, 10, 16))
				.OnFailedRoll(ItemDropRule.Common(ModContent.ItemType<SkywareKey>(), 1, 1, 1));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HardlightAlloy>(), 3, 10, 16))
				.OnFailedRoll(ItemDropRule.Common(ModContent.ItemType<StrangeKey>(), 1, 1, 1));
			npcLoot.Add(notExpertRule);
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<AdvisorRelic>()));
		}
		public override void BossLoot(ref string name, ref int potionType)
		{ 
			SOTSWorld.downedAdvisor = true;
			potionType = ItemID.HealingPotion;
		}
        public override void OnKill()
        {
			int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<OtherworldlySpirit>());
			Main.npc[n].velocity.Y = -10f;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
		}
    }
}