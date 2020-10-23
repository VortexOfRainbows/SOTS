using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using System;
using System.Security.AccessControl;
using Terraria;
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
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		private float tracerPosY
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		public void MoveCursorToPlayer()
		{
			float scaleFactor = 1;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[npc.target];
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
			DisplayName.SetDefault("Twilight Devil");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 350;   
            npc.damage = 40; 
            npc.defense = 10;  
            npc.knockBackResist = 0f;
            npc.width = 28;
            npc.height = 44;
			Main.npcFrameCount[npc.type] = 4;  
            npc.value = 10000;
            npc.npcSlots = 5f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.netUpdate = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			//banner = npc.type;
			//bannerItem = ItemType<BleedingGhastBanner>();
		}
		private int initiateHardmode = 0;
		public override bool PreAI()
		{
			if(Main.hardMode && initiateHardmode == 0)
			{
				initiateHardmode = 1;
				npc.damage = (int)(npc.damage * 1.25f);
			}
			if (npc.ai[0] == 0 && tracerPosX == 0 && tracerPosY == 0)
			{
				tracerPosX = npc.Center.X;
				tracerPosY = npc.Center.Y;
			}
			return true;
		}
		public void createDust(int dist = 64, int dir = 1, int amt = 10, bool scaleScale = false)
		{
			float scale = 2f * ((255 - npc.alpha) / 255);
			if(!scaleScale)
			{
				scale = 1;
			}
			for (int i = 0; i < amt; i++)
			{
				int num1 = Dust.NewDust(new Vector2(npc.position.X - dist/2 - 2, npc.position.Y - dist/2 - 2), npc.width + dist, npc.height + dist, mod.DustType("AvaritianDust"), 0, 0, 0, default, scale);
				Main.dust[num1].noGravity = true;
				float dusDisX = Main.dust[num1].position.X - npc.Center.X;
				float dusDisY = Main.dust[num1].position.Y - npc.Center.Y;
				//double dis = Math.Sqrt((double)(dusDisX * dusDisX + dusDisY * dusDisY))

				dusDisX *= 0.05f * dir;
				dusDisY *= 0.05f * dir;

				Main.dust[num1].velocity.X = dusDisX;
				Main.dust[num1].velocity.Y = dusDisY;
				Main.dust[num1].alpha = npc.alpha;
			}
		}
		public override void AI()
		{
			if(Main.hardMode && npc.life < npc.lifeMax)
			{
				if(npc.ai[0] % 6 == 0)
					npc.life++;
			}
			npc.frameCounter++;
			npc.ai[0]++;
			if (npc.ai[0] <= 390 && npc.ai[1] == 0)
				MoveCursorToPlayer();
			if (npc.ai[0] == 30 && npc.ai[1] == 0)
			{
				int damage2 = npc.damage / 2;
				if (Main.expertMode)
				{
					damage2 = (int)(damage2 / Main.expertDamage);
				}
				for(int i = 0; i < 9; i++)
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("TwilightDart"), damage2, 1f, Main.myPlayer, 0, npc.whoAmI);
			}
			if(npc.ai[0] == 510 && npc.ai[1] == 0) //time of release
			{
				Main.PlaySound(SoundID.Item46, (int)(npc.Center.X), (int)(npc.Center.Y));
			}
			if(npc.ai[0] >= 540 && npc.ai[1] == 0) //teleport out
			{
				if (storeDamage == 0)
				{
					storeDamage = npc.damage;
					npc.damage = 0;
				}
				createDust(16, 1, 2, false);
				npc.alpha += 2;
				if(npc.alpha >= 255)
				{
					while (teleport()) { };
					if(Main.netMode != 1)
					{
						npc.netUpdate = true;
					}
					npc.ai[0] = 0;
					npc.ai[1] = 1;
				}
			}
			if (npc.ai[0] >= 60 && npc.ai[1] == 1) //reappear
			{
				if (storeDamage != 0)
				{
					npc.damage = storeDamage;
					storeDamage = 0;
				}
				createDust(64, -1, 2, false);
				npc.alpha -= 2;
				if (npc.alpha <= 0)
				{
					npc.ai[0] = 0;
					npc.ai[1] = 0;
				}
			}
			if(npc.alpha >= 100)
			{
				npc.dontTakeDamage = true;
			}
			else
			{
				npc.dontTakeDamage = false;
			}
			Player player = Main.player[npc.target];
			if(!player.dead)
			{
				npc.timeLeft = 300;
			}
		}
		public bool teleport() // returns true while inside of blocks
		{
			Player player = Main.player[npc.target];
			float distance = Main.rand.Next(200, 400);
			Vector2 toArea = new Vector2(distance, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
			npc.position = toArea + player.Center - new Vector2(npc.width / 2, npc.height / 2);
			int i = (int)npc.Center.X / 16;
			int j = (int)npc.Center.Y / 16;
			bool flag = false;
			for (int k = -1; k < 2; k++)
			{
				for (int w = -1; w < 2; w++)
				{
					Tile tile = Framing.GetTileSafely(i + k, j + w);
					if (Main.tileSolid[tile.type] == true && tile.active() && Main.tileSolidTop[tile.type] == false)
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
			frame = frameHeight;
			if (npc.frameCounter >= 5f) 
			{
				npc.frameCounter -= 5f;
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
			//spawnrates manually added in SOTSNPCs.EditSpawnPool in order to avoid conflicts in hardmode
			Player player = spawnInfo.player;
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			bool correctBlock = spawnInfo.spawnTileType == mod.TileType("DullPlatingTile") || spawnInfo.spawnTileType == mod.TileType("PortalPlatingTile") || spawnInfo.spawnTileType == mod.TileType("AvaritianPlatingTile");
			if (modPlayer.PlanetariumBiome && correctBlock)
			{
				if (!Main.hardMode)
					return 0.04f;
				return 0.06f;
			}
			return 0;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(3) == 0 && SOTSWorld.downedAdvisor) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwilightShard"), 1);

			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwilightGel"), Main.rand.Next(2) + 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("JarOfSouls"), 1);
			if(Main.rand.Next(20) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwilightBeads"), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 50.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("AvaritianDust"), (float)(2 * hitDirection), -2f, 0, default, 0.5f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 40; k++)
				{
					if (k % 3 == 0)
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, (float)(2 * hitDirection), -2f, 0, default, 1f);
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("AvaritianDust"), (float)(2 * hitDirection), -2f, 0, new Color(100, 100, 100, 250), 1f);
				}
			}		
		}
	}
}





















