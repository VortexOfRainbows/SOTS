using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Otherworld;
using SOTS.Projectiles.Tide;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SOTS.NPCs.Constructs
{
	public class TidalConstruct : ModNPC
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(projectiles[0]);
			writer.Write(projectiles[1]);
			writer.Write(projectiles[2]);
			writer.Write(projectiles[3]);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			projectiles[0] = reader.ReadInt32();
			projectiles[1] = reader.ReadInt32();
			projectiles[2] = reader.ReadInt32();
			projectiles[3] = reader.ReadInt32();
		}
        int timer = 0;
		int ai1 = 0;
		float dir = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Construct");
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =0;
			NPC.lifeMax = 750;
			NPC.damage = 40;
			NPC.defense = 16;
			NPC.knockBackResist = 0f;
			NPC.width = 68;
			NPC.height = 62;
			Main.npcFrameCount[NPC.type] = 12;
			NPC.value = 12550;
			NPC.npcSlots = 6f;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.rarity = 5;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			NPC.damage = 60;
			NPC.lifeMax = 1250;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if(projectiles[0] != -1 && projectiles[1] != -1 && projectiles[2] != -1 && projectiles[3] != -1 && Main.netMode != NetmodeID.Server)
				DrawVines(true);
			return false;
		}
		public void DrawVines(bool draw, bool gore = false)
		{
			bool trail = false;
			if (!draw)
				trail = true;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/TidalConstructVine");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/TidalConstructVineGlow");
			for (int j = 0; j < 4; j++)
			{
				Vector2 modi = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0] * 4));
				float finalMod = 1f;
				if (NPC.ai[0] > 630)
				{
					float currentCounter = NPC.ai[0] - 630f;
					finalMod = 1f - (currentCounter / 90f);
					if (finalMod < -1)
						finalMod = -1;
					if (NPC.ai[0] > 900)
					{
						currentCounter = NPC.ai[0] - 900f;
						finalMod = -1f + (currentCounter / 90f);
						if (finalMod > 1)
							finalMod = 1;
					}
					modi.X = 0.5f * finalMod;
					if (modi.X < 0)
					{
						modi.X *= 0.75f;
					}
				}
				float modifier = 0.5f + modi.X;
				Vector2 last = Vector2.Zero;
				int direction = 1;
				if (j >= 2)
					direction = -1;
				float rotation;
				if (j < 2)
				{
					float rot = 50f * (finalMod - 1);
					int mult = j; // 0, 1
					rotation = NPC.rotation - MathHelper.ToRadians((mult + 0.5f) * 30 * (0.5f + 0.9f * (0.5f - modi.X)) - rot);
				}
				else
				{
					float rot = 50f * (finalMod - 1);
					int mult = j - 2; // 0, 1
					rotation = NPC.rotation + MathHelper.ToRadians((mult + 0.5f) * 30 * (0.5f + 0.9f * (0.5f - modi.X)) - rot);
				}
				int h = -1;
				int max = 6;
				if (trail)
				{
					h = 0;
					max = 1;
				}
				for (int i = h; i < max; i++)
				{
					Rectangle frame = new Rectangle(0, 0, texture.Width, 18);
					if (i != 0)
						frame = new Rectangle(0, 18, texture.Width, 18);
					Vector2 centerOfCircle = NPC.Center - new Vector2(108f * (0.6f + 0.4f * modifier), 0).RotatedBy(rotation);
					Vector2 rotationPos2 = new Vector2(16 * i * (0.45f + 0.55f * modifier), 0);
					Vector2 rotationPos = new Vector2(34f, 0).RotatedBy(MathHelper.ToRadians(30 * i));
					rotationPos.X = rotationPos2.X;
					rotationPos.Y /= 3.5f * direction * (0.45f + 0.55f * modifier);
					rotationPos.Y *= finalMod;
					rotationPos = rotationPos.RotatedBy(rotation);
					Vector2 pos = rotationPos + centerOfCircle;
					if(!trail)
					{
						if(!gore)
						{
							Vector2 drawPos = pos - Main.screenPosition;
							float rotation2 = NPC.rotation;
							if (last != Vector2.Zero)
							{
								rotation2 = (pos - last).ToRotation();
							}
							last = pos;
							Color drawColor = Lighting.GetColor((int)pos.X / 16, (int)(pos.Y / 16));
							if (i != -1)
								Main.spriteBatch.Draw(texture, drawPos, frame, drawColor, rotation2, new Vector2(texture.Width / 2, texture.Height / 4), NPC.scale, SpriteEffects.None, 0f);
							if (i == 0)
								Main.spriteBatch.Draw(texture2, drawPos, frame, Color.White, rotation2, new Vector2(texture.Width / 2, texture.Height / 4), NPC.scale, SpriteEffects.None, 0f);
						}
						else if(Main.rand.NextBool(2))
						{
							if (i == 0)
								Gore.NewGore(pos, NPC.velocity, ModGores.GoreType("Gores/TidalConstructGore6"), 1f);
							else if(i != -1)
								Gore.NewGore(pos, NPC.velocity, ModGores.GoreType("Gores/TidalConstructGore7"), 1f);
						}
					}
					else if(!runOnce)
					{
						Projectile projectile = Main.projectile[projectiles[j]];
						if (Projectile.type == ModContent.ProjectileType<TidalConstructTrail>() && Projectile.active && (int)Projectile.ai[1] == NPC.whoAmI)
						{
							Projectile.Center = pos;
						}
						else
						{
							projectiles[j] = Projectile.NewProjectile(NPC.Center, Vector2.Zero, ModContent.ProjectileType<TidalConstructTrail>(), 0, 0, Main.myPlayer, 0, NPC.whoAmI);
						}
					}
				}
			}
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/TidalConstruct");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/TidalConstructGlow");
			dir = NPC.rotation;
			float rotation = dir + (NPC.spriteDirection - 1) * 0.5f * -MathHelper.ToRadians(180);
			spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, rotation, new Vector2(NPC.width / 2, NPC.height / 2), NPC.scale, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			spriteBatch.Draw(texture2, NPC.Center - Main.screenPosition, NPC.frame, Color.White, rotation, new Vector2(NPC.width / 2, NPC.height / 2), NPC.scale, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			base.PostDraw(spriteBatch, drawColor);
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				Gore.NewGore(NPC.position, NPC.velocity, ModGores.GoreType("Gores/TidalConstructGore1"), 1f);
				Gore.NewGore(NPC.position, NPC.velocity, ModGores.GoreType("Gores/TidalConstructGore2"), 1f);
				Gore.NewGore(NPC.position, NPC.velocity, ModGores.GoreType("Gores/TidalConstructGore3"), 1f);
				Gore.NewGore(NPC.position, NPC.velocity, ModGores.GoreType("Gores/TidalConstructGore4"), 1f);
				Gore.NewGore(NPC.position, NPC.velocity, ModGores.GoreType("Gores/TidalConstructGore5"), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
				if (projectiles[0] != -1 && projectiles[1] != -1 && projectiles[2] != -1 && projectiles[3] != -1 && Main.netMode != NetmodeID.Server)
					DrawVines(true, true);
			}
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			int frame = frameHeight;
			if (NPC.frameCounter >= 7.5f)
			{
				NPC.frameCounter -= 7.5f;
				NPC.frame.Y += frame;
				if (NPC.frame.Y >= 12 * frame)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public int[] projectiles = new int[] { -1, -1, -1, -1 };
		bool runOnce = true;
		Vector2 aimTo = new Vector2(-1, -1);
		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center;
			if (runOnce)
			{
				projectiles = new int[] { -1, -1, -1, -1 };
				NPC.ai[0] = 90;
				runOnce = false;
				for(int i = 0; i < projectiles.Length; i++)
                {
					if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
						projectiles[i] = Projectile.NewProjectile(NPC.Center, Vector2.Zero, ModContent.ProjectileType<TidalConstructTrail>(), 0, 0, Main.myPlayer, 0, NPC.whoAmI);
						//Main.projectile[projectiles[i]].netUpdate = true;
                    }
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
					NPC.netUpdate = true;
			}
			NPC.rotation = toPlayer.ToRotation();
			if (projectiles[0] != -1 && projectiles[1] != -1 && projectiles[2] != -1 && projectiles[3] != -1 && Main.netMode != NetmodeID.Server)
				DrawVines(false);
			NPC.TargetClosest(true);
			if(aimTo.X == -1 && aimTo.Y == -1)
			{
				aimTo = NPC.Center;
				return false;
			}
			aimTo = player.Center;
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center;
			Vector2 dynamicAddition = new Vector2(0.4f, 0).RotatedBy(MathHelper.ToRadians(ai1));
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.25f / 155f, (255 - NPC.alpha) * 0.45f / 155f, (255 - NPC.alpha) * 0.45f / 155f);
			NPC.velocity.X *= 0.925f;
			NPC.velocity.Y *= 0.875f;
			if (NPC.ai[0] < 630)
			{
				NPC.ai[0]++;
				float aiMod = (NPC.ai[0] % 90) - 30;
				aiMod /= 60f;
				if(aiMod > 0)
				{
					NPC.velocity += new Vector2(1.55f * aiMod, 0).RotatedBy(NPC.rotation);
				}
            }
            else
			{
				if(NPC.ai[0] < 810)
				{
					NPC.ai[0] += 2;
				}
				else if(NPC.ai[0] > 900)
				{
					NPC.ai[0] += 2;
				}
				if (NPC.ai[0] >= 810)
				{
					Vector2 inFront = new Vector2(96, 0).RotatedBy(NPC.rotation);
					inFront += NPC.Center;
					NPC.ai[1]++;
					if (NPC.ai[1] == 10)
					{
						if (Main.netMode != 1)
						{
							int damage2 = NPC.damage / 2;
							if (Main.expertMode)
							{
								damage2 = (int)(damage2 / Main.expertDamage);
							}
							Projectile.NewProjectile(inFront, toPlayer.SafeNormalize(new Vector2(1, 0)) * 3.5f, ModContent.ProjectileType<TidalBeam>(), damage2, 5, Main.myPlayer, NPC.target, NPC.whoAmI);
						}
					}
					if (NPC.ai[1] > 450 && NPC.ai[0] <= 900)
						NPC.ai[0]++;
					if(NPC.ai[0] > 1080)
					{
						NPC.ai[0] = 0;
						NPC.ai[1] = 0;
					}
				}
			}
		}
		public override void PostAI()
		{
			Player player = Main.player[NPC.target];
			if (!player.ZoneDungeon)
			{
				NPC.velocity = Collision.TileCollision(NPC.position, NPC.velocity, NPC.width, NPC.height, true);
			}
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TidalSpirit>());	
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].localAI[1] = -1;
			if (Main.netMode != 1)
				Main.npc[n].netUpdate = true;
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height,  ModContent.ItemType<FragmentOfTide>(), Main.rand.Next(4) + 4);
		}	
	}
}