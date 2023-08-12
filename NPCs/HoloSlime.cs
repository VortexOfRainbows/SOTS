using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
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
			NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Poisoned,
					BuffID.Frostburn,
					BuffID.Ichor,
					BuffID.Venom,
					BuffID.OnFire
				}
			});
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			NPC.aiStyle = 1;
            NPC.lifeMax = 50;  
            NPC.damage = 22; 
            NPC.defense = 10;  
            NPC.knockBackResist = 1.25f;
            NPC.width = 36;
            NPC.height = 32;
            AnimationType = NPCID.BlueSlime;
			Main.npcFrameCount[NPC.type] = 2;  
            NPC.value = 200;
            NPC.npcSlots = 1.25f;
            NPC.lavaImmune = true;
            NPC.netAlways = true;
			NPC.alpha = 40;
			NPC.HitSound = SoundID.NPCHit53;
			NPC.DeathSound = SoundID.NPCDeath1;
			//npc.DeathSound = SoundID.NPCDeath14;
			Banner = NPC.type;
			BannerItem = ItemType<HoloSlimeBanner>();
		}
		public bool AirBelow(int i, int j, int dist)
		{
			bool flag = false;
			for(int k = 0; k < dist; k++)
			{
				Tile tile = Framing.GetTileSafely(i, j + k);
				if(tile.HasTile && Main.tileSolid[tile.TileType])
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
			int i = (int)NPC.Center.X / 16;
			int valid = 0;
			int j = (int)NPC.Center.Y / 16;
			for (int k = 0; k < width; k++)
			{
				j = (int)NPC.Center.Y / 16;
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
			Player player = Main.player[NPC.target];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float distX = NPC.Center.X - player.Center.X;
			bool left = distX > 0;
			float distY = NPC.Center.Y - (player.Center.Y - 112);
			Vector2 toPlayer = new Vector2(-distX, -distY);
			float length = toPlayer.Length() + 0.1f;
			Vector2 checkPos = NPC.Center;
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

					int dust = Dust.NewDust(checkPos, 0, 0, DustID.Electric, 0, 0, NPC.alpha, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.1f;
					Vector2 circularPosition = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + autoOverride * 2));
					dust = Dust.NewDust(checkPos + circularPosition, 0, 0, DustID.Electric, 0, 0, NPC.alpha, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.1f;

					NPC.position = checkPos - new Vector2(NPC.width / 2, NPC.height / 2);
					if (valid >= 5 || (w >= 30 * distance && !AirBelow((int)checkPos.X / 16, (int)checkPos.Y / 16 + 6, 6) && AirBelow((int)checkPos.X / 16, (int)checkPos.Y / 16 - 3, 5)))
					{
						break;
					}
				}
				NPC.velocity += new Vector2(toPlayer.X, toPlayer.Y * 0.5f);
				for (int i = 0; i < 20; i++)
				{
					int dust = Dust.NewDust(checkPos, 0, 0, DustID.Electric, 0, 0, NPC.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
				}
			}
		}
		public void WarpToLocation(Vector2 to)
		{
			Player player = Main.player[NPC.target];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float distX = NPC.Center.X - to.X;
			bool left = distX > 0;
			float distY = NPC.Center.Y - to.Y;
			Vector2 toPlayer = new Vector2(-distX, -distY);
			float length = toPlayer.Length() + 0.1f;
			Vector2 checkPos = NPC.Center;
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

					int dust = Dust.NewDust(checkPos, 0, 0, DustID.Electric, 0, 0, NPC.alpha, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.1f;
					Vector2 circularPosition = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + autoOverride * 2));
					dust = Dust.NewDust(checkPos + circularPosition, 0, 0, DustID.Electric, 0, 0, NPC.alpha, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.1f;

					NPC.position = checkPos - new Vector2(NPC.width / 2, NPC.height / 2);
				}
				NPC.velocity += new Vector2(toPlayer.X, toPlayer.Y * 0.5f);
				for (int i = 0; i < 20; i++)
				{
					int dust = Dust.NewDust(checkPos, 0, 0, DustID.Electric, 0, 0, NPC.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
				}
			}
		}
		int justWarped = 0;
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			if (!player.dead)
			{
				NPC.timeLeft = 300;
			}
			int i = (int)NPC.Center.X / 16;
			int j = (int)NPC.Center.Y / 16;
			if(AirBelow(i, j, 36) && NPC.velocity.Y != 0f && justWarped <= 0 && NPC.Center.Y > player.Center.Y)
			{
				int direction = NPC.velocity.X > 0f ? 1 : NPC.velocity.X < 0f ? -1 : 0;
				Vector2? travelTo = findSolidGround(80, 40, direction);
				if(travelTo != null)
				{
					WarpToLocation((Vector2)travelTo);
					justWarped = 60;
					if(Main.netMode != 1)
					{
						NPC.netUpdate = true;
					}
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
				}
				if(direction == 0)
				{
					WarpToPlayer();
					justWarped = 60;
					if (Main.netMode != 1)
					{
						NPC.netUpdate = true;
					}
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
				}
			}
			justWarped--;
			if(justWarped <= -240)
			{
				WarpToPlayer();
				justWarped = 120;
				if (Main.netMode != 1)
				{
					NPC.netUpdate = true;
				}
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
			}
			NPC.velocity.Y *= 0.96f;

			NPC.ai[0]++; //speed up jumping speed
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/HoloSlimeOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/HoloSlimeFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f);
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.25f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.075f;
				float y = Main.rand.Next(-10, 11) * 0.075f;
				if (k == 0)
					spriteBatch.Draw(texture2, new Vector2((float)(NPC.Center.X - (int)screenPos.X), (float)(NPC.Center.Y - (int)screenPos.Y) + 2), new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), color * ((255 - NPC.alpha) / 255f) * 0.5f, 0f, drawOrigin2, NPC.scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, new Vector2((float)(NPC.Center.X - (int)screenPos.X) + x, (float)(NPC.Center.Y - (int)screenPos.Y) + y + 2), new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), color * ((255 - NPC.alpha) / 255f), 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 70.0)
				{
					float scale = 1f;
					int type = DustID.Electric;
					if (Main.rand.NextBool(3))
					{
						type = ModContent.DustType<Dusts.CodeDust2>();
						scale = 2f;
					}
					Dust.NewDust(NPC.position, NPC.width, NPC.height, type, (float)(2 * hitDirection), -2f, 0, default, 0.65f * scale);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 36; k++)
				{
					float scale = 1f;
					int type = DustID.Electric;
					if (Main.rand.NextBool(3))
					{
						type = ModContent.DustType<Dusts.CodeDust2>();
						scale = 2f;
					}
					Dust.NewDust(NPC.position, NPC.width, NPC.height, type, (float)(2 * hitDirection), -2f, 0, default, scale);
				}
			}
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			LeadingConditionRule postAdvisor = new LeadingConditionRule(new Common.ItemDropConditions.DownedAdvisorDropCondition());
			postAdvisor.OnSuccess(ItemDropRule.Common(ItemType<TwilightShard>(), 20));
			npcLoot.Add(postAdvisor);
			npcLoot.Add(ItemDropRule.Common(ItemType<TwilightGel>(), 1, 1, 2));
		}
	}
}