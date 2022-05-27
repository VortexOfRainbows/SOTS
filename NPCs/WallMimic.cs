using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using SOTS.Items.Void;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class WallMimic : ModNPC
	{
		private float aimToX
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float aimToY
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float eyeSpeed
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float aiCounter
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		private float direction
		{
			get => NPC.localAI[0];
			set => NPC.localAI[0] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wall Mimic");
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 150;   
            NPC.damage = 80; 
            NPC.defense = 20;  
            NPC.knockBackResist = 0f;
            NPC.width = 40;
            NPC.height = 40;
			Main.npcFrameCount[NPC.type] = 4;  
            NPC.value = 1500;
            NPC.npcSlots = 1f;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
			NPC.noGravity = true;
			Banner = NPC.type;
			BannerItem = ItemType<WallMimicBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, 25);
			Vector2 drawPos = NPC.Center - screenPos;
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, 50, 50), drawColor, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/WallMimicGlow");
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, 50, 50), Color.White, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Player player = Main.player[NPC.target];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/WallMimicEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = NPC.Center - screenPos;

			float shootToX = aimToX - NPC.Center.X;
			float shootToY = aimToY - NPC.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 1f / distance;

			shootToX *= distance * 3f;
			shootToY *= distance * 2;

			drawColor = NPC.GetAlpha(drawColor);
			drawPos.X += shootToX;
			drawPos.Y += shootToY;
			spriteBatch.Draw(texture, drawPos, null, drawColor, NPC.rotation, drawOrigin, 0.9f, SpriteEffects.None, 0f);
		}
		bool hasMoved = false;
		bool ready = false;
		bool dropSpecial = false;
		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			aimToX = NPC.Center.X - player.Center.X;
			aimToY = NPC.Center.Y - player.Center.Y;
			Vector2 rotation = new Vector2(-100, 0).RotatedBy(new Vector2(aimToX, aimToY).ToRotation()).RotatedBy(MathHelper.ToRadians(eyeSpeed));
			aimToX = rotation.X + NPC.Center.X;
			aimToY = rotation.Y + NPC.Center.Y;
			#region dust
			for(int k = 0; k < 4; k++)
            {
				Vector2 from = new Vector2(24, 0).RotatedBy(Math.PI / 2 * k);
				Dust dust = Dust.NewDustDirect(NPC.Center - new Vector2(5) + from, 0, 0, ModContent.DustType<CurseDust>());
				dust.noGravity = true;
				dust.scale = 0.9f + Main.rand.NextFloat(-0.1f, 0.1f);
				dust.velocity *= 0.45f;
				dust.velocity += from * 0.25f;
			}
			#endregion
			int i = (int)NPC.Center.X / 16;
			int j = (int)NPC.Center.Y / 16;
			Tile tile = Framing.GetTileSafely(i, j);
			bool skyAbove = false;
			if(!SOTSWall.unsafePyramidWall.Contains(tile.WallType))
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
					if (tile2.HasTile && tile2.TileType != TileID.Cloud && tile2.TileType != TileID.RainCloud && Main.tileSolid[tile2.TileType] && !Main.tileSolidTop[tile2.TileType])
					{
						skyAbove = false;
						break;
					}
					skyAbove = true;
				}
			}
			if (!SOTSWall.unsafePyramidWall.Contains(tile.WallType) && skyAbove)
			{
				dropSpecial = true;
			}
			if (dropSpecial)
			{
				eyeSpeed *= 1.04325f;
				eyeSpeed += 2;
				if(eyeSpeed >= 9000)
				{
					NPC.StrikeNPC(666676, 1, 0);
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, 666676, 1, 0, 0, 0, 0);
				}
			}
			return !dropSpecial;
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			bool lineOfSight = Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
			NPC.spriteDirection = -1;

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
						int num1 = Dust.NewDust(new Vector2(NPC.Center.X - 6 + circularRotation.X, NPC.Center.Y - 6 + circularRotation.Y), 0, 0, ModContent.DustType<CurseDust>());
						Main.dust[num1].velocity = -circularRotation * 0.2f;
						Main.dust[num1].noGravity = true;
					}
				}
			}

			if (aiCounter >= 60 && ready)
			{
				if (hasMoved && (NPC.velocity.Length() <= 1))
				{
					hasMoved = false;
					direction /= Math.Abs(direction);
					direction *= -1;
					if (aiCounter >= 63)
					{
						for (int k = 0; k < 60; k++)
						{
							Dust.NewDust(NPC.position, NPC.width, NPC.height, 32, Main.rand.Next(-7, 8), Main.rand.Next(-7, 8));
						}
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
						aiCounter = 0;
						ready = false;
					}
					NPC.velocity *= 0f;
				}
				else
				{
					if (hasMoved)
					{
						if (direction == -1 && NPC.velocity.Length() <= 13.5f)
						{
							NPC.velocity.X--;
						}
						if (direction == -2 && NPC.velocity.Length() <= 13.5f)
						{
							NPC.velocity.X++;
						}
						if (direction == 1 && NPC.velocity.Length() <= 13.5f)
						{
							NPC.velocity.Y--;
						}
						if (direction == 2 && NPC.velocity.Length() <= 13.5f)
						{
							NPC.velocity.Y++;
						}
						eyeSpeed += NPC.velocity.Length();
						eyeSpeed += 2;
						eyeSpeed *= 1.008f;
					}
					else
					{
						if (direction < 0)
						{
							if (NPC.Center.X > player.Center.X)
							{
								direction = -1;
								NPC.velocity.X -= 3;
							}
							else
							{
								direction = -2;
								NPC.velocity.X += 3;
							}
						}
						else
						{
							if (NPC.Center.Y > player.Center.Y)
							{
								direction = 1;
								NPC.velocity.Y -= 3;
							}
							else
							{
								direction = 2;
								NPC.velocity.Y += 3;
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
			NPC.frameCounter++;
			if (NPC.frameCounter >= frameSpeed) 
			{
				NPC.frameCounter -= frameSpeed;
				NPC.frame.Y += frame;
				if(NPC.frame.Y >= 4 * frame)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfEarth>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemType<SoulResidue>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemType<CursedHiveBlock>(), 1, 3, 9));
			npcLoot.Add(ItemDropRule.Common(ItemType<CursedCaviar>(), 20, 1, 1));
		}
        public override void OnKill()
		{
			if (dropSpecial || Main.rand.NextBool(200))
				Item.NewItem(NPC.GetSource_Loot("SOTS:DarkEyeDrop"), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<TheDarkEye>(), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 40.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sand, (float)(2 * hitDirection), -2f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 60; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sand, (float)(2 * hitDirection), -2f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/WallMimicGore1"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/WallMimicGore2"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/WallMimicGore3"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/WallMimicGore4"), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
			}
		}
	}
}





















