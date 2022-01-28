using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Inferno;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class ChaosConstruct : ModNPC
	{
		float dir = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Construct");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 3000;  
			npc.damage = 60; 
			npc.defense = 30;  
			npc.knockBackResist = 0f;
			npc.width = 102;
			npc.height = 100;
			Main.npcFrameCount[npc.type] = 1;
			npc.value = Item.buyPrice(0, 4, 50, 0);
			npc.npcSlots = 4f;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.netAlways = true;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.rarity = 5;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 100;
			npc.lifeMax = 5250;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 origin = new Vector2(npc.width / 2, npc.height / 2);
			Texture2D texture = Main.npcTexture[npc.type];
			DrawWings();
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), null, drawColor, dir, origin, npc.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModContent.GetTexture("SOTS/NPCs/Constructs/ChaosConstructGlow"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), null, Color.White, dir, origin, npc.scale, SpriteEffects.None, 0f);
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
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/ChaosParticle");
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
				float positionalRotation = npc.rotation + MathHelper.ToRadians(wingHeight * -j);
				Vector2 position = npc.Center + new Vector2(-28 * j, 20).RotatedBy(npc.rotation) + new Vector2((width + npc.width) / 2 * j, 0).RotatedBy(positionalRotation);
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
					Color c = VoidPlayer.pastelAttempt(radians + MathHelper.ToRadians(Main.GameUpdateCount));
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
					Main.spriteBatch.Draw(texture, position - Main.screenPosition + circular, null, new Color(c.R, c.G, c.B, 0), radians * j, origin, npc.scale * 0.8f * (0.5f + 0.5f * (float)Math.Sqrt(increaseAmount)), SpriteEffects.None, 0f);
				}
			}
        }
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if(Main.netMode != NetmodeID.Server)
				{
					for (int k = 0; k < 30; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
						Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Fire, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 2.2f);
					}
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChaosConstruct/ChaosConstructGore1"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChaosConstruct/ChaosConstructGore2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChaosConstruct/ChaosConstructGore5"), 1f);
					for (int i = 0; i < 9; i++)
						Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61, 64), 1f);
                }
			}
		}
		public bool runOnce = true;
		Vector2 aimTo = Vector2.Zero;
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(false);
			WingStuff();
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.45f / 155f, (255 - npc.alpha) * 0.25f / 155f, (255 - npc.alpha) * 0.45f / 155f);
			if (npc.ai[0] == 0)
			{
				npc.ai[1]++;
				if (forceWingHeight)
				{
					float mult = npc.ai[1] / 20f;
					wingHeight = MathHelper.Lerp(wingHeight, lastWingHeight, mult);
					if (mult >= 1)
						forceWingHeight = false;
					npc.rotation = npc.velocity.X * 0.06f;
					aimTo = Vector2.Lerp(aimTo, npc.Center + new Vector2(0, 400).RotatedBy(npc.rotation), mult);
				}
				else
				{
					npc.rotation = npc.velocity.X * 0.06f;
					aimTo = npc.Center + new Vector2(0, 400).RotatedBy(npc.rotation);
					DoPassiveMovement();
					if (npc.ai[1] > 180)
					{
						float wingSpeed = (npc.ai[1] - 180) / 60f;
						wingSpeedMult = 1 - wingSpeed;
						if (npc.ai[1] > 240)
						{
							npc.ai[0] = Main.rand.Next(2) + 1;
							npc.ai[1] = 0;
							if(Main.netMode == NetmodeID.Server)
								npc.netUpdate = true;
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
			else if(npc.ai[0] == 1)
			{
				npc.ai[1]++;
				forceWingHeight = true;
				if(npc.ai[1] > 20)
				{
					aimTo = Vector2.Lerp(aimTo, player.Center, 0.08f);
					wingHeight = 50;
					Vector2 toPlayer = aimTo - npc.Center;
					npc.velocity *= 0.96f;
					if (npc.ai[1] > 70)
					{
						Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 91, 1.3f, -0.4f);
						int damage2 = npc.damage / 2;
						if (Main.expertMode)
						{
							damage2 = (int)(damage2 / Main.expertDamage);
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(npc.Center + toPlayer.SafeNormalize(Vector2.Zero) * 32, toPlayer.SafeNormalize(Vector2.Zero) * 4f, ModContent.ProjectileType<ChaosCircle>(), damage2, 0, Main.myPlayer);
						}
						npc.velocity -= toPlayer.SafeNormalize(Vector2.Zero) * 9f;
						npc.ai[1] = 40;
						npc.ai[2]++;
					}
					else
                    {
						float scalingSpeed = (float)Math.Sqrt(toPlayer.Length()) * 0.005f;
						npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * (0.10f + scalingSpeed);
					}
					if(npc.ai[2] > 6 && npc.ai[1] > 60)
					{
						npc.ai[0] = 0;
						npc.ai[1] = 0;
						npc.ai[2] = 0;
					}
                }
				else
				{
					npc.velocity *= 0.96f;
					aimTo = Vector2.Lerp(aimTo, player.Center, npc.ai[1] / 30f);
					wingHeight = MathHelper.Lerp(lastWingHeight, 50, npc.ai[1] / 20f);
                }
			}
			else if (npc.ai[0] == 2)
			{
				npc.ai[1]++;
				forceWingHeight = true;
				if (npc.ai[1] > 20)
				{
					aimTo = Vector2.Lerp(aimTo, player.Center, 0.08f);
					wingHeight = 50;
					Vector2 toPlayer = aimTo - npc.Center;
					npc.velocity *= 0.96f;
					if (npc.ai[1] > 100)
					{
						Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 92, 1.3f, -0.4f);
						int damage2 = npc.damage / 2;
						if (Main.expertMode)
						{
							damage2 = (int)(damage2 / Main.expertDamage);
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{ 
							Projectile.NewProjectile(npc.Center + toPlayer.SafeNormalize(Vector2.Zero) * 32, toPlayer.SafeNormalize(Vector2.Zero) * (11f + 5f * npc.ai[2]), ModContent.ProjectileType<ChaosSphere>(), (int)(damage2 * 1.2f), 0, Main.myPlayer, 220 - npc.ai[2] * 80);
						}
						npc.velocity -= toPlayer.SafeNormalize(Vector2.Zero) * 14f;
						npc.ai[1] = 20;
						npc.ai[2]++;
					}
					else
					{
						float scalingSpeed = (float)Math.Sqrt(toPlayer.Length()) * 0.002f;
						npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * (0.07f + scalingSpeed);
					}
					if (npc.ai[2] > 1 && npc.ai[1] > 70)
					{
						npc.ai[0] = 0;
						npc.ai[1] = 0;
						npc.ai[2] = 0;
					}
				}
				else
				{
					npc.velocity *= 0.96f;
					aimTo = Vector2.Lerp(aimTo, player.Center, npc.ai[1] / 30f);
					wingHeight = MathHelper.Lerp(lastWingHeight, 50, npc.ai[1] / 20f);
				}
			}
			else
            {
				aimTo = npc.Center;
			}
			dir = (aimTo - npc.Center).ToRotation() - MathHelper.ToRadians(90);
			npc.rotation = dir;
			npc.ai[3]++;
		}
		public void DoPassiveMovement()
		{
			Player player = Main.player[npc.target];
			float sinusoid = (float)Math.Sin(MathHelper.ToRadians(npc.ai[3]));
			Vector2 offset = new Vector2(0, -300).RotatedBy(MathHelper.ToRadians(sinusoid * 30));
			offset.X *= 1.5f;
			Vector2 position = player.Center + offset;
			float verticalOffset = 40 + 40 * (float)Math.Sin(MathHelper.ToRadians(npc.ai[1] * 2f));
			position.Y -= verticalOffset;
			Vector2 goTo = position - npc.Center;
			float speed = 12f;
			if(goTo.Length() < speed)
            {
				speed = goTo.Length();
            }
			npc.velocity *= 0.3f;
			npc.velocity += 0.6f * speed * goTo.SafeNormalize(Vector2.Zero);
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<ChaosRubble>());	
			Main.npc[n].velocity.Y = 3f;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
			n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<ChaosSpirit>(), 0, n);
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].velocity += npc.oldVelocity * 0.4f;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfChaos>(), Main.rand.Next(4) + 4);
		}	
	}
}