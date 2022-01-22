using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Celestial;
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
			npc.value = Item.buyPrice(0, 7, 0, 0);
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
		float counter1 = 0;
		float counter2 = 0;
		float lastWingHeight = 0; //wings offset in degrees
		float wingHeight; //wings offset in degrees
		public void WingStuff()
		{
			counter2 += 7.5f;
			float dipAndRise = (float)Math.Sin(MathHelper.ToRadians(counter2));
			//dipAndRise *= (float)Math.sqrt(dipAndRise);
			wingHeight = 19 + dipAndRise * 27;
			lastWingHeight = wingHeight;
		}
		public void DrawWings()
        {
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/ChaosParticle");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float dipAndRise = (float)Math.Sin(MathHelper.ToRadians(counter2));
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
					for (int i = 1; i <= 7; i++)
						Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/InfernoConstruct/InfernoConstructGore" + i), 1f);
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
			aimTo = player.Center;
			WingStuff();
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.45f / 155f, (255 - npc.alpha) * 0.25f / 155f, (255 - npc.alpha) * 0.45f / 155f);
			Vector2 toPlayer = player.Center - npc.Center;
			dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			//npc.rotation = dir;
			if(npc.ai[0] == 0)
            {
				npc.rotation = npc.velocity.X * 0.06f;
				DoPassiveMovement();
            }
			dir = npc.rotation;
		}
		public void DoPassiveMovement()
		{
			Player player = Main.player[npc.target];
			npc.ai[1]++;
			float sinusoid = (float)Math.Sin(MathHelper.ToRadians(npc.ai[1]));
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
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<InfernoSpirit>());	
			Main.npc[n].velocity.Y = -10f;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfChaos>(), Main.rand.Next(4) + 4);
		}	
	}
}