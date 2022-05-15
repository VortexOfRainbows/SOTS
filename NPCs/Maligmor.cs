using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Pyramid;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class Maligmor : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maligmor");
		}
		public override void SetDefaults()
		{
            NPC.lifeMax = 200;   
            NPC.damage = 40; 
            NPC.defense = 10;  
            NPC.knockBackResist = 0f;
            NPC.width = 56;
            NPC.height = 52;
			Main.npcFrameCount[NPC.type] = 1;  
            NPC.value = 1000;
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.netAlways = true;
			Banner = NPC.type;
			BannerItem = ItemType<MaligmorBanner>();
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			NPC.lifeMax = 360;
			NPC.damage = 70;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = NPC.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			texture = GetTexture("SOTS/NPCs/MaligmorGlow");
			spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[NPC.target];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/MaligmorEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = NPC.Center - Main.screenPosition;
			Vector2 aimAt = toPlayer;
			aimAt = aimAt.SafeNormalize(Vector2.Zero);
			aimAt *= 2f;
			drawPos += aimAt;
			spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
		}
		bool runOnce = true;
        public override bool PreAI()
        {
			if(runOnce)
            {
				runOnce = false;
				NPC.ai[1] = 1;
			}
			return base.PreAI();
        }
		Vector2 toPlayerTrue;
		Vector2 toPlayer;
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, NPC.Center - new Vector2(16, 16), 32, 32);
			toPlayer = player.Center - NPC.Center;
			float length = toPlayer.Length();
			NPC.ai[0]++;
			float dashPatternEnd = 100f;
			if (NPC.ai[0] >= dashPatternEnd || NPC.ai[2] == -2)
			{
				NPC.ai[0] = 0;
				NPC.ai[1] *= -1;
				if(NPC.ai[2] < 0)
                {
					NPC.ai[2] = 0;
					NPC.ai[0] -= 50f;
                }
			}
			else if(length <= 640)
			{
				float speed = 2f;
				float firstInterval = 60;
				float intervalLength = 16;
				float finalInt = firstInterval + intervalLength;
				float speedMult = 0.0f;
				if((int)NPC.ai[0] == (int)firstInterval - 4)
				{
					toPlayerTrue = toPlayer.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(15f, 25f) * NPC.ai[1]));
					if (Main.netMode != 1)
					{
						if ((Main.rand.Next(5) <= 1 && length <= 640) || (!lineOfSight && length <= 480))
						{
							NPC.ai[2] = -1;
							NPC.netUpdate = true;
						}
					}
					return;
				}
				if(NPC.ai[0] > firstInterval + intervalLength)
				{
					speedMult = 1.0f + ((finalInt - NPC.ai[0]) / (dashPatternEnd - finalInt)); //slowing dash
				}
				else if (NPC.ai[0] > firstInterval)
				{
					if (NPC.ai[2] == -1)
					{
						SoundEngine.PlaySound(4, (int)NPC.Center.X, (int)NPC.Center.Y, 1, 0.9f, -0.25f);
						if(Main.netMode != 1)
						{
							int total = 0;
							for (int i = 0; i < Main.maxNPCs; i++)
							{
								NPC pet = Main.npc[i];
								if (pet.type == Mod.Find<ModNPC>("MaligmorChild") .Type&& (int)pet.ai[0] == NPC.whoAmI && pet.active)
								{
									total++;
									pet.ai[3] += 44;
									pet.netUpdate = true;
								}
							}
							int amt = 9 - total;
							if (Main.expertMode)
								amt += 3;
							if (amt >= 3)
								amt = 1 + Main.rand.Next(2) + Main.rand.Next(2);
							if(amt > 0)
								for (int i = 0; i < amt; i++)
								{
									int npc1 = NPC.NewNPC((int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height, Mod.Find<ModNPC>("MaligmorChild").Type, 0, NPC.whoAmI, Main.rand.NextFloat(30), i * 120 + Main.rand.Next(120));
									Main.npc[npc1].netUpdate = true;
								}
							for (int k = 0; k < 54; k++)
							{
								Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), 0, 0, 0, default, 1.0f);
								dust.scale *= 1.3f;
								dust.velocity *= 1.4f;
								dust.noGravity = true;
							}
						}
						NPC.ai[2] = -2;
					}
					else
						speedMult = 0.0f - ((firstInterval - NPC.ai[0]) / intervalLength); //accelerating dash
				}
				else if (NPC.ai[0] > 0)
                {
					Vector2 strangeCircular = new Vector2(0, 0.8f).RotatedBy(MathHelper.ToRadians(NPC.ai[0] / 80f * 360 * NPC.ai[1]));
					if(NPC.ai[0] % 20 > 8)
                    {
						NPC.velocity += strangeCircular;
                    }
                }
				NPC.velocity += toPlayerTrue.SafeNormalize(Vector2.Zero) * speed * speedMult;
				NPC.velocity *= 0.9f;
				NPC.velocity.Y *= 0.93f;
			}
			for (int i = 0; i < 1 + Main.rand.Next(2); i++)
			{
				int num1 = Dust.NewDust(NPC.position, NPC.width - 4, 12, ModContent.DustType<CurseDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity.X = NPC.velocity.X;
				Main.dust[num1].velocity.Y = -2 + i * 1.0f;
				Main.dust[num1].scale *= 1.25f + i * 0.15f;
			}
			NPC.velocity = Collision.TileCollision(NPC.position + new Vector2(8, 8), NPC.velocity, NPC.width - 16, NPC.height - 16, true);
			NPC.rotation = NPC.velocity.X * 0.036f;
		}
		public override void FindFrame(int frameHeight) 
		{
			/*
			npc.frameCounter++;
			if (npc.frameCounter >= 5f) 
			{
				npc.frameCounter -= 5f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= 4 * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
			*/
		}
		public override void NPCLoot()
		{
			if(SOTSWorld.downedCurse && Main.rand.NextBool(3))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<CursedMatter>(), Main.rand.Next(2) + 1);	
			else
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<SoulResidue>(), Main.rand.Next(2) + 1);
			int type = ItemType<CursedTumor>();
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, type, Main.rand.Next(6) + 5);
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
			{
				int num = 0;
				while (num < damage / NPC.lifeMax * 50.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), (float)(2 * hitDirection), -2f, 0, default, 1.5f);
					num++;
				}
			}
            else
			{
				for(int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(-28, 0).RotatedBy(MathHelper.ToRadians(-i * 45)) - new Vector2(9, 9);
					Gore.NewGore(NPC.Center + circular, circular * 0.15f, Mod.GetGoreSlot("Gores/Maligmor/MaligmorGore" + i), 1f);
				}
				for (int i = 0; i < 72; i++)
				{
					Vector2 circular = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i * 5));
					Dust dust = Dust.NewDustDirect(NPC.Center - new Vector2(5), 0, 0, 231, 0, 0, NPC.alpha, default, 1f);
					dust.velocity *= 0.5f;
					dust.velocity += circular * 0.75f;
					dust.scale = 2.5f;
					dust.noGravity = true;
					dust = Dust.NewDustDirect(NPC.Center - new Vector2(5), 0, 0, 231, 0, 0, NPC.alpha, default, 1f);
					dust.velocity *= 0.5f;
					dust.velocity += circular * 0.25f;
					dust.scale = 1.25f;
					dust.noGravity = true;
					if(i == 36 || i == 0)
					{
						for(int l = 0; l < 10; l++)
						{
							dust = Dust.NewDustDirect(NPC.Center - new Vector2(5), 0, 0, 231, 0, 0, NPC.alpha, default, 1f);
							dust.velocity *= 0.5f;
							dust.velocity += circular * (0.25f + l * 0.05f);
							dust.scale = 1.25f + (1.25f * l * 0.1f);
							dust.noGravity = true;
						}
					}
				}
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), (float)(2 * hitDirection), -2f, 0, default, 1.5f);
				}
			}		
		}
	}
}





















