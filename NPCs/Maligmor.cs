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
            npc.lifeMax = 200;   
            npc.damage = 40; 
            npc.defense = 10;  
            npc.knockBackResist = 0f;
            npc.width = 56;
            npc.height = 52;
			Main.npcFrameCount[npc.type] = 1;  
            npc.value = 1000;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.netUpdate = true;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
			//banner = npc.type;
			//bannerItem = ItemType<BleedingGhastBanner>();
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			npc.lifeMax = 300;
			npc.damage = 70;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			texture = GetTexture("SOTS/NPCs/MaligmorGlow");
			spriteBatch.Draw(texture, drawPos, npc.frame, Color.White, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[npc.target];
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/MaligmorEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			Vector2 aimAt = toPlayer;
			aimAt = aimAt.SafeNormalize(Vector2.Zero);
			aimAt *= 2f;
			drawPos += aimAt;
			spriteBatch.Draw(texture, drawPos, npc.frame, Color.White, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
		}
		bool runOnce = true;
        public override bool PreAI()
        {
			if(runOnce)
            {
				runOnce = false;
				npc.ai[1] = 1;
			}
			return base.PreAI();
        }
		Vector2 toPlayerTrue;
		Vector2 toPlayer;
		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height);
			toPlayer = player.Center - npc.Center;
			float length = toPlayer.Length();
			npc.ai[0]++;
			float dashPatternEnd = 120f;
			if (npc.ai[0] >= dashPatternEnd || npc.ai[2] == -2)
			{
				npc.ai[0] = 0;
				npc.ai[1] *= -1;
				if(npc.ai[2] < 0)
                {
					npc.ai[2] = 0;
					npc.ai[0] -= 30f;
                }
			}
			else
			{
				float speed = 1.5f;
				float firstInterval = 80;
				float intervalLength = 16;
				float finalInt = firstInterval + intervalLength;
				float speedMult = 0.0f;
				if((int)npc.ai[0] == (int)firstInterval - 4)
				{
					toPlayerTrue = toPlayer.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(15f, 25f) * npc.ai[1]));
					if (Main.netMode != 1)
					{
						if ((Main.rand.NextBool(2) && length <= 640) || (!lineOfSight && length <= 960))
						{
							npc.ai[2] = -1;
							npc.netUpdate = true;
						}
					}
					return;
				}
				if(npc.ai[0] > firstInterval + intervalLength)
				{
					speedMult = 1.0f + ((finalInt - npc.ai[0]) / (dashPatternEnd - finalInt)); //slowing dash
				}
				else if (npc.ai[0] > firstInterval)
				{
					if (npc.ai[2] == -1)
					{
						if(Main.netMode != 1)
						{
							int total = 0;
							for (int i = 0; i < Main.maxNPCs; i++)
							{
								NPC pet = Main.npc[i];
								if (pet.type == mod.NPCType("MaligmorChild") && (int)pet.ai[0] == npc.whoAmI && pet.active)
								{
									total++;
								}
							}
							int amt = 8 - total;
							if (amt >= 3)
								amt = 2 + Main.rand.Next(2);
							for (int i = 0; i < amt; i++)
							{
								int npc1 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height, mod.NPCType("MaligmorChild"), 0, npc.whoAmI, Main.rand.NextFloat(30), i * 120 + Main.rand.Next(120));
								Main.npc[npc1].netUpdate = true;
							}
						}
						npc.ai[2] = -2;
					}
					else
						speedMult = 0.0f - ((firstInterval - npc.ai[0]) / intervalLength); //accelerating dash
				}
				else if (npc.ai[0] > 0)
                {
					Vector2 strangeCircular = new Vector2(0, 0.8f).RotatedBy(MathHelper.ToRadians(npc.ai[0] / 80f * 360 * npc.ai[1]));
					if(npc.ai[0] % 20 > 8)
                    {
						npc.velocity += strangeCircular;
                    }
                }
				npc.velocity += toPlayerTrue.SafeNormalize(Vector2.Zero) * speed * speedMult;
				npc.velocity *= 0.9f;
				npc.velocity.Y *= 0.93f;
			}
			for (int i = 0; i < 1 + Main.rand.Next(2); i++)
			{
				int num1 = Dust.NewDust(npc.position, npc.width - 4, 12, mod.DustType("CurseDust"));
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity.X = npc.velocity.X;
				Main.dust[num1].velocity.Y = -2 + i * 1.0f;
				Main.dust[num1].scale *= 1.25f + i * 0.15f;
			}
			int damage2 = npc.damage / 2;
			if (Main.expertMode)
			{
				damage2 = (int)(damage2 / Main.expertDamage);
			}
			npc.velocity = Collision.TileCollision(npc.position + new Vector2(8, 8), npc.velocity, npc.width - 16, npc.height - 16, true);
		}
		public override void FindFrame(int frameHeight) 
		{
			/*
			npc.frameCounter++;
			if (npc.frameCounter >= 5f) 
			{
				npc.frameCounter -= 5f;
				npc.frame.Y += frameHeight;
				if(npc.frame.Y >= 4 * frameHeight)
				{
					npc.frame.Y = 0;
				}
			}
			*/
		}
		public override void NPCLoot()
		{
			if(SOTSWorld.downedCurse && Main.rand.NextBool(3))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("CursedMatter"), Main.rand.Next(2) + 1);	
			else
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<SoulResidue>(), Main.rand.Next(2) + 1);
			int type = ItemType<CursedTumor>();
			if (!Main.rand.NextBool(3))
				type = ItemType<Maldite>();
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, type, Main.rand.Next(6) + 5);
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
			{
				int num = 0;
				while (num < damage / npc.lifeMax * 50.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
				}
			}		
		}
	}
}





















