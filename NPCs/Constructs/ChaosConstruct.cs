using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Inferno;
using SOTS.Void;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class ChaosConstruct : ModNPC
	{
		float dir = 0f;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
        public override void SetDefaults()
		{
			NPC.aiStyle = 0;
			NPC.lifeMax = 3000;  
			NPC.damage = 60; 
			NPC.defense = 30;  
			NPC.knockBackResist = 0f;
			NPC.width = 102;
			NPC.height = 100;
			NPC.value = Item.buyPrice(0, 4, 50, 0);
			NPC.npcSlots = 4f;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.rarity = 5;
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.damage = (int)(NPC.damage * 5 / 6);
			NPC.lifeMax = (int)(NPC.lifeMax * 7 / 8);
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Vector2 origin = new Vector2(NPC.width / 2, NPC.height / 2);
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			DrawWings();
			spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), null, drawColor, dir, origin, NPC.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/ChaosConstructGlow"), NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), null, Color.White, dir, origin, NPC.scale, SpriteEffects.None, 0f);
			return false;
		}
		float wingSpeedMult = 1f;
		float counter2 = 0;
		bool forceWingHeight = false;
		float lastWingHeight = 0; //wings offset in degrees
		float wingHeight; //wings offset in degrees
		public void WingStuff()
		{
			counter2 += 7.5f * wingSpeedMult;
			float dipAndRise = (float)Math.Sin(MathHelper.ToRadians(counter2));
			//dipAndRise *= (float)Math.sqrt(dipAndRise);
			if(!forceWingHeight)
            {
				wingHeight = 19 + dipAndRise * 27;
				lastWingHeight = wingHeight;
			}
			else
            {

            }
		}
		public void DrawWings()
        {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/ChaosParticle");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float dipAndRise = (float)Math.Sin(MathHelper.ToRadians(counter2));
			if(forceWingHeight)
            {
				float supposedWingHeight = wingHeight - 19;
				dipAndRise = supposedWingHeight / 27f;
				dipAndRise = MathHelper.Clamp(dipAndRise, -1, 1);
			}
			int width = 130;
			int height = 90;
			int amtOfParticles = 120;
			for(int j = -1; j <= 1; j+= 2)
			{
				float positionalRotation = NPC.rotation + MathHelper.ToRadians(wingHeight * -j);
				Vector2 position = NPC.Center + new Vector2(-28 * j, 20).RotatedBy(NPC.rotation) + new Vector2((width + NPC.width) / 2 * j, 0).RotatedBy(positionalRotation);
				float degreesCount = 0f;
				float flapMult = 0.0f;
				float baseGrowth = 0.35f;
				float scaleGrowth = 0.15f;
				float totalSin = 450f;
				if (dipAndRise < 0)
				{
					baseGrowth = MathHelper.Lerp(baseGrowth, 0.2f, -dipAndRise);
					scaleGrowth = MathHelper.Lerp(scaleGrowth, 0.3f, -dipAndRise);
					totalSin = MathHelper.Lerp(totalSin, 405, (float)Math.Pow((-dipAndRise), 1.2f));
				}
				else
				{
					baseGrowth = MathHelper.Lerp(baseGrowth, 0.1f, dipAndRise);
					scaleGrowth = MathHelper.Lerp(scaleGrowth, 0.05f, dipAndRise);
					flapMult = MathHelper.Lerp(scaleGrowth, 0.3f, (float)Math.Pow(dipAndRise, 1.2f));
				}
				for (float i = 0; i < amtOfParticles;)
				{
					float sinusoid = (float)Math.Sin(MathHelper.ToRadians(degreesCount));
					if (degreesCount < 0)
						sinusoid = 0;
					float radians = MathHelper.ToRadians(i * 360f / amtOfParticles);
					Color c = ColorHelpers.pastelAttempt(radians + MathHelper.ToRadians(Main.GameUpdateCount));
					Vector2 circular = new Vector2(-1, 0).RotatedBy(radians);
					float increaseAmount = 1f;
					if (i < amtOfParticles / 2)
                    {
						degreesCount = MathHelper.Lerp(0, totalSin, (float)Math.Pow(i / amtOfParticles * 2f, 1.2f));
						circular.Y *= flapMult;
						circular.Y -= sinusoid * (baseGrowth + scaleGrowth * i / amtOfParticles);
                    }	
					else
                    {
						float mult = (1 - (i - 60f) / amtOfParticles * 7f);
						if (mult < 0)
							mult = 0;
						circular.Y -= (float)Math.Sin(MathHelper.ToRadians(totalSin)) * (scaleGrowth + baseGrowth) * mult;
						if(circular.Y > 0)
                        {
							sinusoid = (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 20f + i * 36));
							if (sinusoid < 0.0f)
							{
								if (sinusoid > -0.2f)
									increaseAmount = 0.35f;
								else if (sinusoid > -0.4f)
									increaseAmount = 0.5f;
								else if (sinusoid > -0.6f)
									increaseAmount = 0.75f;
								sinusoid = 0.0f;
							}
							else
                            {
								increaseAmount = 0.25f;
                            }
							circular.Y *= 1f + sinusoid * 0.4f;
                        }
					}
					i += increaseAmount;
					circular.X *= width / 2 * j;
					circular.Y *= height / 2;
					circular = circular.RotatedBy(positionalRotation);
					Main.spriteBatch.Draw(texture, position - Main.screenPosition + circular, null, new Color(c.R, c.G, c.B, 0), radians * j, origin, NPC.scale * 0.8f * (0.5f + 0.5f * (float)Math.Sqrt(increaseAmount)), SpriteEffects.None, 0f);
				}
			}
        }
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				if(Main.netMode != NetmodeID.Server)
				{
					for (int k = 0; k < 30; k++)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Platinum, 2.5f * (float)hit.HitDirection, -2.5f, 0, default(Color), 0.7f);
					}
					for (int i = 1; i <= 7; i++)
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/ChaosConstruct/ChaosConstructGore" + i), 1f);
					for (int i = 0; i < 9; i++)
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
                }
			}
		}
		public bool runOnce = true;
		Vector2 aimTo = Vector2.Zero;
		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(false);
			WingStuff();
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.45f / 155f, (255 - NPC.alpha) * 0.25f / 155f, (255 - NPC.alpha) * 0.45f / 155f);
			if (NPC.ai[0] == 0)
			{
				NPC.ai[1]++;
				if (forceWingHeight)
				{
					float mult = NPC.ai[1] / 20f;
					wingHeight = MathHelper.Lerp(wingHeight, lastWingHeight, mult);
					if (mult >= 1)
						forceWingHeight = false;
					NPC.rotation = NPC.velocity.X * 0.06f;
					aimTo = Vector2.Lerp(aimTo, NPC.Center + new Vector2(0, 400).RotatedBy(NPC.rotation), mult);
				}
				else
				{
					NPC.rotation = NPC.velocity.X * 0.06f;
					aimTo = NPC.Center + new Vector2(0, 400).RotatedBy(NPC.rotation);
					DoPassiveMovement();
					if (NPC.ai[1] > 180)
					{
						float wingSpeed = (NPC.ai[1] - 180) / 60f;
						wingSpeedMult = 1 - wingSpeed;
						if (NPC.ai[1] > 240)
						{
							NPC.ai[0] = Main.rand.Next(2) + 1;
							NPC.ai[1] = 0;
							if(Main.netMode == NetmodeID.Server)
								NPC.netUpdate = true;
						}
					}
					else
					{
						if (wingSpeedMult < 1)
						{
							wingSpeedMult = MathHelper.Lerp(wingSpeedMult, 1, 0.04f);
						}
					}
				}
			}
			else if(NPC.ai[0] == 1)
			{
				NPC.ai[1]++;
				forceWingHeight = true;
				if(NPC.ai[1] > 20)
				{
					aimTo = Vector2.Lerp(aimTo, player.Center, 0.08f);
					wingHeight = 50;
					Vector2 toPlayer = aimTo - NPC.Center;
					NPC.velocity *= 0.96f;
					if (NPC.ai[1] > 70)
					{
						SOTSUtils.PlaySound(SoundID.Item91, (int)NPC.Center.X, (int)NPC.Center.Y, 1.3f, -0.4f);
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + toPlayer.SafeNormalize(Vector2.Zero) * 32, toPlayer.SafeNormalize(Vector2.Zero) * 4f, ModContent.ProjectileType<ChaosCircle>(), damage2, 0, Main.myPlayer);
						}
						NPC.velocity -= toPlayer.SafeNormalize(Vector2.Zero) * 9f;
						NPC.ai[1] = 40;
						NPC.ai[2]++;
					}
					else
                    {
						float scalingSpeed = (float)Math.Sqrt(toPlayer.Length()) * 0.005f;
						NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * (0.10f + scalingSpeed);
					}
					if(NPC.ai[2] > 6 && NPC.ai[1] > 60)
					{
						NPC.ai[0] = 0;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
					}
                }
				else
				{
					NPC.velocity *= 0.96f;
					aimTo = Vector2.Lerp(aimTo, player.Center, NPC.ai[1] / 30f);
					wingHeight = MathHelper.Lerp(lastWingHeight, 50, NPC.ai[1] / 20f);
                }
			}
			else if (NPC.ai[0] == 2)
			{
				NPC.ai[1]++;
				forceWingHeight = true;
				if (NPC.ai[1] > 20)
				{
					aimTo = Vector2.Lerp(aimTo, player.Center, 0.08f);
					wingHeight = 50;
					Vector2 toPlayer = aimTo - NPC.Center;
					NPC.velocity *= 0.96f;
					if (NPC.ai[1] > 100)
					{
						SOTSUtils.PlaySound(SoundID.Item92, (int)NPC.Center.X, (int)NPC.Center.Y, 1.3f, -0.4f);
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + toPlayer.SafeNormalize(Vector2.Zero) * 32, toPlayer.SafeNormalize(Vector2.Zero) * (11f + 5f * NPC.ai[2]), ModContent.ProjectileType<ChaosSphere>(), (int)(damage2 * 1.2f), 0, Main.myPlayer, 220 - NPC.ai[2] * 80);
						}
						NPC.velocity -= toPlayer.SafeNormalize(Vector2.Zero) * 14f;
						NPC.ai[1] = 20;
						NPC.ai[2]++;
					}
					else
					{
						float scalingSpeed = (float)Math.Sqrt(toPlayer.Length()) * 0.002f;
						NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * (0.07f + scalingSpeed);
					}
					if (NPC.ai[2] > 1 && NPC.ai[1] > 70)
					{
						NPC.ai[0] = 0;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
					}
				}
				else
				{
					NPC.velocity *= 0.96f;
					aimTo = Vector2.Lerp(aimTo, player.Center, NPC.ai[1] / 30f);
					wingHeight = MathHelper.Lerp(lastWingHeight, 50, NPC.ai[1] / 20f);
				}
			}
			else
            {
				aimTo = NPC.Center;
			}
			dir = (aimTo - NPC.Center).ToRotation() - MathHelper.ToRadians(90);
			NPC.rotation = dir;
			NPC.ai[3]++;
		}
		public void DoPassiveMovement()
		{
			Player player = Main.player[NPC.target];
			float sinusoid = (float)Math.Sin(MathHelper.ToRadians(NPC.ai[3]));
			Vector2 offset = new Vector2(0, -300).RotatedBy(MathHelper.ToRadians(sinusoid * 30));
			offset.X *= 1.5f;
			Vector2 position = player.Center + offset;
			float verticalOffset = 40 + 40 * (float)Math.Sin(MathHelper.ToRadians(NPC.ai[1] * 2f));
			position.Y -= verticalOffset;
			Vector2 goTo = position - NPC.Center;
			float speed = 12f;
			if(goTo.Length() < speed)
            {
				speed = goTo.Length();
            }
			NPC.velocity *= 0.3f;
			NPC.velocity += 0.6f * speed * goTo.SafeNormalize(Vector2.Zero);
		}
        public override void OnKill()
		{
			int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ChaosRubble>());
			Main.npc[n].velocity = NPC.oldVelocity + Main.rand.NextVector2Circular(5, 5);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
			n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ChaosSpirit>(), 0, n, 0, counter2);
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].velocity += NPC.oldVelocity * 0.4f;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfChaos>(), 1, 4, 7));
		}
	}
}