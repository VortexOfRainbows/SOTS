using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using SOTS.NPCs.Constructs;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Pyramid;
using SOTS.Void;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Boss.Lux
{
	public class FakeLux : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 10000;   
            NPC.damage = 80; 
            NPC.defense = 10;  
            NPC.knockBackResist = 0f;
            NPC.width = 70;
            NPC.height = 70;
			Main.npcFrameCount[NPC.type] = 1;  
            NPC.value = 0;
            NPC.npcSlots = 1f;
			NPC.dontCountMe = true;
			NPC.HitSound = null;
			NPC.DeathSound = null;
			NPC.lavaImmune = true;
			NPC.netAlways = true;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.dontTakeDamage = true;
		}
		RingManager ring;
        public override bool PreKill()
        {
			return false;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.damage = (int)(NPC.damage * 7 / 8);
		}
		bool runOnce = true;
		float rotationCounter = 0;
		float acceleration = 0;
		public const float timeToAimAtPlayer = 40;
		public float aimToPlayer = 0;
		public float wingHeightLerp = 0;
        public override bool CheckActive()
        {
            return false;
        }
        public void modifyRotation(bool aimAt, Vector2 whereToAim, bool modifyWings = true)
		{
			Vector2 toPlayer = whereToAim - NPC.Center - NPC.velocity;
			NPC.rotation = NPC.velocity.X * 0.06f;
			if (aimAt)
			{
				aimToPlayer++;
			}
			else
			{
				aimToPlayer--;
			}
			aimToPlayer = MathHelper.Clamp(aimToPlayer, 0, timeToAimAtPlayer);
			if (modifyWings)
				wingHeightLerp = aimToPlayer / timeToAimAtPlayer * 0.85f;
			float r = toPlayer.ToRotation() - MathHelper.PiOver2;
			float x = NPC.rotation - r;
			x = MathHelper.WrapAngle(x);
			float lerpedAngle = MathHelper.Lerp(x, 0, aimToPlayer / timeToAimAtPlayer);
			lerpedAngle += r;
			NPC.rotation = lerpedAngle;

		}
		public float counter = 0;
		bool kill = false;
		float fireRateCounter = 0;
		public override bool PreAI()
		{
			int damage = Common.GlobalNPCs.SOTSNPCs.GetBaseDamage(NPC) / 2;
			if (runOnce)
				ring = new RingManager(MathHelper.ToRadians(NPC.ai[1]), 0.6f, 3, 72);
			WingStuff();
			runOnce = false;
			Player player = Main.player[NPC.target];
			Vector2 rotateCenter = player.Center;
			int parentID = (int)NPC.ai[0];
			if (parentID >= 0)
			{
				NPC npc2 = Main.npc[parentID];
				if (npc2.active && npc2.type == NPCType<Lux>())
				{
					Lux lux = npc2.ModNPC as Lux;
					if (lux != null && lux.desperation)
						kill = true;
					else
						rotateCenter = npc2.Center;
				}
				else
					kill = true;
			}
			else
				kill = true;
			float moveBack = 0;
			float max = 1200;
			if(NPC.ai[3] != 0)
            {
				max = 900;
            }
			if(counter > max)
			{
				if (counter > max + 30)
				{
					float rnCounter = (counter - max - 30) / 60f;
					if (rnCounter > 1)
					{
						rnCounter = 1;
						kill = true;
					}
					moveBack = (float)Math.Pow(rnCounter, 1.2f);
				}
				NPC.alpha += 4;
				ring.ResetVariables();
			}
			if (kill)
			{
				if (counter < max)
					for (int i = 0; i < 50; i++)
					{
						Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
						dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), illusionColor());
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.2f;
						dust.velocity *= 4f;
					}
				NPC.active = false;
				return false;
			}
			if (NPC.ai[3] != 0)
			{
				acceleration = (counter - 300) / 600f;
				if (acceleration < 0)
					acceleration = 0;
				acceleration -= 0.3f;
				rotationCounter += 0.8f + acceleration;
			}
			else
				rotationCounter += 0.8f;
			float mult = counter / (max * 0.75f);
			if (mult > 1)
				mult = 1;
			Vector2 rotatePos = new Vector2((960 - 120 * mult) * (1 - moveBack), 0).RotatedBy(MathHelper.ToRadians(rotationCounter + NPC.ai[1]));
			Vector2 toPos = rotatePos + rotateCenter;
			Vector2 goToPos = toPos - NPC.Center;
			float speed = 16;
			float length = goToPos.Length();
			speed += length * 0.01f;
			if (speed > length)
			{
				speed = length;
			}
			goToPos = goToPos.SafeNormalize(Vector2.Zero);
			NPC.velocity = goToPos * speed;
			counter++;
			if (counter > 240 && counter < max)
			{
				if (NPC.ai[3] == 0) //3 different AI()
				{
					Vector2 aimAtCenter = rotateCenter;
					if (ColorType() == 2)
						aimAtCenter = player.Center;
					modifyRotation(true, aimAtCenter, true);
					ring.aiming = true;
					ring.targetRadius = 40;
					if (ColorType() == 2) // blue
					{
						if (counter > 300)
						{
							float localCounter = counter - 300;
							if (localCounter % 30 == 0)
							{
								Vector2 outward = new Vector2(0, 1).RotatedBy(NPC.rotation);
								SOTSUtils.PlaySound(SoundID.Item91, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, 0.2f);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 48, outward * (6f + 6f * mult), ProjectileType<ChaosBall>(), damage, 0, Main.myPlayer, 0, -ColorType());
								}
							}
						}
					}
					else if (ColorType() == 1) //green
					{
						if (counter > 300)
						{
							float localCounter = counter - 300;
							if (localCounter % 90 == 0)
							{
								Vector2 outward = new Vector2(0, 1).RotatedBy(NPC.rotation);
								SOTSUtils.PlaySound(SoundID.Item62, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, 0.2f);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									for (int i = -2; i <= 2; i++)
									{
										outward = new Vector2(0, 1).RotatedBy(NPC.rotation + MathHelper.ToRadians(i * 22.5f));
										Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 48, outward * (2f + 1.5f * mult), ProjectileType<ChaosWave>(), damage, 0, Main.myPlayer, 0, -ColorType());
									}
								}
							}
						}
					}
					else //red
					{
						if (counter == 300)
						{
							Vector2 outward = new Vector2(0, 1).RotatedBy(NPC.rotation);
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 32, outward * 6f, ProjectileType<ChaosEraser2>(), damage, 0, Main.myPlayer, NPC.whoAmI);
							}
						}
					}
				}
				else
				{
					Vector2 aimAtCenter = rotateCenter;
					modifyRotation(true, aimAtCenter, true);
					ring.aiming = true;
					ring.targetRadius = 40;
					if (counter > 300)
					{
						fireRateCounter += 0.8f + acceleration * 2;
						if (fireRateCounter >= 20)
						{
							Vector2 outward = new Vector2(0, 1).RotatedBy(NPC.rotation);
							SOTSUtils.PlaySound(SoundID.Item91, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, 0.2f);
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 48, outward * (6f + 8f * mult), ProjectileType<ChaosBall>(), damage, 0, Main.myPlayer, 0, -ColorType());
							}
							fireRateCounter -= 9;
						}
					}
				}
			}
			else
			{
				modifyRotation(false, player.Center, true);
			}
			return base.PreAI();
		}
        public override void PostAI()
		{
			int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
			Dust dust = Main.dust[dust2];
			dust.color = NPC.GetAlpha(VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), illusionColor() * 1.2f));
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
			ring.CalculationStuff(NPC.Center + NPC.velocity);
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			float max = 1200;
			if (NPC.ai[3] != 0)
			{
				max = 900;
			}
			return counter > 90 && counter < max;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			ChaosSpirit.DrawWings(screenPos, MathHelper.Lerp(wingHeight, 40, wingHeightLerp), NPC.ai[2], NPC.rotation, NPC.Center, NPC.GetAlpha(illusionColor()));
			if (!runOnce)
				DrawRings(spriteBatch, screenPos, false);
			for (int k = 0; k < 7; k++)
			{
				Color color = illusionColor();
				Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
				if (k != 0)
				{
					color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60), color);
				}
				else
					circular *= 0f;
				color.A = 0;
				Main.spriteBatch.Draw(texture, NPC.Center + circular - screenPos, null, NPC.GetAlpha(color), 0f, drawOrigin, NPC.scale * 1.1f, SpriteEffects.None, 0f);
			}
			if (!runOnce)
				DrawRings(spriteBatch, screenPos, true);
		}
        float wingSpeedMult = 1;
		float wingHeight = 0;
		public void WingStuff()
		{
			NPC.ai[2] += 7.5f * wingSpeedMult;
			float dipAndRise = (float)Math.Sin(MathHelper.ToRadians(NPC.ai[2] + NPC.ai[1]));
			//dipAndRise *= (float)Math.sqrt(dipAndRise);
			wingHeight = 19 + dipAndRise * 27;
		}
		public void DrawRings(SpriteBatch spriteBatch, Vector2 screenPos, bool front)
		{
			if(!runOnce)
				ring.Draw(screenPos, spriteBatch, illusionColor(), 3, (255 - NPC.alpha) / 255f, 1, 1, NPC.rotation, front);
		}
		public int ColorType()
		{
			if (NPC.ai[1] == 120)
				return 1; //green
			if (NPC.ai[1] == 240)
				return 2; //blue
			return 0; //red
        }
		public Color illusionColor()
        {
			if(ColorType() == 1)
				return new Color(80, 240, 80);
			if (ColorType() == 2)
				return new Color(60, 140, 200);
			return new Color(200, 100, 100);
        }
	}
}