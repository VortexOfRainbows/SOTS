using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.GhostTown;
using SOTS.Items.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class ArcticGoblin : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arctic Goblin");
		}
		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.GoblinPeon);
			aiType = NPCID.GoblinScout;
			NPC.lifeMax = 100;
			NPC.damage = 24;
			animationType = NPCID.GoblinPeon;
			Main.npcFrameCount[NPC.type] = 16;
			Banner = NPC.type;
			BannerItem = ItemType<ArcticGoblinBanner>();
		}
		public override void AI()
		{
			npc.TargetClosest(true);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (var i = 0; i < 30; ++i)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 53, 2.5f * (float)hitDirection, -2.5f, 0, new Color(), 1f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GoblinEskimoGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GoblinEskimoGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GoblinEskimoGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GoblinEskimoGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GoblinEskimoGore3"), 1f);
			}
			else
			{
				for (int i = 0; i < damage / (float)npc.lifeMax * 50.0; i++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 53, (float)hitDirection, -1f, 0, new Color(), 0.8f);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TatteredCloth, Main.rand.Next(2) + 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<FragmentOfPermafrost>(), Main.rand.Next(2) + 1);

			if(Main.rand.Next(2) == 0 || Main.expertMode)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<AncientSteelBar>(), Main.rand.Next(2) + 1);

			if (Main.rand.NextBool(2) || Main.expertMode)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<StrawberryIcecream>(), 1);

			if (Main.rand.NextBool(2))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SpikyBall, Main.rand.Next(1, 16));
		}
	}
}