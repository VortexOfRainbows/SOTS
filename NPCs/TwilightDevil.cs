using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Planetarium;
using SOTS.Items.Planetarium.Blocks;
using SOTS.Projectiles.Planetarium;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class TwilightDevil : ModNPC
	{
		private int storeDamage = 0;
		private float tracerPosX
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float tracerPosY
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public void MoveCursorToPlayer()
		{
			float scaleFactor = 1;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[NPC.target];
			Vector2 between = new Vector2(tracerPosX, tracerPosY) - player.Center;
			float length = between.Length() + 0.1f;
			float speed = 6.5f * scaleFactor;
			if (length > 1.1f)
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
		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Frostburn,
					BuffID.OnFire,
					BuffID.Poisoned,
					BuffID.Venom
				}
			});
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 350;   
            NPC.damage = 40; 
            NPC.defense = 10;  
            NPC.knockBackResist = 0f;
            NPC.width = 50;
            NPC.height = 52;
            NPC.value = 10000;
            NPC.npcSlots = 5f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
			Banner = NPC.type;
			BannerItem = ItemType<TwilightDevilBanner>();
		}
		private int initiateHardmode = 0;
		public override bool PreAI()
		{
			if(Main.hardMode && initiateHardmode == 0)
			{
				initiateHardmode = 1;
				NPC.damage = (int)(NPC.damage * 1.25f);
			}
			if (NPC.ai[0] == 0 && tracerPosX == 0 && tracerPosY == 0)
			{
				tracerPosX = NPC.Center.X;
				tracerPosY = NPC.Center.Y;
			}
			return true;
		}
		public void createDust(int dist = 64, int dir = 1, int amt = 10, bool scaleScale = false)
		{
			float scale = 2f * ((255 - NPC.alpha) / 255);
			if(!scaleScale)
			{
				scale = 1;
			}
			for (int i = 0; i < amt; i++)
			{
				int num1 = Dust.NewDust(new Vector2(NPC.position.X - dist/2 - 2, NPC.position.Y - dist/2 - 2), NPC.width + dist, NPC.height + dist, ModContent.DustType<Dusts.AvaritianDust>(), 0, 0, 0, default, scale);
				Main.dust[num1].noGravity = true;
				float dusDisX = Main.dust[num1].position.X - NPC.Center.X;
				float dusDisY = Main.dust[num1].position.Y - NPC.Center.Y;
				//double dis = Math.Sqrt((double)(dusDisX * dusDisX + dusDisY * dusDisY))

				dusDisX *= 0.05f * dir;
				dusDisY *= 0.05f * dir;

				Main.dust[num1].velocity.X = dusDisX;
				Main.dust[num1].velocity.Y = dusDisY;
				Main.dust[num1].alpha = NPC.alpha;
			}
		}
		public override void AI()
		{
			if(Main.hardMode && NPC.life < NPC.lifeMax)
			{
				if(NPC.ai[0] % 6 == 0)
					NPC.life++;
			}
			NPC.ai[0]++;
			if (NPC.ai[0] <= 390 && NPC.ai[1] == 0)
				MoveCursorToPlayer();
			if (NPC.ai[0] == 30 && NPC.ai[1] == 0)
			{
				int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
				for(int i = 0; i < 9; i++)
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, 0, ProjectileType<TwilightDart>(), damage2, 1f, Main.myPlayer, 0, NPC.whoAmI);
			}
			if(NPC.ai[0] == 510 && NPC.ai[1] == 0) //time of release
			{
				SOTSUtils.PlaySound(SoundID.Item46, (int)(NPC.Center.X), (int)(NPC.Center.Y));
			}
			if(NPC.ai[0] >= 540 && NPC.ai[1] == 0) //teleport out
			{
				if (storeDamage == 0)
				{
					storeDamage = NPC.damage;
					NPC.damage = 0;
				}
				createDust(16, 1, 2, false);
				NPC.alpha += 2;
				if(NPC.alpha >= 255)
				{
					while (teleport()) { };
					if(Main.netMode != 1)
					{
						NPC.netUpdate = true;
					}
					NPC.ai[0] = 0;
					NPC.ai[1] = 1;
				}
			}
			if (NPC.ai[0] >= 60 && NPC.ai[1] == 1) //reappear
			{
				if (storeDamage != 0)
				{
					NPC.damage = storeDamage;
					storeDamage = 0;
				}
				createDust(64, -1, 2, false);
				NPC.alpha -= 2;
				if (NPC.alpha <= 0)
				{
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
				}
			}
			if(NPC.alpha >= 100)
			{
				NPC.dontTakeDamage = true;
			}
			else
			{
				NPC.dontTakeDamage = false;
			}
			Player player = Main.player[NPC.target];
			if(!player.dead)
			{
				NPC.timeLeft = 300;
			}
		}
		public bool teleport() // returns true while inside of blocks
		{
			Player player = Main.player[NPC.target];
			float distance = Main.rand.Next(200, 400);
			Vector2 toArea = new Vector2(distance, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
			NPC.position = toArea + player.Center - new Vector2(NPC.width / 2, NPC.height / 2);
			int i = (int)NPC.Center.X / 16;
			int j = (int)NPC.Center.Y / 16;
			bool flag = false;
			for (int k = -1; k < 2; k++)
			{
				for (int w = -1; w < 2; w++)
				{
					Tile tile = Framing.GetTileSafely(i + k, j + w);
					if (Main.tileSolid[tile.TileType] == true && tile.HasTile && Main.tileSolidTop[tile.TileType] == false)
					{
						flag = true;
					}
				}
			}
			return flag;
		}
		int frame = 0;
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			frame = frameHeight;
			if (NPC.frameCounter >= 8f) 
			{
				NPC.frameCounter -= 8f;
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
			//spawnrates manually added in SOTSNPCs.EditSpawnPool in order to avoid conflicts in hardmode
			/*Player player = spawnInfo.player;
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			bool correctBlock = spawnInfo.spawnTileType == mod.TileType("DullPlatingTile") || spawnInfo.spawnTileType == mod.TileType("PortalPlatingTile") || spawnInfo.spawnTileType == mod.TileType("AvaritianPlatingTile");
			if (modPlayer.PlanetariumBiome && correctBlock)
			{
				if (!Main.hardMode)
					return 0.04f;
				return 0.06f;
			}
			return 0;*/
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemType<TwilightGel>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemType<JarOfSouls>(), 4).OnFailedRoll(ItemDropRule.Common(ItemType<AvaritianPlating>(), 1, 4, 8)));
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < hit.Damage / (double)NPC.lifeMax * 50.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<AvaritianDust>(), (float)(2 * hit.HitDirection), -2f, 0, default, 0.5f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 40; k++)
				{
					if (k % 3 == 0)
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, (float)(2 * hit.HitDirection), -2f, 0, default, 1f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<AvaritianDust>(), (float)(2 * hit.HitDirection), -2f, 0, new Color(100, 100, 100, 250), 1f);
				}
			}
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 8);
			Vector2 drawPos = NPC.Center - screenPos;
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			texture = (Texture2D)Request<Texture2D>("SOTS/NPCs/TwilightDevilGlow");
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), NPC.GetAlpha(Color.White), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
	}
}





















