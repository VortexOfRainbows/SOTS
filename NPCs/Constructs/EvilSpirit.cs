using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Evil;
using SOTS.Projectiles.Tide;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class EvilSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 1;
			DisplayName.SetDefault("Evil Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;
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
			npc.aiStyle = 10;
            npc.lifeMax = 3000; 
            npc.damage = 80; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 70;
            npc.height = 70;
            npc.value = Item.buyPrice(0, 10, 0, 0);
            npc.npcSlots = 7f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = false;
			npc.rarity = 2;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 145;
			npc.lifeMax = 5000;
		}
		List<EvilEye> eyes = new List<EvilEye>();
		private int InitiateHealth = 10000;
		private float ExpertHealthMult = 1.45f; //13500
		int phase = 1;
		int counter = 0;
		int counter2 = 0;
		public int startEyes = 0;
		public const int range = 96;
		float lastDistMult = 1f;
		public void UpdateEyes(bool draw = false, int ring = -2, float distMult = 1f)
		{
			Player player = Main.player[npc.target];
			lastDistMult = distMult;
			for (int i = 0; i < eyes.Count; i++)
            {
				EvilEye eye = eyes[i];
				float mult = 256f / (eye.offset.Length() + 24);
				int direction = (((int)(eye.offset.Length() + 0.5f) % (2 * range)) / range) % 2 == 0 ? -1 : 1;
				float rotation = (npc.rotation + MathHelper.ToRadians(counter2 * direction)) * mult;
				if (draw)
				{
					eye.Draw(npc.Center, rotation, distMult);
				}
				else
				{
					int ringNumber = (int)(eye.offset.Length() + 0.5f - range) / range;
					if(ringNumber == ring)
					{
						eye.Fire(player.Center);
                    }
					eye.Update(npc.Center, rotation, distMult);
				}
            }
        }
		public override void AI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 255f, (255 - npc.alpha) * 0.25f / 255f, (255 - npc.alpha) * 0.65f / 255f);
			Player player = Main.player[npc.target];
			float mult = (100 + npc.ai[2]) / 100f;
			UpdateEyes(false, -2, mult);
			counter2++;
			if (phase == 3)
			{
				npc.aiStyle = -1;
				npc.dontTakeDamage = false;
				int damage = npc.damage / 2;
				if (Main.expertMode)
				{
					damage = (int)(damage / Main.expertDamage);
				}
				if (npc.ai[0] >= 0 && npc.ai[2] >= 0)
				{
					npc.velocity *= 0.95f;
					int counterR = (int)(npc.ai[0]);
					if(startEyes < 180)
					{
						if (startEyes % 6 == 0)
						{
							Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 30, 0.7f, -0.4f);
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
						Vector2 toPlayer = player.Center - npc.Center;
						float speed = 12 + toPlayer.Length() * 0.01f;
						if (counterR % 180 == 120)
						{
							if (Main.netMode == NetmodeID.Server)
								npc.netUpdate = true;
							npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed;
						}
						if(counterR % 180 == 0)
						{
							npc.ai[1]++;
							if (npc.ai[1] > 6)
							{
								npc.ai[1] = 0;
							}
							if (npc.ai[1] > 0)
                            {
								int ring = (int)npc.ai[1];
								UpdateEyes(false, ring);
                            }
						}
						if (counterR % 180 == 40)
						{
							if(npc.ai[1] > 0)
							{
								Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 46, 1.1f, -0.15f);
							}
							if(npc.ai[1] >= 6)
                            {
								npc.ai[3] = 0;
								npc.ai[2] = -1;
								npc.ai[1] = 0;
								npc.ai[0] = 0;
                            }
						}
						float sin = (float)Math.Sin(MathHelper.ToRadians(counterR * 2));
						Vector2 additional = new Vector2(0, sin * 0.1f);
						npc.velocity += additional;
						npc.rotation += npc.velocity.X * 0.005f;
					}
					if(startEyes < 180)
						startEyes++;
					npc.ai[0]++;
				} 
				else if(npc.ai[2] < 0)
				{
					counter2++;
					npc.velocity *= 0.9912f;
					if (npc.ai[2] > -80 && npc.ai[3] < 3)
						npc.ai[2] -= 0.5f;
					else
					{
						int counterR = (int)(npc.ai[0] - 120);
						Vector2 toPlayer = player.Center - npc.Center;
						float speed = 11 + toPlayer.Length() * 0.0005f;
						if(npc.ai[3] < 3)
						{
							npc.ai[0]++;
							if (counterR % 150 == 30)
							{
								npc.velocity *= 0.1f;
								Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 105, 1.2f, -0.25f);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									int amt = 8;
									for (int i = 0; i < amt; i++)
									{
										Vector2 toPosition = player.Center - npc.Center;
										Vector2 velo = toPosition.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(i * 360f / amt));
										Projectile.NewProjectile(npc.Center + velo * 24, velo * 0.1f, ModContent.ProjectileType<EvilBolt>(), damage, 0, Main.myPlayer, 0.065f + Main.rand.NextFloat(0.02f));
										if (Main.rand.NextBool(3))
										{
											Vector2 secondVelo = velo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45)));
											Projectile.NewProjectile(npc.Center + secondVelo * 24, secondVelo * -0.2f, ModContent.ProjectileType<EvilBolt>(), damage, 0, Main.myPlayer, 0.1f + Main.rand.NextFloat(0.05f));
										}
									}
								}
								for (int i = 0; i < 60; i++)
								{
									Dust dust3 = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
									dust3.color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B);
									dust3.noGravity = true;
									dust3.fadeIn = 0.1f;
									dust3.scale *= 2.25f;
									dust3.velocity *= 7f;
								}
							}
							else if (counterR % 150 == 60)
							{
								npc.ai[3]++;
								npc.velocity *= 0.5f;
								npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed;
							}
							else if (counterR % 30 == 0)
							{
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Vector2 toPosition = player.Center - npc.Center;
									Projectile.NewProjectile(npc.Center, toPosition.SafeNormalize(Vector2.Zero) * 0.1f, ModContent.ProjectileType<EvilBolt>(), damage, 0, Main.myPlayer, 0.05f);
								}
							}
							else
							{
								npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed * 0.04f;
							}
						}
						else
						{
							npc.velocity *= 0.99f;
							npc.ai[2] += 0.5f;
							npc.ai[1] = 0;
							npc.ai[0] = 0;
						}
						npc.rotation += npc.velocity.X * 0.01f;
					}
                }
			}
			if (phase == 2)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
					npc.netUpdate = true;
				npc.dontTakeDamage = false;
				npc.aiStyle = -1;
				npc.ai[0] = 0;
				npc.ai[1] = 0;
				npc.ai[2] = 0;
				npc.ai[3] = 0;
				phase = 3;
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(Main.player[npc.target].dead)
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
					npc.netUpdate = true;
				}
				phase = 1;
				npc.aiStyle = -1;
				npc.velocity.Y -= 0.014f;
				npc.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(Color.Black) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, npc.rotation, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for(int i = 0; i < 50; i ++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
					dust.color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 2f;
					dust.velocity *= 5f;
				}
				if(phase == 1)
				{
					phase = 2;
					npc.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					npc.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
				if(Main.netMode != NetmodeID.Server)
                {
					for (int i = 0; i < eyes.Count; i++)
					{
						EvilEye eye = eyes[i];
						float mult = 256f / (eye.offset.Length() + 24);
						int direction = (((int)(eye.offset.Length() + 0.5f) % (2 * range)) / range) % 2 == 0 ? -1 : 1;
						float rotation = (npc.rotation + MathHelper.ToRadians(counter2 * direction)) * mult;
						eye.Update(npc.Center, rotation, lastDistMult, true);
					}
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Color color = VoidPlayer.EvilColor * 1.3f;
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Main.spriteBatch.Draw(texture, npc.Center + Main.rand.NextVector2Circular(4f, 4f) - Main.screenPosition, null, color, 0f, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			float mult = (100 + npc.ai[2]) / 100f;
			if (Main.netMode != NetmodeID.Server) //pretty sure drawcode doesn't run in multiplayer anyways but may as well
				UpdateEyes(true, -2, mult);
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DissolvingUmbra>(), 1);	
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
			texture = ModContent.GetTexture("SOTS/NPCs/Constructs/EvilEye");
			texturePupil = ModContent.GetTexture("SOTS/NPCs/Constructs/EvilEyePupil");
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
		public void Update(Vector2 center, float rotation, float distMult, bool dust2 = false)
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
							Projectile.NewProjectile(center + trueOffset, toPosition.SafeNormalize(Vector2.Zero) * speed, type, damage, 0, Main.myPlayer, ai0);
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
						dust.color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B, 100);
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
					dust.color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.velocity *= 0.9f;
					dust.scale *= 1.5f;
				}
			}
        }
		public void Draw(Vector2 center, float rotation, float distMult, float alphaMult = 1f)
		{
			Color color = VoidPlayer.EvilColor;
			color.A = 50;
			Vector2 drawPosition = center + offset.RotatedBy(rotation) * distMult - Main.screenPosition;
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
			color = VoidPlayer.EvilColor;
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
