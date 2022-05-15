using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
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
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;  
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
			writer.Write(NPC.localAI[1]);
			writer.Write(collectorId);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
			NPC.localAI[1] = reader.ReadSingle();
			collectorId = reader.ReadInt32();
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =10;
            NPC.lifeMax = 1500; 
            NPC.damage = 80; 
            NPC.defense = 0;   
            NPC.knockBackResist = 0f;
            NPC.width = 58;
            NPC.height = 58;
			Main.npcFrameCount[NPC.type] = 1;   
            NPC.value = 35075;
            NPC.npcSlots = 4f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = false;
			NPC.rarity = 2;
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
			color = Color.Lerp(color, color2, 1 - (NPC.life / (float)NPC.lifeMax) * (phase == 3 ? 0 : 1));
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.15f / 255f, (255 - NPC.alpha) * 0.25f / 255f, (255 - NPC.alpha) * 0.65f / 255f);
			Player player = Main.player[NPC.target];
			if(phase == 3) //attack
			{
				if(collectorId != -1)
                {
					NPC.velocity *= 0f;
					NPC collector = Main.npc[collectorId];
					if(collector.type != Mod.Find<ModNPC>("Collector").Type)
					{
						int n = NPC.NewNPC((int)NPC.Center.X + 6, (int)NPC.position.Y, Mod.Find<ModNPC>("Collector").Type);
						Main.npc[n].netUpdate = true;
						collectorId = n;
						NPC.netUpdate = true;
					}
					else
                    {
						float ai3 = collector.ai[3];
						if(ai3 < 100 && ai3 > 0)
						{
							NPC.scale -= 0.005f;
							int dust3 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 267);
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
									int item = Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("DissolvingAether").Type, 1);
									Main.item[item].velocity = new Vector2(-5, 0).RotatedBy(MathHelper.ToRadians(-10 - ai3 * 4 / 3f));
									NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
								}
							}
						}
						if(ai3 >= 100)
						{
							NPC.StrikeNPC(10000, 0, 0);
							if (Main.netMode != 0)
								NetMessage.SendData(28, -1, -1, null, NPC.whoAmI, 10000, 0, 0, 0, 0, 0);
						}
					}
                }
				NPC.dontTakeDamage = true;
			}
			if(phase == 2) //upon "death"
			{
				NPC.velocity *= 0f;
				if (Main.netMode != 1)
				{
					int n = NPC.NewNPC((int)NPC.Center.X + 6, (int)NPC.position.Y, Mod.Find<ModNPC>("Collector").Type);
					Main.npc[n].netUpdate = true;
					collectorId = n;
					NPC.netUpdate = true;
				}
				if(collectorId != -1)
				{
					NPC.aiStyle =-1;
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					phase = 3;
				}
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(NPC.localAI[1] == -1)
			{
				counter += 19;
			}
			if(Main.player[NPC.target].dead)
			{
				counter++;
			}
			if(counter >= 1320) //run
			{
				if (Main.netMode != 1)
				{
					NPC.netUpdate = true;
				}
				phase = 1;
				NPC.aiStyle =-1;
				NPC.velocity.Y -= 0.03f;
				NPC.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height,  267);
			Dust dust = Main.dust[dust2];
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Constructs/OtherworldlySpiritOld").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(lightColor) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
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
					int dust3 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 267);
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 2.5f;
					dust4.color = new Color(64, 178, 172, 0);
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.5f;
				}
				if(phase == 1 && NPC.localAI[1] != -1)
				{
					phase = 2;
					NPC.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					NPC.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Color color = new Color(167, 45, 225, 0);
			Color color2 = new Color(64, 178, 172, 0);
			color = Color.Lerp(color, color2, 1 - (NPC.life/(float)NPC.lifeMax) * (phase == 3 ? 0 : 1));
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.45f * NPC.scale;
				float y = Main.rand.Next(-10, 11) * 0.45f * NPC.scale;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(NPC.Center.X - (int)Main.screenPosition.X) + x, (float)(NPC.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color, 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			if (NPC.localAI[1] == -1)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<DissolvingAether>(), 1);	
		}	
	}
}
