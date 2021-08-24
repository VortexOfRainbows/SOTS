using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using SOTS.Items.GelGear.Furniture;
using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class HoloEye : ModNPC
	{
		private float tracerPosX
		{
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		private float tracerPosY
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(rotateCursor);
			writer.Write(lookAtPos.X);
			writer.Write(lookAtPos.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			rotateCursor = reader.ReadSingle();
			lookAtPos.X = reader.ReadSingle();
			lookAtPos.Y = reader.ReadSingle();
		}
		private Vector2 lookAtPos = new Vector2(-1, -1);
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Holo Eye");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 100;   
            npc.damage = 42; 
            npc.defense = 24;  
            npc.knockBackResist = 0f;
            npc.width = 36;
            npc.height = 46;
			Main.npcFrameCount[npc.type] = 1;  
            npc.value = 275;
            npc.npcSlots = 2.5f;
            npc.HitSound = SoundID.NPCHit53;
            npc.DeathSound = SoundID.NPCDeath14;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			banner = npc.type;
			bannerItem = ItemType<HoloEyeBanner>();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = mod.GetTexture("NPCs/HoloEyeBase");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X), (float)(npc.Center.Y - (int)Main.screenPosition.Y) + 18);
			Main.spriteBatch.Draw(texture, drawPos, null, drawColor * ((255 - npc.alpha) / 255f), 0f, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			Color color = new Color(100, 100, 100, 0); 
			texture = mod.GetTexture("NPCs/HoloEyeBaseGlow");
			for (int k = 0; k < 5; k++)
			{
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.25f * k;
				Main.spriteBatch.Draw(texture, drawPos + offset, null, color * 0.66f, 0f, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = mod.GetTexture("NPCs/HoloEyeOutline");
			Texture2D texture2 = mod.GetTexture("NPCs/HoloEyeReticle");
			Texture2D texture3 = mod.GetTexture("NPCs/HoloEyePupil");
			Texture2D texture4 = mod.GetTexture("NPCs/HoloEyeFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 drawOrigin3 = new Vector2(texture3.Width * 0.5f, texture3.Height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Vector2 between = new Vector2(lookAtPos.X, lookAtPos.Y) - npc.Center;
				if(between.Length() > 1.1f)
				{
					between.Normalize();
				}
				else
				{
					between = Vector2.Zero;
				}

				if (k == 0)
					Main.spriteBatch.Draw(texture4, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X), (float)(npc.Center.Y - (int)Main.screenPosition.Y) - 4), null, color * 0.5f, 0f, drawOrigin, npc.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y - 4), null, color * ((255 - npc.alpha) / 255f), 0f, drawOrigin, npc.scale, SpriteEffects.None, 0f);

				float scaleFactor = counter / 180f;
				scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
				if (lookAtPos.X != -1 && lookAtPos.Y != -1)
				{
					Main.spriteBatch.Draw(texture3, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y - 4) + between * (6 + -14 * npc.ai[1]), null, color * ((255 - npc.alpha) / 255f), 0f, drawOrigin3, 0.5f + npc.scale - npc.ai[1], SpriteEffects.None, 0f);
					Main.spriteBatch.Draw(texture2, new Vector2((float)(tracerPosX - (int)Main.screenPosition.X) + x, (float)(tracerPosY - (int)Main.screenPosition.Y) + y), null, color * scaleFactor, tracerXVelo * 0.04f, drawOrigin2, scaleFactor, SpriteEffects.None, 0f);
				}
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		float tracerXVelo = 0;
		public void MoveCursorToPlayer()
		{
			float scaleFactor = counter / 360f;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[npc.target];
			Vector2 between = new Vector2(tracerPosX, tracerPosY) - player.Center;
			float length = between.Length() + 0.1f;
			float speed = 6.5f * scaleFactor;
			if(length > 1.1f)
			{
				between.Normalize();
				if (length > speed)
				{
					length = speed;
				}
				between *= -length;
				tracerPosX += between.X;
				tracerPosY += between.Y;
			}
		}
		public void MoveEyesToCursor()
		{
			float scaleFactor = counter / 360f;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[npc.target];
			Vector2 between = new Vector2(tracerPosX, tracerPosY) - lookAtPos;
			float length = between.Length() + 0.1f;
			float speed = 5f * scaleFactor;
			if (length > 1.1f)
			{
				between.Normalize();
				if (length > speed)
				{
					length = speed;
				}
				between *= length;
				lookAtPos.X += between.X;
				lookAtPos.Y += between.Y;
			}
		}
		int counter = 0;
		float rotateCursor = 0;
		bool hasSeenPlayer = false;
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			float distanceX = player.Center.X - npc.Center.X;
			float distanceY = player.Center.Y - npc.Center.Y;
			distanceX = Math.Abs(distanceX);
			distanceY = Math.Abs(distanceY);
			if (distanceX < 800 && distanceY < 400)
			{
				hasSeenPlayer = true;
			}
			if(!hasSeenPlayer)
			{
				if (npc.ai[0] == 0 && tracerPosX == 0 && tracerPosY == 0)
				{
					tracerPosX = npc.Center.X;
					tracerPosY = npc.Center.Y;
					lookAtPos = new Vector2(tracerPosX, tracerPosY);
				}
			}
			return hasSeenPlayer;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			if (rotateCursor < 0)
			{
				rotateCursor = MathHelper.ToRadians(360) + rotateCursor;
			}
			while (rotateCursor > MathHelper.ToRadians(360))
			{
				rotateCursor -= MathHelper.ToRadians(360);
			}
			tracerXVelo = tracerPosX;
			counter++;
			if (npc.ai[0] <= 180)
			{
				if(npc.ai[0] == 0 && tracerPosX == 0 && tracerPosY == 0)
				{
					tracerPosX = npc.Center.X;
					tracerPosY = npc.Center.Y;
					lookAtPos = new Vector2(tracerPosX, tracerPosY);
				}
				MoveCursorToPlayer();
				MoveEyesToCursor();
			}
			tracerXVelo = tracerPosX - tracerXVelo;
			if (npc.ai[0] == 180)
			{
				Vector2 between = lookAtPos - npc.Center;
				rotateCursor = between.ToRotation();
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.netUpdate = true;
				}
			}
			if (npc.ai[0] > 180 && npc.ai[0] < 240)
			{
				lookAtPos = new Vector2(tracerPosX, tracerPosY);
				Vector2 between = lookAtPos - npc.Center;
				between = new Vector2(120, 0).RotatedBy(rotateCursor);
				lookAtPos = npc.Center + between;
				if (rotateCursor != MathHelper.ToRadians(270))
				{
					if (rotateCursor > MathHelper.ToRadians(272) || rotateCursor < MathHelper.ToRadians(90))
					{
						rotateCursor -= MathHelper.ToRadians(2.5f);
					}
					else if (rotateCursor < MathHelper.ToRadians(268))
					{
						rotateCursor += MathHelper.ToRadians(2.5f);
					}
					else //trigger effects her ig
					{
						rotateCursor = MathHelper.ToRadians(270);
						npc.ai[0] = 240;
					}
				}
			}
			else
			{
				npc.ai[0]++;
			}
			if (npc.ai[1] > 0)
			{
				npc.ai[1] -= 0.05f;
			}
			if (npc.ai[1] < 0)
			{
				npc.ai[1] = 0;
			}
			if (npc.ai[0] >= 240 && npc.ai[0] <= 360)
			{
				int damage2 = npc.damage / 2;
				if (Main.expertMode)
				{
					damage2 = (int)(damage2 / Main.expertDamage);
				}
					
				if(npc.ai[0] % 11 == 0)
				{
					Vector2 between = lookAtPos - npc.Center;
					between.Normalize();

					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center.X + between.X * 24, npc.Center.Y + between.Y * 24, 0, -1.666f, mod.ProjectileType("FallingBolt"), damage2, 1f, Main.myPlayer, tracerPosX + Main.rand.Next(-14, 15), tracerPosY + Main.rand.Next(-14, 15));

					Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 92, 0.5f);
					npc.ai[1] = 1;
				}
			}
			if (npc.ai[0] >= 440)
			{
				npc.ai[0] = 0;
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
			//spawnrates manually added in SOTSNPCs.EditSpawnPool in order to avoid conflicts in hardmode
			Player player = spawnInfo.player;
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			bool correctBlock = spawnInfo.spawnTileType == mod.TileType("DullPlatingTile") || spawnInfo.spawnTileType == mod.TileType("PortalPlatingTile") || spawnInfo.spawnTileType == mod.TileType("AvaritianPlatingTile");
			//correctBlock = true;
			if (modPlayer.PlanetariumBiome && correctBlock)
			{
				return 0.1f;
			}
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwilightGel"), Main.rand.Next(2) + 1);

			if (Main.rand.Next(5) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfOtherworld"), 1);

			if (Main.rand.Next(5) == 0 && SOTSWorld.downedAdvisor) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwilightShard"), 1);
				//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SporeSprayer"), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 40.0)
				{
					float scale = 1f;
					int type = DustID.Electric;
					if (Main.rand.NextBool(3))
					{
						type = ModContent.DustType<Dusts.CodeDust2>();
						scale = 2f;
					}
					Dust.NewDust(npc.position, npc.width, npc.height, type, (float)(2 * hitDirection), -2f, 0, default, 0.6f * scale);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					float scale = 1f;
					int type = DustID.Electric;
					if (Main.rand.NextBool(3))
					{
						type = ModContent.DustType<Dusts.CodeDust2>();
						scale = 2f;
					}
					Dust.NewDust(npc.position, npc.width, npc.height, type, (float)(2 * hitDirection), -2f, 0, default, scale);
					if (k % 2 == 0)
						Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("AvaritianDust"), (float)(2 * hitDirection), -2f, 0, new Color(100, 100, 100, 250), 1f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HoloEyeGore1"), 1f);
			}
		}
	}
}