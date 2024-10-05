using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Evil;
using SOTS.Projectiles.Tide;
using SOTS.Void;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class EvilSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;  
			NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
        }
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
		}
		public override void SetDefaults()
		{
			NPC.aiStyle = 10;
            NPC.lifeMax = 3000; 
            NPC.damage = 80; 
            NPC.defense = 0;   
            NPC.knockBackResist = 0f;
            NPC.width = 70;
            NPC.height = 70;
            NPC.value = Item.buyPrice(0, 10, 0, 0);
            NPC.npcSlots = 7f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = false;
			NPC.rarity = 2;
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.damage = (int)(NPC.damage * 29 / 32);
			NPC.lifeMax = (int)(NPC.lifeMax * 5 / 6);
		}
		List<EvilEye> eyes = new List<EvilEye>();
		private int InitiateHealth = 10000;
		private float ExpertHealthMult = 1.45f; //14500
		private float MasterHealthMult = 2.0f; //20000
		int phase = 1;
		int counter = 0;
		int counter2 = 0;
		public int startEyes = 0;
		public const int range = 96;
		float lastDistMult = 1f;
		public void UpdateEyes(Vector2 screenPos, bool draw = false, int ring = -2, float distMult = 1f)
		{
			Player player = Main.player[NPC.target];
			lastDistMult = distMult;
			for (int i = 0; i < eyes.Count; i++)
            {
				EvilEye eye = eyes[i];
				float mult = 256f / (eye.offset.Length() + 24);
				int direction = (((int)(eye.offset.Length() + 0.5f) % (2 * range)) / range) % 2 == 0 ? -1 : 1;
				float rotation = (NPC.rotation + MathHelper.ToRadians(counter2 * direction)) * mult;
				if (draw)
				{
					eye.Draw(NPC.Center, screenPos, rotation, distMult);
				}
				else
				{
					int ringNumber = (int)(eye.offset.Length() + 0.5f - range) / range;
					if(ringNumber == ring)
					{
						eye.Fire(player.Center);
                    }
					eye.Update(NPC, NPC.Center, rotation, distMult);
				}
            }
        }
		public override void AI()
		{
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.15f / 255f, (255 - NPC.alpha) * 0.25f / 255f, (255 - NPC.alpha) * 0.65f / 255f);
			NPC.TargetClosest(false); //this should fix the multiplayer lose targetting
			Player player = Main.player[NPC.target];
			float mult = (100 + NPC.ai[2]) / 100f;
			UpdateEyes(Vector2.Zero, false, -2, mult);
			counter2++;
			if (phase == 3)
			{
				NPC.aiStyle = -1;
				NPC.dontTakeDamage = false;
				int damage = NPC.GetBaseDamage() / 2;
				if (NPC.ai[0] >= 0 && NPC.ai[2] >= 0)
				{
					NPC.velocity *= 0.95f;
					int counterR = (int)(NPC.ai[0]);
					if(startEyes < 180)
					{
						if (startEyes % 6 == 0)
						{
							SOTSUtils.PlaySound(SoundID.Item30, (int)NPC.Center.X, (int)NPC.Center.Y, 0.7f, -0.4f);
						}
					}
					if(startEyes < 120)
					{
						if (startEyes < 60)
						{
							if (startEyes % 6 == 0)
							{
								Vector2 circular = new Vector2(3 * range, 0).RotatedBy(MathHelper.ToRadians(startEyes * 4f));
								eyes.Add(new EvilEye(circular, damage));
								circular = new Vector2(-2 * range, 0).RotatedBy(MathHelper.ToRadians(startEyes * 4f));
								eyes.Add(new EvilEye(circular, damage));
							}
						}
						else
						{
							if (startEyes % 5 == 0)
							{
								Vector2 circular = new Vector2(5 * range, 0).RotatedBy(MathHelper.ToRadians(startEyes * 4f));
								eyes.Add(new EvilEye(circular, damage));
								circular = new Vector2(-4 * range, 0).RotatedBy(MathHelper.ToRadians(startEyes * 4f));
								eyes.Add(new EvilEye(circular, damage));
							}
						}
                    }
					else
					{
						if (startEyes % 4 == 0 && startEyes < 180)
						{
							Vector2 circular = new Vector2(7 * range, 0).RotatedBy(MathHelper.ToRadians(startEyes * 4f));
							eyes.Add(new EvilEye(circular, damage));
							circular = new Vector2(-6 * range, 0).RotatedBy(MathHelper.ToRadians(startEyes * 4f));
							eyes.Add(new EvilEye(circular, damage));
						}
						Vector2 toPlayer = player.Center - NPC.Center;
						float speed = 12 + toPlayer.Length() * 0.01f;
						if (counterR % 180 == 120)
						{
							if (Main.netMode == NetmodeID.Server)
								NPC.netUpdate = true;
							NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed;
						}
						if(counterR % 180 == 0)
						{
							NPC.ai[1]++;
							if (NPC.ai[1] > 6)
							{
								NPC.ai[1] = 0;
							}
							if (NPC.ai[1] > 0)
                            {
								int ring = (int)NPC.ai[1];
								UpdateEyes(Vector2.Zero, false, ring);
                            }
						}
						if (counterR % 180 == 40)
						{
							if(NPC.ai[1] > 0)
							{
								SOTSUtils.PlaySound(SoundID.Item46, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, -0.15f);
							}
							if(NPC.ai[1] >= 6)
                            {
								NPC.ai[3] = 0;
								NPC.ai[2] = -1;
								NPC.ai[1] = 0;
								NPC.ai[0] = 0;
                            }
						}
						float sin = (float)Math.Sin(MathHelper.ToRadians(counterR * 2));
						Vector2 additional = new Vector2(0, sin * 0.1f);
						NPC.velocity += additional;
						NPC.rotation += NPC.velocity.X * 0.005f;
					}
					if(startEyes < 180)
						startEyes++;
					NPC.ai[0]++;
				} 
				else if(NPC.ai[2] < 0)
				{
					counter2++;
					NPC.velocity *= 0.9912f;
					if (NPC.ai[2] > -80 && NPC.ai[3] < 3)
						NPC.ai[2] -= 0.5f;
					else
					{
						int counterR = (int)(NPC.ai[0] - 120);
						Vector2 toPlayer = player.Center - NPC.Center;
						float speed = 11 + toPlayer.Length() * 0.0005f;
						if(NPC.ai[3] < 3)
						{
							NPC.ai[0]++;
							if (counterR % 150 == 30)
							{
								NPC.velocity *= 0.1f;
								SOTSUtils.PlaySound(SoundID.Item105, (int)NPC.Center.X, (int)NPC.Center.Y, 1.2f, -0.25f);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									int amt = 8;
									for (int i = 0; i < amt; i++)
									{
										Vector2 toPosition = player.Center - NPC.Center;
										Vector2 velo = toPosition.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(i * 360f / amt));
										Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + velo * 24, velo * 0.1f, ModContent.ProjectileType<EvilBolt>(), damage, 0, Main.myPlayer, 0.065f + Main.rand.NextFloat(0.02f));
										if (Main.rand.NextBool(3))
										{
											Vector2 secondVelo = velo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45)));
											Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + secondVelo * 24, secondVelo * -0.2f, ModContent.ProjectileType<EvilBolt>(), damage, 0, Main.myPlayer, 0.1f + Main.rand.NextFloat(0.05f));
										}
									}
								}
								for (int i = 0; i < 60; i++)
								{
									Dust dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
									dust3.color = new Color(ColorHelper.EvilColor.R, ColorHelper.EvilColor.G, ColorHelper.EvilColor.B);
									dust3.noGravity = true;
									dust3.fadeIn = 0.1f;
									dust3.scale *= 2.25f;
									dust3.velocity *= 7f;
								}
							}
							else if (counterR % 150 == 60)
							{
								NPC.ai[3]++;
								NPC.velocity *= 0.5f;
								NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed;
							}
							else if (counterR % 30 == 0)
							{
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Vector2 toPosition = player.Center - NPC.Center;
									Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, toPosition.SafeNormalize(Vector2.Zero) * 0.1f, ModContent.ProjectileType<EvilBolt>(), damage, 0, Main.myPlayer, 0.05f);
								}
							}
							else
							{
								NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed * 0.04f;
							}
						}
						else
						{
							NPC.velocity *= 0.99f;
							NPC.ai[2] += 0.5f;
							NPC.ai[1] = 0;
							NPC.ai[0] = 0;
						}
						NPC.rotation += NPC.velocity.X * 0.01f;
					}
                }
			}
			if (phase == 2)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
					NPC.netUpdate = true;
				NPC.dontTakeDamage = false;
				NPC.aiStyle =-1;
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
				NPC.ai[2] = 0;
				NPC.ai[3] = 0;
				phase = 3;
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(Main.player[NPC.target].dead)
			{
				counter++;
			}
			else if(phase != 1 && counter > 0)
			{
				counter--;
			}
			if(counter >= 1440)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
				phase = 1;
				NPC.aiStyle = -1;
				NPC.velocity.Y -= 0.014f;
				NPC.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(ColorHelper.EvilColor.R, ColorHelper.EvilColor.G, ColorHelper.EvilColor.B);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(Color.Black) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, NPC.rotation, drawOrigin, NPC.scale * 1.1f, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0)
			{
				if (Main.netMode != NetmodeID.Server)
					for (int i = 0; i < 50; i ++)
					{
						Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
						dust.color = new Color(ColorHelper.EvilColor.R, ColorHelper.EvilColor.G, ColorHelper.EvilColor.B);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 2f;
						dust.velocity *= 5f;
					}
				if(phase == 1)
				{
					phase = 2;
					NPC.lifeMax = (int)(InitiateHealth * (Main.masterMode ? MasterHealthMult : Main.expertMode ? ExpertHealthMult : 1));
					NPC.life = NPC.lifeMax;
				}
				if(Main.netMode != NetmodeID.Server)
                {
					for (int i = 0; i < eyes.Count; i++)
					{
						EvilEye eye = eyes[i];
						float mult = 256f / (eye.offset.Length() + 24);
						int direction = (((int)(eye.offset.Length() + 0.5f) % (2 * range)) / range) % 2 == 0 ? -1 : 1;
						float rotation = (NPC.rotation + MathHelper.ToRadians(counter2 * direction)) * mult;
						eye.Update(NPC, NPC.Center, rotation, lastDistMult, true);
					}
				}
			}
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Color color = ColorHelper.EvilColor * 1.3f;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				spriteBatch.Draw(texture, NPC.Center + Main.rand.NextVector2Circular(4f, 4f) - screenPos, null, color, 0f, drawOrigin, NPC.scale * 1.1f, SpriteEffects.None, 0f);
			}
			float mult = (100 + NPC.ai[2]) / 100f;
			if (Main.netMode != NetmodeID.Server) //pretty sure drawcode doesn't run in multiplayer anyways but may as well
				UpdateEyes(screenPos, true, -2, mult);
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DissolvingUmbra>()));
		}
	}
	public class EvilEye
    {
		public bool friendly;
		public Texture2D texture;
		public Texture2D texturePupil;
		public Vector2 offset;
		public Vector2 fireTo;
		public float counter = 0;
		public float shootCounter = 0;
		public bool firing = false;
		public int damage;
		public int target;
		private float warmUp = 40f;
		public EvilEye(Vector2 offset, int damage, bool friendly = false)
        {
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/EvilEye");
			texturePupil = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/EvilEyePupil");
			this.offset = offset;
			this.damage = damage;
			this.friendly = friendly;
			if (friendly)
				warmUp = 10f;
			target = -1;
		}
		public void Fire(Vector2 fireAt, int target = -1)
        {
			fireTo = fireAt;
			firing = true;
			this.target = target;
		}
		public void Update(Entity owner, Vector2 center, float rotation, float distMult, bool dust2 = false)
        {
			Vector2 trueOffset = offset.RotatedBy(rotation) * distMult;
			if(!dust2)
			{
				if (firing)
				{
					shootCounter++;
					if (shootCounter >= warmUp)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int type = ModContent.ProjectileType<EvilBolt>();
							float speed = 1;
							float ai0 = 0.1f;
							if (friendly)
							{
								ai0 = target;
								speed = 4f;
								type = ModContent.ProjectileType<Projectiles.Minions.EvilSpear>();
							}
							Vector2 toPosition = fireTo - trueOffset - center;
							if(owner is NPC)
								Projectile.NewProjectile(owner.GetSource_FromAI(), center + trueOffset, toPosition.SafeNormalize(Vector2.Zero) * speed, type, damage, 0, Main.myPlayer, ai0);
							if(owner is Projectile)
								Projectile.NewProjectile(owner.GetSource_FromThis(), center + trueOffset, toPosition.SafeNormalize(Vector2.Zero) * speed, type, damage, 0, Main.myPlayer, ai0);
						}
						firing = false;
					}
				}
				else if (shootCounter > 0)
					shootCounter--;
				if (counter == 0)
				{
					for (int i = 0; i < 3; i++)
					{
						Dust dust = Dust.NewDustDirect(center + trueOffset - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						dust.color = new Color(ColorHelper.EvilColor.R, ColorHelper.EvilColor.G, ColorHelper.EvilColor.B, 100);
						dust.alpha = 100;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.velocity *= 0.9f;
						dust.scale *= 1.5f;
					}
				}
				if (counter < 40)
					counter++;
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					Dust dust = Dust.NewDustDirect(center + trueOffset - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.color = new Color(ColorHelper.EvilColor.R, ColorHelper.EvilColor.G, ColorHelper.EvilColor.B);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.velocity *= 0.9f;
					dust.scale *= 1.5f;
				}
			}
        }
		public void Draw(Vector2 center, Vector2 screenPos, float rotation, float distMult, float alphaMult = 1f)
		{
			Color color = ColorHelper.EvilColor;
			color.A = 50;
			Vector2 drawPosition = center + offset.RotatedBy(rotation) * distMult - screenPos;
			Vector2 origin = texture.Size() / 2;
			float mult = 1.10f + 0.5f * shootCounter / warmUp;
			float alpha2 = shootCounter / warmUp * 2f;
			if (alpha2 > 1)
				alpha2 = 1;
			float alpha = 0.5f * counter / 40f + 0.5f * alpha2;
			if (alpha > 1)
				alpha = 1;
			for(int i = 0; i < 5; i++)
			{
				int length = 0;
				if (i != 0)
					length = 1;
				Vector2 circular = new Vector2(length, 0).RotatedBy(i * MathHelper.Pi / 2f);
				Main.spriteBatch.Draw(texture, drawPosition + circular, null, color * alpha * alphaMult, 0f, origin, mult, SpriteEffects.None, 0f);
			}
			color = ColorHelper.EvilColor;
			color.A = 50;
			for (int i = 0; i < 4; i++)
			{
				int length = 1;
				Vector2 circular = new Vector2(length, 0).RotatedBy(i * MathHelper.Pi / 2f);
				Main.spriteBatch.Draw(texturePupil, drawPosition + circular, null, color * alpha2 * alphaMult, 0f, origin, mult, SpriteEffects.None, 0f);
			}
		}
    }
}
