using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class HoloSlime : ModNPC
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(justWarped);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			justWarped = reader.ReadInt32();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holo Slime");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			npc.aiStyle = 1;
            npc.lifeMax = 50;  
            npc.damage = 22; 
            npc.defense = 10;  
            npc.knockBackResist = 1.25f;
            npc.width = 36;
            npc.height = 32;
            animationType = NPCID.BlueSlime;
			Main.npcFrameCount[npc.type] = 2;  
            npc.value = 200;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.netAlways = true;
			npc.alpha = 40;
			npc.HitSound = SoundID.NPCHit53;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			//npc.DeathSound = SoundID.NPCDeath14;
			//banner = npc.type;
			//bannerItem = ItemType<FrozenTreasureSlimeBanner>();
		}
		public bool AirBelow(int i, int j, int dist)
		{
			bool flag = false;
			for(int k = 0; k < dist; k++)
			{
				Tile tile = Framing.GetTileSafely(i, j + k);
				if(tile.active() && Main.tileSolid[tile.type])
				{
					flag = true;
					break;
				}
			}
			return !flag;
		}
		public Vector2? findSolidGround(int width, int height, int direction = 1)
		{
			if(direction == 0)
			{
				return null;
			}
			int i = (int)npc.Center.X / 16;
			int valid = 0;
			int j = (int)npc.Center.Y / 16;
			for (int k = 0; k < width; k++)
			{
				j = (int)npc.Center.Y / 16;
				i += direction;
				for (int h = 0; h < height && h < k + 10; h++)
				{
					j -= 1;
					if(AirBelow(i, j, 3) && !AirBelow(i, j + 3, 1))
					{
						valid++;
						return new Vector2(i * 16, j * 16) + new Vector2(8, 8);
					}
				}
			}
			return null;
		}
		public void WarpToPlayer()
		{
			Player player = Main.player[npc.target];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			float distX = npc.Center.X - player.Center.X;
			bool left = distX > 0;
			float distY = npc.Center.Y - (player.Center.Y - 96);
			Vector2 toPlayer = new Vector2(-distX, -distY);
			float length = toPlayer.Length() + 0.1f;
			Vector2 checkPos = npc.Center;
			if(length >= 1.1f)
			{
				int valid = 0;
				int autoOverride = 0;
				int distance = 8;
				toPlayer.Normalize();
				toPlayer *= distance;
				for (int w = 0; w < length; w += distance)
				{
					autoOverride += 4;
					checkPos += toPlayer;

					int dust = Dust.NewDust(checkPos, 0, 0, DustID.Electric, 0, 0, npc.alpha, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.1f;
					Vector2 circularPosition = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + autoOverride * 2));
					dust = Dust.NewDust(checkPos + circularPosition, 0, 0, DustID.Electric, 0, 0, npc.alpha, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.1f;

					npc.position = checkPos - new Vector2(npc.width / 2, npc.height / 2);
					if (valid >= 5 || (w >= 30 * distance && !AirBelow((int)checkPos.X / 16, (int)checkPos.Y / 16 + 6, 6) && AirBelow((int)checkPos.X / 16, (int)checkPos.Y / 16 - 3, 5)))
					{
						break;
					}
				}
				npc.velocity += new Vector2(toPlayer.X, toPlayer.Y * 0.5f);
				for (int i = 0; i < 20; i++)
				{
					int dust = Dust.NewDust(checkPos, 0, 0, DustID.Electric, 0, 0, npc.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
				}
			}
		}
		public void WarpToLocation(Vector2 to)
		{
			Player player = Main.player[npc.target];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			float distX = npc.Center.X - to.X;
			bool left = distX > 0;
			float distY = npc.Center.Y - to.Y;
			Vector2 toPlayer = new Vector2(-distX, -distY);
			float length = toPlayer.Length() + 0.1f;
			Vector2 checkPos = npc.Center;
			if (length >= 1.1f)
			{
				int valid = 0;
				int autoOverride = 0;
				int distance = 8;
				toPlayer.Normalize();
				toPlayer *= distance;
				for (int w = 0; w < length; w += distance)
				{
					autoOverride += 4;
					checkPos += toPlayer;

					int dust = Dust.NewDust(checkPos, 0, 0, DustID.Electric, 0, 0, npc.alpha, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.1f;
					Vector2 circularPosition = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + autoOverride * 2));
					dust = Dust.NewDust(checkPos + circularPosition, 0, 0, DustID.Electric, 0, 0, npc.alpha, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.1f;

					npc.position = checkPos - new Vector2(npc.width / 2, npc.height / 2);
				}
				npc.velocity += new Vector2(toPlayer.X, toPlayer.Y * 0.5f);
				for (int i = 0; i < 20; i++)
				{
					int dust = Dust.NewDust(checkPos, 0, 0, DustID.Electric, 0, 0, npc.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
				}
			}
		}
		int justWarped = 0;
		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			if (!player.dead)
			{
				npc.timeLeft = 300;
			}
			int i = (int)npc.Center.X / 16;
			int j = (int)npc.Center.Y / 16;
			if(AirBelow(i, j, 36) && npc.velocity.Y != 0f && justWarped <= 0 && npc.Center.Y > player.Center.Y)
			{
				int direction = npc.velocity.X > 0f ? 1 : npc.velocity.X < 0f ? -1 : 0;
				Vector2? travelTo = findSolidGround(80, 40, direction);
				if(travelTo != null)
				{
					WarpToLocation((Vector2)travelTo);
					justWarped = 60;
					if(Main.netMode != 1)
					{
						npc.netUpdate = true;
					}
					Main.PlaySound(SoundID.Item8, npc.Center);
				}
				if(direction == 0)
				{
					WarpToPlayer();
					justWarped = 60;
					if (Main.netMode != 1)
					{
						npc.netUpdate = true;
					}
					Main.PlaySound(SoundID.Item8, npc.Center);
				}
			}
			justWarped--;
			if(justWarped <= -240)
			{
				WarpToPlayer();
				justWarped = 120;
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				Main.PlaySound(SoundID.Item8, npc.Center);
			}
			npc.velocity.Y *= 0.96f;

			npc.ai[0]++; //speed up jumping speed
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = mod.GetTexture("NPCs/HoloSlimeOutline");
			Texture2D texture2 = mod.GetTexture("NPCs/HoloSlimeFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f);
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.25f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.075f;
				float y = Main.rand.Next(-10, 11) * 0.075f;

				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X), (float)(npc.Center.Y - (int)Main.screenPosition.Y) + 2), new Rectangle(0, npc.frame.Y, npc.width, npc.height), color * ((255 - npc.alpha) / 255f) * 0.5f, 0f, drawOrigin2, npc.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y + 2), new Rectangle(0, npc.frame.Y, npc.width, npc.height), color * ((255 - npc.alpha) / 255f), 0f, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override bool PreAI()
		{
			//if(initiateSize == 1)
			//{
			//	initiateSize = -1;
				//	npc.scale = 1.3f;
				//	npc.width = (int)(npc.width * npc.scale);
				//	npc.height = (int)(npc.height * npc.scale);
			//}
			return true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
			//spawnrates manually added in SOTSNPCs.EditSpawnPool in order to avoid conflicts in hardmode
			Player player = spawnInfo.player;
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			bool correctBlock = spawnInfo.spawnTileType == mod.TileType("DullPlatingTile") || spawnInfo.spawnTileType == mod.TileType("PortalPlatingTile") || spawnInfo.spawnTileType == mod.TileType("AvaritianPlatingTile");
			if (modPlayer.PlanetariumBiome && correctBlock)
			{
				return 0.4f;
			}
			return 0;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 70.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, (float)hitDirection, -1f, npc.alpha, default, 0.65f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 36; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, (float)(2 * hitDirection), -2f, npc.alpha, default, 1f);
				}
			}
		}
		public override void NPCLoot()
		{
		}
	}
}