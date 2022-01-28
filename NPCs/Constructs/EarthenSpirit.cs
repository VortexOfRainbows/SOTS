using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Earth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class EarthenSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthen Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;   
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 10;
            npc.lifeMax = 450; 
            npc.damage = 50; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 58;
            npc.height = 58;
			Main.npcFrameCount[npc.type] = 1;   
            npc.value = 32500;
            npc.npcSlots = 4f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
			npc.rarity = 2;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 70;
			npc.lifeMax = 750;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
			writer.Write(saveData);
			writer.Write(dmg);
			writer.Write(owner);
			writer.Write(npc.scale);
			writer.Write(npc.width);
			writer.Write(npc.height);
			writer.Write(reticlePos.X);
			writer.Write(reticlePos.Y);
			writer.Write(npc.alpha);
			writer.Write(reticleAlpha);
			writer.Write(npc.dontTakeDamage);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
			saveData = reader.ReadSingle();
			dmg = reader.ReadInt32();
			owner = reader.ReadInt32();
			npc.scale = reader.ReadSingle();
			npc.width = reader.ReadInt32();
			npc.height = reader.ReadInt32();
			reticlePos.X = reader.ReadSingle();
			reticlePos.Y = reader.ReadSingle();
			npc.alpha = reader.ReadInt32();
			reticleAlpha = reader.ReadSingle();
			npc.dontTakeDamage = reader.ReadBoolean();
		}
		private int InitiateHealth = 900;
		private float ExpertHealthMult = 1.5f;
		private Vector2 reticlePos = new Vector2(-1, -1);
		private float reticleAlpha = 0;
		int phase = 1;
		int dmg = -1;
		int counter = 0;
		float saveData = -1;
		int owner = -1;
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			if (npc.HasPlayerTarget)
			{
				npc.TargetClosest(false);
			}
			else
			{
				npc.TargetClosest(true);
			}
			if (npc.ai[1] > 0 && saveData == -1)
			{
				npc.scale = 5f / (npc.ai[1] + 6);
				npc.width = (int)(npc.width * npc.scale);
				npc.height = (int)(npc.height * npc.scale);
				saveData = npc.ai[1] *= 20;
				owner = (int)npc.ai[2];
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.netUpdate = true;
				}
				return false;
			}
			else if(saveData == -1)
			{
				saveData = 0;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.netUpdate = true;
				}
				return false;
			}
			if(npc.alpha >= 200 && dmg == -1)
			{
				npc.dontTakeDamage = true;
				dmg = npc.damage;
				npc.damage = 0;
				for(int i = 0; i < 10; i++)
				{
					reticlePos = player.Center + new Vector2(Main.rand.Next(-256, 257), Main.rand.Next(-256, 257));
					int x = (int)reticlePos.X / 16;
					int y = (int)reticlePos.Y / 16;
					Tile tile = Framing.GetTileSafely(x, y);
					if(!tile.active() || !Main.tileSolid[tile.type])
					{
						break;
					}
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.netUpdate = true;
				}
			}
			else if (dmg != -1 && npc.alpha < 200)
			{
				npc.damage = dmg;
				dmg = -1;
				npc.dontTakeDamage = false;
			}
			if (saveData > 0)
			{
				npc.dontTakeDamage = true;
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
							npc.netUpdate = true;
						}
					}
				}
				else
				{
					counter = 100000;
				}
			}
			npc.timeLeft = 300000;
			return saveData != -1;
		}
		public override void AI()
		{	
			Player player = Main.player[npc.target];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (phase == 3)
			{
				npc.aiStyle = -1;
				if (owner != -1)
				{
					npc.ai[0] = Main.npc[owner].ai[0];
				}
				else
				{
					npc.ai[0]++;
				}
				if (npc.ai[0] >= 180 + saveData)
				{
					if (npc.alpha >= 255 && npc.ai[0] >= 320 - saveData)
					{
						npc.velocity *= 0;
						npc.alpha = 255;
						reticleAlpha += 0.007f;

						if (Main.netMode != 1)
							npc.position = reticlePos - new Vector2(npc.width / 2, npc.height / 2);

						if (reticleAlpha >= 1)
						{
							reticlePos = new Vector2(-1,-1);
							reticleAlpha = 0;
							npc.alpha = 0;
							npc.ai[0] = -20;
							Main.PlaySound(2, (int)(npc.Center.X), (int)(npc.Center.Y), 14, 2.0f - saveData * 0.02f);
							for (int i = 0; i < 360; i += 10)
							{
								Vector2 circularLocation = new Vector2(16 - npc.width, 0).RotatedBy(MathHelper.ToRadians(i));

								int num1 = Dust.NewDust(new Vector2(npc.Center.X + circularLocation.X - 4, npc.Center.Y + circularLocation.Y - 4), 4, 4, 222);
								Main.dust[num1].noGravity = true;
								Main.dust[num1].scale = 1.2f;
								Main.dust[num1].velocity = circularLocation * 0.25f + new Vector2(Main.rand.Next(-20,21), Main.rand.Next(-20, 21)) * 0.1f;


								num1 = Dust.NewDust(new Vector2(npc.Center.X + circularLocation.X - 4, npc.Center.Y + circularLocation.Y - 4), 4, 4, 222);
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
										Projectile.NewProjectile(npc.Center, rotate, ModContent.ProjectileType<EarthenBolt>(), 20, 0, Main.myPlayer);
								}
							}
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							npc.netUpdate = true;
						}
					}
					else if(npc.velocity.X != 0 || npc.velocity.Y != 0)
					{
						npc.alpha += 5;
					}
				}
				else if (npc.ai[0] >= 0)
				{
					Vector2 rotatePos = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 1.4f - saveData * 2));
					rotatePos += player.Center;
					rotatePos.Y -= 324;

					Vector2 velo = rotatePos - npc.Center;
					velo.Normalize();
					if (6 >= (rotatePos - npc.Center).Length() + 1)
					{
						velo *= (rotatePos - npc.Center).Length() + 1;
					}
					else
					{
						velo *= 6;
					}
					npc.velocity = velo;
					npc.active = true;
				}
			}
			if(phase == 2)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.netUpdate = true;
				}
				npc.aiStyle = -1;
				npc.ai[0] = 0;
				npc.ai[1] = 0;
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
			if(counter >= 1200)
			{
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				phase = 1;
				npc.aiStyle = -1;
				npc.velocity.Y -= 0.014f;
				npc.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<CopyDust4>());
			Dust dust = Main.dust[dust2];
			dust.color = new Color(255, 191, 0);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
			dust.alpha = npc.alpha;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + new Vector2(npc.width/2, npc.height/2);
				Color color = lightColor * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f * ((255 - npc.alpha) / 255f), npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int i = 0; i < 50; i++)
				{
					int dust3 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
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
					npc.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					npc.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Texture2D texture2 = mod.GetTexture("NPCs/Constructs/EarthenReticle");
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.45f;
				float y = Main.rand.Next(-10, 11) * 0.45f;
				Main.spriteBatch.Draw(texture,new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y),null, color * ((255 - npc.alpha)/255f), 0f, drawOrigin, npc.scale, SpriteEffects.None, 0f);

				x = Main.rand.Next(-10, 11) * 0.125f;
				y = Main.rand.Next(-10, 11) * 0.125f;
				if (reticlePos.X != -1 && reticlePos.Y != -1)
					Main.spriteBatch.Draw(texture2,new Vector2((float)(reticlePos.X - (int)Main.screenPosition.X) + x, (float)(reticlePos.Y - (int)Main.screenPosition.Y) + y),null, color * reticleAlpha, MathHelper.ToRadians(npc.ai[0] * 6f), drawOrigin, npc.scale * reticleAlpha, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DissolvingEarth>(), 1);	
		}	
	}
}
