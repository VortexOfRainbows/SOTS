using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class OtherworldlySpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Otherworldly Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
			writer.Write(npc.localAI[1]);
			writer.Write(collectorId);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
			npc.localAI[1] = reader.ReadSingle();
			collectorId = reader.ReadInt32();
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 10;
            npc.lifeMax = 1500; 
            npc.damage = 80; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 58;
            npc.height = 58;
			Main.npcFrameCount[npc.type] = 1;   
            npc.value = 35075;
            npc.npcSlots = 4f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = false;
			npc.rarity = 2;
		}
		private int InitiateHealth = 6000;
		private float ExpertHealthMult = 1.5f;
		
		int phase = 1;
		int counter = 0;
		int collectorId = -1;
		public override void AI()
		{
			Color color = new Color(167, 45, 225, 0);
			Color color2 = new Color(64, 178, 172, 0);
			color = Color.Lerp(color, color2, 1 - (npc.life / (float)npc.lifeMax) * (phase == 3 ? 0 : 1));
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 255f, (255 - npc.alpha) * 0.25f / 255f, (255 - npc.alpha) * 0.65f / 255f);
			Player player = Main.player[npc.target];
			if(phase == 3) //attack
			{
				if(collectorId != -1)
                {
					npc.velocity *= 0f;
					NPC collector = Main.npc[collectorId];
					if(collector.type != mod.NPCType("Collector"))
					{
						int n = NPC.NewNPC((int)npc.Center.X + 6, (int)npc.position.Y, mod.NPCType("Collector"));
						Main.npc[n].netUpdate = true;
						collectorId = n;
						npc.netUpdate = true;
					}
					else
                    {
						float ai3 = collector.ai[3];
						if(ai3 < 100 && ai3 > 0)
						{
							npc.scale -= 0.005f;
							int dust3 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
							Dust dust4 = Main.dust[dust3];
							dust4.velocity *= 2.5f;
							dust4.color = color;
							dust4.noGravity = true;
							dust4.fadeIn = 0.1f;
							dust4.scale *= 2.5f;
							if (ai3 % 30 == 0)
							{
								if (Main.netMode != 1)
								{
									int item = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DissolvingAether"), 1);
									Main.item[item].velocity = new Vector2(-5, 0).RotatedBy(MathHelper.ToRadians(-10 - ai3 * 4 / 3f));
									NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
								}
							}
						}
						if(ai3 >= 100)
						{
							npc.StrikeNPC(10000, 0, 0);
							if (Main.netMode != 0)
								NetMessage.SendData(28, -1, -1, null, npc.whoAmI, 10000, 0, 0, 0, 0, 0);
						}
					}
                }
				npc.dontTakeDamage = true;
			}
			if(phase == 2) //upon "death"
			{
				npc.velocity *= 0f;
				if (Main.netMode != 1)
				{
					int n = NPC.NewNPC((int)npc.Center.X + 6, (int)npc.position.Y, mod.NPCType("Collector"));
					Main.npc[n].netUpdate = true;
					collectorId = n;
					npc.netUpdate = true;
				}
				if(collectorId != -1)
				{
					npc.aiStyle = -1;
					npc.ai[0] = 0;
					npc.ai[1] = 0;
					phase = 3;
				}
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(npc.localAI[1] == -1)
			{
				counter += 19;
			}
			if(Main.player[npc.target].dead)
			{
				counter++;
			}
			if(counter >= 1200) //run
			{
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				phase = 1;
				npc.aiStyle = -1;
				npc.velocity.Y -= 0.03f;
				npc.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height,  267);
			Dust dust = Main.dust[dust2];
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("NPCs/Constructs/OtherworldlySpiritOld");
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
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
					int dust3 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 2.5f;
					dust4.color = new Color(64, 178, 172, 0);
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.5f;
				}
				if(phase == 1)
				{
					phase = 2;
					npc.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					npc.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Color color = new Color(167, 45, 225, 0);
			Color color2 = new Color(64, 178, 172, 0);
			color = Color.Lerp(color, color2, 1 - (npc.life/(float)npc.lifeMax) * (phase == 3 ? 0 : 1));
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.45f * npc.scale;
				float y = Main.rand.Next(-10, 11) * 0.45f * npc.scale;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color, 0f, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("DissolvingAether"), 1);	
		}	
	}
}
