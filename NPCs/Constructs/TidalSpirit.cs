using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Tide;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class TidalSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Spirit");
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;  
			NPCID.Sets.TrailingMode[NPC.type] = 0;
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
            NPC.value = 35075;
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
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.damage = NPC.damage * 2 / 3;
			NPC.lifeMax = NPC.lifeMax * 25 / 32;
		}
		Vector2 projectileVelo = Vector2.Zero;
		private int InitiateHealth = 3000;
		private float ExpertHealthMult = 1.25f;
		int phase = 1;
		int counter = 0;
		int direction = 1;
		public override void AI()
		{
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.15f / 255f, (255 - NPC.alpha) * 0.25f / 255f, (255 - NPC.alpha) * 0.65f / 255f);
			Player player = Main.player[NPC.target];
			if(phase == 3)
			{
				NPC.dontTakeDamage = false;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
				if(NPC.ai[3] % 2 == 0)
				{
					if (NPC.ai[1] < 360)
					{
						if (NPC.ai[1] > 270)
						{
							NPC.velocity *= 0.1f;
							if (NPC.ai[2] % 14 == 0)
							{
								Vector2 toPlayer = player.Center - NPC.Center;
								toPlayer = toPlayer.SafeNormalize(new Vector2(1, 0));
								if (projectileVelo == Vector2.Zero)
									projectileVelo = toPlayer;
								int damage2 = NPC.GetBaseDamage() / 2;
								SOTSUtils.PlaySound(SoundID.Item21, (int)NPC.Center.X, (int)NPC.Center.Y, 0.8f);
								int last = -1;
								for (int i = 0; i < 2; i++)
								{
									float spread = 11.5f;
									Vector2 circleGen = new Vector2(spread, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[2] * 2.0f));
									Vector2 velo = projectileVelo.RotatedBy(MathHelper.ToRadians(circleGen.X - (i * 2 - 1) * 11.5f));
									float speed = 7f;
									if (Main.netMode != 1)
									{
										int temp = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velo * speed, ModContent.ProjectileType<TidalWave>(), damage2, 0f, Main.myPlayer, last, 0);
										last = temp;
									}
								}
								NPC.ai[1] += 5;
							}
							NPC.ai[2]++;
						}
						else
						{
							NPC.ai[1]++;
							NPC.ai[0]++;
							Vector2 circleGen = new Vector2(20f, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0]));
							Vector2 rotatePos = new Vector2(720 * direction, 0).RotatedBy(MathHelper.ToRadians(circleGen.X));
							//Vector2 rotateAround = new Vector2(npc.ai[1], 0).RotatedBy(MathHelper.ToRadians(npc.ai[1] * 2));
							Vector2 toCircle = rotatePos + player.Center - NPC.Center;
							float dist = toCircle.Length();
							toCircle = toCircle.SafeNormalize(Vector2.Zero);
							float speed = 9.5f;
							if (speed > dist)
							{
								speed = dist;
							}
							toCircle *= speed;
							NPC.velocity = toCircle;
						}
					}
					else
					{
						if (Main.netMode != 1)
							NPC.netUpdate = true;
						NPC.ai[0] += Main.rand.Next(180);
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
						NPC.ai[3]++;
						projectileVelo = Vector2.Zero;
						direction *= -1;
					}
				}
				else
				{
					NPC.ai[0]++;
					if (NPC.ai[1] < 440)
					{
						NPC.ai[1] += 1.15f;
					}
					else
					{
						NPC.ai[2]++;
						if (NPC.ai[2] % 100 == 0)
						{
							Vector2 toPlayer = player.Center - NPC.Center;
							toPlayer = toPlayer.SafeNormalize(new Vector2(1, 0)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-25f, 25f)));
							projectileVelo = toPlayer;
							int damage2 = NPC.GetBaseDamage() / 2;
							SOTSUtils.PlaySound(SoundID.Item21, (int)NPC.Center.X, (int)NPC.Center.Y, 0.8f);
							int last = -1;
							for (int i = 0; i < 2; i++)
							{
								Vector2 velo = projectileVelo.RotatedBy(MathHelper.ToRadians((i * 2 - 1) * -11.5f));
								float speed2 = 5.7f;
								if (Main.netMode != 1)
								{
									int temp = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velo * speed2, ModContent.ProjectileType<TidalWave>(), damage2, 0f, Main.myPlayer, last, 0);
									last = temp;
								}
							}
						}
						else if(NPC.ai[2] > 600) //6 * 72
						{
							if (Main.netMode != 1)
								NPC.netUpdate = true;
							NPC.ai[0] += Main.rand.Next(180);
							NPC.ai[1] = 0;
							NPC.ai[2] = 0;
							NPC.ai[3]++;
							projectileVelo = Vector2.Zero;
							direction *= -1;
						}
					}
					Vector2 circleGen = new Vector2(20f + NPC.ai[1] * 0.015f, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0] * 2.15f));
					Vector2 rotatePos = new Vector2(0, (NPC.ai[1] + 60) * -1).RotatedBy(MathHelper.ToRadians(circleGen.X));
					rotatePos.Y *= 0.85f;
					Vector2 toCircle = rotatePos + player.Center - NPC.Center;
					float dist = toCircle.Length();
					toCircle = toCircle.SafeNormalize(Vector2.Zero);
					float speed = 12.5f;
					if (speed > dist)
					{
						speed = dist;
					}
					toCircle *= speed;
					NPC.velocity = toCircle;
				}
			}
			if (phase == 2)
			{
				if (Main.netMode != 1)
					NPC.netUpdate = true;
				direction = Main.rand.Next(2) * 2 - 1;
				NPC.dontTakeDamage = false;
				NPC.aiStyle =-1;
				NPC.ai[0] = Main.rand.Next(180);
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
			if(counter >= 1440)
			{
				if (Main.netMode != 1)
				{
					NPC.netUpdate = true;
				}
				phase = 1;
				NPC.aiStyle =-1;
				NPC.velocity.Y -= 0.014f;
				NPC.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 267);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(64, 72, 178);
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
				Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for(int i = 0; i < 50; i ++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 267);
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
					NPC.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.45f;
				float y = Main.rand.Next(-10, 11) * 0.45f;
				spriteBatch.Draw(texture,
				new Vector2((float)(NPC.Center.X - (int)screenPos.X) + x, (float)(NPC.Center.Y - (int)screenPos.Y) + y),
				null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DissolvingDeluge>()));
		}
	}
}
