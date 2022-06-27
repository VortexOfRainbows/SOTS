using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Earth;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class EarthenSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthen Spirit");
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;  
			NPCID.Sets.TrailingMode[NPC.type] = 0;   
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =10;
            NPC.lifeMax = 450; 
            NPC.damage = 50; 
            NPC.defense = 0;   
            NPC.knockBackResist = 0f;
            NPC.width = 58;
            NPC.height = 58;
			Main.npcFrameCount[NPC.type] = 1;   
            NPC.value = 32500;
            NPC.npcSlots = 4f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
			NPC.rarity = 2;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.damage = (int)(NPC.damage * 7 / 10);
			NPC.lifeMax = (int)(NPC.lifeMax * 5 / 6);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
			writer.Write(saveData);
			writer.Write(dmg);
			writer.Write(owner);
			writer.Write(NPC.scale);
			writer.Write(NPC.width);
			writer.Write(NPC.height);
			writer.Write(reticlePos.X);
			writer.Write(reticlePos.Y);
			writer.Write(NPC.alpha);
			writer.Write(reticleAlpha);
			writer.Write(NPC.dontTakeDamage);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
			saveData = reader.ReadSingle();
			dmg = reader.ReadInt32();
			owner = reader.ReadInt32();
			NPC.scale = reader.ReadSingle();
			NPC.width = reader.ReadInt32();
			NPC.height = reader.ReadInt32();
			reticlePos.X = reader.ReadSingle();
			reticlePos.Y = reader.ReadSingle();
			NPC.alpha = reader.ReadInt32();
			reticleAlpha = reader.ReadSingle();
			NPC.dontTakeDamage = reader.ReadBoolean();
		}
		private int InitiateHealth = 900;
		private float ExpertHealthMult = 1.5f;
		private float MasterHealthMult = 2f;
		private Vector2 reticlePos = new Vector2(-1, -1);
		private float reticleAlpha = 0;
		int phase = 1;
		int dmg = -1;
		int counter = 0;
		float saveData = -1;
		int owner = -1;
		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			if (NPC.HasPlayerTarget)
			{
				NPC.TargetClosest(false);
			}
			else
			{
				NPC.TargetClosest(true);
			}
			if (NPC.ai[1] > 0 && saveData == -1)
			{
				NPC.scale = 5f / (NPC.ai[1] + 6);
				NPC.width = (int)(NPC.width * NPC.scale);
				NPC.height = (int)(NPC.height * NPC.scale);
				saveData = NPC.ai[1] *= 20;
				owner = (int)NPC.ai[2];
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
				return false;
			}
			else if(saveData == -1)
			{
				saveData = 0;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
				return false;
			}
			if(NPC.alpha >= 200 && dmg == -1)
			{
				NPC.dontTakeDamage = true;
				dmg = NPC.damage;
				NPC.damage = 0;
				for(int i = 0; i < 10; i++)
				{
					reticlePos = player.Center + new Vector2(Main.rand.Next(-256, 257), Main.rand.Next(-256, 257));
					int x = (int)reticlePos.X / 16;
					int y = (int)reticlePos.Y / 16;
					Tile tile = Framing.GetTileSafely(x, y);
					if(!tile.HasTile || !Main.tileSolid[tile.TileType])
					{
						break;
					}
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
			}
			else if (dmg != -1 && NPC.alpha < 200)
			{
				NPC.damage = dmg;
				dmg = -1;
				NPC.dontTakeDamage = false;
			}
			if (saveData > 0)
			{
				NPC.dontTakeDamage = true;
			}
			if(owner != -1)
			{
				NPC head = Main.npc[owner];
				if(head.type == ModContent.NPCType<EarthenSpirit>())
				{
					if(!head.active)
					{
						counter = 100000;
					}
					if(head.lifeMax >= InitiateHealth && phase == 1)
					{
						phase = 2;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NPC.netUpdate = true;
						}
					}
				}
				else
				{
					counter = 100000;
				}
			}
			NPC.timeLeft = 300000;
			return saveData != -1;
		}
		public override void AI()
		{	
			Player player = Main.player[NPC.target];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (phase == 3)
			{
				NPC.aiStyle =-1;
				if (owner != -1)
				{
					NPC.ai[0] = Main.npc[owner].ai[0];
				}
				else
				{
					NPC.ai[0]++;
				}
				if (NPC.ai[0] >= 180 + saveData)
				{
					if (NPC.alpha >= 255 && NPC.ai[0] >= 320 - saveData)
					{
						NPC.velocity *= 0;
						NPC.alpha = 255;
						reticleAlpha += 0.007f;

						if (Main.netMode != 1)
							NPC.position = reticlePos - new Vector2(NPC.width / 2, NPC.height / 2);

						if (reticleAlpha >= 1)
						{
							reticlePos = new Vector2(-1,-1);
							reticleAlpha = 0;
							NPC.alpha = 0;
							NPC.ai[0] = -20;
							SOTSUtils.PlaySound(SoundID.Item14, (int)NPC.Center.X, (int)NPC.Center.Y, 2.0f - saveData * 0.02f);
							for (int i = 0; i < 360; i += 10)
							{
								Vector2 circularLocation = new Vector2(16 - NPC.width, 0).RotatedBy(MathHelper.ToRadians(i));

								int num1 = Dust.NewDust(new Vector2(NPC.Center.X + circularLocation.X - 4, NPC.Center.Y + circularLocation.Y - 4), 4, 4, 222);
								Main.dust[num1].noGravity = true;
								Main.dust[num1].scale = 1.2f;
								Main.dust[num1].velocity = circularLocation * 0.25f + new Vector2(Main.rand.Next(-20,21), Main.rand.Next(-20, 21)) * 0.1f;


								num1 = Dust.NewDust(new Vector2(NPC.Center.X + circularLocation.X - 4, NPC.Center.Y + circularLocation.Y - 4), 4, 4, 222);
								Main.dust[num1].noGravity = true;
								Main.dust[num1].scale = 1.5f;
								Main.dust[num1].velocity = circularLocation * 0.45f + new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21)) * 0.2f;
							}
							if(owner == -1)
							{
								for(int i = 0; i < 8; i ++)
								{
									float degrees = i * 45;
									Vector2 rotate = new Vector2(2.75f, 0).RotatedBy(MathHelper.ToRadians(degrees));
									if (Main.netMode != NetmodeID.MultiplayerClient)
										Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, rotate, ModContent.ProjectileType<EarthenBolt>(), 20, 0, Main.myPlayer);
								}
							}
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NPC.netUpdate = true;
						}
					}
					else if(NPC.velocity.X != 0 || NPC.velocity.Y != 0)
					{
						NPC.alpha += 5;
					}
				}
				else if (NPC.ai[0] >= 0)
				{
					Vector2 rotatePos = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 1.4f - saveData * 2));
					rotatePos += player.Center;
					rotatePos.Y -= 324;

					Vector2 velo = rotatePos - NPC.Center;
					velo.Normalize();
					if (6 >= (rotatePos - NPC.Center).Length() + 1)
					{
						velo *= (rotatePos - NPC.Center).Length() + 1;
					}
					else
					{
						velo *= 6;
					}
					NPC.velocity = velo;
					NPC.active = true;
				}
			}
			if(phase == 2)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
				NPC.aiStyle =-1;
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
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
			if(counter >= 1200)
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
			int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<CopyDust4>());
			Dust dust = Main.dust[dust2];
			dust.color = new Color(255, 191, 0);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
			dust.alpha = NPC.alpha;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				Vector2 drawPos = NPC.oldPos[k] - screenPos + new Vector2(NPC.width/2, NPC.height/2);
				Color color = drawColor * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f * ((255 - NPC.alpha) / 255f), NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 50; i++)
				{
					int dust3 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 267);
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 2.5f;
					dust4.color = new Color(255, 191, 0);
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.5f;
				}
				if(phase == 1)
				{
					phase = 2;
					NPC.lifeMax = (int)(InitiateHealth * (Main.masterMode ? MasterHealthMult : Main.expertMode ? ExpertHealthMult : 1));
					NPC.life = NPC.lifeMax;
				}
			}
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        { 
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/EarthenReticle").Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.45f;
				float y = Main.rand.Next(-10, 11) * 0.45f;
				spriteBatch.Draw(texture,new Vector2((float)(NPC.Center.X - (int)screenPos.X) + x, (float)(NPC.Center.Y - (int)screenPos.Y) + y),null, color * ((255 - NPC.alpha)/255f), 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
				x = Main.rand.Next(-10, 11) * 0.125f;
				y = Main.rand.Next(-10, 11) * 0.125f;
				if (reticlePos.X != -1 && reticlePos.Y != -1)
					spriteBatch.Draw(texture2,new Vector2((float)(reticlePos.X - (int)screenPos.X) + x, (float)(reticlePos.Y - (int)screenPos.Y) + y),null, color * reticleAlpha, MathHelper.ToRadians(NPC.ai[0] * 6f), drawOrigin, NPC.scale * reticleAlpha, SpriteEffects.None, 0f);
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DissolvingEarth>()));
		}
	}
}
