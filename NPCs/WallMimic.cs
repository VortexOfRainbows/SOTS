using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class WallMimic : ModNPC
	{
		private float aimToX
		{
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}
		private float aimToY
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}
		private float eyeSpeed
		{
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		private float aiCounter
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		private float direction
		{
			get => npc.localAI[0];
			set => npc.localAI[0] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wall Mimic");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 150;   
            npc.damage = 80; 
            npc.defense = 20;  
            npc.knockBackResist = 0f;
            npc.width = 40;
            npc.height = 40;
			Main.npcFrameCount[npc.type] = 4;  
            npc.value = 1500;
            npc.npcSlots = 1f;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.netAlways = true;
			npc.noGravity = true;
			banner = npc.type;
			bannerItem = ItemType<WallMimicBanner>();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, 25);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, npc.frame.Y, 50, 50), drawColor, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[npc.target];
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/WallMimicEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;

			float shootToX = aimToX - npc.Center.X;
			float shootToY = aimToY - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 1f / distance;

			shootToX *= distance * 4;
			shootToY *= distance * 4;

			drawColor = npc.GetAlpha(drawColor);
			drawPos.X += shootToX;
			drawPos.Y += shootToY;
			spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
		}
		bool hasMoved = false;
		bool ready = false;
		bool dropSpecial = false;
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			aimToX = npc.Center.X - player.Center.X;
			aimToY = npc.Center.Y - player.Center.Y;
			Vector2 rotation = new Vector2(-100, 0).RotatedBy(new Vector2(aimToX, aimToY).ToRotation()).RotatedBy(MathHelper.ToRadians(eyeSpeed));
			aimToX = rotation.X + npc.Center.X;
			aimToY = rotation.Y + npc.Center.Y;
			#region dust
			int num1 = Dust.NewDust(new Vector2(npc.Center.X - 6, npc.position.Y - 6), 0, 0, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity.Y = -7f;
			num1 = Dust.NewDust(new Vector2(npc.Center.X - 6, npc.position.Y + npc.height - 6), 0, 0, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity.Y = 7f;
			num1 = Dust.NewDust(new Vector2(npc.position.X - 6, npc.Center.Y - 6), 0, 0, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity.X = -7f;
			num1 = Dust.NewDust(new Vector2(npc.position.X - 6 + npc.width, npc.Center.Y - 6), 0, 0, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity.X = 7f;
			#endregion


			int i = (int)npc.Center.X / 16;
			int j = (int)npc.Center.Y / 16;
			Tile tile = Framing.GetTileSafely(i, j);
			bool skyAbove = false;
			if(tile.wall != mod.WallType("PyramidWallTile"))
			{
				for (int k = 0; k < 300; k++)
				{
					j--;
					if (j <= 20)
					{
						skyAbove = true;
						break;
					}
					Tile tile2 = Framing.GetTileSafely(i, j);
					if (tile2.active() && tile2.type != TileID.Cloud && tile2.type != TileID.RainCloud && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type])
					{
						skyAbove = false;
						break;
					}
					skyAbove = true;
				}
			}
			if (tile.wall != mod.WallType("PyramidWallTile") && skyAbove)
			{
				dropSpecial = true;
			}
			if (dropSpecial)
			{
				eyeSpeed *= 1.04325f;
				eyeSpeed += 2;
				if(eyeSpeed >= 9000)
				{
					npc.StrikeNPC(666676, 1, 0);
				}
			}
			return !dropSpecial;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			bool lineOfSight = Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height);
			npc.spriteDirection = -1;

			if (aiCounter != 30)
			{
				if(eyeSpeed == 0 || aiCounter >= 60)
				{
					aiCounter++;
				}
				else if(aiCounter == 0)
				{
					eyeSpeed *= 0.97f;
					eyeSpeed--;
					if(eyeSpeed <= 0)
					{
						eyeSpeed = 0;
					}
				}
			}
			else
			{
				if (lineOfSight)
				{
					aiCounter++;
					ready = true;
					for(int i = 0; i < 360; i += 20)
					{
						Vector2 circularRotation = new Vector2(-48, 0).RotatedBy(MathHelper.ToRadians(i));
						int num1 = Dust.NewDust(new Vector2(npc.Center.X - 6 + circularRotation.X, npc.Center.Y - 6 + circularRotation.Y), 0, 0, mod.DustType("CurseDust"));
						Main.dust[num1].velocity = -circularRotation * 0.2f;
						Main.dust[num1].noGravity = true;
					}
				}
			}

			if (aiCounter >= 60 && ready)
			{
				if (hasMoved && (npc.velocity.Length() <= 1))
				{
					hasMoved = false;
					direction /= Math.Abs(direction);
					direction *= -1;
					if (aiCounter >= 63)
					{
						for (int k = 0; k < 60; k++)
						{
							Dust.NewDust(npc.position, npc.width, npc.height, 32, Main.rand.Next(-7, 8), Main.rand.Next(-7, 8));
						}
						Main.PlaySound(SoundID.Item14, npc.Center);
						aiCounter = 0;
						ready = false;
					}
					npc.velocity *= 0f;
				}
				else
				{
					if (hasMoved)
					{
						if (direction == -1 && npc.velocity.Length() <= 13.5f)
						{
							npc.velocity.X--;
						}
						if (direction == -2 && npc.velocity.Length() <= 13.5f)
						{
							npc.velocity.X++;
						}
						if (direction == 1 && npc.velocity.Length() <= 13.5f)
						{
							npc.velocity.Y--;
						}
						if (direction == 2 && npc.velocity.Length() <= 13.5f)
						{
							npc.velocity.Y++;
						}
						eyeSpeed += npc.velocity.Length();
						eyeSpeed += 2;
						eyeSpeed *= 1.008f;
					}
					else
					{
						if (direction < 0)
						{
							if (npc.Center.X > player.Center.X)
							{
								direction = -1;
								npc.velocity.X -= 3;
							}
							else
							{
								direction = -2;
								npc.velocity.X += 3;
							}
						}
						else
						{
							if (npc.Center.Y > player.Center.Y)
							{
								direction = 1;
								npc.velocity.Y -= 3;
							}
							else
							{
								direction = 2;
								npc.velocity.Y += 3;
							}
						}
						hasMoved = true;
					}
				}
			}
			else if (aiCounter >= 60)
			{
				aiCounter = 0;
			}
		}
        int frame = 0;
		public override void FindFrame(int frameHeight) 
		{
			frame = frameHeight;
			float frameSpeed = 8f;
			npc.frameCounter++;
			if (npc.frameCounter >= frameSpeed) 
			{
				npc.frameCounter -= frameSpeed;
				npc.frame.Y += frame;
				if(npc.frame.Y >= 4 * frame)
				{
					npc.frame.Y = 0;
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfEarth"), Main.rand.Next(2) + 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SoulResidue"), Main.rand.Next(2) + 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedHiveBlock"), Main.rand.Next(7) + 3);
			if(dropSpecial || Main.rand.Next(200) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TheDarkEye"), 1);
			if(Main.rand.Next(20) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedCaviar"), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 40.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 32, (float)(2 * hitDirection), -2f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 60; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 32, (float)(2 * hitDirection), -2f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/WallMimicGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/WallMimicGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/WallMimicGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/WallMimicGore4"), 1f);

				for (int i = 0; i < 9; i++)
					Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61, 64), 1f);
			}
		}
	}
}





















