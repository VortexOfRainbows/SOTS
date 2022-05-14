using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Tide;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class TidalSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
			writer.Write(direction);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
			direction = reader.ReadInt32();
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =10;
            NPC.lifeMax = 960; 
            NPC.damage = 60; 
            NPC.defense = 0;   
            NPC.knockBackResist = 0f;
            NPC.width = 58;
            NPC.height = 58;
			Main.npcFrameCount[NPC.type] = 1;   
            npc.value = 35075;
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
			NPC.damage = 80;
			NPC.lifeMax = 1500;
		}
		Vector2 projectileVelo = Vector2.Zero;
		private int InitiateHealth = 3000;
		private float ExpertHealthMult = 1.25f;
		int phase = 1;
		int counter = 0;
		int direction = 1;
		public override void AI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 255f, (255 - npc.alpha) * 0.25f / 255f, (255 - npc.alpha) * 0.65f / 255f);
			Player player = Main.player[npc.target];
			if(phase == 3)
			{
				npc.dontTakeDamage = false;
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}

				if(npc.ai[3] % 2 == 0)
				{
					if (npc.ai[1] < 360)
					{
						if (npc.ai[1] > 270)
						{
							npc.velocity *= 0.1f;
							if (npc.ai[2] % 14 == 0)
							{
								Vector2 toPlayer = player.Center - npc.Center;
								toPlayer = toPlayer.SafeNormalize(new Vector2(1, 0));
								if (projectileVelo == Vector2.Zero)
									projectileVelo = toPlayer;
								int damage2 = npc.damage / 2;
								if (Main.expertMode)
								{
									damage2 = (int)(damage2 / Main.expertDamage);
								}
								SoundEngine.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 21, 0.8f);
								int last = -1;
								for (int i = 0; i < 2; i++)
								{
									float spread = 11.5f;
									Vector2 circleGen = new Vector2(spread, 0).RotatedBy(MathHelper.ToRadians(npc.ai[2] * 2.0f));
									Vector2 velo = projectileVelo.RotatedBy(MathHelper.ToRadians(circleGen.X - (i * 2 - 1) * 11.5f));
									float speed = 7f;
									if (Main.netMode != 1)
									{
										int temp = Projectile.NewProjectile(npc.Center, velo * speed, ModContent.ProjectileType<TidalWave>(), damage2, 0f, Main.myPlayer, last, 0);
										last = temp;
									}
								}
								npc.ai[1] += 5;
							}
							npc.ai[2]++;
						}
						else
						{
							npc.ai[1]++;
							npc.ai[0]++;
							Vector2 circleGen = new Vector2(20f, 0).RotatedBy(MathHelper.ToRadians(npc.ai[0]));
							Vector2 rotatePos = new Vector2(720 * direction, 0).RotatedBy(MathHelper.ToRadians(circleGen.X));
							//Vector2 rotateAround = new Vector2(npc.ai[1], 0).RotatedBy(MathHelper.ToRadians(npc.ai[1] * 2));
							Vector2 toCircle = rotatePos + player.Center - npc.Center;
							float dist = toCircle.Length();
							toCircle = toCircle.SafeNormalize(Vector2.Zero);
							float speed = 9.5f;
							if (speed > dist)
							{
								speed = dist;
							}
							toCircle *= speed;
							npc.velocity = toCircle;
						}
					}
					else
					{
						if (Main.netMode != 1)
							npc.netUpdate = true;
						npc.ai[0] += Main.rand.Next(180);
						npc.ai[1] = 0;
						npc.ai[2] = 0;
						npc.ai[3]++;
						projectileVelo = Vector2.Zero;
						direction *= -1;
					}
				}
				else
				{
					npc.ai[0]++;
					if (npc.ai[1] < 440)
					{
						npc.ai[1] += 1.15f;
					}
					else
					{
						npc.ai[2]++;
						if (npc.ai[2] % 100 == 0)
						{
							Vector2 toPlayer = player.Center - npc.Center;
							toPlayer = toPlayer.SafeNormalize(new Vector2(1, 0)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-25f, 25f)));
							projectileVelo = toPlayer;
							int damage2 = npc.damage / 2;
							if (Main.expertMode)
							{
								damage2 = (int)(damage2 / Main.expertDamage);
							}
							SoundEngine.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 21, 0.8f);
							int last = -1;
							for (int i = 0; i < 2; i++)
							{
								Vector2 velo = projectileVelo.RotatedBy(MathHelper.ToRadians((i * 2 - 1) * -11.5f));
								float speed2 = 5.7f;
								if (Main.netMode != 1)
								{
									int temp = Projectile.NewProjectile(npc.Center, velo * speed2, ModContent.ProjectileType<TidalWave>(), damage2, 0f, Main.myPlayer, last, 0);
									last = temp;
								}
							}
						}
						else if(npc.ai[2] > 600) //6 * 72
						{
							if (Main.netMode != 1)
								npc.netUpdate = true;
							npc.ai[0] += Main.rand.Next(180);
							npc.ai[1] = 0;
							npc.ai[2] = 0;
							npc.ai[3]++;
							projectileVelo = Vector2.Zero;
							direction *= -1;
						}
					}
					Vector2 circleGen = new Vector2(20f + npc.ai[1] * 0.015f, 0).RotatedBy(MathHelper.ToRadians(npc.ai[0] * 2.15f));
					Vector2 rotatePos = new Vector2(0, (npc.ai[1] + 60) * -1).RotatedBy(MathHelper.ToRadians(circleGen.X));
					rotatePos.Y *= 0.85f;
					Vector2 toCircle = rotatePos + player.Center - npc.Center;
					float dist = toCircle.Length();
					toCircle = toCircle.SafeNormalize(Vector2.Zero);
					float speed = 12.5f;
					if (speed > dist)
					{
						speed = dist;
					}
					toCircle *= speed;
					npc.velocity = toCircle;
				}
			}
			if (phase == 2)
			{
				if (Main.netMode != 1)
					npc.netUpdate = true;
				direction = Main.rand.Next(2) * 2 - 1;
				npc.dontTakeDamage = false;
				NPC.aiStyle =-1;
				npc.ai[0] = Main.rand.Next(180);
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
			if(counter >= 1440)
			{
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				phase = 1;
				NPC.aiStyle =-1;
				npc.velocity.Y -= 0.014f;
				npc.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(64, 72, 178);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[npc.type].Value.Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for(int i = 0; i < 50; i ++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
					dust.color = new Color(64, 72, 178);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 2f;
					dust.velocity *= 5f;
				}
				if(phase == 1)
				{
					phase = 2;
					NPC.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					npc.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[npc.type].Value.Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.45f;
				float y = Main.rand.Next(-10, 11) * 0.45f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("DissolvingDeluge"), 1);	
		}	
	}
}
