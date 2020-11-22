using System;
using System.IO;
using System.Security.Authentication;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{	[AutoloadBossHead]
	public class TheAdvisorHead : ModNPC
	{
		private int expertModifier = 1;
		int despawn = 0;
		private float attackPhase1 {
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}

		private float attackPhase2
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}

		private float attackTimer1 {
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}

		private float attackTimer2 {
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		bool runOnce = true;
		float fireToX = 0;
		float fireToY = 0;
		float eyeReset = 2.5f;
		bool glow = false;
		Vector2[] hookPos = {new Vector2(-1, -1) , new Vector2(-1, -1) , new Vector2(-1, -1) , new Vector2(-1, -1) };
		Vector2[] hookPosTrue = { new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1) };
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Advisor");
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
			writer.Write(npc.dontTakeDamage);
			writer.Write(npc.dontCountMe);
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
			npc.dontTakeDamage = reader.ReadBoolean();
			npc.dontCountMe = reader.ReadBoolean();
		}
		public override bool CheckActive()
		{
			return !dormant;
		}
		public override void SetDefaults()
        {
            npc.aiStyle = 0;
            npc.lifeMax = 10000;
            npc.damage = 54;
            npc.defense = 30;
            npc.knockBackResist = 0f;
            npc.width = 78;
            npc.height = 98;
            Main.npcFrameCount[npc.type] = 1;
            npc.value = 150000;
            npc.npcSlots = 10f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.buffImmune[BuffID.ShadowFlame] = true;
            bossBag = mod.ItemType("TheAdvisorBossBag");
			music = -1;
			musicPriority = (MusicPriority)(-1);
			//bossBag = mod.ItemType("BossBagBloodLord");
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
		public void DrawGlow(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/TheAdvisorHead_Spirit");
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			Color color = new Color(100, 100, 100, 0);
			if (attackPhase2 == 0)
			{
				Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Boss/AdvisorMissileAttachment");
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
						Main.spriteBatch.Draw(texture2, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X), (float)(npc.Center.Y - (int)Main.screenPosition.Y) + 20) + circularRotation, null, lightColor, 0f, drawOrigin2, 1f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					}
				}
			}
			if (attackPhase2 == 1)
			{
				Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Boss/AdvisorLaserAttachment");
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
						Main.spriteBatch.Draw(texture2, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X), (float)(npc.Center.Y - (int)Main.screenPosition.Y) + Math.Abs(shift.X) + 4) + circularRotation, null, lightColor, MathHelper.ToRadians(laserDirection), drawOrigin2, 1f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					}
				}
			}
			if (attackPhase2 == 2)
			{
				Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Boss/AdvisorTazerAttachment");
				Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
				if (attackTimer2 > 0)
				{
					float speed = 90f;
					float deg = new Vector2(fireToX - npc.Center.X, fireToY - npc.Center.Y).ToRotation();
					float amt = 56f * attackTimer2 / speed;
					if (amt > 56f)
					{
						amt = 56f;
					}
					float att = attackTimer2 - (990f - speed);
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
					Main.spriteBatch.Draw(texture2, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X), (float)(npc.Center.Y - (int)Main.screenPosition.Y) + Math.Abs(shift.X) + 4) + circularRotation, null, lightColor, deg, drawOrigin2, 1f, SpriteEffects.FlipHorizontally, 0f);
				}
			}
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				y += 4;
				Main.spriteBatch.Draw(texture, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y), null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for(int j = 0; j < 4;)
			{
				float scale = npc.scale;
				int ai2 = 0;
				if (j > 1)
					ai2 += 180;
				float npcRadians = npc.rotation;
				Vector2 toPos = npc.Center + hookPos[j].RotatedBy(npcRadians);
				Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/OtherworldVine");
				Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Constructs/OtherworldVine_Highlight");
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Vector2 npcPos = new Vector2(npc.Center.X + (-96 + 64 * j) * 1f * 0.25f, npc.position.Y + npc.height).RotatedBy(npcRadians);
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
					Vector2 drawPos = pos - Main.screenPosition;
					dynamicAddition += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)) * hookDistortionShake;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, npc.GetAlpha(lightColor), pointNpcTo1.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
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
					Vector2 drawPos = pos + circularLocation - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, npc.GetAlpha(lightColor), point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
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
					Vector2 drawPos = pos + circularLocation - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, npc.GetAlpha(lightColor), point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
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
					Vector2 drawPos = pos - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, npc.GetAlpha(lightColor), pointEndTo2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
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
			DrawGlow(spriteBatch, lightColor);
			return true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.6f);  
            npc.damage = (int)(npc.damage * 0.8f);  
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
			if(NPC.CountNPCS(mod.NPCType("TheAdvisorHead")) > 1)
            {
				npc.active = false;
            }
			npc.boss = !dormant;
			npc.TargetClosest(false);
			Player player = Main.player[npc.target];
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
				if (npc.velocity.X == 0f)
					ai3++;
				ai3 += npc.velocity.X * 1.2f;
				for (int i = 0; i < 4; i++)
				{
					Vector2 toPos = hookPos[i] - hookPosTrue[i];
					Vector2 rotatePos = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(ai3 + 90 * i));
					if(dormant && ConstructIds[i] != -1)
                    {
						NPC construct = Main.npc[ConstructIds[i]];
						if(construct.active && construct.type == mod.NPCType("OtherworldlyConstructHead2"))
						{
							Vector2 direction = hookPos[i] + npc.Center - new Vector2(construct.ai[2], construct.ai[3]);
							direction = direction.SafeNormalize(Vector2.Zero);
							Vector2 worldPos = hookPos[i] + npc.Center;
							int i2 = (int)(worldPos.X / 16);
							int j2 = (int)(worldPos.Y / 16);

							if (Main.tile[i2, j2].active() && !Main.tile[i2, j2].inActive() && Main.tileSolid[Main.tile[i2, j2].type] || Main.tileSolidTop[Main.tile[i2, j2].type])
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
							hookPos[i] -= npc.velocity * 0.01f;
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
			bool lineOfSight = Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height);
			if (dormant)
			{
				attackPhase1 = -1;
				attackPhase2 = -1;
				npc.velocity.Y = new Vector2(0.08f, 0).RotatedBy(MathHelper.ToRadians(ai1)).Y;
				npc.dontTakeDamage = true;
				npc.dontCountMe = true;
				bool constructsActive = false;
				for (int i = 0; i < 4; i++)
				{
					if (ConstructIds[i] != -1)
					{
						NPC construct = Main.npc[ConstructIds[i]];
						if (construct.active && construct.type == mod.NPCType("OtherworldlyConstructHead2"))
						{
							constructsActive = true;
						}
					}
				}
				if (!constructsActive)
				{
					if ((npc.Center - player.Center).Length() < 400f && lineOfSight)
					{
						dormantCounter++;
					}
				}
				if(dormantCounter > 90)
				{
					Main.PlaySound(15, (int)(npc.Center.X), (int)(npc.Center.Y), 0, 1.25f);
					Main.NewText("The Advisor has awoken!", 175, 75, byte.MaxValue);
					dormant = false;
					npc.dontTakeDamage = false;
					npc.dontCountMe = false;
				}
				return false;
			}
			if (Main.expertMode)
			{
				expertModifier = 2;
			}
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/TheAdvisorHead_Eye");
			Texture2D texture3 = ModContent.GetTexture("SOTS/NPCs/Boss/TheAdvisorHead_EyeClosed");
			Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Boss/TheAdvisorHead_Highlight");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = npc.Center - Main.screenPosition;

			float shootToX = fireToX - npc.Center.X;
			float shootToY = fireToY - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			float reset = eyeReset;
			if (dormant)
            {
				reset = 0f;
				texture = texture3;
			}
			distance = reset * 1f * npc.scale / distance;

			shootToX *= distance;
			shootToY *= distance;

			drawPos.X += shootToX;
			drawPos.Y += shootToY + 4 + (dormant ? 2 : 0);
			Color color = new Color(100, 100, 100, 0);
			spriteBatch.Draw(texture, drawPos, null, npc.GetAlpha(drawColor), npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			if (glow)
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
					y += 4;
					Main.spriteBatch.Draw(texture2, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y), null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
		}
		public override void AI()
		{
			music = MusicID.Boss4;
			musicPriority = MusicPriority.BossLow;
			npc.TargetClosest(false);
			npc.spriteDirection = 1;
			bool phase2 = npc.life < npc.lifeMax * (0.45f + (Main.expertMode ? 0.2f : 0));
			if(attackPhase1 == -1 && (attackPhase2 == -1 || phase2))
            {	
				if(Main.netMode != 1)
				{
					attackPhase1 = Main.rand.Next(3);
					attackTimer1 = 0;
					npc.netUpdate = true;
				}
			}
			if(attackPhase2 == -1 && (attackPhase1 == -2 || phase2))
			{
				if (Main.netMode != 1)
				{
					attackPhase2 = Main.rand.Next(3);
					attackTimer2 = 0;
					npc.netUpdate = true;
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
				npc.velocity *= 0.98f;
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
			Player player = Main.player[npc.target];
			if (player.dead && !dormant)
				despawn++;
			else
				despawn = 0;
			if(despawn >= 480)
            {
				npc.active = false;
            }

			base.PostAI();
        }
		int tracerCounter = 0;
        public void DoAttack1() //body and PHASE attacks
		{
			Player player = Main.player[npc.target];
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
						Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 15, 0.7f);
					if (attackTimer1 == 420)
						Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 15, 1f);
					if(attackTimer1 == 450)
						Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 15, 1.3f);
					if (attackTimer1 >= 480)
					{
						if (attackTimer1 == 480)
							Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 96, 1.4f);
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
								Vector2 worldPos = hookPos[i] + npc.Center;
								int i2 = (int)(worldPos.X / 16);
								int j2 = (int)(worldPos.Y / 16);
								if (Main.tile[i2, j2].active() && !Main.tile[i2, j2].inActive() && Main.tileSolid[Main.tile[i2, j2].type] || Main.tileSolidTop[Main.tile[i2, j2].type]) ;
								else
								{
									Vector2 downStrike = new Vector2(0, 8).RotatedBy(MathHelper.ToRadians(extraDeg));
									if (attackTimer1 == 480 && j == 0)
										if (Main.netMode != 1)
										{
											int damage2 = npc.damage / 2;
											if (Main.expertMode)
											{
												damage2 = (int)(damage2 / Main.expertDamage);
											}
											Projectile.NewProjectile(hookPos[i].X + npc.Center.X - downStrike.X, hookPos[i].Y + npc.Center.Y - downStrike.Y, downStrike.X * 1f, downStrike.Y * 1f, mod.ProjectileType("PhaseSpear"), damage2, 0, Main.myPlayer);
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
						if (!NPC.AnyNPCs(mod.NPCType("PhaseEye")))
							for (int i = 0; i < 4; i++)
							{
								if (Main.netMode != 1)
								{
									int npc1 = NPC.NewNPC((int)(hookPos[i].X + npc.Center.X), (int)(hookPos[i].Y + npc.Center.Y - 20), mod.NPCType("PhaseEye"));
									Main.npc[npc1].netUpdate = true;
								}
								{
									for (int j = 0; j < 20; j++)
									{
										int dust = Dust.NewDust(new Vector2((hookPos[i].X + npc.Center.X) - 8, (int)(hookPos[i].Y + npc.Center.Y) - 8), 4, 4, 242);
										Main.dust[dust].velocity *= 2f;
										Main.dust[dust].scale *= 4f;
										Main.dust[dust].velocity += new Vector2(0, -5);
										Main.dust[dust].noGravity = true;
									}
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

			if (attackPhase1 == 1)
			{
				Vector2 circularAddition = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(attackTimer1 * 7));
				Vector2 toLocation = player.Center + new Vector2(0, -200 + circularAddition.Y);
				MoveTo(toLocation, 1.4f, 0.0002f, 10f);
				if (attackTimer1 >= 120)
				{
					glow = true;
					npc.velocity *= 0.9f;
					if (attackTimer1 % 45 == 0)
					{
						int damage = npc.damage / 2;
						if (Main.expertMode)
						{
							damage = (int)(damage / Main.expertDamage);
						}
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
								if (Main.tileSolid[Main.tile[i, j].type] && Main.tile[i, j].active() && !Main.tileSolidTop[Main.tile[i, j].type])
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
							Main.PlaySound(2, (int)locX, (int)locY, 30, 0.2f);
							if (Main.netMode != 1)
								Projectile.NewProjectile(locX, locY, 0, 0, mod.ProjectileType("OtherworldlyTracer"), damage, 0f, Main.myPlayer, (1071) - (attackTimer1 * 2), npc.whoAmI);
						}
					}
					if (tracerCounter >= 9)
					{
						tracerCounter = 0;
						attackTimer1 = -150;
						attackPhase1 = -2;
						glow = false;
						Main.PlaySound(SoundID.Item92, npc.Center);
						for (int i = 0; i < Main.projectile.Length; i++)
						{
							Projectile proj = Main.projectile[i];
							if (proj.active && proj.type == mod.ProjectileType("OtherworldlyTracer") && proj.ai[1] == npc.whoAmI)
							{
								int damage = npc.damage / 2;
								if (Main.expertMode)
								{
									damage = (int)(damage / Main.expertDamage);
								}
								Vector2 toProj = proj.Center - npc.Center;
								toProj /= 30f;
								if (Main.netMode != 1)
									Projectile.NewProjectile(npc.Center.X, npc.Center.Y, toProj.X, toProj.Y, mod.ProjectileType("OtherworldlyBall"), damage, 0, Main.myPlayer);
							}
						}
						MoveTo(toLocation, -12f, 0, 1);
						ai3 += npc.velocity.X * 1.2f;
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
						npc.velocity *= 0f;
						Vector2 skyPos = new Vector2(player.Center.X + (attackTimer1 % 240 - 30) * 30 * ((npc.whoAmI % 2) * 2 - 1) * (attackTimer1 % 480 >= 240 ? 1 : -1), npc.Center.Y - 200f);
						fireToX = skyPos.X;
						fireToY = skyPos.Y;
						if (attackTimer1 % 10 == 0)
						{
							eyeReset = -3f;
							if (Main.netMode != 1)
							{
								int damage2 = npc.damage / 2;
								if (Main.expertMode)
								{
									damage2 = (int)(damage2 / Main.expertDamage);
								}
								damage2 = (int)(damage2 * 1.75f);
								Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -10f, mod.ProjectileType("ThunderColumnFast"), damage2, npc.target, Main.myPlayer, fireToX, fireToY);
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
					npc.velocity *= 0f;
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
			int i = (int)(npc.Center.X / 16);
			int j = (int)(npc.Center.Y / 16);
			if (Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true)
            {
				if (speed < 4f)
					speed = 4f;
				speed *= 2f;
				speedMultDist *= 1.5f;
			}
			speed = speed + (npc.Center - toLocation).Length() * speedMultDist;
			if (speed > (npc.Center - toLocation).Length())
			{
				speed = (npc.Center - toLocation).Length();
			}
			Vector2 velo = (npc.Center - toLocation).SafeNormalize(new Vector2(1, 0));
			npc.velocity.Y *= 0f;
			npc.velocity.X *= 0f;
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
				npc.velocity += -new Vector2(velo.X, velo.Y) * speed;
			}
			else
				npc.velocity += -new Vector2(Math.Abs(velo.X) * steepTurn / steepCoef, velo.Y) * speed;
		}
		public void DoAttack2() //weapon and holo attacks
		{
			Player player = Main.player[npc.target];
			attackTimer2++;
			if(attackPhase2 == 0)
			{
				Vector2 circularAddition = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(attackTimer2 * 7));
				Vector2 toLocation = player.Center + new Vector2(0, -200 + circularAddition.Y);
				if(attackPhase1 == -1 || attackPhase1 == -2)
				{
					MoveTo(toLocation, 0.8f, 0.0001f, 10f);
					ai3 += npc.velocity.X * 1.2f;
				}
				if (attackTimer2 == 30)
					Main.PlaySound(SoundID.Mech, (int)npc.Center.X, (int)npc.Center.Y, 0, 1f);
				if (attackTimer2 == 60)
					Main.PlaySound(SoundID.Mech, (int)npc.Center.X, (int)npc.Center.Y, 0, 1.15f);
				if (attackTimer2 == 90)
					Main.PlaySound(SoundID.Mech, (int)npc.Center.X, (int)npc.Center.Y, 0, 1.3f);
				if (attackTimer2 == 810)
					Main.PlaySound(SoundID.Mech, (int)npc.Center.X, (int)npc.Center.Y, 0, 1.3f);
				if (attackTimer2 == 840)
					Main.PlaySound(SoundID.Mech, (int)npc.Center.X, (int)npc.Center.Y, 0, 1.15f);
				if (attackTimer2 == 870)
					Main.PlaySound(SoundID.Mech, (int)npc.Center.X, (int)npc.Center.Y, 0, 1f);
				if(attackTimer2 > 90 && attackTimer2 < 810)
				{
					float FasterRate = 1f;
					if (Main.expertMode == true)
						FasterRate = 1.25f;
					if(attackTimer2 % (int)(60 / FasterRate) == 0)
					{
						if (Main.netMode != 1)
						{
							int damage2 = npc.damage / 2;
							if (Main.expertMode)
							{
								damage2 = (int)(damage2 / Main.expertDamage);
							}
							damage2 = (int)(damage2 * 0.8f);
							Projectile.NewProjectile(npc.Center.X - 54, npc.Center.Y + 20, Main.rand.Next(-10, 11) * 0.5f, Main.rand.Next(-10, 11) * 0.5f, mod.ProjectileType("HoloMissile"), damage2, 0, Main.myPlayer, 0, npc.target);
						}
						Main.PlaySound(2, (int)npc.Center.X - 54, (int)npc.Center.Y + 20, 61, 1f);
						for (int i = 0; i < 15; i++)
						{
							int dust = Dust.NewDust(npc.Center + new Vector2(-54 - 8, 20 - 8), 8, 8, DustID.Electric, 0, 0, 0, default, 1.25f);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity *= 2f;
						}
					}
					if (attackTimer2 % (int)(60 / FasterRate) == (int)(30 / FasterRate))
					{
						if (Main.netMode != 1)
						{
							int damage2 = npc.damage / 2;
							if (Main.expertMode)
							{
								damage2 = (int)(damage2 / Main.expertDamage);
							}
							Projectile.NewProjectile(npc.Center.X + 54, npc.Center.Y + 20, Main.rand.Next(-10, 11) * 0.5f, Main.rand.Next(-10, 11) * 0.5f, mod.ProjectileType("HoloMissile"), damage2, 0, Main.myPlayer, 0, npc.target);
						}
						Main.PlaySound(2, (int)npc.Center.X + 54, (int)npc.Center.Y + 20, 61, 1f);
						for (int i = 0; i < 15; i++)
						{
							int dust = Dust.NewDust(npc.Center + new Vector2(54 - 8, 20 - 8), 8, 8, DustID.Electric, 0, 0, 0, default, 1.25f);
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
						ai3 += npc.velocity.X * 1.2f;
					}
				}
				else
                {
					npc.velocity *= 0f;
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
							if (Main.netMode != 1)
							{
								int damage2 = npc.damage / 2;
								if (Main.expertMode)
								{
									damage2 = (int)(damage2 / Main.expertDamage);
								}
								damage2 = (int)(damage2 * 2f);
								Projectile.NewProjectile(npc.Center.X + velo.X, npc.Center.Y + Math.Abs(shift.X) + 4 + velo.Y, velo.SafeNormalize(Vector2.Zero).X * 4f, velo.SafeNormalize(Vector2.Zero).Y * 4f, mod.ProjectileType("ChargeBeam"), damage2, 0, Main.myPlayer);
							}
						}
					}
					else if(attackTimer2 % 160 < 32)
					{
						Main.PlaySound(SoundID.Mech, (int)npc.Center.X, (int)npc.Center.Y, 0, 1f);
					}
					if (attackTimer2 % 160 == 0)
					{
						nextLaserDirection += 30 * ((npc.whoAmI % 2) * 2 - 1);
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
				if (attackTimer2 > 180 && attackTimer2 % 90 <= 45 && attackTimer2 < 900)
				{
					float speedMod = attackTimer2 % 90;
					speedMod /= 90f;
					if (attackPhase1 == -1 || attackPhase1 == -2)
					{
						MoveTo(toLocation, 16f * speedMod, 0.01f * speedMod, 10f);
						ai3 += npc.velocity.X * 1.2f;
					}
				}
				else
				{
					npc.velocity *= 0f;
				}
				fireToX = player.Center.X;
				fireToY = player.Center.Y;
				eyeReset = 2.5f;
				if (attackTimer2 >= 180 && attackTimer2 % 90 == 0 && attackTimer2 < 990)
				{
					Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 93, 1.3f);
					Vector2 shift = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(new Vector2(fireToX, fireToY).ToRotation()));
					Vector2 playerCenter = new Vector2(fireToX, fireToY);
					Vector2 fromCenter = playerCenter - npc.Center;
					fromCenter = fromCenter.SafeNormalize(Vector2.Zero);
					fromCenter *= 72f;
					if (Main.netMode != 1)
					{
						int damage2 = npc.damage / 2;
						if (Main.expertMode)
						{
							damage2 = (int)(damage2 / Main.expertDamage);
						}
						damage2 = (int)(damage2 * 0.8f);
						Projectile.NewProjectile(npc.Center.X + fromCenter.X, npc.Center.Y + Math.Abs(shift.X) + 4 + fromCenter.Y, fromCenter.SafeNormalize(Vector2.Zero).X * 3.75f, fromCenter.SafeNormalize(Vector2.Zero).Y * 3.75f, mod.ProjectileType("ThunderColumnBlue"), damage2, 0, Main.myPlayer, 3f);
					}
				}
				if (attackTimer2 > 990)
				{
					watchPlayer = true;
					attackPhase2 = -1;
					attackTimer2 = -90;
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				for (int i = 0; i < 50; i++)
				{
					int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("BigAetherDust"));
					Main.dust[dust].velocity *= 5f;
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TheAdvisorGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TheAdvisorGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TheAdvisorGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TheAdvisorGore4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TheAdvisorGore5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TheAdvisorGore6"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TheAdvisorGore7"), 1f);
				for (int i = 0; i < 24; i++)
                {
					Gore.NewGore(npc.position + new Vector2((float)(npc.width * Main.rand.Next(100)) / 100f, (float)(npc.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, npc.velocity, Main.rand.Next(61, 64), 1f);
					Gore.NewGore(npc.position + new Vector2((npc.width * Main.rand.Next(100)) / 100f, npc.height) - Vector2.One * 10f, npc.velocity, mod.GetGoreSlot("Gores/OtherworldVineGore"), 1f);
				}
			}
		}
		public override void BossLoot(ref string name, ref int potionType)
		{ 
			SOTSWorld.downedAdvisor = true;
			potionType = ItemID.HealingPotion;
		
			if(Main.expertMode)
			{ 
				npc.DropBossBags();
			} 
			else 
			{
				int type = Main.rand.Next(3);
				if (type  == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeteoriteKey"), 1);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SkywareKey"), 1);
					if(Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StrangeKey"), 1);
					}
				}
				if (type == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SkywareKey"), 1);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StrangeKey"), 1);
					if (Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeteoriteKey"), 1);
					}
				}
				if (type == 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeteoriteKey"), 1);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StrangeKey"), 1);
					if (Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SkywareKey"), 1);
					}
				}
			}
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OtherworldlySpirit"));
			Main.npc[n].velocity.Y = -10f;
			if (Main.netMode != 1)
				Main.npc[n].netUpdate = true;
		}
	}
}