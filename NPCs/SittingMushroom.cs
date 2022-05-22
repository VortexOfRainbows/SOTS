using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class SittingMushroom : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Sitting Mushroom");
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 40;   
            NPC.damage = 20; 
            NPC.defense = 6;  
            NPC.knockBackResist = 0.1f;
            NPC.width = 30;
            NPC.height = 30;
			Main.npcFrameCount[NPC.type] = 8;  
            NPC.value = 125;
            NPC.npcSlots = 1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath16;
            NPC.netAlways = true;
			Banner = NPC.type;
			BannerItem = ItemType<SittingMushroomBanner>();
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.velocity.X *= 0.9f;
			if(Main.rand.Next(40) == 0)
			{
				Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), 28, 10, 41, 0, -2f, 250, new Color(100, 100, 100, 250), 0.8f);
			}
			NPC.ai[0]++;
			if(NPC.ai[0] >= 180)
			{
				Vector2 distance = NPC.Center - player.Center;
				if(distance.Length() <= 150 || NPC.ai[1] >= 1)
				{
					NPC.ai[1]++;
				}
			}
			if(NPC.ai[1] == 1)
			{
				NPC.velocity.Y -= 7f;
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
			}
			if((int)NPC.ai[1] % 7 == 0 && NPC.ai[1] >= 7)
			{
				Vector2 distance = NPC.Center - player.Center;
				distance = distance.SafeNormalize(Vector2.Zero);
				distance *= -8f;
				int damage2 = NPC.damage / 2;
				if (Main.expertMode)
				{
					damage2 = (int)(damage2 / Main.expertDamage);
				}
				if(Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, distance.X, distance.Y, Mod.Find<ModProjectile>("SporeCloud").Type, damage2, 2f, Main.myPlayer);
				Terraria.Audio.SoundEngine.PlaySound(2, NPC.Center, 34);
			}
			if (NPC.ai[1] >= 42)
			{
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
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
				if(NPC.frame.Y >= 8 * frame)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.Player.ZoneGlowshroom || spawnInfo.spawnTileType == TileID.MushroomGrass)
			{
				if(!Main.hardMode)
					return 0.2f;
				return 0.1f;
			}
			if (spawnInfo.Player.ZoneRockLayerHeight)
			{
				return 0.005f;
			}
			return 0;
		}
		public override void NPCLoot()
		{
			if(NPC.HasBuff(BuffID.OnFire) || Main.rand.Next(10) == 0)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("CookedMushroom").Type, Main.rand.Next(2) + 3);
			}
			else
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.GlowingMushroom, Main.rand.Next(6) + 7);
			}
			if(Main.rand.Next(2) == 0 || Main.expertMode)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("FragmentOfEarth").Type, Main.rand.Next(2) + 1);

			if(Main.rand.Next(100) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("SporeSprayer").Type, 1);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 20.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 41, (float)(2 * hitDirection), -2f, 250, new Color(100, 100, 100, 250), 0.8f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 10; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 41, (float)(2 * hitDirection), -2f, 250, new Color(100, 100, 100, 250), 0.8f);
				}
				for (int k = 0; k < 7; k++)
				{
					Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
					int damage2 = NPC.damage / 2;
					if (Main.expertMode)
					{
						damage2 = (int)(damage2 / Main.expertDamage);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, circularLocation.X, circularLocation.Y, Mod.Find<ModProjectile>("SporeCloud").Type, damage2, 2f, Main.myPlayer);
				}
				Terraria.Audio.SoundEngine.PlaySound(2, NPC.Center, 34);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/SittingMushroomGore1"), 1f);
			}
		}
	}
}





















