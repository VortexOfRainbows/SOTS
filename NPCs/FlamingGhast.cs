using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class FlamingGhast : ModNPC
	{	float ai1 = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Flaming Ghast");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = -1; 
            npc.lifeMax = 150;   
            npc.damage = 50; 
            npc.defense = 16;  
            npc.knockBackResist = 0f;
            npc.width = 48;
            npc.height = 56;
			Main.npcFrameCount[npc.type] = 4;  
            npc.value = 4450;
            npc.npcSlots = 0.6f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
            npc.netUpdate = true;
			banner = npc.type;
			bannerItem = ItemType<FlamingGhastBanner>();

		}
		public override void AI()
		{
			ai1++;
			int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 48, 24, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity.X = npc.velocity.X;
			Main.dust[num1].velocity.Y = -5;
		}
		int frame = 0;
		public override void FindFrame(int frameHeight) 
		{
			frame = frameHeight;
			
				if (ai1 >= 5f) 
				{
					ai1 -= 5f;
					npc.frame.Y += frame;
					if(npc.frame.Y >= 4 * frame)
					{
						npc.frame.Y = 0;
					}
				}
				
			
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			/*
			if(spawnInfo.player.GetModPlayer<SOTSPlayer>().PyramidBiome && spawnInfo.spawnTileType == (ushort)mod.TileType("PyramidSlabTile") && Main.hardMode)
			{
				return 0.2f;
			} */
			return 0;
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("CursedMatter"), Main.rand.Next(2) + 2);	
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.CursedFlame, Main.rand.Next(4) + 2);
			if (Main.rand.Next(20) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedCaviar"), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 50.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
					num++;
				}
			}
            else
			{
				if (Main.netMode != 1)
				{
					for (int k = 0; k < 30; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
					}
					for (int k = 0; k < 8; k++)
					{
						Projectile.NewProjectile((npc.Center.X), npc.Center.Y, Main.rand.Next(-7, 8), Main.rand.Next(-7, 8), 96, 32, 0);
					}
				}
            }		
		}
	}
}





















